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

        public Department Department { get; set; }
    }
}