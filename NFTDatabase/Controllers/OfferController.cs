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
    /// Offer Database Controller
    /// </summary>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class OfferController : ControllerBase
    {
        private readonly IPostgreSql _db;
        private readonly ILogger<OfferController> _logger;

        /// <summary>
        /// Dependency Injection Contstructor
        /// </summary>
        /// <param name="db">Database Singleton</param>
        /// <param name="logger">Logger</param>
        public OfferController(IPostgreSql db, ILogger<OfferController> logger)
        {
            _db = db;
            _logger = logger;
        }


        /// <summary>
        /// Gets the Offers
        /// </summary>
        /// <returns>List of Offer records</returns>
        /// <response code="200">List of Offer records</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet()]
        [Route("GetOffers")]
        [ProducesResponseType(typeof(List<Offer>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetOffers()
        {
            try
            {
                var result = await _db.RetrieveOffers();

                return Ok(result);
            }
            catch (Exception ex)
            {
                var msg = $"Method: GetOffers, Exception: {ex.Message}";

                _logger.LogError(msg);

                return Problem(title: "/Offer/GetOffers", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }


        /// <summary>
        /// Gets a Offer record based in the primary key id
        /// </summary>
        /// <param name="offerId">Primary Key</param>
        /// <returns>Offer</returns>
        /// <response code="200">Offer</response>
        /// <response code="404">Record not found</response>
        [HttpGet()]
        [Route("GetOffer/{offerId:int}")]
        [ProducesResponseType(typeof(Offer), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOffer(int offerId)
        {
            try
            {
                var result = await _db.RetrieveOffer(offerId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                var msg = $"Method: GetOffer, Exception: {ex.Message}";

                _logger.LogError(msg);

                return NotFound(ex.Message);
            }

        }


        /// <summary>
        /// Add a Offer record
        /// </summary>
        /// <param name="record">Offer</param>
        /// <returns>Offer</returns>
        /// <response code="200">Offer</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost()]
        [Route("PostOffer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostOffer([FromBody]Offer record)
        {
            try
            {
               await _db.CreateOffer(record);

                return Ok();
            }
            catch (Exception ex)
            {
                var msg = $"Method: PostOffer, Exception: {ex.Message}";

                _logger.LogError(msg);

                return Problem(title: "/Offer/PostOffer", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }


        /// <summary>
        /// Update a Offer record
        /// </summary>
        /// <param name="record">Offer</param>
        /// <returns></returns>
        /// <response code="200"></response>
        /// <response code="404">Not Found</response>
        [HttpPut()]
        [Route("PutOffer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutOffer([FromBody]Offer record)
        {
            try
            {
               await _db.UpdateOffer(record);

                return Ok();
            }
            catch (Exception ex)
            {
                var msg = $"Method: PutOffer, Exception: {ex.Message}";

                _logger.LogError(msg);

                return NotFound(ex.Message);
            }

        }


        /// <summary>
        /// Delete a Offer record
        /// </summary>
        /// <param name="offerId">Primary Key</param>
        /// <returns></returns>
        /// <response code="200"></response>
        /// <response code="404">Not Found</response>
        [HttpDelete()]
        [Route("DeleteOffer/{offerId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteOffer(int offerId)
        {
            try
            {
                await _db.DeleteOffer(offerId);

                return Ok();
            }
            catch (Exception ex)
            {
                var msg = $"Method: DeleteOffer, Exception: {ex.Message}";

                _logger.LogError(msg);

                return NotFound(ex.Message);
            }

        }
        
        /// <summary>
        /// Gets my offer
        /// </summary>
        /// <returns>my offer</returns>
        /// <response code="200">my offer</response>
        /// <response code="500">Internal Server Error</response>
        [HttpDelete()]
        [Route("DeletePastOffers/{itemId::int}/{currentOwnerId::int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeletePastOffers(int itemId, int currentOwnerId)
        {
            try
            {
                await _db.DeletePastOffers(itemId, currentOwnerId);

                return Ok();
            }
            catch (Exception ex)
            {
                var msg = $"Method: DeletePastOffers, Exception: {ex.Message}";

                _logger.LogError(msg);

                return Problem(title: "/Offer/DeletePastOffers", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }

        /// <summary>
        /// Gets my offers
        /// </summary>
        /// <returns>List of my offers</returns>
        /// <response code="200">List of my offers</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet()]
        [Route("GetMyOffers/{userId::int}")]
        [ProducesResponseType(typeof(List<OfferUserCollectionItemCategory>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMyOffers(int userId)
        {
            try
            {
                var result = await _db.RetrieveMyOffers(userId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                var msg = $"Method: GetMyOffers, Exception: {ex.Message}";

                _logger.LogError(msg);

                return Problem(title: "/Offer/GetMyOffers", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }

        /// <summary>
        /// Gets my offer
        /// </summary>
        /// <returns>my offer</returns>
        /// <response code="200">my offer</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet()]
        [Route("GetMyOffer/{userId::int}/{offerId::int}")]
        [ProducesResponseType(typeof(OfferUserCollectionItemCategory), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMyOffer(int userId, int offerId)
        {
            try
            {
                var result = await _db.RetrieveMyOffer(userId, offerId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                var msg = $"Method: GetMyOffer, Exception: {ex.Message}";

                _logger.LogError(msg);

                return Problem(title: "/Offer/GetMyOffer", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }

        /// <summary>
        /// Accept Offer
        /// </summary>
        /// <param name="record">Offer</param>
        /// <returns></returns>
        /// <response code="200"></response>
        /// <response code="404">Not Found</response>
        [HttpPut()]
        [Route("AcceptMyOffer")]
        [ProducesResponseType(typeof(string),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AcceptMyOffer([FromBody] OfferAccept record)
        {
            try
            {
                if (record.TransactionHash == null)
                    throw new ArgumentNullException(nameof(record.TransactionHash));

                await _db.AcceptMyOffer(record.UserId, record.OfferId, record.TransactionHash);

                return Ok();
            }
            catch (Exception ex)
            {
                var msg = $"Method: PutOffer, Exception: {ex.Message}";

                _logger.LogError(msg);

                return NotFound(ex.Message);
            }

        }

    }
}

