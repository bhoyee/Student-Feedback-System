using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    public class FeedbackRecipient
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Feedback))]
        public int FeedbackId { get; set; }
        public Feedback Feedback { get; set; }

        [ForeignKey(nameof(AppUser))]
        public int RecipientId { get; set; }
        public AppUser Recipient { get; set; }
        
        public bool IsRead { get; set; } = false;
    }
}