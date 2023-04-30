using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using API.DTOs;
using AutoMapper;
using API.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using API.Extensions;
using API.Helpers;
using Microsoft.AspNetCore.Identity;

namespace API.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        public readonly IMapper _mapper;
        public readonly IUserRepository _userRepository;
        public readonly IPhotoService _photoService;
        private readonly IFeedbackRepository _feedbackRepository;
        public readonly IDeparmtentRepo _departmentRepo;
        public readonly  UserManager<AppUser> _userManager;
        public readonly IHttpContextAccessor _httpContextAccessor;
        public readonly DataContext _context;
        public UsersController(DataContext context,UserManager<AppUser> userManager, IUserRepository userRepository, IDeparmtentRepo departmentRepo, IMapper mapper, IPhotoService photoService, IFeedbackRepository feedbackRepository, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _departmentRepo = departmentRepo;
            _feedbackRepository = feedbackRepository;
            _photoService = photoService;
            _userRepository = userRepository;
            _mapper = mapper;
            
        }

        //[Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers([FromQuery]UserParams userParams)
        {
            //   var users = await _userRepository.GetUsersAsync();
            //   var usersToReturn = _mapper.Map<IEnumerable<MemberDto>>(users);
            //   return Ok(usersToReturn);

                var users = await _userRepository.GetMembersAsync(userParams);
                Response.AddPaginationHeader(users.CurrentPage, users.PageSize,
                    users.TotalCount, users.TotalPages);
                return Ok(users);
        }

        

      //  [Authorize(Roles = "Memeber")]
        // api/users/3 or userid
        [HttpGet("{username}", Name = "GetUser")]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            return await _userRepository.GetMemberAsync(username);
          //  return _mapper.Map<MemberDto>(user);

        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
            var username = User.GetUsername();
           // var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userRepository.GetUserByUsernameAsync(username);

            _mapper.Map(memberUpdateDto, user);
            _userRepository.Update(user);

            if (await _userRepository.SaveAllAsync()) return NoContent();
            
            return BadRequest("Failed to update user");
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

            var result = await _photoService.AddPhotoAsync(file);

            if(result.Error != null) return BadRequest(result.Error.Message);

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            if(user.Photos.Count == 0)
            {
                photo.IsMain = true;
            }
            user.Photos.Add(photo);

            if (await _userRepository.SaveAllAsync())
            {
                return CreatedAtRoute("GetUser", new {username = user.UserName}, _mapper.Map<PhotoDto>(photo));
             // return _mapper.Map<PhotoDto>(photo);

            }

            return BadRequest("Problem adding photo");
        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
            
            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photo.IsMain) return BadRequest("This is already your main photo");

            var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);

            if (currentMain !=null) currentMain.IsMain = false;
            photo.IsMain = true;

            if (await _userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to set main photo");
        }

        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photo == null) return NotFound();

            if(photo.IsMain) return BadRequest("You cannot delete your main phot");

            if (photo.PublicId != null)
            {
               var result = await _photoService.DeletePhotoAsync(photo.PublicId);
                if (result.Error != null) return BadRequest(result.Error.Message);
            }

            user.Photos.Remove(photo);

            if (await _userRepository.SaveAllAsync()) return Ok();

            return BadRequest("Failed to delete photo");
        }

        // get feedback from department
        [HttpGet("feedback")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> GetFeedbackForStudent()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var feedback = await _feedbackRepository.GetFeedbackForStudentAsync(userId);

            return Ok(feedback);
        }

        //get list of departments available for user
        [HttpGet("departments")]
        public async Task<IActionResult> GetDepartmentsForUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var departments = await _departmentRepo.GetDepartmentsAsync();

            var userDepartments = await _userRepository.GetUserDepartmentsByUserIdAsync(userId);

            var academicDepartments = userDepartments
                .Where(ud => ud.Category == "academic")
                .Select(ud => ud.DepartmentName)
                .ToList();

            var nonAcademicDepartments = departments
                .Where(d => d.Category == "non-academic")
                .Select(d => d.DepartmentName)
                .ToList();

            var allDepartments = academicDepartments.Concat(nonAcademicDepartments).ToList();

            var result = new {
                AllDepartments = allDepartments
            };

            return Ok(result);
        }

        //Staff-admin adding Department moderator
        [Authorize(Policy = "RequireStaffAdminRole")]
        [HttpPost("dept/edit-roles/{username}")]
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

                //remove role from user
        [Authorize(Policy = "RequireStaffAdminRole")]
        [HttpPost("dept/remove-roles/{username}")]
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

        [Authorize(Roles = "Staff")]
        [HttpGet("total-students")]
        public async Task<IActionResult> GetTotalStudentUsersInDepartment()
        {
            var username = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
            var totalStudents = await _userRepository.GetTotalStudentUsersInDepartmentAsync(username);
            return Ok(totalStudents);
        }

        // admin deleting user
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound("No such user found");
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok("User deleted successfully");
        }



    }
}