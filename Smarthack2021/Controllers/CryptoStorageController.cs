using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Smarthack2021.Core.BusinessObject;
using Smarthack2021.Core.CryptoAbstractions;

namespace Smarthack2021.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        public ICryptoOrchestrator _cryptoOrchestrator { get; set; }
        
        private readonly UserManager<User> _userManager;

        public TestController(ICryptoOrchestrator cryptoOrchestrator, UserManager<User> userManager)
        {
            _cryptoOrchestrator = cryptoOrchestrator;
            _userManager = userManager;
        }
        
        [HttpGet("encrypt")]
        public async Task<ObjectResult> Encrypt(string password)
        {
            var claims = ((ClaimsIdentity) User.Identity)?.Claims.ToList();

            if (claims == null || !claims.Any()) return BadRequest("You must login first!");

            var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            
            if(email == null) return BadRequest("No email found");

            var user = _userManager.FindByEmailAsync(email);

            if (user == null) return NotFound("User not found!");
            
            var res = await _cryptoOrchestrator.AddPassword(password, user.Id);

            return Ok(res);
        }
        
        [HttpGet("decrypt")]
        public async Task<string> Decrypt()
        {
            var res = await _cryptoOrchestrator.GetPassword("bTV1mk5ROoIAWquaQMcfXCFenxydJ+s9hv6/zB0CxSfYoVTVFdRCcpwJ5xUvPOv+lhCaDv3coEfVOeP6gchUzyWhZ/kKj+LKPmvRFE4pSESD33P0AMiD4+vsksDUmiu787/VL9HHDrJLXUm3XF506RR5jJsKeXjsdmx3x4n4jrgpx/EYlROZwbU9ncXQ8M/T7aQQw79/A+fwSZtvXBsDUhIV5zHsa+aPB20LDcYl4s4bZdiGu3O9UQxMISDEJgKCerUU4Kym6nXQqTMMfL/jgUdCb2w37H8AMtCITIfnK+yItwxhO8eMMtdtvQx5tw5O0aiAJnloZXj22VrwrwymoA==");

            return res;
        }
    }
}