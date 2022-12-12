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
    /// Item Database Controller
    /// </summary>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ItemController : ControllerBase
    {
        private readonly IPostgreSql _db;
        private readonly ILogger<ItemController> _logger;

        /// <summary>
        /// Dependency Injection Contstructor
        /// </summary>
        /// <param name="db">Database Singleton</param>
        /// <param name="logger">Logger</param>
        public ItemController(IPostgreSql db, ILogger<ItemController> logger)
        {
            _db = db;
            _logger = logger;
        }


        /// <summary>
        /// Gets the Items by CollectionId
        /// </summary>
        /// <returns>List of Item records</returns>
        /// <response code="200">List of Item records</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet()]
        [Route("GetItems/{collectionId::int}")]
        [ProducesResponseType(typeof(List<Item>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetItems(int collectionId)
        {
            try
            {
                var result = await _db.RetrieveItems(collectionId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                var msg = $"Method: GetItems, Exception: {ex.Message}";

                _logger.LogError(msg);

                return Problem(title: "/Item/GetItems", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }

        /// <summary>
        /// Gets the Items by CategoryId
        /// </summary>
        /// <returns>List of Item records</returns>
        /// <response code="200">List of Item records</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet()]
        [Route("GetItemsByCategoryId/{categoryId::int}")]
        [ProducesResponseType(typeof(List<Item>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetItemsByCategoryId(int categoryId)
        {
            try
            {
                var result = await _db.RetrieveItemsByCategoryId(categoryId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                var msg = $"Method: GetItemsByCategoryId, Exception: {ex.Message}";

                _logger.LogError(msg);

                return Problem(title: "/Item/GetItems", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }


        /// <summary>
        /// Gets the Items the auction has ended
        /// </summary>
        /// <returns>List of Item records</returns>
        /// <response code="200">List of Item records</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet()]
        [Route("GetAuctionEndedItems")]
        [ProducesResponseType(typeof(List<Item>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAuctionEndedItems()
        {
            try
            {
                var result = await _db.RetrieveAuctionEndedItems();

                return Ok(result);
            }
            catch (Exception ex)
            {
                var msg = $"Method: GetAuctionEndedItems, Exception: {ex.Message}";

                _logger.LogError(msg);

                return Problem(title: "/Item/GetAuctionEndedItems", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }



        /// <summary>
        /// Gets all Items
        /// </summary>
        /// <returns>List of Item records</returns>
        /// <response code="200">List of Item records</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet()]
        [Route("GetAllItems")]
        [ProducesResponseType(typeof(List<Item>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllItems()
        {
            try
            {
                var result = await _db.RetrieveAllItems();

                return Ok(result);
            }
            catch (Exception ex)
            {
                var msg = $"Method: GetAllItems, Exception: {ex.Message}";

                _logger.LogError(msg);

                return Problem(title: "/Item/GetAllItems", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }

        /// <summary>
        /// Gets Items
        /// </summary>
        /// <returns>List of Item records</returns>
        /// <response code="200">List of Item records</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet()]
        [Route("GetMyItems/{collectionId::int}")]
        [ProducesResponseType(typeof(List<Item>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMyItems(int collectionId)
        {
            try
            {
                var result = await _db.RetrieveMyItems(collectionId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                var msg = $"Method: GeMytItems, Exception: {ex.Message}";

                _logger.LogError(msg);

                return Problem(title: "/Item/GetMyItems", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }


        /// <summary>
        /// Gets a Item record based in the primary key id
        /// </summary>
        /// <param name="itemId">Primary Key</param>
        /// <returns>Item</returns>
        /// <response code="200">Item</response>
        /// <response code="404">Record not found</response>
        [HttpGet()]
        [Route("GetItem/{itemId:int}")]
        [ProducesResponseType(typeof(Item), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetItem(int itemId)
        {
            try
            {
                var result = await _db.RetrieveItem(itemId);

                if (result == null)
                    throw new PostgreSql.RecordNotFound("Item not found");

                return Ok(result);
            }
            catch (PostgreSql.RecordNotFound nf)
            {
                var msg = $"Method: GetItem, Exception: {nf.Message}";

                _logger.LogError(msg);

                return NotFound(nf.Message);
            }
            catch (Exception ex)
            {
                var msg = $"Method: GetItem, Exception: {ex.Message}";

                _logger.LogError(msg);

                return Problem(title: "/Item/GetItem", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }


        /// <summary>
        /// Gets a Item record based in the primary key id
        /// </summary>
        /// <param name="itemId">Primary Key</param>
        /// <returns>Item</returns>
        /// <response code="200">Item</response>
        /// <response code="404">Record not found</response>
        [HttpGet()]
        [Route("GetMyItem/{itemId:int}")]
        [ProducesResponseType(typeof(Item), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMyItem(int itemId)
        {
            try
            {
                var result = await _db.RetrieveMyItem(itemId);

                if (result == null)
                    throw new PostgreSql.RecordNotFound("Item not found");

                return Ok(result);
            }
            catch (PostgreSql.RecordNotFound nf)
            {
                var msg = $"Method: GetMyItem, Exception: {nf.Message}";

                _logger.LogError(msg);

                return NotFound(nf.Message);
            }
            catch (Exception ex)
            {
                var msg = $"Method: GetMyItem, Exception: {ex.Message}";

                _logger.LogError(msg);

                return Problem(title: "/Item/GetMyItem", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }


        /// <summary>
        /// Add a Item record
        /// </summary>
        /// <param name="record">Item</param>
        /// <returns>Item</returns>
        /// <response code="200">Item</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost()]
        [Route("PostItem")]
        [ProducesResponseType(typeof(int),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostItem([FromBody]Item record)
        {
            try
            {
               var iNewId = await _db.CreateItem(record);

                return Ok(iNewId);
            }
            catch (Exception ex)
            {
                var msg = $"Method: PostItem, Exception: {ex.Message}";

                _logger.LogError(msg);

                return Problem(title: "/Item/PostItem", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }


        /// <summary>
        /// Add a Sale for an Item
        /// </summary>
        /// <param name="record">SaleItem</param>
        /// <returns>Item</returns>
        /// <response code="200"></response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost()]
        [Route("PostItemSale")]
        [ProducesResponseType(typeof(string),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostItemSale([FromBody] ItemSale record)
        {
            try
            {
                await _db.PostItemSale(record);

                return Ok("Sale Posted");
            }
            catch (Exception ex)
            {
                var msg = $"Method: PostItemSale, Exception: {ex.Message}";

                _logger.LogError(msg);

                return Problem(title: "/Item/PostItemSale", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }


        /// <summary>
        /// Update a Item record
        /// </summary>
        /// <param name="record">Item</param>
        /// <returns></returns>
        /// <response code="200"></response>
        /// <response code="404">Not Found</response>
        [HttpPut()]
        [Route("PutItem")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutItem([FromBody]Item record)
        {
            try
            {
                await _db.UpdateItem(record);

                return Ok("Item Updated");
            }
            catch (Exception ex)
            {
                var msg = $"Method: PutItem, Exception: {ex.Message}";

                _logger.LogError(msg);

                return Problem(title: "/Item/PutItem", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }


        /// <summary>
        /// Update a Item record
        /// </summary>
        /// <param name="itemId">Item</param>
        /// <returns></returns>
        /// <response code="200"></response>
        /// <response code="404">Not Found</response>
        [HttpPut()]
        [Route("PutAuctionEndItem/{itemId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutAuctionEndItem(int itemId)
        {
            try
            {
                await _db.PutAuctionEndItem(itemId);

                return Ok("Auction ended");
            }
            catch (Exception ex)
            {
                var msg = $"Method: PutAuctionEndItem, Exception: {ex.Message}";

                _logger.LogError(msg);

                return Problem(title: "/Item/PutAuctionEndItem", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }


        /// <summary>
        /// Delete a Item record
        /// </summary>
        /// <param name="itemId">Primary Key</param>
        /// <returns></returns>
        /// <response code="200"></response>
        /// <response code="404">Not Found</response>
        [HttpDelete()]
        [Route("DeleteItem/{itemId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteItem(int itemId)
        {
            try
            {
                await _db.DeleteItem(itemId);

                return Ok("Item Deleted");
            }
            catch (Exception ex)
            {
                var msg = $"Method: DeleteItem, Exception: {ex.Message}";

                _logger.LogError(msg);

                return Problem(title: "/Item/DeleteItem", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }

        /// <summary>
        /// Accept Offer
        /// </summary>
        /// <param name="itemId">Item</param>
        /// <param name="acceptOffer"></param>
        /// <returns></returns>
        /// <response code="200"></response>
        /// <response code="404">Not Found</response>
        [HttpPut()]
        [Route("PutMyItemAcceptOffer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> MyItemAcceptOffer(int itemId, bool acceptOffer)
        {
            try
            {
                await _db.MyItemAcceptOffer(itemId, acceptOffer);

                return Ok("Accept Offer Updated");
            }
            catch (Exception ex)
            {
                var msg = $"Method: MyItemAcceptOffer, Exception: {ex.Message}";

                _logger.LogError(msg);

                return Problem(title: "/Item/MyItemAcceptOffer", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }


        /// <summary>
        /// Gets the Market Place Items
        /// </summary>
        /// <returns>List of MarketPlaceResponse records</returns>
        /// <response code="200">List of MarketPlaceResponse records</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost()]
        [Route("GetMarketPlaceItems")]
        [ProducesResponseType(typeof(List<MarketPlaceResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMarketPlaceItems(MarketPlaceRequest request)
        {
            try
            {
                var result = await _db.GetMarketPlaceItems(request);

                return Ok(result);
            }
            catch (Exception ex)
            {
                var msg = $"Method: GetMarketPlaceItems, Exception: {ex.Message}";

                _logger.LogError(msg);

                return Problem(title: "/Item/GetMarketPlaceItems", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }


    }
}

