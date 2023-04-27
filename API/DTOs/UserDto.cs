using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Data
{
    public class UserDto
    {
        public string Username { get; set; }
        public string FullName {get; set;}
        public string Token { get; set; }
        public string PhotoUrl { get; set; }

        public List<string> Role {get; set;}

        //public string deptName { get; set; }
    }
}