using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;

namespace API.DTOs
{
    public class FeedbackReplyDto
    {
        public int Id { get; set; }
        public int FeedbackId { get; set; }
        public string Content { get; set; }
        public int UserId { get; set; }
         public string UserFullName { get; set; }
         public AppUser User { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsPublic { get; set; } = true;
        public DateTime UpdatedAt { get; set; }
        public int? Status { get; set; }



    }
}
