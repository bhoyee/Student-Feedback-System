using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static API.Controllers.FeedbacksController;
using Microsoft.Extensions.DependencyInjection;



namespace API.Data
{
    public class DepartmentRepository : IDeparmtentRepo
    {
        private readonly DataContext _context;
         private readonly IMapper _mapper;
      
        public readonly UserManager<AppUser> _userManager;
        public readonly IServiceProvider _serviceProvider;
        public readonly RoleManager<IdentityRole> _roleManager;
        public readonly IEmailService _emailService;

    public DepartmentRepository(DataContext context, IMapper mapper, RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager, IServiceProvider serviceProvider, IEmailService emailService)
    {
            _emailService = emailService;
            _roleManager = roleManager;
            _serviceProvider = serviceProvider;
            _userManager = userManager;
      
        _context = context;
        _mapper = mapper;
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
            //return await _context.Departments.ToListAsync();
               return await _context.Departments
               // .Include(u => u.Users)
               // .Include(x => x.Department)
                .ToListAsync();
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

        
        public async Task<int> CreateFeedbackAsync(FeedbackDto feedbackDto)
        {
            var feedback = new Feedback
            {
                Title = feedbackDto.Title,
                Content = feedbackDto.Content,
                SenderId = feedbackDto.SenderId,
                IsAnonymous = feedbackDto.IsAnonymous,
                DepartmentId = feedbackDto.DepartmentId,
                DateCreated = DateTime.Now,
            };

            // Get the list of feedback recipients based on the selected target audience
            List<FeedbackRecipient> recipients = new List<FeedbackRecipient>();

            var roleManager = _serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            if (feedbackDto.TargetAudience == FeedbackTargetAudience.Department.ToString())
            {
                var studentsInDepartment = await _userManager.GetUsersInRoleAsync("Student");
                foreach (var student in studentsInDepartment)
                {
                    if (student.DepartmentId == feedbackDto.DepartmentId)
                    {
                        recipients.Add(new FeedbackRecipient { RecipientId = student.Id, IsRead = false });
                    }
                }
            }
            else if (feedbackDto.TargetAudience == FeedbackTargetAudience.AllStudents.ToString())
            {
                var students = await _userManager.GetUsersInRoleAsync("Student");
                foreach (var student in students)
                {
                    recipients.Add(new FeedbackRecipient { RecipientId = student.Id, IsRead = false });
                }
            }

            feedback.Recipients = recipients;

            _context.Feedbacks.Add(feedback);

            await _context.SaveChangesAsync();

            foreach (var recipient in feedback.Recipients)
            {
                var student = await _userManager.FindByIdAsync(recipient.RecipientId.ToString());
                if (student != null)
                {
                    await _emailService.SendFeedbackNotificationEmailAsync(student.Email, feedbackDto.Title, feedback.Id);
                }
            }

            return feedback.Id;
        }



        
    }

}