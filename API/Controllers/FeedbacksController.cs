using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace API.Controllers
{
    public class FeedbacksController : BaseApiController
    {
        private readonly IMapper _mapper;
        public readonly IFeedbackRepository _feedbackRepository;
        private readonly IFeedbackReplyRepository _feedbackReplyRepository;
        public readonly UserManager<AppUser> _userManager;
        public readonly DataContext _context;
        private readonly IDeparmtentRepo _deparmtentRepo;
        private readonly IServiceProvider _serviceProvider;
        public readonly IEmailService _emailService;
        public FeedbacksController(DataContext context, UserManager<AppUser> userManager, IEmailService emailService, IMapper mapper, IServiceProvider serviceProvider,  IFeedbackRepository feedbackRepository, IFeedbackReplyRepository feedbackReplyRepository, IDeparmtentRepo deparmtentRepo)
        {       
            _emailService = emailService;
            _serviceProvider = serviceProvider;
            _deparmtentRepo = deparmtentRepo;
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
        public async Task<IActionResult> GetFeedbackById(int departmentId, int feedbackId)
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

    //    [HttpPost]
    //     public async Task<ActionResult<FeedbackDto>> CreateFeedback(FeedbackCreateDto feedbackCreateDto)
    //     {
    //         try
    //         {               
    //             var userId = User.GetUserId();
    //             var feedback = await _feedbackRepository.CreateFeedbackAsync(feedbackCreateDto, userId);
    //             return CreatedAtRoute(nameof(GetFeedback), new { id = feedback.Id }, feedback);
    //         }
    //         catch(Exception ex)
    //         {
    //             return StatusCode(500, new { message = ex.Message });
    //         }
    //     }
// [HttpPost]
// public async Task<ActionResult<FeedbackDto>> CreateFeedback(Feedback feedback)
// {
//     var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

//     var department = await _deparmtentRepo.GetDepartmentByIdAsync(feedback.DepartmentId);
//     if (department == null)
//         return BadRequest("Invalid department id");

//     feedback.SenderId = userId;
//     feedback.Status = FeedbackStatus.Open;
//     feedback.DateCreated = DateTime.Now;

//     await _feedbackRepository.CreateFeedbackAsync(feedback);
//     await _feedbackRepository.SaveChangesAsync();

//     var feedbackDto = _mapper.Map<FeedbackDto>(feedback);
//     return Ok("Inserted successfully");

//    // return CreatedAtAction(nameof(GetFeedbackById), new { id = feedbackDto.Id }, feedbackDto);
// }

[HttpPost]
public async Task<IActionResult> CreateFeedback([FromBody] FeedbackDto feedbackDto)
{
    if (!ModelState.IsValid)
    {
        return BadRequest(ModelState);
    }

    // Check if the sender is a student
    var senderId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
    var student = await _userManager.FindByIdAsync(senderId);
    if (student == null || !await _userManager.IsInRoleAsync(student, "Student"))
    {
        return BadRequest("Invalid sender id or sender is not a student");
    }

    // // Check if the student belongs to the specified department
    // if (student.DepartmentId != feedbackDto.DepartmentId)
    // {
    //     return BadRequest("Student does not belong to the specified department");
    // }

    var feedback = new Feedback
    {
        Title = feedbackDto.Title,
        Content = feedbackDto.Content,
        SenderId = student.Id,
        IsAnonymous = feedbackDto.IsAnonymous,
        DepartmentId = feedbackDto.DepartmentId,
        DateCreated = DateTime.Now,
    };

    // Get the list of feedback recipients based on the selected target audience
    var recipients = new List<FeedbackRecipient>();

    if (feedbackDto.TargetAudience == FeedbackTargetAudience.Department.ToString())
    {
        // Get staffs with roles of 'Moderator' and 'Dept-Head' in the department
        var roleManager = _serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var users = await _userManager.GetUsersInRoleAsync("Moderator");
        users = users.Concat(await _userManager.GetUsersInRoleAsync("Dept-Head")).Distinct().ToList();
        
        var moderators = new List<AppUser>();
        var deptHeads = new List<AppUser>();

        foreach (var user in users)
        {
            if (await _userManager.IsInRoleAsync(user, "Moderator"))
            {
                moderators.Add(user);
            }
            else if (await _userManager.IsInRoleAsync(user, "Dept-Head") && user.DepartmentId == feedbackDto.DepartmentId)
            {
                deptHeads.Add(user);
            }
        }

        var recipientUsers = moderators.Concat(deptHeads).ToList();

        foreach (var recipientUser in recipientUsers)
        {
            recipients.Add(new FeedbackRecipient { RecipientId = recipientUser.Id, IsRead = false });

            // Send email notification to staff
            await _emailService.SendFeedbackNotificationEmailAsync(recipientUser.Email, feedbackDto.Title, feedback.Id);
        }
    }

    feedback.Recipients = recipients;

    await _feedbackRepository.CreateFeedbackAsync(feedback);
    await _feedbackRepository.SaveChangesAsync();

    return Ok("Inserted successfully");
}



      
        // [HttpPost]
        // public async Task<ActionResult<FeedbackDto>> CreateFeedback(FeedbackCreateDto feedbackCreateDto)
        // {
        //     try
        //     {
        //         var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        //         if(userId == 0) { return BadRequest("no user identiy"); }
        //         var feedback = await _feedbackRepository.CreateFeedbackAsync(feedbackCreateDto, userId);
        //         return CreatedAtRoute(nameof(GetFeedback), new { id = feedback.Id }, feedback);
        //     }
        //     catch(Exception ex)
        //     {
        //         return StatusCode(500, new { message = ex.Message });
        //     }
        // }





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