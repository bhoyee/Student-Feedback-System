using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Extensions
{
    public static class ClaimsPrincipleExtensions
    {
        public static string GetUsername(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Name)?.Value;
        }

           
        public static int GetUserId(this ClaimsPrincipal user)
        {
            return int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }

        // public static int GetUserDepartmentId(this ClaimsPrincipal user)
        // {
        //     var departmentIdClaim = user.FindFirst("DepartmentId");
        //     if (departmentIdClaim == null)
        //     {
        //         return -1; // or any other default value that makes sense for your application
        //     }

        //     return int.Parse(departmentIdClaim.Value);
        // }
    }
}