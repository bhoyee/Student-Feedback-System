using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace API.Controllers
{
    public class FeedbacksController : BaseApiController
    {
        private readonly IMapper _mapper;
        public readonly IFeedbackRepository _feedbackRepository;
        private readonly IFeedbackReplyRepository _feedbackReplyRepository;
        public readonly UserManager<AppUser> _userManager;
        public readonly DataContext _context;
        public FeedbacksController(DataContext context, UserManager<AppUser> userManager, IMapper mapper, IFeedbackRepository feedbackRepository, IFeedbackReplyRepository feedbackReplyRepository)
        {       
            _context = context;
            
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
             _userManager = userManager;
            _feedbackRepository = feedbackRepository ?? throw new ArgumentNullException(nameof(feedbackRepository));
            _feedbackReplyRepository = feedbackReplyRepository ?? throw new ArgumentNullException(nameof(feedbackReplyRepository));
        }

        // [HttpGet]
        // public async Task<IActionResult> GetFeedbacks(int departmentId)
        // {
        //     var feedbacks = await _feedbackRepository.GetFeedbacksByDepartmentIdAsync(departmentId);
        //     var feedbackDtos = _mapper.Map<IEnumerable<FeedbackDto>>(feedbacks);

        //     foreach (var feedbackDto in feedbackDtos)
        //     {
        //         var sender = await _userManager.GetUserIdAsync(feedbackDto.SenderId);
        //         feedbackDto.SenderName = sender.UserName;

        //         var department = await _departmentRepository.GetDepartmentByIdAsync(feedbackDto.DepartmentId);
        //         feedbackDto.DepartmentName = department.Name;
        //     }

        //     return Ok(feedbackDtos);
        // }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserFeedbacks(int userId)
        {
            var feedbacks = await _context.Feedbacks
                                .Include(f => f.Sender)
                                .Include(f => f.Department)
                                .Include(f => f.AssignedTo)
                                .Where(f => f.SenderId == userId)
                                .ToListAsync();

            var feedbackDtos = feedbacks.Select(f => new FeedbackDto
            {
                Id = f.Id,
                Title = f.Title,
                Content = f.Content,
                SenderName = f.Sender != null ? f.Sender.UserName : null,
                DepartmentName = f.Department != null ? f.Department.DepartmentName : null,
               // Status = f.Status,
                AssignedToName = f.AssignedTo != null ? f.AssignedTo.UserName : null,
                DateCreated = f.DateCreated
            });

            return Ok(feedbackDtos);
        }


        [HttpGet("{feedbackId}")]
        public async Task<IActionResult> GetFeedback(int departmentId, int feedbackId)
        {
            var feedback = await _feedbackRepository.GetFeedbackByIdAsync(feedbackId);
            if (feedback == null)
            {
                return NotFound();
            }

            var feedbackDto = _mapper.Map<FeedbackDto>(feedback);
            return Ok(feedbackDto);
        }

        // [HttpPost]
        // public async Task<IActionResult> CreateFeedback([FromBody] FeedbackCreateDto feedbackCreateDto)
        // {
        //     if (feedbackCreateDto == null)
        //     {
        //         return BadRequest();
        //     }

        //     var feedbackDto = await _feedbackRepository.CreateFeedbackAsync(feedbackCreateDto);
        //     return CreatedAtAction(nameof(GetFeedback), new { departmentId = feedbackCreateDto.DepartmentId, feedbackId = feedbackDto.Id }, feedbackDto);
        // }
        [HttpPost]
[HttpPost]
public async Task<ActionResult<FeedbackDto>> CreateFeedback(FeedbackCreateDto feedbackCreateDto)
{
    try
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        var feedback = await _feedbackRepository.CreateFeedbackAsync(feedbackCreateDto, userId);
        return CreatedAtRoute(nameof(GetFeedback), new { id = feedback.Id }, feedback);
    }
    catch(Exception ex)
    {
        return StatusCode(500, new { message = ex.Message });
    }
}





        [HttpPut("{feedbackId}")]
        public async Task<IActionResult> UpdateFeedback(int departmentId, int feedbackId, [FromBody] FeedbackUpdateDto feedbackUpdateDto)
        {
            if (feedbackUpdateDto == null)
            {
                return BadRequest();
            }

            await _feedbackRepository.UpdateFeedbackAsync(feedbackId, feedbackUpdateDto);
            return NoContent();
        }
        
        [HttpDelete("{feedbackId}")]
        public async Task<IActionResult> DeleteFeedback(int departmentId, int feedbackId)
        {
            await _feedbackRepository.DeleteFeedbackAsync(feedbackId);
            return NoContent();
        }
        
        [HttpGet("{feedbackId}/replies")]
        public async Task<IActionResult> GetFeedbackReplies(int departmentId, int feedbackId)
        {
            var feedbackReplies = await _feedbackReplyRepository.GetFeedbackRepliesAsync(feedbackId);
            var feedbackReplyDtos = _mapper.Map<IEnumerable<FeedbackReplyDto>>(feedbackReplies);
            return Ok(feedbackReplyDtos);
        }

        [HttpPost("{feedbackId}/replies")]
        public async Task<IActionResult> CreateFeedbackReply(int departmentId, int feedbackId, [FromBody] FeedbackReplyCreateDto feedbackReplyCreateDto)
        {
            if (feedbackReplyCreateDto == null)
            {
                return BadRequest();
            }

            var feedbackReplyDto = await _feedbackReplyRepository.CreateFeedbackReplyAsync(feedbackId, feedbackReplyCreateDto);
            return CreatedAtAction(nameof(GetFeedbackReply), new { departmentId, feedbackId, feedbackReplyId = feedbackReplyDto.Id }, feedbackReplyDto);
        }

        private async Task<FeedbackReplyDto> GetFeedbackReply(int feedbackReplyId)
        {
            var feedbackReply = await _feedbackReplyRepository.GetFeedbackReplyByIdAsync(feedbackReplyId);
            if (feedbackReply == null)
            {
                throw new NotFoundException($"Feedback reply with id {feedbackReplyId} not found.");
            }
            var feedbackReplyDto = _mapper.Map<FeedbackReplyDto>(feedbackReply);
            return feedbackReplyDto;
        }


        [HttpPut("{departmentId}/feedbacks/{feedbackId}/replies/{feedbackReplyId}")]
        public async Task<IActionResult> UpdateFeedbackReply(int departmentId, int feedbackId, int feedbackReplyId, [FromBody] FeedbackReplyUpdateDto feedbackReplyUpdateDto)
        {
            var feedbackReplyDto = await _feedbackReplyRepository.GetFeedbackReplyByIdAsync(feedbackReplyId);
            if (feedbackReplyDto == null)
            {
                return NotFound($"Feedback reply with id {feedbackReplyId} not found.");
            }

            var feedbackReplyUpdate = _mapper.Map<FeedbackReplyUpdateDto>(feedbackReplyDto);
            feedbackReplyUpdate = _mapper.Map(feedbackReplyUpdateDto, feedbackReplyUpdate);

            try
            {
                await _feedbackReplyRepository.UpdateFeedbackReplyAsync(feedbackReplyId, feedbackReplyUpdate);

                var updatedFeedbackReply = await _feedbackReplyRepository.GetFeedbackReplyByIdAsync(feedbackReplyId);
                var updatedFeedbackReplyDto = _mapper.Map<FeedbackReplyDto>(updatedFeedbackReply);

                return Ok(updatedFeedbackReplyDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error updating feedback reply: {ex.Message}");
            }
        }


        [HttpPut("{feedbackId}/replies/{feedbackReplyId}")]
        public async Task<IActionResult> UpdateFeedbackReply(int departmentId, int feedbackId, int feedbackReplyId, [FromBody] FeedbackReplyDto feedbackReplyDto)
        {
            if (feedbackReplyDto == null)
            {
                return BadRequest();
            }

            var existingFeedbackReply = await _feedbackReplyRepository.GetFeedbackReplyAsync(feedbackReplyId);
            if (existingFeedbackReply == null)
            {
                return NotFound();
            }

            // Create a new instance of FeedbackReplyUpdateDto and copy properties from FeedbackReplyDto
            var feedbackReplyUpdateDto = new FeedbackReplyUpdateDto
            {
                id = existingFeedbackReply.Id,
            Message = feedbackReplyDto.Content
                // copy other relevant properties
            };

            await _feedbackReplyRepository.UpdateFeedbackReplyAsync(feedbackReplyId, feedbackReplyUpdateDto);

            return NoContent();
        }

        public class NotFoundException : Exception
        {
            public NotFoundException() : base() { }
            public NotFoundException(string message) : base(message) { }
            public NotFoundException(string message, Exception innerException) : base(message, innerException) { }
        }



    }
}