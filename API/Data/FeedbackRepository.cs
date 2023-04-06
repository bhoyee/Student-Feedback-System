using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class FeedbackRepository : IFeedbackRepository
    {
        public readonly DataContext _context;
        private readonly IMapper _mapper;
    public FeedbackRepository(DataContext context, IMapper mapper)
    {
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
    
    public async Task<FeedbackDto> CreateFeedbackAsync(FeedbackCreateDto feedbackCreateDto, int userId)
{
    var user = await _context.Users.FindAsync(userId);
    if (user == null)
    {
        throw new ArgumentException("Invalid user id");
    }

    var feedback = _mapper.Map<Feedback>(feedbackCreateDto);
    feedback.SenderId = userId;
    feedback.DepartmentId = user.DepartmentId;

    _context.Feedbacks.Add(feedback);
    await _context.SaveChangesAsync();

    var feedbackDto = _mapper.Map<FeedbackDto>(feedback);
    feedbackDto.SenderName = user.UserName;
    feedbackDto.DepartmentName = user.Department.DepartmentName;

    return feedbackDto;
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

        public async Task<FeedbackDto> GetFeedbackByIdAsync(int feedbackId)
        {
             var feedback = await _context.Feedbacks
                .Include(f => f.Department)
                .Include(f => f.Sender)
                .FirstOrDefaultAsync(f => f.Id == feedbackId);

             return _mapper.Map<FeedbackDto>(feedback);
        }

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

        // public async Task<List<FeedbackDto>> GetFeedbacksByDepartmentIdAndStatusAsync(int departmentId, FeedbackStatus status)
        // {
        //     // var feedbacks = await _context.Feedbacks
        //     //     .Where(f => f.DepartmentId == departmentId)
        //     //     .Where(f => f.Status == status)
        //     //     .ToListAsync();

        //     // return "te";//feedbacks;
        // }

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