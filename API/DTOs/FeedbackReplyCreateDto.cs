using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class FeedbackReplyCreateDto
    {
        public int FeedbackId { get; set; }
        public int UserId { get; set; }
        public string Reply { get; set; }
        public DateTime DateCreated { get; set; }
    }
}