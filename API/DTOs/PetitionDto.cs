using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;

namespace API.DTOs
{
    public class PetitionDto
    {
        public int Id { get; set; }
        public string PetitionType { get; set; }
        public string Title { get; set; }   
        public string Message { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public int Anonymous { get; set; } = 0;
        public string Status { get; set; } = "Pending";

        public int UserId { get; set; }
        public AppUser AppUser { get; set; }

        public int VotesCount { get; set; }

    }
}