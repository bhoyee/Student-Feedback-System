using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class VotesRepository : IVVoteRepository
    {
        private readonly DataContext _context;

        public VotesRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<AppUser> GetPetitonWithVotes(int pId)
        {
            return await _context.Users
               // .Include(x => x.Votes)
                .FirstOrDefaultAsync(x => x.Id == pId);
        }

        public async Task<Vote> GetUserVote(int appUserSourceId, int votedPetitionId)
        {
            return await _context.Votes
                .SingleOrDefaultAsync(x => x.UserId == appUserSourceId && x.PetitionId == votedPetitionId);
        }


        public async Task<IEnumerable<VoteDto>> GetUserVotes(string predicate, int userid)
        {
             var users = _context.Users.OrderBy(u => u.UserName).AsQueryable();
             var petitions = _context.Petitions.AsQueryable();
             var votes = _context.Votes.AsQueryable();
            // var uerss = _context.Users.AsQueryable();

            

            if (predicate == "votedBy")
            {
               
            var userVotes = _context.Votes
                .Include(v => v.Petition)
                .Where(v => v.UserId == userid)
                .ToList()
                .Where(v => _context.Petitions
                    .Any(p => p.Id == v.PetitionId));


                // votes = votes.Where(vote => vote.AppUserSourceId == userid);
                // users = votes.Select(vote => vote.AppUserSource);
                // petitions = votes.Select(vote => vote.VotedPetition);

             //   votes = votes.Where(vote => vote.AppUserSourceId == userid);
                
               // petitions = votes.Select(vote => vote.VotedPetition);
               // users =
            }

            if (predicate == "voted")
            {
                votes = votes.Where(vote => vote.UserId == userid );
                users = votes.Select(vote => vote.User);
                petitions = votes.Select(vote => vote.Petition);


            }

            return await petitions.Select(vote => new VoteDto
            {
                Id = vote.Id,
                Title = vote.Title,
                Message = vote.Message,
                Created = vote.Created,
                Status = vote.Status
 
                

            }).ToListAsync();
        }
        public async Task<IEnumerable<Vote>> GetUserVotess(string predicate, int userId)
        {
            // var votes = _context.Users.AsQueryable();
            var query = _context.Votes
                .Include(v => v.User)
                .Include(v => v.Petition)
                .Where(v => v.UserId == userId);
                
            if (!string.IsNullOrEmpty(predicate))
            {
                query = query.Where(v => v.Petition.Title.Contains(predicate));
            }
            
            // Group the votes by the VotedPetitionId and count the number of distinct UserIds
            var voteCounts = await query.GroupBy(v => v.PetitionId)
                                        .Select(g => new { PetitionId = g.Key, VoteCount = g.Select(v => v.UserId).Distinct().Count() })
                                        .ToListAsync();
            
            // Join the vote counts with the votes to return the complete list of user's votes with the vote count for each petition
            var votes = await query.Join(voteCounts, v => v.PetitionId, vc => vc.PetitionId, (v, vc) => new Vote
            {
                Id = v.Id,
                Petition = v.Petition,
                User = v.User,
                //User = v.User,
                PetitionId = v.PetitionId,
                UserId = v.UserId,
                //IsAgree = v.IsAgree,
                //voteCount = vc.VoteCount,
                
                
                //CreatedAt = v.CreatedAt,
                //VoteCount = vc.VoteCount // Add the vote count property to the Vote model
            })
            .ToListAsync();

            return votes;
        }




        // public async Task<IEnumerable<VoteDto>> GetPetitionVotes(string predicate, int pId)
        // {
        //     var petitions = _context.Petitions.OrderBy(u => u.Id).AsQueryable();
        //     var votes = _context.PetitiionVotes.AsQueryable();

        //     if (predicate == "voted")
        //     {
        //         votes = votes.Where(vote => vote.SourcePetitionId == pId);
        //         petitions = votes.Select (vote => vote.SourcePetition);
        //     }

        //     if (predicate == "votedBy")
        //     {
        //         votes = votes.Where(vote => vote.SourcePetitionId == pId );
        //         petitions = votes.Select(vote => vote.SourcePetition);
        //     }

        //     return await petitions.Select(vote => new VoteDto
        //     {
        //         Petitionname = vote.Title

        //     }).ToListAsync();
        // }

        // get list petitions that users has voted
        // public async Task<Petition> GetPetitonWithVotes(int pId)
        // {
        //    return await _context.Petitions
        //         .Include(x => x.VotedPetitionUser)
        //         .FirstOrDefaultAsync(x => x.Id == pId);
        // }

        // public async Task<Vote> GetUserVote(int AppUserSourceId, int votedPetitionId)
        // {
        //     return await _context.Votes.FindAsync(AppUserSourceId, votedPetitionId);
        // }


    }
}