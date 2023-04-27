using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AdminController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly DataContext _context;
        public AdminController(UserManager<AppUser> userManager, IUserRepository userRepository,DataContext context)
        {
            _context = context;
            _userRepository = userRepository;
            _userManager = userManager;
        }

        public UserManager<AppUser> _userManager;

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("users-with-roles")]
        public async Task<ActionResult> GetUsersWithRoles() 
        {
            var users = await _userManager.Users
                .Include(r => r.UserRoles)
                .ThenInclude(r => r.Role)
                .OrderBy(u => u.UserName)
                .Select(u => new
                {
                    u.Id,
                    Username =u.UserName,
                    Roles = u.UserRoles.Select(R => R.Role.Name).ToList()
                })
                .ToListAsync();

             return Ok(users);
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("edit-roles/{username}")]
        public async Task<ActionResult> EditRoles(string username, [FromQuery] string roles) 
        {
            var selecedRoles = roles.Split(",").ToArray();
            var user = await _userManager.FindByNameAsync(username);
             if (user == null) return NotFound("Could not find user");
            var userRoles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.AddToRolesAsync(user, selecedRoles.Except(userRoles));

            if (!result.Succeeded) return  BadRequest("Failed to add to role");

          //  result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selecedRoles));

            if (!result.Succeeded) return BadRequest("Failed to remov from roles");

            return Ok(await _userManager.GetRolesAsync(user));

        }

        // get all staffs per department
        [HttpGet("department/{departmentId}/staff")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetStaffUsersInDepartment(int departmentId)
        {
            var users = await _userRepository.GetStaffUsersInDepartmentAsync(departmentId);

            var userDtos = users.Select(u => new UserDto 
            {
                Username = u.Username,
                FullName = u.FullName
            });

            return Ok(userDtos);
        }
        //remove role from user
        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("remove-roles/{username}")]
        public async Task<ActionResult> RemoveRoles(string username, [FromQuery] string roles) 
        {
            var selectedRoles = roles.Split(",").ToArray();
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return NotFound("Could not find user");
            var userRoles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, selectedRoles.Intersect(userRoles));

            if (!result.Succeeded) return  BadRequest("Failed to remove role");

            return Ok(await _userManager.GetRolesAsync(user));
        }



        
    }
}