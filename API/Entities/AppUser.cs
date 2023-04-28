using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace API.Entities
{
    public class AppUser : IdentityUser<int>
    {
        public string FullName {get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        
        public DateTime LastActive { get; set; } = DateTime.Now;

        public string Interest { get; set; }
        
        public ICollection<Photo> Photos{ get; set; }

        public int DepartmentId { get; set; }

        public Department Department { get; set; }

        public ICollection<Petition> Petitions { get; set; }

        public ICollection<Vote> Votes { get; set; }

        public ICollection<AppUserRole> UserRoles{get; set;}

        public ICollection<Feedback> Feedbacks{get; set;}
        public ICollection<FeedbackRecipient> FeedbackRecipients { get; set; }



        /// use case for petition voting
      //  public ICollection<Vote> VotedPetitions { get; set; }  // who has liked the current login user

       // public ICollection<Petition> MyPetitions { get; set; } // the person the current user as liked (petition d person as liked)


    }

    // public enum UserRole
    // {
    //     Student,
    //     Staff,
    //     Admin
    // }
}