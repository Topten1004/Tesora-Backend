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
    /// Option Database Controller
    /// </summary>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class OptionController : ControllerBase
    {
        private readonly IPostgreSql _db;
        private readonly ILogger<OptionController> _logger;

        /// <summary>
        /// Dependency Injection Contstructor
        /// </summary>
        /// <param name="db">Database Singleton</param>
        /// <param name="logger">Logger</param>
        public OptionController(IPostgreSql db, ILogger<OptionController> logger)
        {
            _db = db;
            _logger = logger;
        }


        /// <summary>
        /// Gets the Options
        /// </summary>
        /// <returns>List of Option records</returns>
        /// <response code="200">List of Option records</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet()]
        [Route("GetOptions")]
        [ProducesResponseType(typeof(List<Option>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetOptions()
        {
            try
            {
                var result = await _db.RetrieveOptions();

                return Ok(result);
            }
            catch (Exception ex)
            {
                var msg = $"Method: GetOptions, Exception: {ex.Message}";

                _logger.LogError(msg);

                return Problem(title: "/Option/GetOptions", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }


        /// <summary>
        /// Gets a Option record based in the primary key id
        /// </summary>
        /// <param name="optionId">Primary Key</param>
        /// <returns>Option</returns>
        /// <response code="200">Option</response>
        /// <response code="404">Record not found</response>
        [HttpGet()]
        [Route("GetOption/{optionId:int}")]
        [ProducesResponseType(typeof(Option), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOption(int optionId)
        {
            try
            {
                var result = await _db.RetrieveOption(optionId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                var msg = $"Method: GetOption, Exception: {ex.Message}";

                _logger.LogError(msg);

                return NotFound(ex.Message);
            }

        }


        /// <summary>
        /// Add a Option record
        /// </summary>
        /// <param name="record">Option</param>
        /// <returns>Option</returns>
        /// <response code="200">Option</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost()]
        [Route("PostOption")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostOption([FromBody]Option record)
        {
            try
            {
               await _db.CreateOption(record);

                return Ok();
            }
            catch (Exception ex)
            {
                var msg = $"Method: PostOption, Exception: {ex.Message}";

                _logger.LogError(msg);

                return Problem(title: "/Option/PostOption", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }


        /// <summary>
        /// Update a Option record
        /// </summary>
        /// <param name="record">Option</param>
        /// <returns></returns>
        /// <response code="200"></response>
        /// <response code="404">Not Found</response>
        [HttpPut()]
        [Route("PutOption")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutOption([FromBody]Option record)
        {
            try
            {
               await _db.UpdateOption(record);

                return Ok();
            }
            catch (Exception ex)
            {
                var msg = $"Method: PutOption, Exception: {ex.Message}";

                _logger.LogError(msg);

                return NotFound(ex.Message);
            }

        }


        /// <summary>
        /// Delete a Option record
        /// </summary>
        /// <param name="optionId">Primary Key</param>
        /// <returns></returns>
        /// <response code="200"></response>
        /// <response code="404">Not Found</response>
        [HttpDelete()]
        [Route("DeleteOption/{optionId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteOption(int optionId)
        {
            try
            {
                await _db.DeleteOption(optionId);

                return Ok();
            }
            catch (Exception ex)
            {
                var msg = $"Method: DeleteOption, Exception: {ex.Message}";

                _logger.LogError(msg);

                return NotFound(ex.Message);
            }

        }
    }
}

