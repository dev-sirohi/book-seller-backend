using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SupportController : ControllerBase
    {
        // Example endpoint reformatted like Login
        [HttpGet("top-picks")]
        public async Task<IActionResult> GetTopPicks()
        {
            // TODO: Implement logic
            return Ok();
        }
    }
}