using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class PetitionReply
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        public AppUser User { get; set; }
        public int PetitionId { get; set; }
        public Petition Petition { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
