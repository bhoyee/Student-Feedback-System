using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;

namespace API.Interfaces
{
    public interface IVVoteRepository
    {

        Task<Vote> GetUserVote(int appUserSourceId, int votedPetitionId);
        Task<AppUser> GetPetitonWithVotes (int pId);
        Task<IEnumerable<VoteDto>> GetUserVotes(string predicate, int pid);

        Task<IEnumerable<Vote>> GetUserVotess(string predicate, int userId);

    }
}