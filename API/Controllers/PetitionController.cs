using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class PetitionController : BaseApiController   
    {
        public readonly IPetitionRepository _petitionRepository;
        private readonly IMapper _mapper;
        private readonly IVVoteRepository _voteRepository;

        private readonly IUserRepository _userRepository;
        public PetitionController(IPetitionRepository petitionRepository, IUserRepository userRepository, IVVoteRepository voteRepository, IMapper mapper)
        {
            _voteRepository = voteRepository;
            _mapper = mapper;
            _petitionRepository = petitionRepository;
            _userRepository = userRepository;
        }

         [HttpPost]
        public async Task<ActionResult<PetitionDto>> AddPetition(PetitionDto petitionDto)
        {
            var user = User.GetUserId();
            var petition  = _mapper.Map<Petition>(petitionDto);
            petition.PetitionType = petitionDto.PetitionType.ToLower();
            petition.Title = petitionDto.Title.ToLower();
            petition.Message = petitionDto.Message.ToLower();
            petition.Created = petitionDto.Created;
            petition.Anonymous = petitionDto.Anonymous;
            petition.Status = petitionDto.Status;
            petition.UserId = user;

           await _petitionRepository.AddPetition(petition);
           return Ok();


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

    }
}