using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class RegisterDto
    {
        [Required(ErrorMessage ="Fullname Required")]
        public string FullName { get; set; }
        public string Username { get; set; }

        [Required(ErrorMessage ="Password Required")]
        [StringLength(25, ErrorMessage ="Must be between 5 and 25 characters", MinimumLength =5)]
        public string Password { get; set; }

        [Required(ErrorMessage ="Confirm Password is Required")]
        [DataType(DataType.Password)]
        [Compare("Password")]
        [NotMapped]
        public string ConfirmPassword { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+(@hull\.ac\.uk)$", ErrorMessage = "Registration limited to school email domain only")]
        public string Email { get; set; }
        
        [Required (ErrorMessage = "Select Your Department")]
        public int DepartmentId { get; set; }
    }
}