using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;

namespace API.DTOs
{
    public class DepartmentDto
    {
        
        public string DepartmentName { get; set; }

        //public ICollection<AppUser> AppUser { get; set; }
    }
}