// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//
using Microsoft.AspNetCore.Mvc;


namespace NFTApplicationAdmin.Controllers
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
        /// AWS Health Check
        /// </summary>
        /// <returns>Home View Model</returns>
        /// <response code="200">Home View Model</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet()]
        [Route("/")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetHealthCheck()
        {
            return Ok("Server responsive");
        }

    }
}

