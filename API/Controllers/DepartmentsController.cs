using System;
using System.Collections.Generic;
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

        public DepartmentsController(IDeparmtentRepo departmentRepo, IMapper mapper)
        {
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

        [HttpGet] // api/department
        public async Task<ActionResult<IEnumerable<DepartmentCreationDTO>>> GetDepartments()
        {
            var departments = await _DepartmentRepo.GetDepartmentsAsync();
            var returnDepts =  _mapper.Map<IEnumerable<DepartmentCreationDTO>>(departments);
            return Ok(returnDepts);
              
        }

        // api/departments/3 or userid
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Department>> GetDepartment(int id)
        {
       
           try
           {
              var depts = await _DepartmentRepo.GetDepartmentByIdAsync(id);
              var deptToReturn = _mapper.Map<DepartmentCreationDTO>(depts);
      
            if(depts == null)
            {
                return NotFound($"Department with Id = {id} not found");

            }
            return Ok(deptToReturn);

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
               
    }

}