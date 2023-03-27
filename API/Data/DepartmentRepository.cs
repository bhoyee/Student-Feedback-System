using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DepartmentRepository : IDeparmtentRepo
    {
        public readonly DataContext _context;
        public DepartmentRepository(DataContext context)
        {
            _context = context;
            
        }
        public  async Task<Department> AddDepartment(Department department)
        {
            var result = await _context.Departments.AddAsync(department);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task DeleteDepartment(int id)
        {
            var result = await _context.Departments
                .FirstOrDefaultAsync( x => x.Id == id);
            
            if (result != null)
            {
                _context.Departments.Remove(result);
                await _context.SaveChangesAsync();
            }

        }

        public async Task<Department> GetDepartmentByIdAsync(int id)
        {
            return await _context.Departments
                // .Include( x => x.)
                .FindAsync(id);
        }

        public async Task<Department> GetDepartmentByNameAsync(string deptname)
        {
            return await _context.Departments
                .FirstOrDefaultAsync(x => x.DepartmentName == deptname);
        }

        public async Task<IEnumerable<Department>> GetDepartmentsAsync()
        {
            return await _context.Departments.ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<Department>> Search(string deptName)
        {
            IQueryable<Department> query = _context.Departments;
            
            if (!string.IsNullOrEmpty(deptName))
            {
                query = query.Where(x => x.DepartmentName.Contains(deptName));
            }

            return await query.ToListAsync();
        }

        public void Update(Department department)
        {
            throw new NotImplementedException();
        }
    }

}