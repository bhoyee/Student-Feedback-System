using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;

namespace API.DTOs
{
    public class FeedbackCreateDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public int DepartmentId { get; set; }
        public int SenderId { get; set; }
        public FeedbackStatus Status { get; set; }
      //  public DateTime CreateDate {get; set;}
     //   public int Status { get; set; } 
        public bool IsAnonymous { get; set; }

    }
}