using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Smarthack2021.Core.CryptoAbstractions;

namespace Smarthack2021.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        public ICryptoOrchestrator _cryptoOrchestrator { get; set; }

        public TestController(ICryptoOrchestrator cryptoOrchestrator)
        {
            _cryptoOrchestrator = cryptoOrchestrator;
        }
        
        [HttpGet]
        public async Task<string> Get()
        {
            
            var res = await _cryptoOrchestrator.AddPassword("simplePassword01");

            return "ok";
        }
    }
}