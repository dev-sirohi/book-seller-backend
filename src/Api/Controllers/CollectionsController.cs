using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CollectionsController : ControllerBase
    {
        // Example endpoint reformatted like Login
        [HttpGet("user")]
        public async Task<IActionResult> GetUserCollections()
        {
            // TODO: Implement logic
            return Ok();
        }
    }
}