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
using Smarthack2021.Core.CryptoGenerators;
using Smarthack2021.Core.Enums;
using Smarthack2021.Dto;

namespace Smarthack2021.Controllers
{
    [ApiController]
    [Authorize]
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
        public async Task<ObjectResult> AddPassword([FromBody] PasswordDto password)
        {
            var claims = ((ClaimsIdentity) User.Identity)?.Claims.ToList();

            if (claims == null || !claims.Any()) return BadRequest("You must login first!");

            var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            
            if(email == null) return BadRequest("No email found");

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null) return NotFound("User not found!");
            
            var res = await _cryptoOrchestrator.AddPassword(password.Username, password.Password, user.Id);
            
            var resDto = _mapper.Map<PasswordDto>(res);

            return Ok(resDto);
        }
        
        [HttpPost("generatePassword")]
        public async Task<ObjectResult> GeneratePassword([FromBody] PasswordGeneratorDto password)
        {
            var claims = ((ClaimsIdentity) User.Identity)?.Claims.ToList();

            if (claims == null || !claims.Any()) return BadRequest("You must login first!");

            var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            
            if(email == null) return BadRequest("No email found");

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null) return NotFound("User not found!");
            
            var res = _cryptoOrchestrator.GeneratePassword(_mapper.Map<PasswordGenerator>(password), user.Id);

            return Ok(new { res });
        }
        
        [HttpGet("getPassword/{passwordId}")]
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
    
        [HttpPost("addKey")]
        public async Task<ObjectResult> AddKey([FromBody] CryptoKeyDto key)
        {
            var claims = ((ClaimsIdentity)User.Identity)?.Claims.ToList();
            if (claims == null || !claims.Any()) return BadRequest("You must login first!");

            var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            if (email == null) return BadRequest("No email found");

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return NotFound("User not found!");

            var result = await _cryptoOrchestrator.AddCryptoKey(key.Name, key.PublicKey, key.PrivateKey, user.Id, key.Type);
            
            return Ok(result);
        }

        [HttpGet("getKey/{keyId}")]
        public async Task<ObjectResult> GetKey(string keyId)
        {
            var claims = ((ClaimsIdentity)User.Identity)?.Claims.ToList();

            if (claims == null || !claims.Any()) return BadRequest("You must login first!");

            var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            if (email == null) return BadRequest("No email found");

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null) return NotFound("User not found!");

            var res = await _cryptoOrchestrator.GetCryptographicalKey(new Guid(keyId), user.Id);

            var resDto = _mapper.Map<CryptoKeyDto>(res);
            return Ok(resDto);
        }
        
        [HttpDelete("deleteKey/{keyId}")]
        public async Task<ObjectResult> DeleteKey(string keyId)
        {
            var claims = ((ClaimsIdentity) User.Identity)?.Claims.ToList();

            if (claims == null || !claims.Any()) return BadRequest("You must login first!");

            var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            
            if(email == null) return BadRequest("No email found");

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null) return NotFound("User not found!");
            
            var res = await _cryptoOrchestrator.DeleteKey(new Guid(keyId), user.Id);

            return Ok(res);
        }
        
        [HttpGet("getKeys")]
        public async Task<ObjectResult> GetKeys()
        {
            var claims = ((ClaimsIdentity) User.Identity)?.Claims.ToList();

            if (claims == null || !claims.Any()) return BadRequest("You must login first!");

            var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            
            if(email == null) return BadRequest("No email found");

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null) return NotFound("User not found!");
            
            var res = await _cryptoOrchestrator.GetAllKeys(user.Id);

            var resDto = _mapper.Map<List<CryptoKeyDto>>(res);

            return Ok(resDto);
        }

        [HttpGet("generateKey/{type}")]
        public async Task<ObjectResult> GenerateKey(int type)
        {

            var key = new object();

            switch (type)
            {
                case 0:
                    var aesGenerator = new AESGenerator();
                    key = aesGenerator.Generate();
                    break;
                case 1:
                    var tripleDesGenerator= new TripleDESGenerator();
                    key = tripleDesGenerator.EncryptDES3_CBC();

                    break;

                case 2:
                    var rsaGenerator = new RSAGenerator();
                    key = rsaGenerator.RsaGenerator();

                    break;
                case 3:
                    var eccGenerator = new ECCGenerator();
                    key = eccGenerator.GenerateKeys();
                    break;
            }

            return Ok(new { key });
        }


    }
}