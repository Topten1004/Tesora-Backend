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
    /// History Database Controller
    /// </summary>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class HistoryController : ControllerBase
    {
        private readonly IPostgreSql _db;
        private readonly ILogger<HistoryController> _logger;

        /// <summary>
        /// Dependency Injection Contstructor
        /// </summary>
        /// <param name="db">Database Singleton</param>
        /// <param name="logger">Logger</param>
        public HistoryController(IPostgreSql db, ILogger<HistoryController> logger)
        {
            _db = db;
            _logger = logger;
        }


        /// <summary>
        /// Gets the Histories
        /// </summary>
        /// <returns>List of History records</returns>
        /// <response code="200">List of History records</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet()]
        [Route("GetHistories")]
        [ProducesResponseType(typeof(List<History>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetHistories()
        {
            try
            {
                var result = await _db.RetrieveHistories();

                return Ok(result);
            }
            catch (Exception ex)
            {
                var msg = $"Method: GetHistories, Exception: {ex.Message}";

                _logger.LogError(msg);

                return Problem(title: "/History/GetHistories", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }


        /// <summary>
        /// Gets a History record based in the primary key id
        /// </summary>
        /// <param name="historyId">Primary Key</param>
        /// <returns>History</returns>
        /// <response code="200">History</response>
        /// <response code="404">Record not found</response>
        [HttpGet()]
        [Route("GetHistory/{historyId:int}")]
        [ProducesResponseType(typeof(History), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetHistory(int historyId)
        {
            try
            {
                var result = await _db.RetrieveHistory(historyId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                var msg = $"Method: GetHistory, Exception: {ex.Message}";

                _logger.LogError(msg);

                return NotFound(ex.Message);
            }

        }


        /// <summary>
        /// Add a History record
        /// </summary>
        /// <param name="record">History</param>
        /// <returns>History</returns>
        /// <response code="200">History</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost()]
        [Route("PostHistory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostHistory([FromBody]History record)
        {
            try
            {
               await _db.CreateHistory(record);

                return Ok();
            }
            catch (Exception ex)
            {
                var msg = $"Method: PostHistory, Exception: {ex.Message}";

                _logger.LogError(msg);

                return Problem(title: "/History/PostHistory", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }


        /// <summary>
        /// Update a History record
        /// </summary>
        /// <param name="record">History</param>
        /// <returns></returns>
        /// <response code="200"></response>
        /// <response code="404">Not Found</response>
        [HttpPut()]
        [Route("PutHistory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutHistory([FromBody]History record)
        {
            try
            {
               await _db.UpdateHistory(record);

                return Ok();
            }
            catch (Exception ex)
            {
                var msg = $"Method: PutHistory, Exception: {ex.Message}";

                _logger.LogError(msg);

                return NotFound(ex.Message);
            }

        }


        /// <summary>
        /// Delete a History record
        /// </summary>
        /// <param name="historyId">Primary Key</param>
        /// <returns></returns>
        /// <response code="200"></response>
        /// <response code="404">Not Found</response>
        [HttpDelete()]
        [Route("DeleteHistory/{historyId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteHistory(int historyId)
        {
            try
            {
                await _db.DeleteHistory(historyId);

                return Ok();
            }
            catch (Exception ex)
            {
                var msg = $"Method: DeleteHistory, Exception: {ex.Message}";

                _logger.LogError(msg);

                return NotFound(ex.Message);
            }

        }
    }
}

