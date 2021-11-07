using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Smarthack2021.Core.BusinessObject;
using Smarthack2021.Core.CryptoAbstractions;
using Smarthack2021.Dto;

namespace Smarthack2021.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CryptoStorageController : ControllerBase
    {
        private ICryptoOrchestrator _cryptoOrchestrator { get; set; }
        
        private readonly UserManager<User> _userManager;
        
        private IMapper _mapper { get; set; }

        public CryptoStorageController(ICryptoOrchestrator cryptoOrchestrator, UserManager<User> userManager, IMapper mapper)
        {
            _cryptoOrchestrator = cryptoOrchestrator;
            _userManager = userManager;
            _mapper = mapper;
        }
        
        [HttpPost("addPassword")]
        [Authorize]
        public async Task<ObjectResult> AddPassword([FromBody] PasswordDto password)
        {
            var claims = ((ClaimsIdentity) User.Identity)?.Claims.ToList();

            if (claims == null || !claims.Any()) return BadRequest("You must login first!");

            var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            
            if(email == null) return BadRequest("No email found");

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null) return NotFound("User not found!");
            
            var res = await _cryptoOrchestrator.AddPassword(password.Username, password.Password, user.Id);
            
            return Ok(res);
        }
        
        [HttpPost("generatePassword")]
        [Authorize]
        public async Task<ObjectResult> GeneratePassword([FromBody] PasswordGeneratorDto password)
        {
            var claims = ((ClaimsIdentity) User.Identity)?.Claims.ToList();

            if (claims == null || !claims.Any()) return BadRequest("You must login first!");

            var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            
            if(email == null) return BadRequest("No email found");

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null) return NotFound("User not found!");
            
            var res = await _cryptoOrchestrator.GeneratePassword(_mapper.Map<PasswordGenerator>(password), user.Id);
            
            return Ok(res);
        }
        
        [HttpGet("getPassword/{passwordId}")]
        [Authorize]
        public async Task<ObjectResult> GetPassword(string passwordId)
        {
            var claims = ((ClaimsIdentity) User.Identity)?.Claims.ToList();

            if (claims == null || !claims.Any()) return BadRequest("You must login first!");

            var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            
            if(email == null) return BadRequest("No email found");

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null) return NotFound("User not found!");
            
            var res = await _cryptoOrchestrator.GetPassword(new Guid(passwordId), user.Id);

            var resDto = _mapper.Map<PasswordDto>(res);
            return Ok(resDto);
        }

        [HttpGet("getPasswords")]
        [Authorize]
        public async Task<ObjectResult> GetPasswords()
        {
            var claims = ((ClaimsIdentity) User.Identity)?.Claims.ToList();

            if (claims == null || !claims.Any()) return BadRequest("You must login first!");

            var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            
            if(email == null) return BadRequest("No email found");

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null) return NotFound("User not found!");
            
            var res = await _cryptoOrchestrator.GetAllPasswords(user.Id);

            var resDto = _mapper.Map<List<PasswordDto>>(res);

            return Ok(resDto);
        }
        
        [HttpDelete("deletePassword/{passwordId}")]
        [Authorize]
        public async Task<ObjectResult> DeletePassword(string passwordId)
        {
            var claims = ((ClaimsIdentity) User.Identity)?.Claims.ToList();

            if (claims == null || !claims.Any()) return BadRequest("You must login first!");

            var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            
            if(email == null) return BadRequest("No email found");

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null) return NotFound("User not found!");
            
            var res = await _cryptoOrchestrator.DeletePassword(new Guid(passwordId), user.Id);

            return Ok(res);
        }
    }
}