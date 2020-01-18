using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using OneCore.Data.Models;
using OneCore.Models;
using OneCore.ViewModels;

namespace OneCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AccountController> _logger;
        private readonly IMapper _mapper;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration congiuration,
            ILogger<AccountController> logger,
            IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = congiuration;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Creata a new User Account
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRequest user)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError(GetModelStateErrorMessage());
                return BadRequest(ModelState);
            }

            var applicationUser = new ApplicationUser()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.Email,
                Email = user.Email,
                Address = user.Address
            };
            
            var result = await _userManager.CreateAsync(applicationUser, user.Password);

            if (result.Succeeded)
            {
                return StatusCode(StatusCodes.Status201Created);
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
        }

        /// <summary>
        /// User Account Login
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     POST /login
        ///     {
        ///         "email": "user@email.com",
        ///         "Password": "mypass"
        ///     }
        /// </remarks>
        /// <param name="model"></param>
        /// <returns>Returns JWT Token</returns>
        /// <response code="200">Return JWT access token</response>
        /// <response code="401">Unauthorized</response>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login(Login model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError(GetModelStateErrorMessage());
                return BadRequest(ModelState);
            }

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

            if (!result.Succeeded)
            {
                _logger.LogError("Invalid Credentials");
                return StatusCode(StatusCodes.Status401Unauthorized, "Invalid Credentials");
            }

            var appUser = _userManager.Users.SingleOrDefault(r => r.Email == model.Email);
            var tokenResult = await GenerateJwtToken(model.Email, appUser);
            return Ok(tokenResult);
        }

        /// <summary>
        /// Return list of all registered users
        /// </summary>
        /// <returns></returns>
        [HttpGet("users")]
        public IActionResult Users()
        {
            return Ok(_mapper.Map<UserResponse[]>(_userManager.Users));
        }

        private async Task<string> GenerateJwtToken(string email, ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };


            var roles = await _userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["JwtExpireDays"]));

            var token = new JwtSecurityToken(
                _configuration["JwtIssuer"],
                _configuration["JwtIssuer"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        
        private string GetModelStateErrorMessage()
        {
            return string.Join(" ", ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage));
        }
    }
} 