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
    public class CartController : ControllerBase
    {
        private readonly IPostgreSql _db;
        private readonly ILogger<CartController> _logger;

        /// <summary>
        /// Dependency Injection Contstructor
        /// </summary>
        /// <param name="db">Database Singleton</param>
        /// <param name="logger">Logger</param>
        public CartController(IPostgreSql db, ILogger<CartController> logger)
        {
            _db = db;
            _logger = logger;
        }


        /// <summary>
        /// Gets the Carts
        /// </summary>
        /// <returns>List of Cart records</returns>
        /// <response code="200">List of Cart records</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet()]
        [Route("GetCartItems/{userId:int}")]
        [ProducesResponseType(typeof(List<CartItem>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCartItems(int userId)
        {
            try
            {
                var result = await _db.RetrieveCartItems(userId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: {Method}, Exception: {Message}", "GetCartItems", ex.Message);

                return Problem(title: "/Cart/GetCartItems", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }


        /// <summary>
        /// Gets a Cart record based in the primary key id
        /// </summary>
        /// <param name="lineId">Primary Key</param>
        /// <returns>Cart</returns>
        /// <response code="200">Cart</response>
        /// <response code="404">Record not found</response>
        [HttpGet()]
        [Route("GetCartItem/{lineId:int}")]
        [ProducesResponseType(typeof(CartItem), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCartItem(int lineId)
        {
            try
            {
                var result = await _db.RetrieveCartItem(lineId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: {Method}, Exception: {Message}", "GetCartItem", ex.Message);

                return Problem(title: "/Cart/GetCartItem", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }

        /// <summary>
        /// Add a Cart record
        /// </summary>
        /// <param name="record">Cart</param>
        /// <returns>Cart</returns>
        /// <response code="200">Cart</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost()]
        [Route("PostCartItem")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostCartItem([FromBody] CartItem record)
        {
            try
            {
                await _db.CreateCartItem(record);

                return Ok("Cart item added");
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: {Method}, Exception: {Message}", "PostCartItem", ex.Message);

                return Problem(title: "/Cart/PostCartItem", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }


        /// <summary>
        /// Delete a Cart record
        /// </summary>
        /// <param name="lineId">Primary Key</param>
        /// <returns></returns>
        /// <response code="200"></response>
        /// <response code="404">Not Found</response>
        [HttpDelete()]
        [Route("DeleteCartItem/{lineId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteCartItem(int lineId)
        {
            try
            {
                await _db.DeleteCartItem(lineId);

                return Ok("Cart item deleted");
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: {Method}, Exception: {Message}", "DeleteCartItem", ex.Message);

                return Problem(title: "/Cart/DeleteCartItem", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }

        /// <summary>
        /// Delete all Cart records for a user
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns></returns>
        /// <response code="200"></response>
        /// <response code="404">Not Found</response>
        [HttpDelete()]
        [Route("DeleteCartItems/{userId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteCartItems(int userId)
        {
            try
            {
                await _db.DeleteCartItems(userId);

                return Ok("Cart items deleted");
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: {Method}, Exception: {Message}", "DeleteCartItems", ex.Message);

                return Problem(title: "/Cart/DeleteCartItems", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }

    }
}

