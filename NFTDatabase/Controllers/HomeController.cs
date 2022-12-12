// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//
using Microsoft.AspNetCore.Mvc;


namespace NFTDatabase.Controllers
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
        /// Health Check
        /// </summary>
        /// <returns>Server Responsive</returns>
        /// <response code="200">Server responsive</response>
        [HttpGet()]
        [Route("/")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetHealthCheck()
        {
            return Ok("Server responsive");
        }

    }
}

