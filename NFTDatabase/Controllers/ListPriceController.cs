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
    /// ListPrice Database Controller
    /// </summary>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ListPriceController : ControllerBase
    {
        private readonly IPostgreSql _db;
        private readonly ILogger<ListPriceController> _logger;

        /// <summary>
        /// Dependency Injection Contstructor
        /// </summary>
        /// <param name="db">Database Singleton</param>
        /// <param name="logger">Logger</param>
        public ListPriceController(IPostgreSql db, ILogger<ListPriceController> logger)
        {
            _db = db;
            _logger = logger;
        }


        /// <summary>
        /// Gets the ListPrices
        /// </summary>
        /// <returns>List of ListPrice records</returns>
        /// <response code="200">List of ListPrice records</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet()]
        [Route("GetListPrices")]
        [ProducesResponseType(typeof(List<ListPrice>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetListPrices()
        {
            try
            {
                var result = await _db.RetrieveListPrices();

                return Ok(result);
            }
            catch (Exception ex)
            {
                var msg = $"Method: GetListPrices, Exception: {ex.Message}";

                _logger.LogError(msg);

                return Problem(title: "/ListPrice/GetListPrices", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }


        /// <summary>
        /// Gets a ListPrice record based in the primary key id
        /// </summary>
        /// <param name="listPriceId">Primary Key</param>
        /// <returns>ListPrice</returns>
        /// <response code="200">ListPrice</response>
        /// <response code="404">Record not found</response>
        [HttpGet()]
        [Route("GetListPrice/{listPriceId:int}")]
        [ProducesResponseType(typeof(ListPrice), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetListPrice(int listPriceId)
        {
            try
            {
                var result = await _db.RetrieveListPrice(listPriceId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                var msg = $"Method: GetListPrice, Exception: {ex.Message}";

                _logger.LogError(msg);

                return NotFound(ex.Message);
            }

        }


        /// <summary>
        /// Add a ListPrice record
        /// </summary>
        /// <param name="record">ListPrice</param>
        /// <returns>ListPrice</returns>
        /// <response code="200">ListPrice</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost()]
        [Route("PostListPrice")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostListPrice([FromBody]ListPrice record)
        {
            try
            {
               await _db.CreateListPrice(record);

                return Ok();
            }
            catch (Exception ex)
            {
                var msg = $"Method: PostListPrice, Exception: {ex.Message}";

                _logger.LogError(msg);

                return Problem(title: "/ListPrice/PostListPrice", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }


        /// <summary>
        /// Update a ListPrice record
        /// </summary>
        /// <param name="record">ListPrice</param>
        /// <returns></returns>
        /// <response code="200"></response>
        /// <response code="404">Not Found</response>
        [HttpPut()]
        [Route("PutListPrice")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutListPrice([FromBody]ListPrice record)
        {
            try
            {
               await _db.UpdateListPrice(record);

                return Ok();
            }
            catch (Exception ex)
            {
                var msg = $"Method: PutListPrice, Exception: {ex.Message}";

                _logger.LogError(msg);

                return NotFound(ex.Message);
            }

        }


        /// <summary>
        /// Delete a ListPrice record
        /// </summary>
        /// <param name="listPriceId">Primary Key</param>
        /// <returns></returns>
        /// <response code="200"></response>
        /// <response code="404">Not Found</response>
        [HttpDelete()]
        [Route("DeleteListPrice/{listPriceId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteListPrice(int listPriceId)
        {
            try
            {
                await _db.DeleteListPrice(listPriceId);

                return Ok();
            }
            catch (Exception ex)
            {
                var msg = $"Method: DeleteListPrice, Exception: {ex.Message}";

                _logger.LogError(msg);

                return NotFound(ex.Message);
            }

        }
    }
}

