using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class DepartmentDisplayDto
    {
        public int Id { get; set; }
        public string DepartmentName { get; set; }
        public string Category { get; set; }

        public int totalFeedback { get; set; }

      //  public List<UserDto> Users { get; set; }
     //   public List<MemberDto> Users { get; set; }

        public int totalUsers { get; set; }

        public int totalStudents {get; set;}

        public int totalStaffs {get; set;}

        public int totalOpenFeedback { get; set; } // new property for count of feedback with 'Open' status

    //  public ICollection<Feedback> Feedbacks { get; set; }

        //public ICollection<AppUser> AppUser { get; set; }
       // public IEnumerable<FeedbackDto> Feedbacks { get; set; } // change type to IEnumerable<FeedbackDto>
    }
}