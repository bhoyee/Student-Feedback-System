using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class FeedbackReply
    {    
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public int FeedbackId { get; set; }
        public Feedback Feedback { get; set; }
        public int UserId { get; set; }
        public AppUser User { get; set; }
        public bool IsPublic { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}