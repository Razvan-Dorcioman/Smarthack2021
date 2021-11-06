using Microsoft.AspNetCore.Mvc;

namespace Smarthack2021.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController
    {
        [HttpGet]
        public string Get()
        {
            return "Hello from backend";
        }
    }
}