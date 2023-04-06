using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class Feedback
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int SenderId { get; set; }
        public AppUser Sender { get; set; }
        public int? AssignedToId { get; set; }
        public AppUser AssignedTo { get; set; }
        public FeedbackStatus Status { get; set; }
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
        public DateTime DateCreated { get; set; }  = DateTime.Now;
        public ICollection<FeedbackReply> Replies { get; set; }
      //  public ICollection<FeedbackStatus> Status { get; set; }



    }

    

}