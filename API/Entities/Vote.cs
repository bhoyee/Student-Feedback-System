using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class Vote
    {
        public int Id { get; set; }
        public AppUser User {get; set;}

        public int UserId { get; set; }

        public Petition Petition { get; set; }
        public int PetitionId { get; set; }

       public DateTime VotedAt { get; set; }
       
    }
}