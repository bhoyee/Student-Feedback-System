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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    public class PetitionController : BaseApiController   
    {
        public readonly IPetitionRepository _petitionRepository;
        private readonly IMapper _mapper;
        private readonly IVVoteRepository _voteRepository;
        public readonly UserManager<AppUser> _userManager;
        private readonly ILogger<PetitionController> _logger;
        public readonly DataContext _context;
        private readonly IUserRepository _userRepository;
        public PetitionController(DataContext context, ILogger<PetitionController> logger, UserManager<AppUser> userManager, IPetitionRepository petitionRepository, IUserRepository userRepository, IVVoteRepository voteRepository, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
            _voteRepository = voteRepository;
            _mapper = mapper;
            _petitionRepository = petitionRepository;
            _userRepository = userRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePetition(PetitionDto petitionDto)
        {
            try
            {
                string userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                int userIdInt = int.Parse(userId);

                var petition = new Petition
                {
                    PetitionType = petitionDto.PetitionType,
                    Title = petitionDto.Title,
                    Message = petitionDto.Message,
                    Created = DateTime.Now,
                    Anonymous = petitionDto.Anonymous,
                    Status = "Pending",
                    UserId = userIdInt,
                    DepartmentId = petitionDto.DepartmentId
                };

                using (var transaction = await _petitionRepository.BeginTransactionAsync())
                {
                    try
                    {
                        await _petitionRepository.AddPetition(petition);
                        await _petitionRepository.SaveAllAsync();
                        await transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        throw new Exception("Error saving the petition to the database.", ex);
                    }
                }

                return Ok("Petition created successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message, details = ex.StackTrace });
            }
        }


        [HttpGet("vote-counts")]
        public async Task<ActionResult<IEnumerable<VoteDto>>> GetPetitionVoteCounts()
        {
            var petitions = await _petitionRepository.GetPetitionsAsync();
            var voteCounts = new List<VoteDto>();
            foreach (var petition in petitions)
            {
                var voteCount = await _voteRepository.GetUserVotess("voted",petition.UserId);
                var dto = new VoteDto
                {
                    
                    Id = petition.Id,
                    Title = petition.Title,
                   // voteCount = petition.voteCount
                    
                };
                voteCounts.Add(dto);
            }
            return Ok(voteCounts);
        }

        // get all petitions with total number of votes 

        [HttpGet("petitionsWithVotes")]
        public async Task<ActionResult<IEnumerable<PetitionWithVotesDto>>> GetPetitionsWithVotes()
        {
            var petitions = await _context.Petitions.Include(p => p.Votes).ToListAsync();

            var petitionsWithVotes = petitions.Select(p => new PetitionWithVotesDto
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Message,
                Votes = p.Votes.Count
            }).ToList();

            return petitionsWithVotes;
        }

        //single petition and total number of votes 
        [HttpGet("petitionsWithVotes/{id}")]
        public async Task<ActionResult<PetitionWithVotesDto>> GetPetitionWithVotes(int id)
        {
            var petition = await _context.Petitions.Include(p => p.Votes).FirstOrDefaultAsync(p => p.Id == id);

            if (petition == null)
            {
                return NotFound();
            }

            var petitionWithVotes = new PetitionWithVotesDto
            {
                Id = petition.Id,
                Title = petition.Title,
                Description = petition.Message,
                Votes = petition.Votes.Count
            };

            return petitionWithVotes;
        }



    }
}