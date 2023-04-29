using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class FeedbackRepository : IFeedbackRepository
    {
        public readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
    public FeedbackRepository(DataContext context, IMapper mapper, UserManager<AppUser> userManager)
    {
             _userManager = userManager;
            _mapper = mapper;
            _context = context;
    }

        // public async Task<FeedbackDto> CreateFeedbackAsync(FeedbackCreateDto feedbackCreateDto)
        // {
        //     var feedback = _mapper.Map<Feedback>(feedbackCreateDto);

        //         _context.Feedbacks.Add(feedback);
        //         await _context.SaveChangesAsync();

        //         return _mapper.Map<FeedbackDto>(feedback);
        // }
    
    // public async Task<FeedbackDto> CreateFeedbackAsync(FeedbackCreateDto feedbackCreateDto, int userId)
    // {
        
    //     var user = await _context.Users.FindAsync(userId);
    //     if (user == null)
    //     {
    //         throw new ArgumentException("Invalid user id");
    //     }

    //     var feedback = _mapper.Map<Feedback>(feedbackCreateDto);
    //     feedback.SenderId = userId;
    //     feedback.DepartmentId = user.DepartmentId;
    //     feedback.IsAnonymous = feedbackCreateDto.IsAnonymous;
    
    //     _context.Feedbacks.Add(feedback);
    //     await _context.SaveChangesAsync();

    //     var feedbackDto = _mapper.Map<FeedbackDto>(feedback);
    //     feedbackDto.SenderName = user.UserName;
    //     feedbackDto.DepartmentName = user.Department.DepartmentName;

    //     return feedbackDto;
    // }
//     public async Task<FeedbackDto> CreateFeedbackAsync(FeedbackCreateDto feedbackCreateDto, int userId)
// {
    
    
//     if (feedbackCreateDto == null)
//     {
//         throw new ArgumentNullException(nameof(feedbackCreateDto));
//     }

//     var user = await _context.Users.FindAsync(userId);
//     if (user == null)
//     {
//         throw new ArgumentException("Invalid user id");
//     }

//     var department = await _context.Departments.FindAsync(feedbackCreateDto.DepartmentId);
//     if (department == null)
//     {
//         throw new ArgumentException("Invalid department id");
//     }

//     var feedback = _mapper.Map<Feedback>(feedbackCreateDto);
//     feedback.SenderId = userId;
//     feedback.DepartmentId = department.Id;
//     feedback.IsAnonymous = feedbackCreateDto.IsAnonymous;
    
//     _context.Feedbacks.Add(feedback);
//     await _context.SaveChangesAsync();

//   var feedbackDto = _mapper.Map<FeedbackDto>(feedback);
// feedbackDto.SenderName = user.UserName;
// feedbackDto.DepartmentName = user.Department.DepartmentName;
// feedbackDto.IsAnonymous = feedbackCreateDto.IsAnonymous;


//     return feedbackDto;
// }


    // public async Task<Feedback> CreateFeedbackAsync(FeedbackCreateDto feedbackCreateDto, int userId)
    // {
    //     var user = await _context.Users.FindAsync(userId);
    //     if (user == null)
    //     {
    //         throw new ArgumentException("Invalid user id");
    //     }

    //     var feedback = new Feedback
    //     {
    //         Title = feedbackCreateDto.Title,
    //        Content = feedbackCreateDto.Content,
    //         DepartmentId = feedbackCreateDto.DepartmentId,
    //         SenderId = feedbackCreateDto.SenderId,
    //         IsAnonymous = feedbackCreateDto.IsAnonymous,
    //         DateCreated = DateTime.Now
    //     };

    //     _context.Feedbacks.Add(feedback);
    //     await _context.SaveChangesAsync();

    //     return feedback;
    // }

        // public async Task<Feedback> CreateFeedbackAsync(string title, string content, bool isAnonymous, int departmentId, int userId)
        // {
        // // int inputStatus = 0;
        //     var feedback = new Feedback
        //     {
            
        // //FeedbackStatus status = (FeedbackStatus)Enum.Parse(typeof(FeedbackStatus), inputStatus);
        //         Title = title,
        //         Content = content,
        //         IsAnonymous = isAnonymous,
        //     //  Status = (FeedbackStatus)Enum.Parse(typeof(FeedbackStatus), inputStatus.ToString()),
        //         DepartmentId = departmentId,
        //         SenderId = userId
        //     };

        //     await _context.Feedbacks.AddAsync(feedback);
        //     await _context.SaveChangesAsync();

        //     return feedback;
        // }

        public async Task CreateFeedbackAsync(Feedback feedback)
        {
            await _context.Feedbacks.AddAsync(feedback);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }



    public async Task DeleteFeedbackAsync(int feedbackId)
    {
                var feedback = await _context.Feedbacks.FindAsync(feedbackId);

                if (feedback != null)
                {
                    //throw new EntityNotFoundException(nameof(Feedback), feedbackId);
                    
                    _context.Feedbacks.Remove(feedback);
                    await _context.SaveChangesAsync();
                }

    }

    // public async Task<FeedbackDto> GetFeedbackByIdAsync(int feedbackId)
    // {
    //     try
    //     {
    //         var feedback = await _context.Feedbacks
    //             .Include(f => f.Department)
    //             .Include(f => f.Sender)
    //             .Include(f => f.AssignedTo)
    //             .FirstOrDefaultAsync(f => f.Id == feedbackId);

    //         if (feedback == null)
    //         {
    //             return null;
    //         }

    //         var feedbackDto = _mapper.Map<FeedbackDto>(feedback);
    //         feedbackDto.SenderName = feedback.Sender.UserName; // set the senderName property
    //         return feedbackDto;
    //     }
    //     catch(Exception ex)
    //     {
    //         // Log the exception or return a custom error message
    //         return null;
    //     }
    // }
    public async Task<FeedbackDto> GetFeedbackByIdAsync(int feedbackId)
{
    try
    {
        var feedback = await _context.Feedbacks
            .Include(f => f.Department)
            .Include(f => f.Sender)
            .Include(f => f.AssignedTo)
            .Include(f => f.Replies)
            .FirstOrDefaultAsync(f => f.Id == feedbackId);

        if (feedback == null)
        {
            throw new Exception($"Feedback with ID {feedbackId} not found");
        }

        var feedbackDto = _mapper.Map<FeedbackDto>(feedback);

        if (feedback.IsAnonymous)
        {
            feedbackDto.SenderName = "Anonymous";
        }

      
        feedbackDto.SenderName = feedback.Sender.UserName;
        feedbackDto.DepartmentName = feedback.Department.DepartmentName; // include department name
        feedbackDto.FeedbackReplies = _mapper.Map<List<FeedbackReplyDto>>(feedback.Replies); // include feedback replies

        return feedbackDto;
    }
    catch (Exception ex)
    {
        throw new Exception($"An error occurred while getting the feedback with ID {feedbackId}. {ex.Message}");
    }
}



 public async Task<List<FeedbackDto>> GetFeedbacksByDepartmentIdAndStatusAsync(int departmentId, string status)
{
    FeedbackStatus feedbackStatus;
    if (!Enum.TryParse(status, true, out feedbackStatus))
    {
        // handle invalid feedback status string
        return null;
    }

    var feedbacks = await _context.Feedbacks
        .Include(f => f.Department)
        .Include(f => f.Sender)
        .Include(f => f.AssignedTo)
        .Where(f => f.DepartmentId == departmentId && f.Status == feedbackStatus)
        .Select(f => new FeedbackDto
        {
            Id = f.Id,
            Title = f.Title,
            Content = f.Content,
            SenderId = f.SenderId,
            SenderName = f.Sender.UserName,
            Status = f.Status, // directly map f.Status to Status property
            OpenFeedbackCount = f.Department.Feedbacks.Count(f => f.Status == FeedbackStatus.Open),
            AssignedToName = f.AssignedTo.UserName,
            DateCreated = f.DateCreated
        })
        .ToListAsync();

    return feedbacks;
}



        // public async Task<List<Feedback>> GetFeedbacksByDepartmentIdAsync(int departmentId)
        // {
        //     return await _context.Feedbacks
        //         .Include(f => f.Department)
        //         .Include(f => f.Sender)
        //         .Include(f => f.AssignedTo)
        //         .Where(f => f.DepartmentId == departmentId)
        //         .ToListAsync();
        // }








        // public async Task<IEnumerable<FeedbackDto>> GetFeedbacksAsync()
        // {
        //      var feedbacks = await _context.Feedbacks
        //         .Include(f => f.Department)
        //         .Include(f => f.Sender)
        //         .ToListAsync();

        //     return _mapper.Map<IEnumerable<FeedbackDto>>(feedbacks);
        // }
        public async Task<List<string>> GetFeedbacksAsync(string username)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == username);

            var feedbacks = await _context.Feedbacks
                .Where(x => x.SenderId == user.Id)
                .Select(x => x.Title)
                .ToListAsync();

            return feedbacks;
        }

        public Task<IEnumerable<FeedbackDto>> GetFeedbacksAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<List<Feedback>> GetFeedbacksByDeptIdAsync(int departmentId)
        {
            return await _context.Feedbacks
                .Include(f => f.Department)
                .Include(f => f.Sender)
                .Include(f => f.AssignedTo)
                .Where(f => f.DepartmentId == departmentId)
                .ToListAsync();
        }


        public async Task<IEnumerable<FeedbackDto>> GetFeedbacksByDepartmentIdAsync(int departmentId)
        {
            var feedbacks = await _context.Feedbacks
                .Include(f => f.Department)
                .Include(f => f.Sender)
                .Where(f => f.DepartmentId == departmentId)
                .ToListAsync();

            return _mapper.Map<IEnumerable<FeedbackDto>>(feedbacks);
        }

        public async Task<IEnumerable<Feedback>> GetFeedbacksByUserIdAsync(int userId)
        {
            var feedbacks = await _context.Feedbacks
                                .Include(f => f.Sender)
                                .Include(f => f.Department)
                                .Where(f => f.SenderId == userId)
                                .ToListAsync();

            return feedbacks;
        }

        public async Task<IEnumerable<FeedbackDto>> GetFeedbackByUserAsync(int userId)
        {
            var feedback = await _context.Feedbacks
                .Include(f => f.Department)
                .Include(f => f.Sender)
                .Where(f => f.SenderId == userId)
                .ToListAsync();

            var feedbackDtos = _mapper.Map<IEnumerable<FeedbackDto>>(feedback);

            return feedbackDtos;
        }



        public async Task UpdateFeedbackAsync(int feedbackId, FeedbackUpdateDto feedbackUpdateDto)
        {
            var feedback = await _context.Feedbacks.FindAsync(feedbackId);

                if (feedback != null)
                {
                    //throw new EntityNotFoundException(nameof(Feedback), feedbackId);
                    _mapper.Map(feedbackUpdateDto, feedback);

                    await _context.SaveChangesAsync();
                    
                }

           
        }
        public async Task<int> GetOpenFeedbackCountByDepartmentAsync(int departmentId)
        {
        var count = await _context.Feedbacks
            .Where(f => f.DepartmentId == departmentId && f.Status == (FeedbackStatus.Open))
            .CountAsync();

            return count;
        }

        public async Task<List<FeedbackDto>> GetFeedbacksByDepartmentIdAndStatusAsync(int departmentId, FeedbackStatus status)
        {
            var feedbacks = await _context.Feedbacks
                .Where(f => f.DepartmentId == departmentId)
                .Where(f => f.Status == status)
                .ToListAsync();

            var feedbackDtos = _mapper.Map<List<FeedbackDto>>(feedbacks);

            return feedbackDtos;
        }

        public async Task<List<Feedback>> GetAllhhFeedbacksByDeptIdAsync(int departmentId)
        {
            return await _context.Feedbacks
                .Include(f => f.Department)
                .Include(f => f.Sender)
                .Include(f => f.AssignedTo)
                .Where(f => f.DepartmentId == departmentId)
                .ToListAsync();
        }

        public async Task<List<FeedbackDto>> GetAllFeedbacksByDepartmentIdAsync(int departmentId)
        {
            var feedbacks = await _context.Feedbacks
                .Include(f => f.Replies) // Include feedback replies
                .Where(f => f.DepartmentId == departmentId)
                .ToListAsync();

            var feedbackDtos = feedbacks.Select(f => new FeedbackDto
            {
               Id = f.Id,
                Title = f.Title,
                Content = f.Content,
                SenderId = f.SenderId,
                IsAnonymous = f.IsAnonymous,
                SenderName = f.Sender != null ? f.Sender.UserName : null,
               // Status = f.Status.ToString(),
               Status = f.Status,

               // Status = (int)f.Status, // return the integer value of the Status enum

                DateCreated = f.DateCreated,
                //OpenFeedbackCount = f.Feedbacks.Count(f => f.Status == FeedbackStatus.Open),
                DepartmentName = f.Department != null ? f.Department.DepartmentName : null,
   
            }).ToList();

            return feedbackDtos;
        }

    public async Task<IEnumerable<FeedbackDto>> GetFeedbackForStudentAsync(string studentId)
    {
        var student = await _userManager.FindByIdAsync(studentId);
        var departmentId = student.DepartmentId;

        var feedback = await _context.Feedbacks
            .Where(f =>
                (f.TargetAudience == FeedbackTargetAudience.Departments && f.DepartmentId == departmentId) ||
                f.TargetAudience == FeedbackTargetAudience.AllStudents ||
                f.Recipients.Any(r => r.RecipientId == int.Parse(studentId)))
            .Where(f => f.SenderId != int.Parse(studentId)) // Exclude feedback created by the student
            .OrderByDescending(f => f.DateCreated) // Order by latest feedback first
            .Select(f => new FeedbackDto
            {
                Id = f.Id,
                Title = f.Title,
                Content = f.Content,
                SenderName = $"{f.Sender.UserName}",
                DateCreated = f.DateCreated,
                //IsRead = f.Recipients.Any(r => r.RecipientId == studentId && r.IsRead)
            })
            .ToListAsync();

        return feedback;
    }




        // public Task<List<Feedback>> GetFeedbacksByDepartmentIdAndStatusAsync(int departmentId, string status)
        // {
        //     throw new NotImplementedException();
        // }

        // Task<List<FeedbackDto>> IFeedbackRepository.GetFeedbacksByDepartmentIdAndStatusAsync(int departmentId, FeedbackStatus status)
        // {
        //     throw new NotImplementedException();
        // }
    }
}