using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;

namespace API.Interfaces
{
    public interface IDeparmtentRepo
    {
        void Update(Department department);
        Task<bool> SaveAllAsync();
        Task<IEnumerable<Department>> GetDepartmentsAsync();
      //  Task<Department> GetDepartmentByIdAsync(int id);
        Task<Department> GetDepartmentByIdAsync(int id);

        Task<Department> GetDepartmentByNameAsync(string username);
        Task<IEnumerable<Department>> Search (string deptName);
        Task<Department> AddDepartment(Department department);
        Task DeleteDepartment(int id);
        Task<int> CreateFeedbackAsync(FeedbackDto feedbackDto);


    }
}