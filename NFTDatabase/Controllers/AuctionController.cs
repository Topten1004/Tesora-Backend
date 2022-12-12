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
    /// Auction Database Controller
    /// </summary>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AuctionController : ControllerBase
    {
        private readonly IPostgreSql _db;
        private readonly ILogger<AuctionController> _logger;

        /// <summary>
        /// Dependency Injection Contstructor
        /// </summary>
        /// <param name="db">Database Singleton</param>
        /// <param name="logger">Logger</param>
        public AuctionController(IPostgreSql db, ILogger<AuctionController> logger)
        {
            _db = db;
            _logger = logger;
        }


        /// <summary>
        /// Gets a Auction record based in the primary key id
        /// </summary>
        /// <param name="auctionId">Primary Key</param>
        /// <returns>Auction</returns>
        /// <response code="200">Auction</response>
        /// <response code="404">Record not found</response>
        [HttpGet()]
        [Route("GetAuction/{auctionId:int}")]
        [ProducesResponseType(typeof(Auction), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAuction(int auctionId)
        {
            try
            {
                var result = await _db.RetrieveAuction(auctionId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: {Method}, Exception: {Message}", "GetAuction", ex.Message);

                return Problem(title: "/Auction/GetAuction", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }

        /// <summary>
        /// Gets a Auction records based in the itemId
        /// </summary>
        /// <param name="itemId">Primary Key of Item</param>
        /// <returns>List of Auctions</returns>
        /// <response code="200">List of Auctions</response>
        /// <response code="404">Record not found</response>
        [HttpGet()]
        [Route("GetAuctionsByItemId/{itemId:int}")]
        [ProducesResponseType(typeof(Auction), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAuctionsByItemId(int itemId)
        {
            try
            {
                var result = await _db.RetrieveAuctionsByItemId(itemId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: {Method}, Exception: {Message}", "GetAuctionsByItemId", ex.Message);

                return Problem(title: "/Auction/GetAuctionsByItemId", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }

        /// <summary>
        /// Add a Auction record
        /// </summary>
        /// <param name="record">Auction</param>
        /// <returns>Auction</returns>
        /// <response code="200">Auction</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost()]
        [Route("PostAuction")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostAuction([FromBody]Auction record)
        {
            try
            {
               await _db.CreateAuction(record);

                return Ok("Auction created");
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: {Method}, Exception: {Message}", "PostAuction", ex.Message);

                return Problem(title: "/Auction/PostAuction", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }


        /// <summary>
        /// Update a Auction record
        /// </summary>
        /// <param name="record">Auction</param>
        /// <returns></returns>
        /// <response code="200"></response>
        /// <response code="404">Not Found</response>
        [HttpPut()]
        [Route("PutAuction")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutAuction([FromBody]Auction record)
        {
            try
            {
               await _db.UpdateAuction(record);

                return Ok("Auction updated");
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: {Method}, Exception: {Message}", "PutAuction", ex.Message);

                return NotFound(ex.Message);
            }
        }


        /// <summary>
        /// Delete a Auction record
        /// </summary>
        /// <param name="auctionId">Primary Key</param>
        /// <returns></returns>
        /// <response code="200"></response>
        /// <response code="404">Not Found</response>
        [HttpDelete()]
        [Route("DeleteAuction/{auctionId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAuction(int auctionId)
        {
            try
            {
                await _db.DeleteAuction(auctionId);

                return Ok("Auction deleted");
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: {Method}, Exception: {Message}", "DeleteAuction", ex.Message);

                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Gets my auctions
        /// </summary>
        /// <returns>List of my auctions</returns>
        /// <response code="200">List of my auctions</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet()]
        [Route("GetMyAuctions/{userId::int}")]
        [ProducesResponseType(typeof(List<AuctionUserCollectionItemCategory>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMyAuctions(int userId)
        {
            try
            {
                var result = await _db.RetrieveMyAuctions(userId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: {Method}, Exception: {Message}", "GetMyAuctions", ex.Message);

                return Problem(title: "/Auction/GetMyAuctions", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }

        /// <summary>
        /// Gets my auction
        /// </summary>
        /// <returns>my auction</returns>
        /// <response code="200">my auction</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet()]
        [Route("GetMyAuction/{userId::int}/{auctionId::int}")]
        [ProducesResponseType(typeof(AuctionUserCollectionItemCategory), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMyAuction(int userId, int auctionId)
        {
            try
            {
                var result = await _db.RetrieveMyAuction(userId, auctionId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: {Method}, Exception: {Message}", "GetMyAuction", ex.Message);

                return Problem(title: "/Auction/GetMyAuction", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Accept Auction
        /// </summary>
        /// <param name="record">Auction</param>
        /// <returns></returns>
        /// <response code="200"></response>
        /// <response code="404">Not Found</response>
        [HttpPut()]
        [Route("AcceptMyAuction")]
        [ProducesResponseType(typeof(string),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AcceptMyAuction([FromBody] AuctionAccept record)
        {
            try
            {
                if (record.TransactionHash == null)
                    throw new ArgumentNullException(nameof(record.TransactionHash));

                await _db.AcceptMyAuction(record.UserId, record.AuctionId, record.TransactionHash);

                return Ok("Auction accepted");
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: {Method}, Exception: {Message}", "AcceptMyAuction", ex.Message);

                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Get auction bids for an item, sorted highest to lowest
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        [HttpGet()]
        [Route("GetAuctionBids/{itemId:int}")]
        [ProducesResponseType(typeof(Auction), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAuctionBids(int itemId)
        {
            try
            {
                var result = await _db.GetAuctionBids(itemId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: {Method}, Exception: {Message}", "GetAuctionBids", ex.Message);

                return Problem(title: "/Auction/GetAuctionBids", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }

    }
}

