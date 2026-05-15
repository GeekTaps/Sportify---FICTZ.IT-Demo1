using Microsoft.AspNetCore.Mvc;

namespace Sportify.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestConnectionController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { message = "¡Conexión exitosa desde el backend de .NET!" });
        }
    }
}
