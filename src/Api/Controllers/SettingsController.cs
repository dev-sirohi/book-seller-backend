using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SettingsController : ControllerBase
    {
        // Actions for settings features

        // Example endpoint reformatted like Login
        [HttpGet]
        public async Task<IActionResult> GetSettings()
        {
            // TODO: Implement logic
            return Ok();
        }
    }
}