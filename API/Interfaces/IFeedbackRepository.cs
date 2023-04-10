using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;

namespace API.Interfaces
{
    public interface IFeedbackRepository
    {
        Task<IEnumerable<FeedbackDto>> GetFeedbacksAsync();
        Task<FeedbackDto> GetFeedbackByIdAsync(int feedbackId);
        Task<IEnumerable<FeedbackDto>> GetFeedbacksByDepartmentIdAsync(int departmentId);
      //  Task<FeedbackDto> CreateFeedbackAsync(FeedbackCreateDto feedbackCreateDto);
        // Task<FeedbackDto> CreateFeedbackAsync(FeedbackCreateDto feedbackCreateDto, int userId);
        // Task<Feedback> CreateFeedbackAsync(FeedbackCreateDto feedbackCreateDto, int userId);
     //  Task<Feedback> CreateFeedbackAsync(string title, string content, bool isAnonymous, int departmentId, int userId);
       Task CreateFeedbackAsync(Feedback feedback);
        Task SaveChangesAsync(); 

        Task UpdateFeedbackAsync(int feedbackId, FeedbackUpdateDto feedbackUpdateDto);
        Task DeleteFeedbackAsync(int feedbackId);
        Task<IEnumerable<Feedback>> GetFeedbacksByUserIdAsync(int userId);
      //  Task<IEnumerable<Feedback>> GetFeedbackByUserAsync(int userId);
      //  Task<List<Feedback>> GetFeedbacksByDepartmentIdAndStatusAsync(int departmentId, string status);
        Task<List<FeedbackDto>> GetFeedbacksByDepartmentIdAndStatusAsync(int departmentId, FeedbackStatus status);

         Task<int> GetOpenFeedbackCountByDepartmentAsync(int departmentId);

    Task<List<FeedbackDto>> GetAllFeedbacksByDepartmentIdAsync(int departmentId);



   


    }
}