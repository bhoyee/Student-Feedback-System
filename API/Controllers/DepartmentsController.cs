using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
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

        public DepartmentsController(DataContext context, UserManager<AppUser> userManager, IDeparmtentRepo departmentRepo, IMapper mapper, IUserRepository userRepository, IFeedbackRepository feedbackRepository)
        {
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
            try{
                if (department == null)
                    return BadRequest();
                
                var createDepartment = await _DepartmentRepo.AddDepartment(department);

                return CreatedAtAction(nameof(GetDepartment),
                    new { id = createDepartment.Id}, createDepartment);
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error creating new department record");
            }
        
        } 
        [HttpGet] // api/department (this get all departments)
        public async Task<ActionResult<IEnumerable<DepartmentDto>>> GetDepartments()
        {
            var departments = await _DepartmentRepo.GetDepartmentsAsync();
            var returnDepts = _mapper.Map<IEnumerable<DepartmentDisplayDto>>(departments);

            foreach (var dept in returnDepts)
            {
                var users = await _userRepository.GetUsersByDepartmentAsync(dept.Id);
                dept.totalUsers = users.Count();

                var student_users = await _userRepository.GetUsersByDepartmentAndRoleAsync(dept.Id, "Student");
                dept.totalStudents = student_users.Count();

                var staff_users = await _userRepository.GetUsersByDepartmentAndRoleAsync(dept.Id, "Staff");
                dept.totalStaffs = staff_users.Count();


                var fback = await _feedbackRepository.GetFeedbacksByDepartmentIdAsync(dept.Id);
                dept.totalFeedback = fback.Count();

                var openFeedbackCount = await _feedbackRepository.GetOpenFeedbackCountByDepartmentAsync(dept.Id);
                    dept.totalOpenFeedback = openFeedbackCount;


               // Debug.WriteLine($"Department ID: {dept.Id}, Feedback Count: {dept.FeedbackCount}, Open Feedback Count: {dept.OpenFeedbackCount}");

            }

            return Ok(returnDepts);
        }

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


        [HttpGet("{departmentId}/feedbacks")]
        public async Task<IActionResult> GetFeedbacksByDepartment(int departmentId)
        {
            var feedbackDtos = await _feedbackRepository.GetAllFeedbacksByDepartmentIdAsync(departmentId);

            return Ok(feedbackDtos);
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
        [HttpGet("{id:int}/users")] // this get all users in department
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
        [Authorize(Roles = "Dept_Head, Moderator")]
        public async Task<IActionResult> CreateFeedback(FeedbackDto feedbackDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await _userManager.FindByIdAsync(userId);

            // Check if user has the necessary role
            if (!await _userManager.IsInRoleAsync(user, "dept_head") && !await _userManager.IsInRoleAsync(user, "moderator"))
            {
                return Unauthorized();
            }

            // Check if feedback is targeted to the user's department
            if (feedbackDto.TargetAudience == FeedbackTargetAudience.Department.ToString() && feedbackDto.DepartmentId != user.DepartmentId)
            {
                return BadRequest(new { message = "Feedback target audience is invalid" });
            }

            // Check if feedback is targeted to all students and user is not a moderator
            if (feedbackDto.TargetAudience == FeedbackTargetAudience.AllStudents.ToString() && !await _userManager.IsInRoleAsync(user, "moderator"))
            {
                return Unauthorized();
            }

            feedbackDto.SenderId = user.Id;
            feedbackDto.DepartmentId = user.DepartmentId;

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




               
    }

}