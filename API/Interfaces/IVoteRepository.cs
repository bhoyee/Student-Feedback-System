using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;

namespace API.Interfaces
{
    public interface IVoteRepository
    {
        Task<PetitiionVote> GetUserVote(int petitionVoteId, int votedPetitionId);
        Task<Petition> GetPetitonWithVotes (int userId);
        Task<IEnumerable<VoteDto>> GetPetitionVotes(string predicate, int userId);
    }
}