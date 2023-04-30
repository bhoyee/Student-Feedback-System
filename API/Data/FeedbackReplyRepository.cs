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
    public class FeedbackReplyRepository : IFeedbackReplyRepository
    {
        public readonly DataContext _context;
        private readonly IMapper _mapper;
        public FeedbackReplyRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
          
        }

        public async Task<FeedbackReplyDto> CreateFeedbackReplyAsync(int feedbackId, FeedbackReplyCreateDto feedbackReplyCreateDto)
        {
            var feedbackReply = _mapper.Map<FeedbackReply>(feedbackReplyCreateDto);
                feedbackReply.FeedbackId = feedbackId;
                feedbackReply.CreatedAt = DateTime.UtcNow;

                _context.FeedbackReplies.Add(feedbackReply);
                await _context.SaveChangesAsync();

            return _mapper.Map<FeedbackReplyDto>(feedbackReply);
        }

        public async Task DeleteFeedbackReplyAsync(int feedbackReplyId)
        {
            var feedbackReply = await _context.FeedbackReplies.FindAsync(feedbackReplyId);

            if (feedbackReply == null)
            {
                throw new ArgumentException("Invalid feedback reply id.");
            }

            _context.FeedbackReplies.Remove(feedbackReply);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<FeedbackReplyDto>> GetFeedbackRepliesAsync(int feedbackId)
        {
            var feedbackReplies = await _context.FeedbackReplies
                .Where(fr => fr.FeedbackId == feedbackId)
                .ToListAsync();

            return _mapper.Map<IEnumerable<FeedbackReplyDto>>(feedbackReplies);
        }

        public async Task<FeedbackReplyDto> GetFeedbackReplyAsync(int feedbackReplyId)
        {
            var feedbackReply = await _context.FeedbackReplies.FindAsync(feedbackReplyId);

            return feedbackReply == null ? null : new FeedbackReplyDto
            {
                Id = feedbackReply.Id,
                Content = feedbackReply.Content,
                IsPublic = feedbackReply.IsPublic,
                DateCreated = feedbackReply.CreatedAt,
                UpdatedAt = feedbackReply.UpdatedAt
            };
    
        }

        public async Task<FeedbackReplyDto> GetFeedbackReplyByIdAsync(int feedbackReplyId)
        {
                var feedbackReply = await _context.FeedbackReplies
                .Include(f => f.Feedback)
                .FirstOrDefaultAsync(f => f.Id == feedbackReplyId);

            if (feedbackReply == null)
            {
                return null;
            }

            var feedbackReplyDto = _mapper.Map<FeedbackReplyDto>(feedbackReply);

            return feedbackReplyDto;
        }

        public async Task UpdateFeedbackReplyAsync(int feedbackReplyId, FeedbackReplyUpdateDto feedbackReplyUpdateDto)
        {
            var feedbackReply = await _context.FeedbackReplies.FindAsync(feedbackReplyId);

            if (feedbackReply != null)
            {
                //throw new ArgumentException("Invalid feedback reply id.");
                feedbackReply.Content = feedbackReplyUpdateDto.Message;
                feedbackReply.IsPublic = feedbackReplyUpdateDto.IsPublic;

                await _context.SaveChangesAsync();
            }

 
        }
        public async Task<List<FeedbackReply>> GetFeedbackRepliesByFeedbackIdAsync(int feedbackId)
        {
            return await _context.FeedbackReplies
                .Where(f => f.FeedbackId == feedbackId)
                .ToListAsync();
        }
        public async Task<List<FeedbackReply>> GetRepliesByFeedbackIdAsync(int feedbackId)
        {
            return await _context.FeedbackReplies
                .Where(r => r.FeedbackId == feedbackId)
                .ToListAsync();
        }
        public async Task DeleteAsync(FeedbackReply reply)
        {
            _context.FeedbackReplies.Remove(reply);
            await _context.SaveChangesAsync();
        }


    }
}