using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface IUserRepository
    {
        void Update(AppUser user);
        Task<bool> SaveAllAsync();
        Task<IEnumerable<AppUser>> GetUsersAsync();
        Task<AppUser> GetUserByIdAsync(int id);
        Task<AppUser> GetUserByUsernameAsync(string username);
        Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams);
        Task<MemberDto> GetMemberAsync(string username);
        //Task<MemberDto> GetMemberAsync(int id);

        Task<IEnumerable<AppUser>> GetUsersByDepartmentAsync(int departmentId);

       // Task<List<AppUser>> GetUsersByDepartmentAndRoleAsync(int departmentId, string roleName);

        Task<IEnumerable<AppUser>> GetUsersByDepartmentAndRoleAsync(int departmentId, string roleName);

        Task<List<Department>> GetUserDepartmentsByUserIdAsync(string userId);
        Task<IEnumerable<UserDto>> GetStaffUsersInDepartmentAsync(int departmentId);
       // Task<int> GetTotalStudentUsersInDepartmentAsync(int departmentId);
        Task<int> GetTotalStudentUsersInDepartmentAsync(string staffUsername);





    }
}