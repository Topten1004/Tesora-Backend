// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using NFTDatabaseService;
using NFTDatabaseEntities;

using NFTApplicationAdmin.Models;



namespace NFTApplicationAdmin.Controllers
{

    /// <summary>
    /// Category Database Controller
    /// </summary>
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ItemController : ControllerBase
    {
        private readonly INFTDatabaseService _db;
        private readonly ILogger<ItemController> _logger;

        /// <summary>
        /// Dependency Injection Contstructor
        /// </summary>
        /// <param name="db">Database Singleton</param>
        /// <param name="logger">Logger</param>
        public ItemController(INFTDatabaseService db, ILogger<ItemController> logger)
        {
            _db = db;
            _logger = logger;
        }


        /// <summary>
        /// Gets the Items
        /// </summary>
        /// <returns>List of Item records</returns>
        /// <response code="200">List of Item records</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet()]
        [Route("GetAllItems")]
        [ProducesResponseType(typeof(List<GetItemResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllItems()
        {
            try
            {
                var response = new List<GetItemResponse>();

                var records = await _db.GetAllItems();

                foreach (var record in records)
                {
                    response.Add(new GetItemResponse
                    {
                        ItemId = record.ItemId,
                        Name = record.Name,
                        Description = record.Description,
                        Media = record.MediaIpfs,
                        LikeCount = record.LikeCount,
                        Price = record.Price,
                        Status = record.Status.ToString(),
                        CreateDate = record.CreateDate
                    });
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                var msg = $"Method: GetItems, Exception: {ex.Message}";

                _logger.LogError(msg);

                return Problem(title: "/Item/GetItems", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }

        /// <summary>
        /// Gets the Items
        /// </summary>
        /// <returns>List of Item records</returns>
        /// <response code="200">List of Item records</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet()]
        [Route("GetItems/{collectionId::int}")]
        [ProducesResponseType(typeof(List<GetItemResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetItems(int collectionId)
        {
            try
            {
                var response = new List<GetItemResponse>();

                var records = await _db.GetItems(collectionId);

                foreach (var record in records)
                {
                    response.Add(new GetItemResponse
                    {
                        ItemId = record.ItemId,
                        Name = record.Name,
                        Description = record.Description,
                        Media = record.MediaIpfs,
                        LikeCount = record.LikeCount,
                        Price = record.Price,
                        Status = record.Status.ToString(),
                        CreateDate = record.CreateDate
                    });
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                var msg = $"Method: GetItems, Exception: {ex.Message}";

                _logger.LogError(msg);

                return Problem(title: "/Item/GetItems", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }


        /// <summary>
        /// Gets a Item record based in the primary key id
        /// </summary>
        /// <param name="ItemId">Primary Key</param>
        /// <returns>Item</returns>
        /// <response code="200">Item</response>
        /// <response code="404">Record not found</response>
        [HttpGet()]
        [Route("GetItem/{ItemId:int}")]
        [ProducesResponseType(typeof(GetItemResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetItem(int ItemId)
        {
            try
            {
                var record = await _db.GetItem(ItemId);

                var response = new GetItemResponse
                {
                    ItemId = record.ItemId,
                    Name = record.Name,
                    Description = record.Description,
                    Media = record.MediaIpfs,
                    LikeCount = record.LikeCount,
                    Price = record.Price,
                    Status = record.Status.ToString(),
                    CreateDate = record.CreateDate
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                var msg = $"Method: GetItem, Exception: {ex.Message}";

                _logger.LogError(msg);

                return Problem(title: "/Item/GetItem", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }
    }
}