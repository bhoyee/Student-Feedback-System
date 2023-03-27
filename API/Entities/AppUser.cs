using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class AppUser
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        
        public string Email { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        public DateTime Created { get; set; } = DateTime.Now;
        
        public DateTime LastActive { get; set; } = DateTime.Now;

        public string Interest { get; set; }
        
        public ICollection<Photo> Photos{ get; set; }
        public int DepartmentId { get; set; }

        public Department Department { get; set; }


        /// use case for petition voting
       // public ICollection<UserLike> LikedByUsers { get; set; }  // who has liked the current login user

        public ICollection<Petition> MyPetitions { get; set; } // the person the current user as liked (petition d person as liked)


    }
}