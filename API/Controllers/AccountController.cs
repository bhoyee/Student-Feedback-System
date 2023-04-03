using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
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

        private readonly IEmailService _emailService;
        
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService, IEmailService emailService, IMapper mapper)
        {
            _emailService = emailService;
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
            
            if (await UserExists(registerDto.Username))
            {
                return BadRequest("Username is taken");
            }

            if(await EmailExists(registerDto.Email))
            {
                return BadRequest("Email is taken");
            }

            Match match = regex.Match(registerDto.Email);
            var user  = _mapper.Map<AppUser>(registerDto);
            user.UserName = registerDto.Username.ToLower();

            string role = match.Success ? "Student" : "Staff";

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            var roleResult = await _userManager.AddToRoleAsync(user, role);

            if (!roleResult.Succeeded)
            {
                return BadRequest(roleResult.Errors);
            }

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.Action("verify-email", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme, host: HttpContext.Request.Host.Value);
            callbackUrl = callbackUrl.Replace("/Account/verify-email", "/api/Account/verify-email");

            //var callbackUrl = Url.Action("verify-email", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);

            await _emailService.SendEmailAsync(user.Email, "Confirm your email", $"Please confirm your Student Feedback Portal account by clicking this link: <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>link</a>");

            return Ok("Registration Successful . Check email to verify your account");
        }
     

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.Users
                .Include(p => p.Photos)          
                .SingleOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());

            if (user == null) return Unauthorized("Invalid username");

            if (!user.EmailConfirmed)
                return BadRequest("Email is not confirmed");

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded) return Unauthorized();

            //var roleNames = user.UserRoles?.Select(r => r.Role.Name).ToList() ?? new List<string>();

            var roleNames = await _userManager.GetRolesAsync(user);

            return new UserDto
            {
                Username = user.UserName,
                Token = await _tokenService.CreateToken(user),
                PhotoUrl = user.Photos?.FirstOrDefault(x => x.IsMain)?.Url,
                Role = roleNames.ToList()
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

        [HttpGet("verify-email")]
        public async Task<IActionResult> VerifyEmail(string userId, string code)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(code))
            {
                return BadRequest("User ID and token are required.");
            }

            // Verify the email token using UserManager
            var user = await _userManager.FindByIdAsync(userId);
            var result = await _userManager.ConfirmEmailAsync(user, code);

            if (result.Succeeded)
            {
                return Ok("Email verified successfully.");
            }
            else
            {
                return BadRequest("Email verification failed.");
            }
        }
        
    }
}