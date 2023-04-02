using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
       // private readonly DataContext _context;
        public readonly ITokenService _tokenService;
        public readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService, IMapper mapper)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _mapper = mapper;
            _tokenService = tokenService;
           // _context = context;
            
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            string pattern = @"-\d{4}";
            
            Regex regex = new Regex(pattern);
            
            if (await UserExists(registerDto.Username)) return BadRequest("Username is taken");

            if(await EmailExists(registerDto.Email)) return BadRequest("Email is taken");
            Match match = regex.Match(registerDto.Email);
              
            var user  = _mapper.Map<AppUser>(registerDto);
            user.UserName = registerDto.Username.ToLower();

            if(match.Success)
            {
                // register as student
                var result = await _userManager.CreateAsync(user, registerDto.Password);

                if (!result.Succeeded) return BadRequest(result.Errors);

                var roleResult = await _userManager.AddToRoleAsync(user, "Student");
                
                if(!roleResult.Succeeded) return BadRequest(result.Errors);

            }
            else
            {
                // regiter as staff
                var result = await _userManager.CreateAsync(user, registerDto.Password);

                if (!result.Succeeded) return BadRequest(result.Errors);

                var roleResult = await _userManager.AddToRoleAsync(user, "Staff");
                
                if(!roleResult.Succeeded) return BadRequest(result.Errors);
            }
            

            return new UserDto
            {
                Username = user.UserName,
                Token = await _tokenService.CreateToken(user),
               // deptName = user.Department.DepartmentName
            };
        }

     

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.Users
                .Include(p => p.Photos)               
                .SingleOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());
            
            if (user == null) return Unauthorized("Invalid username");

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded) return Unauthorized();
         
            return new UserDto
            {
                Username = user.UserName,
                Token = await _tokenService.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,                  
                Role = user.UserRoles.Select(R => R.Role.Name).ToList()
                
            };
        }

        private async Task<bool> UserExists(string username)
        {
            return await _userManager.Users.AnyAsync(x => x.UserName == username.ToLower());
        }

         private async Task<bool> EmailExists(string email)
        {
            return await _userManager.Users.AnyAsync(x => x.Email == email.ToLower());
        }

        
    }
}