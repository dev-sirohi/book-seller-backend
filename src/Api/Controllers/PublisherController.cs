using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PublisherController : ControllerBase
    {
        // Example endpoint reformatted like Login
        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboard()
        {
            // TODO: Implement logic
            return Ok();
        }
    }
}