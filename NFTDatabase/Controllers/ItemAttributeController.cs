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
    /// Item Attribute Database Controller
    /// </summary>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ItemAttributeController : ControllerBase
    {
        private readonly IPostgreSql _db;
        private readonly ILogger<ItemAttributeController> _logger;

        /// <summary>
        /// Dependency Injection Contstructor
        /// </summary>
        /// <param name="db">Database Singleton</param>
        /// <param name="logger">Logger</param>
        public ItemAttributeController(IPostgreSql db, ILogger<ItemAttributeController> logger)
        {
            _db = db;
            _logger = logger;
        }


        /// <summary>
        /// Gets the Item Attributes
        /// </summary>
        /// <returns>List of Item Attribute records</returns>
        /// <response code="200">List of Item Attribute records</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet()]
        [Route("GetItemAttributes")]
        [ProducesResponseType(typeof(List<ItemAttribute>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetItemAttributes()
        {
            try
            {
                var result = await _db.RetrieveItemAttributes();

                return Ok(result);
            }
            catch (Exception ex)
            {
                var msg = $"Method: GetItemAttributess, Exception: {ex.Message}";

                _logger.LogError(msg);

                return Problem(title: "/ItemAttribute/GetItemAttributes", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }


        /// <summary>
        /// Gets a Item Attribute record based in the primary key id
        /// </summary>
        /// <param name="itemAttributeId">Primary Key</param>
        /// <returns>Item Attribute</returns>
        /// <response code="200">Item Attribute</response>
        /// <response code="404">Record not found</response>
        [HttpGet()]
        [Route("GetItemAttribute/{itemAttributeId:int}")]
        [ProducesResponseType(typeof(ItemAttribute), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetItemAttribute(int itemAttributeId)
        {
            try
            {
                var result = await _db.RetrieveItemAttribute(itemAttributeId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                var msg = $"Method: GetItemAttribute, Exception: {ex.Message}";

                _logger.LogError(msg);

                return NotFound(ex.Message);
            }

        }


        /// <summary>
        /// Add a Item Attribute record
        /// </summary>
        /// <param name="record">Item Attribute</param>
        /// <returns>Item Attribute</returns>
        /// <response code="200">Item Attribute</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost()]
        [Route("PostItemAttribute")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostItemAttribute([FromBody] ItemAttribute record)
        {
            try
            {
                await _db.CreateItemAttribute(record);

                return Ok("Item Attribute Created");
            }
            catch (Exception ex)
            {
                var msg = $"Method: PostItemAttribute, Exception: {ex.Message}";

                _logger.LogError(msg);

                return Problem(title: "/ItemAttribute/PostItemAttributes", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }


        /// <summary>
        /// Update a Item Attribute record
        /// </summary>
        /// <param name="record">Item Attribute</param>
        /// <returns></returns>
        /// <response code="200"></response>
        /// <response code="404">Not Found</response>
        [HttpPut()]
        [Route("PutItemAttribute")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutItemAttribute([FromBody] ItemAttribute record)
        {
            try
            {
                await _db.UpdateItemAttribute(record);

                return Ok("Item Attribute Updated");
            }
            catch (Exception ex)
            {
                var msg = $"Method: PutItemAttribute, Exception: {ex.Message}";

                _logger.LogError(msg);

                return NotFound(ex.Message);
            }

        }


        /// <summary>
        /// Delete a Item Attribute record
        /// </summary>
        /// <param name="itemAttributeId">Primary Key</param>
        /// <returns></returns>
        /// <response code="200"></response>
        /// <response code="404">Not Found</response>
        [HttpDelete()]
        [Route("DeleteItemAttribute/{itemAttributeId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteItemAttribute(int itemAttributeId)
        {
            try
            {
                await _db.DeleteItemAttribute(itemAttributeId);

                return Ok("Item Attribute Deleted");
            }
            catch (Exception ex)
            {
                var msg = $"Method: DeleteItem, Exception: {ex.Message}";

                _logger.LogError(msg);

                return NotFound(ex.Message);
            }

        }
    }
}

