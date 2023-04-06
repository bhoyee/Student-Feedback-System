using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;

namespace API.DTOs
{
    public class MemberDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        
        public string PhotoUrl { get; set; }
        public string Email { get; set; }

        public DateTime Created { get; set; } = DateTime.Now;
        
        public DateTime LastActive { get; set; } = DateTime.Now;

        public string Interest { get; set; }
        
        public ICollection<PhotoDto> Photo { get; set; }
        public int DepartmentId { get; set; }

        public string deptName {get; set;}

        public DepartmentNameDto Department { get; set; }

     //   public int FeedbackId {get; set;}
      //  public Feedback Feedback {get; set;}

    //  public ICollection<FeedbackDto> Feedbacks { get; set; }
    
      //public ICollection<FeedbackDto> Feedback { get; set; }

      //  public List<string> Feedbacks {get; set;}
     //   public DepartmentCreationDTO DeptName { get; set; }

      //  public string Department { get; set; }
      //  public List<string> Role {get; set;}
     // public ICollection<AppRole> Role { get; set; }

      public List<string> Role {get; set;}
       // public string Role { get; set; }

       // public ICollection<AppRole> Role {get; set;}



        public MemberDto(string userName, string email)
    {
        UserName = userName;
        Email = email;
    }

    
    }
}