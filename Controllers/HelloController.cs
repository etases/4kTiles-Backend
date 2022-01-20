using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace _4kTiles_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HelloController : ControllerBase
    {
        [HttpGet]
        public string PrintHello()
        {
            return "Hello World";
        }
    }
}
