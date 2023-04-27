using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class UserRepository : IUserRepository
    {
        public readonly DataContext _context;
        public readonly IMapper _mapper;
        public readonly UserManager<AppUser> _userManager;
        public UserRepository(DataContext context, IMapper mapper, UserManager<AppUser> userManager)
        {
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
                .Where(u => u.DepartmentId == departmentId)
                .Select(u => new AppUser {
                    UserName = u.UserName,
                    Email = u.Email
                })
                .ToListAsync();
        }
        // public async Task<List<AppUser>> GetUsersByDepartmentAndRoleAsync(int departmentId, string roleName)
        // {
        //     var role = await _userManager.FindByNameAsync(roleName);
        //     if (role == null)
        //     {
        //         throw new ApplicationException($"Role '{roleName}' does not exist.");
        //     }

        //     var users = await _context.Users
        //         .Include(u => u.Department)
        //         .Where(u => u.Department.Id == departmentId)
        //         .Where(u => u.UserRoles.Any(ur => ur.RoleId == role.Id))
        //         .ToListAsync();

        //     return users;
        // }
        public async Task<IEnumerable<AppUser>> GetUsersByDepartmentAndRoleAsync(int departmentId, string roleName)
        {
            var users = await _context.Users
                .Where(u => u.DepartmentId == departmentId)
                .Where(u => u.UserRoles.Any(ur => ur.Role.Name == roleName))
                .ToListAsync();

            return _mapper.Map<IEnumerable<AppUser>>(users);
        }












        // public async Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams)
        // {
        //     var query = _context.Users
        //         .Include(u => u.Feedbacks)
        //         .OrderByDescending(u => u.LastActive)
        //         .AsQueryable();

        //     if (userParams.MinAge != 18 || userParams.MaxAge != 99)
        //     {
        //         var minDob = DateTime.Today.AddYears(-userParams.MaxAge - 1);
        //         var maxDob = DateTime.Today.AddYears(-userParams.MinAge);

        //         query = query.Where(u => u.DateOfBirth >= minDob && u.DateOfBirth <= maxDob);
        //     }

        //     if (!string.IsNullOrEmpty(userParams.Gender))
        //     {
        //         query = query.Where(u => u.Gender == userParams.Gender);
        //     }

        //     if (!string.IsNullOrEmpty(userParams.OrderBy))
        //     {
        //         switch (userParams.OrderBy)
        //         {
        //             case "created":
        //                 query = query.OrderByDescending(u => u.Created);
        //                 break;
        //             default:
        //                 query = query.OrderByDescending(u => u.LastActive);
        //                 break;
        //         }
        //     }

        //     var members = query.ProjectTo<MemberDto>(_mapper.ConfigurationProvider).AsNoTracking();
        //     return await PagedList<MemberDto>.CreateAsync(members, userParams.pageNumber, userParams.PageSize);
        // }


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

  

        // public async Task<MemberDto> GetMemberAsync(string username)
        // {
        //     var user = await _context.Users
        //         .SingleOrDefaultAsync(x => x.UserName == username);

        //     var feedbacks = await _context.Feedbacks
        //         .Where(x => x.SenderId == user.Id)
        //         .ToListAsync();

        //     var member = _mapper.Map<MemberDto>(user);
        //     member.Feedbacks = feedbacks.Select(x => _mapper.Map<FeedbackDto>(x)).ToList();




        //     return member;
        // }








    }
}