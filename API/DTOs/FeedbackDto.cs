using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;

namespace API.DTOs
{
    public class FeedbackDto
    {
    
    public int Id { get; set; }
     public string Title { get; set; }
     public string Content { get; set; }
     public int SenderId { get; set; }
     public string SenderName { get; set; }
     public FeedbackStatus Status { get; set; } =0; // modified type to FeedbackStatus
     public bool IsAnonymous { get; set; }

    public int OpenFeedbackCount { get; set; } // New property to hold the count of feedbacks with an open status for that department

    public int DepartmentId { get; set; }
    public string DepartmentName { get; set; }
 //   public FeedbackStatus Status { get; set; }
     public int? AssignedToId { get; set; }

    public string AssignedToName { get; set; }
    public DateTime DateCreated { get; set; }
    public List<FeedbackReplyDto> FeedbackReplies { get; set; } = new List<FeedbackReplyDto>();


   // public DepartmentDto Deparmtent { get; set; }

    
  //  public ICollection<MemberDto> AppUser { get; set; }
    }
}