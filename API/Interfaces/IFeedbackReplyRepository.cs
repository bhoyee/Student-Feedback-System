using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;

namespace API.Interfaces
{
    public interface IFeedbackReplyRepository
    {


        Task<IEnumerable<FeedbackReplyDto>> GetFeedbackRepliesAsync(int feedbackId);
        Task<FeedbackReplyDto> CreateFeedbackReplyAsync(int feedbackId, FeedbackReplyCreateDto feedbackReplyCreateDto);
        Task UpdateFeedbackReplyAsync(int feedbackReplyId, FeedbackReplyUpdateDto feedbackReplyUpdateDto);
        Task DeleteFeedbackReplyAsync(int feedbackReplyId);
        Task<FeedbackReplyDto> GetFeedbackReplyAsync(int feedbackReplyId);
        Task<FeedbackReplyDto> GetFeedbackReplyByIdAsync(int feedbackReplyId);
        Task<List<FeedbackReply>> GetFeedbackRepliesByFeedbackIdAsync(int feedbackId);

        Task<List<FeedbackReply>> GetRepliesByFeedbackIdAsync(int feedbackId);
        Task DeleteAsync(FeedbackReply reply);




       
     
    }
}