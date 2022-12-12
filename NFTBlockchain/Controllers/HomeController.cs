using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NFTBlockchain.Controllers
{
    /// <summary>
    /// Home Page Controller
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class HomeController : ControllerBase
    {

        /// <summary>
        /// Server Health Check
        /// </summary>
        /// <returns>Success Message</returns>
        [HttpGet()]
        [Route("/")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetHealthCheck()
        {
            return Ok("Server responsive");
        }


    }
}
