using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;

namespace API.DTOs
{
    public class DepartmentDto
    {
        public int Id { get; set; }
        public string DepartmentName { get; set; }

        public int FeedbackCount { get; set; }

      //  public List<UserDto> Users { get; set; }
        public List<MemberDto> Users { get; set; }

        public int UserCount { get; set; }

        public int OpenFeedbackCount { get; set; } // new property for count of feedback with 'Open' status

    //  public ICollection<Feedback> Feedbacks { get; set; }

        //public ICollection<AppUser> AppUser { get; set; }
     public IEnumerable<FeedbackDto> Feedbacks { get; set; } // change type to IEnumerable<FeedbackDto>

    }
}