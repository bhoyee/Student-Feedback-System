using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    public class DepartmentsController : BaseApiController
    {
       // private readonly DataContext _context;
        private readonly IMapper _mapper;
        public readonly IDeparmtentRepo _DepartmentRepo;
        private readonly IUserRepository _userRepository;
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly UserManager<AppUser> _userManager;
        public readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<DepartmentsController> _logger;
        private readonly IFeedbackReplyRepository _feedbackReplyRepository;

        public DepartmentsController(IFeedbackReplyRepository feedbackReplyRepository, ILogger<DepartmentsController> logger,IHttpContextAccessor httpContextAccessor, DataContext context, UserManager<AppUser> userManager, IDeparmtentRepo departmentRepo, IMapper mapper, IUserRepository userRepository, IFeedbackRepository feedbackRepository)
        {
            _feedbackReplyRepository = feedbackReplyRepository;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            this._userManager = userManager;
            _feedbackRepository = feedbackRepository;
            _userRepository = userRepository;
            _DepartmentRepo = departmentRepo;
           // _context = context;
            _mapper = mapper;
        }
        [HttpPost]
        public async Task<ActionResult<Department>> CreateDepartment(Department department)
        {
            try {
                if (department == null)
                    return BadRequest();

                department.CreatedAt = DateTime.UtcNow; // set createdAt to current UTC time

                var createdDepartment = await _DepartmentRepo.AddDepartment(department);

                return CreatedAtAction(nameof(GetDepartment),
                    new { id = createdDepartment.Id }, createdDepartment);
            } catch(Exception) {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error creating new department record");
            }
        }
        [HttpGet("all/departments")]
        public async Task<IActionResult> GetAllDepartments()
        {
            var departments = await _context.Departments.ToListAsync();
            return Ok(departments);
        }


        [HttpGet] // api/department (this get all departments)
        public async Task<ActionResult<IEnumerable<DepartmentDto>>> GetDepartments()
        {
            var departments = await _DepartmentRepo.GetDepartmentsAsync();
            var returnDepts = _mapper.Map<IEnumerable<DepartmentDto>>(departments);

            return Ok(returnDepts);
        }


        // [HttpGet] // api/department (this get all departments)
        // public async Task<ActionResult<IEnumerable<DepartmentDto>>> GetDepartments()
        // {
        //     var departments = await _DepartmentRepo.GetDepartmentsAsync();
        //    // var returnDepts = _mapper.Map<IEnumerable<DepartmentDisplayDto>>(departments);
        //     var returnDepts = _mapper.Map<IEnumerable<DepartmentDisplayDto>>(departments, opt => opt.Items["UserRepository"] = _userRepository);


        //     foreach (var dept in returnDepts)
        //     {
        //         var users = await _userRepository.GetUsersByDepartmentAsync(dept.Id);
        //         dept.totalUsers = users.Count();

        //         var student_users = await _userRepository.GetUsersByDepartmentAndRoleAsync(dept.Id, "Student");
        //         dept.totalStudents = student_users.Count();

        //         var staff_users = await _userRepository.GetUsersByDepartmentAndRoleAsync(dept.Id, "Staff");
        //         dept.totalStaffs = staff_users.Count();


        //         var fback = await _feedbackRepository.GetFeedbacksByDepartmentIdAsync(dept.Id);
        //         dept.totalFeedback = fback.Count();

        //         var openFeedbackCount = await _feedbackRepository.GetOpenFeedbackCountByDepartmentAsync(dept.Id);
        //             dept.totalOpenFeedback = openFeedbackCount;


        //        // Debug.WriteLine($"Department ID: {dept.Id}, Feedback Count: {dept.FeedbackCount}, Open Feedback Count: {dept.OpenFeedbackCount}");

        //     }

        //     return Ok(returnDepts);
        // }

        [HttpGet("{id}")]  //(this get a specific department)
     
        public async Task<ActionResult<DepartmentDisplayDto>> GetDept(int id)
        {
            var dept = await _DepartmentRepo.GetDepartmentByIdAsync(id);

            if (dept == null)
            {
                return NotFound();
            }

            var deptDto = _mapper.Map<DepartmentDisplayDto>(dept);

            // Get total number of users in department
               var users = await _userRepository.GetUsersByDepartmentAsync(dept.Id);
               var totaluser = users.Count();
               deptDto.totalUsers = totaluser;

            // Get total number of feedback    
                var fback = await _feedbackRepository.GetFeedbacksByDepartmentIdAsync(dept.Id);
                deptDto.totalFeedback = fback.Count();

            // Get total number of student
                var student_users = await _userRepository.GetUsersByDepartmentAndRoleAsync(dept.Id, "Student");
                deptDto.totalStudents = student_users.Count();

            // get total number of staff
                var staff_users = await _userRepository.GetUsersByDepartmentAndRoleAsync(dept.Id, "Staff");
                deptDto.totalStaffs = staff_users.Count();

            // get total open feedback
                var openFeedbackCount = await _feedbackRepository.GetOpenFeedbackCountByDepartmentAsync(dept.Id);
                deptDto.totalOpenFeedback = openFeedbackCount;

            return Ok(deptDto);
        }


        [HttpGet("{id}/open-feedbacks")]
        public async Task<ActionResult<IEnumerable<FeedbackDto>>> GetOpenFeedbacksByDepartmentId(int id)
        {
            var feedbacks = await _feedbackRepository.GetFeedbacksByDepartmentIdAndStatusAsync(id, FeedbackStatus.Open);


            foreach (var feedback in feedbacks)
            {
                var sender = await _userManager.FindByIdAsync(feedback.SenderId.ToString()); // convert SenderId to a string
                feedback.SenderName = sender?.UserName;
            }
                return Ok(feedbacks);
         
        }

        
        [HttpGet("{id}/inprogress-feedbacks")]
        public async Task<ActionResult<IEnumerable<FeedbackDto>>> GetInProgressFeedbacksByDepartmentId(int id)
        {
            var feedbacks = await _feedbackRepository.GetFeedbacksByDepartmentIdAndStatusAsync(id, FeedbackStatus.InProgress);


            foreach (var feedback in feedbacks)
            {
                var sender = await _userManager.FindByIdAsync(feedback.SenderId.ToString()); // convert SenderId to a string
                feedback.SenderName = sender?.UserName;
            }
                return Ok(feedbacks);
         
        }

          
        [HttpGet("{id}/closed-feedbacks")]  //departments/deptid/closed-feedback
        public async Task<ActionResult<IEnumerable<FeedbackDto>>> GetCloseedFeedbacksByDepartmentId(int id)
        {
            var feedbacks = await _feedbackRepository.GetFeedbacksByDepartmentIdAndStatusAsync(id, FeedbackStatus.Closed);


            foreach (var feedback in feedbacks)
            {
                var sender = await _userManager.FindByIdAsync(feedback.SenderId.ToString()); // convert SenderId to a string
                feedback.SenderName = sender?.UserName;
            }
                return Ok(feedbacks);
         
        }

        // get all feedbacks in department by providing the departmentId
        [HttpGet("{departmentId}/feedbacks")]
        public async Task<IActionResult> GetFeedbacksByDepartment(int departmentId)
        {
            var feedbackDtos = await _feedbackRepository.GetAllFeedbacksByDepartmentIdAsync(departmentId);

            return Ok(feedbackDtos);
        }

        [Authorize(Roles = "Moderator,Staff-admin")]
        [HttpGet("department-feedback")]
        public async Task<ActionResult<IEnumerable<FeedbackDto>>> GetDepartmentFeedback()
        {
            try
            {
                var currentUser = await _userRepository.GetUserByUsernameAsync(User.FindFirstValue(ClaimTypes.Name));
                if (currentUser == null)
                {
                    return NotFound($"Unable to retrieve user with username {User.FindFirstValue(ClaimTypes.Name)}");
                }

                int departmentId = 0;
                if (currentUser.DepartmentId != null)
                {
                    departmentId = currentUser.DepartmentId;
                }

                var feedback = await _feedbackRepository.GetAllFeedbacksByDepartmentIdAsync(departmentId);
                var feedbackDtos = _mapper.Map<IEnumerable<FeedbackDto>>(feedback);

                foreach (var feedbackDto in feedbackDtos)
                {
                    if (feedbackDto.IsAnonymous)
                    {
                        feedbackDto.SenderName = "Anonymous";
                        feedbackDto.SenderFullName = "Anonymous";
                    }
                    else
                    {
                        var sender = await _userRepository.GetUserByIdAsync(feedbackDto.SenderId);
                        if (sender != null)
                        {
                            feedbackDto.SenderName = sender.UserName;
                            feedbackDto.SenderFullName = $"{sender.FullName}";
                        }
                    }
                }

                return Ok(feedbackDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error in retrieving record from database");
            }
        }

        // get specific feedback with replies (staff)
[Authorize(Roles = "Staff-admin,Moderator")]
[HttpGet("department-feedback/{feedbackId}")]
public async Task<ActionResult<FeedbackDto>> GetFeedback(int feedbackId)
{
    var currentUser = await _context.Users.SingleOrDefaultAsync(u => u.UserName == User.Identity.Name);
    if (currentUser == null)
    {
        return NotFound($"Unable to retrieve user with username {User.Identity.Name}");
    }

    var feedback = await _context.Feedbacks
        .Include(f => f.Sender)
        .Include(f => f.Replies).ThenInclude(r => r.User)
        .FirstOrDefaultAsync(f => f.Id == feedbackId && f.DepartmentId == currentUser.DepartmentId);

    if (feedback == null)
    {
        _logger.LogInformation($"Feedback with ID {feedbackId} not found.");
        return NotFound($"Feedback with ID {feedbackId} not found.");
    }

    var senderName = feedback.IsAnonymous ? "Anonymous" : feedback.Sender.FullName;

    var feedbackDto = new FeedbackDto
    {
        Id = feedback.Id,
        Title = feedback.Title,
        Content = feedback.Content,
        DateCreated = feedback.DateCreated,
        DepartmentId = feedback.DepartmentId,
        Status = feedback.Status,
        SenderName = senderName,
        SenderFullName = senderName,
        FeedbackReplies = feedback.Replies.Select(r => new FeedbackReplyDto
        {
            Id = r.Id,
            Content = r.Content,
            ModifiedAt = r.ModifiedAt,
            UserId = r.UserId,
           // User = r.User == null ? null : new AppUser { FullName = r.User.FullName },
           UserFullName = r.User.FullName, // Populate the UserFullName property with the full name of the user
            IsPublic = r.IsPublic,
            Status = (int?)r.Status,
        }).ToList()
    };

    return feedbackDto;
}


        // Assuming you have a FeedbackRepository class with a GetFeedbackByIdAsync method
        [HttpGet("{deptID}/feedbacks/{feedbackId}")]

        public async Task<ActionResult<FeedbackDto>> GetFeedback(int deptID, int feedbackId)
        {
            var feedback = await _feedbackRepository.GetFeedbackByIdAsync(feedbackId);
            if (feedback == null)
            {
                return NotFound($"Feedback with ID {feedbackId} not found");
            }
        if (feedback.DepartmentId != deptID)
            {
                return BadRequest($"Feedback with ID {feedbackId} does not belong to department with ID {deptID}");
            }

            var department = await _DepartmentRepo.GetDepartmentByIdAsync(deptID);

            if (department == null)
            {
                return NotFound();
            }

            var feedbackToReturn = _mapper.Map<FeedbackDto>(feedback);

            if (feedback.IsAnonymous)
            {
                feedbackToReturn.SenderName = "Anonymous";
            }
            else
            {
                var sender = await _userRepository.GetUserByIdAsync(feedback.SenderId);
            //  feedbackToReturn.SenderName = sender.FirstName + " " + sender.LastName;
                feedbackToReturn.SenderName = sender.UserName;
            }

            return Ok(feedbackToReturn);
        }


        // api/departments/3 or userid
        [HttpGet("{id:int}/users")] // this get all users(staff) in department
        public async Task<ActionResult<Department>> GetDepartment(int id)
        {
       
           try
           {
               var users = await _userRepository.GetUsersByDepartmentAsync(id);
               var userDtos = _mapper.Map<IEnumerable<useremailDTO>>(users);
      
            if(users == null)
            {
                return NotFound($"Department with Id = {id} not found");

            }
            return Ok(userDtos);

           }
           catch(Exception)
           {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error in retriving record from database");
           }
        }
        // get all staffs in the department of login staff
        [HttpGet("staffs")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetDepartmentUsers()
        {
            try
            {
                var currentUser = await _userRepository.GetUserByUsernameAsync(User.FindFirstValue(ClaimTypes.Name));
                if (currentUser == null)
                {
                    return NotFound($"Unable to retrieve user with username {User.FindFirstValue(ClaimTypes.Name)}");
                }

                if (currentUser.DepartmentId == null)
                {
                    return BadRequest("User does not belong to any department");
                }

            int? departmentId = currentUser.DepartmentId;


                var users = await _userRepository.GetUsersByDepartmentAndRoleAsync(departmentId.Value, "staff");
                var userDtos = _mapper.Map<IEnumerable<useremailDTO>>(users);

                return Ok(userDtos);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error in retrieving record from database");
            }
        }




        // private async Task<bool> DepartmentExists(string deptName)
        // {
        //     return await _DepartmentRepo.Search.AnyAsync(x => x.Email == email.ToLower());

        // }  

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteDepartment(int id)
        {
       
           try
           {
              var deleteDepts = await _DepartmentRepo.GetDepartmentByIdAsync(id);
             // var deptToReturn = _mapper.Map<DepartmentCreationDTO>(deleteDepts);
      
            if(deleteDepts == null)
            {
                return NotFound($"Department with Id = {id} not found");
            }
            await _DepartmentRepo.DeleteDepartment(id);
            return Ok($"Department with Id = {id} deleted");

           }
           catch(Exception)
           {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error in deleting department record from database");
           }

        }


        [HttpPost("feedback/create")]
        [Authorize(Roles = "Dept_Head, Moderator, Staff-admin, Admin")]
        public async Task<IActionResult> CreateFeedback(FeedbackDto feedbackDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await _userManager.FindByIdAsync(userId);

            // Check if user has the necessary role
            if (!await _userManager.IsInRoleAsync(user, "dept_head") &&
                !await _userManager.IsInRoleAsync(user, "moderator") &&
                !await _userManager.IsInRoleAsync(user, "staff-admin") &&
                !await _userManager.IsInRoleAsync(user, "admin"))
            {
                return Unauthorized();
            }

            if (feedbackDto.TargetAudience == FeedbackTargetAudience.AllStudents.ToString())
            {
        
                // If feedback is targeted to all students, allow only users with the "Moderator", "Staff-admin", and "Admin" roles
                if (!await _userManager.IsInRoleAsync(user, "Staff-admin") ||
                    (await _userManager.IsInRoleAsync(user, "Staff-admin") && user.Department.Category == "academic") &&
                    !await _userManager.IsInRoleAsync(user, "Moderator") ||
                    (await _userManager.IsInRoleAsync(user, "Moderator") && user.Department.Category == "academic"))
                {
                    return Unauthorized("You can only send feedback to students in your department");
                }
            }
            
            feedbackDto.SenderId = user.Id;
            feedbackDto.DepartmentId = user.DepartmentId;

            _logger.LogInformation("User Department ID: {0}", user.DepartmentId);

            var result = await _DepartmentRepo.CreateFeedbackAsync(feedbackDto);

            if (result > 0)
            {
                return Ok(new { message = "Feedback created successfully" });
            }
            else
            {
                return BadRequest(new { message = "Error creating feedback" });
            }
        }        


        // GET total feedback count per department
        [HttpGet("dept/{departmentId}/feedback-counts")]
        public async Task<ActionResult<DepartmentFeedbackCountsDto>> GetDepartmentFeedbackCounts(int departmentId)
        {
            var department = await _context.Departments.FindAsync(departmentId);

            if (department == null)
            {
                return NotFound("Such Department does not exist");
            }

            var totalFeedbacks = await _context.Feedbacks
                .Where(f => f.DepartmentId == departmentId)
                .CountAsync();

            var totalOpenFeedbacks = await _context.Feedbacks
                .Where(f => f.DepartmentId == departmentId && f.Status == FeedbackStatus.Open)
                .CountAsync();

            var totalClosedFeedbacks = await _context.Feedbacks
                .Where(f => f.DepartmentId == departmentId && f.Status == FeedbackStatus.Closed)
                .CountAsync();

            var countsDto = new DepartmentFeedbackCountsDto
            {
                TotalFeedbacks = totalFeedbacks,
                TotalOpenFeedbacks = totalOpenFeedbacks,
                TotalClosedFeedbacks = totalClosedFeedbacks
            };

            return countsDto;
        }

        // get all staffs in department 
        [HttpGet("dept/{departmentId}/staffs")]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetStaffsInDepartment(int departmentId)
        {
            var usersInDepartment = await _context.Users
                .Include(u => u.Department)
                .Where(u => u.Department.Id == departmentId && u.UserRoles.Any(ur => ur.Role.Name == "Staff"))
                .ToListAsync();

            var staffsInDepartment = _mapper.Map<IEnumerable<MemberDto>>(usersInDepartment);

            return Ok(staffsInDepartment);
        }






               
    }

}