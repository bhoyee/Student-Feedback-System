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

namespace API.Controllers
{
    [Authorize]
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
        public async Task<ActionResult> AddVote(int id)
        {
            var sourceUserId = User.GetUserId();
            
            var votedPetition = await _petitionRepository.GetPetitionByIdAsync(id);
            var sourceUser = await _votesRepository.GetPetitonWithVotes(sourceUserId);
            

            //check petition status
            if (votedPetition == null) return NotFound("No such petition");

            var userVote = await _votesRepository.GetUserVote(sourceUserId, votedPetition.Id);

            if (userVote != null) return BadRequest("You already vote on this Petition");

            userVote = new Vote
            {
                UserId = sourceUserId,
                PetitionId = votedPetition.Id

                // VotedUserId = sourceUserId,
                // SourcePetitionId = 5
        
                
            };

            //sourceUser.Votes.Add(userVote);

            if (await _userRepository.SaveAllAsync()) return Ok();

            return BadRequest("faile to vote petition");

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