using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class PetitiionVote
    {
        public Petition SourcePetition { get; set; }
        public int SourcePetitionId { get; set; }
        
        public AppUser VotedUser {get; set;}
        public int VotedUserId { get; set; }
    }
}