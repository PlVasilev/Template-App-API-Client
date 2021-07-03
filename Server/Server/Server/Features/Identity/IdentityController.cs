﻿
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Server.Data.Models;
using Server.Features.Identity.Models;
using Server.Features.Identity.Services;
using Server.Infrastructure;

namespace Server.Features.Identity
{
    public class IdentityController : ApiController
    {
        private readonly UserManager<User> _userManager;
        private readonly AppSettings _appSettings;
        private readonly IIdentityService _identityService;
    


        public IdentityController(UserManager<User> userManager,
            IOptions<AppSettings> appSettings, 
            IIdentityService identityService)
        {
            _userManager = userManager;
            _identityService = identityService;
            _appSettings = appSettings.Value;
      }

        [HttpPost]
        [Route(nameof(Register))]
        public async Task<ActionResult> Register(RegisterUserRequestModel model)
        {
            if (User.GetId() != null)
                return BadRequest("To Register new user you must log out first.");

            var user = new User
            {
                UserName = model.Username,
                Email = model.Email
            };

            if (await _userManager.FindByEmailAsync(user.Email) != null)
                return BadRequest("User with that Email Exists.");
            
            if (await _userManager.FindByNameAsync(user.UserName) != null)
                return BadRequest("User with that Username Exists.");
            
            var result = await _userManager.CreateAsync(user, model.Password);

            if (_userManager.Users.Count() == 1)
                await _userManager.AddToRoleAsync(user, "Admin");
            
            await _userManager.AddToRoleAsync(user, "User");

            if (result.Succeeded) return Ok();
            
            return BadRequest(result.Errors);
        }

        [HttpPost]
        [Route(nameof(Login))]
        public async Task<ActionResult<LoginResponseModel>> Login(LoginUserRequestModel model)
        {
            if (User.GetId() != null)
             return BadRequest("You are logged In.");
            

            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null) 
                return Unauthorized();

            var passwordValid = _userManager.CheckPasswordAsync(user, model.Password);
            if (passwordValid == null) 
                return Unauthorized();

            var roles = await this._userManager.GetRolesAsync(user);

            return _identityService.GenerateJwtToken(user.Id, user.UserName, _appSettings.Secret, roles);
        }
    }
}
