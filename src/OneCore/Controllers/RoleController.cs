using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OneCore.Data.Models;
using OneCore.Models;

namespace OneCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<RoleController> _logger;

        public RoleController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<RoleController> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        /// <summary>
        /// Create a new Role
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("createrole")]
        public async Task<IActionResult> CreateRole([FromBody]RoleView model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError(GetModelStateErrorMessage());
                return BadRequest(ModelState);
            }

            var isRoleExist = await _roleManager.RoleExistsAsync(model.Role);

            if (isRoleExist)
            {
                _logger.LogError($"Role {model.Role} already exist");
                return StatusCode(StatusCodes.Status409Conflict, $"Role `{model.Role}` already exist");
            }

            var result = await _roleManager.CreateAsync(new IdentityRole(model.Role));

            if (!result.Succeeded)
            {
                string errorString = string.Join(" | ", result.Errors.Select(c => c.Description).ToList());
                _logger.LogError(errorString);
                return StatusCode(StatusCodes.Status500InternalServerError, errorString);
            }

            return StatusCode(StatusCodes.Status201Created);
        }

        /// <summary>
        /// Assign a role to an user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("assignerole")]
        public async Task<IActionResult> AssignRole(AssignRole model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError(GetModelStateErrorMessage());
                return BadRequest(ModelState);
            }

            ApplicationUser user = await _userManager.FindByEmailAsync(model.Email);

            var result = await _userManager.AddToRoleAsync(user, model.Role);

            if (!result.Succeeded)
            {
                string errorString = string.Join(" | ", result.Errors.Select(c => c.Description).ToList());
                _logger.LogError(errorString);
                return StatusCode(StatusCodes.Status500InternalServerError, errorString);
            }

            return Ok("Success");
        }

        private string GetModelStateErrorMessage()
        {
            return string.Join(" ", ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage));
        }
    }
}