using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class VoteDto
    {
        public int Id { get; set; }
        public string Title { get; set; }   
        public string Message { get; set; }
        public DateTime Created { get; set; }
        
        public string Status { get; set; } 

        public int voteCount {get; set;}

    }
}