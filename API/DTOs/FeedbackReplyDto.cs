using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class FeedbackReplyDto
    {
        public int Id { get; set; }
        public int FeedbackId { get; set; }
        public string Content { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsPublic { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}