using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using API.Extensions;
using API.Entities;
using API.DTOs;
using Microsoft.EntityFrameworkCore;
using API.Data;
using System.Security.Claims;

namespace API.Controllers
{
    
    public class VotesController : BaseApiController
    {
        public readonly IPetitionRepository _petitionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IVVoteRepository _votesRepository;
        public readonly DataContext _context;
        public VotesController(IPetitionRepository petitionRepository, IUserRepository userRepository, IVVoteRepository votesRepository, DataContext context)

        {
            _context = context;
            _votesRepository = votesRepository;
            _userRepository = userRepository;
            _petitionRepository = petitionRepository;

        }

[HttpPost("{id}")]
[Authorize(Roles = "Student")]
public async Task<IActionResult> AddVote(int id)
{
    var sourceUserId = User.GetUserId();

    // Check if user has already voted on the petition
    var userVote = await _votesRepository.GetUserVote(sourceUserId, id);
    if (userVote != null)
    {
        return BadRequest("You have already voted on this petition");
    }

    // Get the petition
    var votedPetition = await _petitionRepository.GetPetitionByIdAsync(id);
    if (votedPetition == null)
    {
        return NotFound("No such petition");
    }

    // Check if the user who created the petition is trying to vote on it
    if (votedPetition.UserId == sourceUserId)
    {
        return BadRequest("You cannot vote on your own petition");
    }

    // Check if the user is a student
    if (!User.IsInRole("Student"))
    {
        return Forbid("You are not authorised to vote");
    }

    // Add the user's vote to the petition
    var vote = new Vote
    {
        UserId = sourceUserId,
        PetitionId = votedPetition.Id,
        VotedAt = DateTime.UtcNow // set the VotedAt time to the current UTC time
    };

    _context.Votes.Add(vote);

    if (await _context.SaveChangesAsync() > 0)
    {
        return Ok("Vote Successfully");
    }

    return BadRequest("Failed to vote petition");
}



        [HttpGet]
        public async Task<ActionResult<IEnumerable<VoteDto>>> GetUserPetitions(string predicate)
        {
                    
            var users = await _votesRepository.GetUserVotes(predicate, User.GetUserId());

            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<VoteDto>>> GetUserP(int uid)
        {
           // await _votesRepository.GetPetitonWithVotes();
            return Ok();
        }

        public async Task<ActionResult<IEnumerable<PetitionDto>>> GetUserPetitionss(string predicate)
        {
            var user = User.GetUserId();
            var userId = user;

            var petitions = await _context.Petitions
                .Where(p => p.UserId == userId && (string.IsNullOrEmpty(predicate) || p.Title.ToLower().Contains(predicate.ToLower())))
                .Select(p => new PetitionDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Message = p.Message,
                    Created = p.Created,

                    // Description = p.Description,
                    // DateCreated = p.DateCreated,
                    // DateClosed = p.DateClosed,
                    UserId = p.UserId,
                   // VotesCount = p.Votes.Count()
                   VotesCount = p.Votes.Count()
                })
                .ToListAsync();

            return petitions;
        }



    }


}