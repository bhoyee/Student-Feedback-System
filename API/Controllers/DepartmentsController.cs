using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http;
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

        public DepartmentsController(IDeparmtentRepo departmentRepo, IMapper mapper, IUserRepository userRepository, IFeedbackRepository feedbackRepository)
        {
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

        // DepartmentsController.cs
        [HttpGet("{id}")]
     
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
         

            // var usersInDept = await _userRepository.GetUsersByDepartmentIdAsync(id);
            // var totalUsers = usersInDept.Count();
            // deptDto.TotalUsers = totalUsers;

            // Get total number of feedback in department
            // var feedbackInDept = await _feedbackRepo.GetFeedbackByDepartmentIdAsync(id);
            // var totalFeedback = feedbackInDept.Count();
            // deptDto.TotalFeedback = totalFeedback;
            // get total open feedback
                var openFeedbackCount = await _feedbackRepository.GetOpenFeedbackCountByDepartmentAsync(dept.Id);
                deptDto.totalOpenFeedback = openFeedbackCount;

            return Ok(deptDto);
        }


        // [HttpGet("{id}")] // api/department/{dept_id} (this get a specific department)
        // public async Task<ActionResult<IEnumerable<DepartmentDto>>> GetDept(int id)
        // {
        //     var departments = await _DepartmentRepo.GetDepartmentByIdAsync(id);
        //     var returnDepts = _mapper.Map<IEnumerable<DepartmentDisplayDto>>(departments);

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

// [HttpGet("{id}/open-feedbacks")]
//     public async Task<ActionResult<IEnumerable<FeedbackDto>>> GetOpenFeedbacksByDepartmentId(int id)
//     {
//         var department = await _DepartmentRepo.GetDepartmentByIdAsync(id);

//         if (department == null)
//         {
//             return NotFound();
//         }

//         var feedbacks = await _feedbackRepository.GetFeedbacksByDepartmentIdAndStatusAsync(department.Id, FeedbackStatus.InProgress);

//         var feedbackDtos = feedbacks.Select(f => new FeedbackDto
//         {
//             Id = f.Id,
//             Title = f.Title,
//             Content = f.Content,
//            // SnderId = f.SenderId,
//           //  SenderName = f.Sender.UserName,
//          //   DepartmentName = f.Department.DepartmentName,
//            // DepartmentId = f.DepartmentId,
//             DateCreated = f.DateCreated
//         }).ToList();

//         return Ok(feedbackDtos);
//     }

    





        // [HttpGet("departmentid/{}")] // api/department
        // public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsersByDepartmentAsync(int departmentId)
        // {
        //     var users = await _userRepository.GetUsersByDepartmentAsync(departmentId);
        //     var memberDtos = _mapper.Map<IEnumerable<MemberDto>>(users);

        //     return Ok(memberDtos);
        // }

        // api/departments/3 or userid
        [HttpGet("{id:int}/users")] // this get all users in department
        public async Task<ActionResult<Department>> GetDepartment(int id)
        {
       
        //    try
        //    {
        //       var depts = await _DepartmentRepo.GetDepartmentByIdAsync(id);
        //       var deptToReturn = _mapper.Map<DepartmentCreationDTO>(depts);
      
        //     if(depts == null)
        //     {
        //         return NotFound($"Department with Id = {id} not found");

        //     }
        //     return Ok(deptToReturn);

        //    }
        //    catch(Exception)
        //    {
        //         return StatusCode(StatusCodes.Status500InternalServerError,
        //             "Error in retriving record from database");
        //    }
 var users = await _userRepository.GetUsersByDepartmentAsync(id);
    var userDtos = _mapper.Map<IEnumerable<useremailDTO>>(users);

    return Ok(userDtos);


    //     var department = await _DepartmentRepo.GetDepartmentByIdAsync(id);
    // var users = await _userRepository.GetUsersByDepartmentAsync(id);
    // var userDtos = _mapper.Map<IEnumerable<useremailDTO>>(users);

    // var departmentDto = _mapper.Map<DepartmentDto>(department);
    // //DepartmentDto
    // departmentDto.UserCount = userDtos.Count();

    // return Ok(departmentDto);

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
               
    }

}