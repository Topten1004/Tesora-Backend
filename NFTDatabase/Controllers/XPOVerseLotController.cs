// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using NFTDatabase.DataAccess;
using NFTDatabaseEntities;


namespace NFTDatabase.Controllers
{

    /// <summary>
    /// Cart Database Controller
    /// </summary>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class XPOVerseLotController : ControllerBase
    {
        private readonly IPostgreSql _db;
        private readonly ILogger<XPOVerseLotController> _logger;

        /// <summary>
        /// Dependency Injection Contstructor
        /// </summary>
        /// <param name="db">Database Singleton</param>
        /// <param name="logger">Logger</param>
        public XPOVerseLotController(IPostgreSql db, ILogger<XPOVerseLotController> logger)
        {
            _db = db;
            _logger = logger;
        }


        /// <summary>
        /// Gets the list of Rings for Sale
        /// </summary>
        /// <returns>List Rings for Sale</returns>
        /// <response code="200">List of Rings for Sale</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet()]
        [Route("GetRingsForSale")]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetRingsForSale()
        {
            try
            {
                return Ok(await _db.GetRingsForSale());
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: {Method}, Exception: {Message}", "GetRingsForSale", ex.Message);

                return Problem(title: "/XPOVerseLot/GetRingsForSale", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Gets the list of Sections for Sale within a Ring
        /// </summary>
        /// <returns>List Sections for Sale</returns>
        /// <response code="200">List of SEctions for Sale</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet()]
        [Route("GetSectionsForSale/{ring}")]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSectionsForSale(string ring)
        {
            try
            {
                return Ok(await _db.GetSectionsForSale(ring));
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: {Method}, Exception: {Message}", "GetSectionsForSale", ex.Message);

                return Problem(title: "/XPOVerseLot/GetSectionsForSale", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Gets the list of Blocks for Sale within a Ring and Section
        /// </summary>
        /// <returns>List Blocks for Sale</returns>
        /// <response code="200">List of Blocks for Sale</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet()]
        [Route("GetBlocksForSale/{ring}/{section}")]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBlocksForSale(string ring, string section)
        {
            try
            {
                return Ok(await _db.GetBlocksForSale(ring, section));
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: {Method}, Exception: {Message}", "GetBlocksForSale", ex.Message);

                return Problem(title: "/XPOVerseLot/GetBlocksForSale", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Gets the list of Lots for Sale within a Ring, Section and Block
        /// </summary>
        /// <returns>List Lots for Sale</returns>
        /// <response code="200">List of Lots for Sale</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet()]
        [Route("GetLotsForSale/{ring}/{section}")]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetLotsForSale(string ring, string section, string block)
        {
            try
            {
                return Ok(await _db.GetLotsForSale(ring, section, block));
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: {Method}, Exception: {Message}", "GetLotsForSale", ex.Message);

                return Problem(title: "/XPOVerseLot/GetLotsForSale", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

    }
}

