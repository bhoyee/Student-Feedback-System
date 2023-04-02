using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    public class Department
    {
        public int Id { get; set; }
    
        public string DepartmentName { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<AppUser> Users { get; set; }
        public ICollection<Petition> Petitions { get; set; }


    }
}