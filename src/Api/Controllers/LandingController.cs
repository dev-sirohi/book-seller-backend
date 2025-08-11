using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LandingController : ControllerBase
    {
        // Example endpoint reformatted like Login
        [HttpGet("featured-books")]
        public async Task<IActionResult> GetFeaturedBooks()
        {
            // TODO: Implement logic
            return Ok();
        }
    }
}