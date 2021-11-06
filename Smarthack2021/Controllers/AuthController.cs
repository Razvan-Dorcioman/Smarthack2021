using Microsoft.AspNetCore.Mvc;
using System;
using Smarthack2021.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Smarthack2021.Core.BusinessObject;
using AutoMapper;
using Smarthack2021.Core.LoginAbstractions;
using System.Security.Claims;

namespace Smarthack2021.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AuthController : ControllerBase
    {

        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly ITokenLogic _tokenLogic;

        public AuthController(UserManager<User> userManager, IMapper mapper, ITokenLogic tokenLogic)
        {
            _userManager = userManager;
            _mapper = mapper;
            _tokenLogic = tokenLogic;
        }

        public async Task<IActionResult> Login([FromBody] LoginInfoDto userLoginInfo)
        {
            if (userLoginInfo == null)
                return BadRequest("Invalid client request");

            User user = await _userManager.FindByEmailAsync(userLoginInfo.Email);
            if (user == null)
                return Unauthorized("Email is invalid!");


            var passwordCheck = await _userManager.CheckPasswordAsync(user, userLoginInfo.Password);
            if (!passwordCheck)
                return Unauthorized("Password is invalid!");

            var userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var accessToken = _tokenLogic.GenerateAccessToken(userClaims);
            var refreshToken = _tokenLogic.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpirationTime = DateTime.Now.AddDays(7);

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return StatusCode(500);

            return Ok(new { accessToken, refreshToken });

        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterInfoDto userRegistrationInfo)
        {

            if (userRegistrationInfo == null || !ModelState.IsValid)
                return BadRequest();

            User user = _mapper.Map<User>(userRegistrationInfo);

            var result = await _userManager.CreateAsync(user, userRegistrationInfo.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);

                return BadRequest(new { Errors = errors });
            }

            return StatusCode(201);
        }

        [HttpPost]
        public async Task<IActionResult> Refresh([FromBody] TokenApiModel tokenApiModel)
        {
            if (tokenApiModel is null)
            {
                return BadRequest("Invalid client request");
            }

            string accessToken = tokenApiModel.AccessToken;
            string refreshToken = tokenApiModel.RefreshToken;

            var principal = _tokenLogic.GetPrincipalFromExpiredToken(accessToken);
            string email = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;

            var user = await _userManager.FindByEmailAsync(email);

            string newAccessToken = _tokenLogic.GenerateAccessToken(principal.Claims.ToList());
            string newRefreshToken = _tokenLogic.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;

            return new ObjectResult(new
            {
                accessToken = newAccessToken,
                refreshToken = newRefreshToken
            });

        }

        public class TokenApiModel
        {
            public string AccessToken { get; set; }
            public string RefreshToken { get; set; }
        }
    }


}
