using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class UserRepository : IUserRepository
    {
        public readonly DataContext _context;
        public readonly IMapper _mapper;
        public readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
  
        public UserRepository(DataContext context, IMapper mapper, UserManager<AppUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        
            _userManager = userManager;
            _mapper = mapper;
            _context = context;

        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await _context.Users
                .Include(p => p.Photos)
                .Include(x => x.Department)
                .ToListAsync();
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
           return await _context.Users.FindAsync(id);
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            return await _context.Users
                .Include(p => p.Photos)
                .SingleOrDefaultAsync(x => x.UserName == username); 
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(AppUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }

        // public async Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams)
        // {
        //      var query = _context.Users
        //         .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
        //         .AsNoTracking();
        //     return await PagedList<MemberDto>.CreateAsync(query, userParams.pageNumber, userParams.PageSize);
        // }

        public async Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams)
        {
            var query = _context.Users
                .Include(u => u.Feedbacks)
                .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                .AsNoTracking();

            return await PagedList<MemberDto>.CreateAsync(query, userParams.pageNumber, userParams.PageSize);
        }

        // public async Task<IEnumerable<AppUser>> GetUsersByDepartmentAsync(int departmentId)
        // {
        //     return await _context.Users
        //         .Where(u => u.DepartmentId == departmentId)
        //         .ToListAsync();
        // }

        public async Task<IEnumerable<AppUser>> GetUsersByDepartmentAsync(int departmentId)
        {
            
               return await _context.Users
                .Where(u => u.DepartmentId == departmentId && u.UserRoles.Any(r => r.Role.Name == "Staff"))
                .Select(u => new AppUser {
                    UserName = u.UserName,
                    FullName = u.FullName,
                    Email = u.Email,
                  
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<AppUser>> GetUsersByDepartmentAndRoleAsync(int departmentId, string roleName)
        {
            var users = await _context.Users
                .Where(u => u.DepartmentId == departmentId)
                .Where(u => u.UserRoles.Any(ur => ur.Role.Name == roleName))
                .ToListAsync();

            return _mapper.Map<IEnumerable<AppUser>>(users);
        }

        public async Task<MemberDto> GetMemberAsync(string username)
        {
              return await _context.Users
                .Where(x => x.UserName == username)
                .Include(f => f.Feedbacks.Where(c =>c.Sender.UserName == username))
                .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }

        public async Task<List<Department>> GetUserDepartmentsByUserIdAsync(string userId)
        {
            var departments = await _context.Departments
                .Where(d => d.Users.Any(ud => ud.Id == int.Parse(userId)))
                .ToListAsync();

            return departments;
        }
        public async Task<IEnumerable<UserDto>> GetStaffUsersInDepartmentAsync(int departmentId)
        {
            var users = await _context.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .Where(u => u.UserRoles.Any(ur => ur.Role.Name == "staff" && ur.User.DepartmentId == departmentId))
                .ToListAsync();

            return users
                .Select(u => new UserDto {
                    Username = u.UserName,
                    FullName = u.FullName
                });
        }

        public async Task<int> GetTotalStudentUsersInDepartmentAsync(string staffUsername)
        {
            var currentUser = _httpContextAccessor.HttpContext.User;
            if (currentUser.HasClaim(c => c.Type == ClaimTypes.Role && c.Value == "Staff"))
            {
                var department = await _context.Departments
                    .Include(d => d.Users)
                    .FirstOrDefaultAsync(d => d.Users.Any(u => u.UserName == staffUsername));

                if (department == null)
                    throw new ArgumentException("Invalid department id");

                var totalStudents = await _context.Users
                    .Where(u => u.DepartmentId == department.Id && u.UserRoles != null && u.UserRoles.Any(r => r.Role.Name == "Student"))
                    .CountAsync();

                return totalStudents;
            }
            else
            {
                throw new UnauthorizedAccessException("User must have staff role to access this resource");
            }
        }
        public async Task<IEnumerable<AppUser>> GetUsersByDepartmentIdAsync(int departmentId)
        {
            var studentIds = await _context.Users
                .Where(u => u.DepartmentId == departmentId && u.UserRoles.Any(ur => ur.Role.Name == "Student"))
                .Select(s => s.Id)
                .ToListAsync();

            var students = await _context.Users
                .Where(s => studentIds.Contains(s.Id))
                .ToListAsync();

            return students;
        }
            // public async Task<List<int>> GetUserIdsByRoleAsync(List<string> roles, int departmentId)
            // {
            //     var userIds = await _context.UserRoles
            //         .Where(ur => roles.Contains(ur.Role.Name) && ur.User.DepartmentId == departmentId)
            //         .Select(ur => ur.UserId)
            //         .ToListAsync();

            //     return userIds;
            // }

            public async Task<List<int>> GetUserIdsByRoleAsync(List<string> roleNames)
            {
                return await _context.UserRoles
                    .Where(ur => roleNames.Contains(ur.Role.Name))
                    .Select(ur => ur.UserId)
                    .ToListAsync();
            }



    }
}