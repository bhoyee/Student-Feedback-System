using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class FeedbackCreateDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public int DepartmentId { get; set; }
        public int SenderId { get; set; }
      //  public DateTime CreateDate {get; set;}
        public string Status { get; set; }
    }
}