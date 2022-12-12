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
    public class OfferController : ControllerBase
    {
        private readonly INFTDatabaseService _db;
        private readonly ILogger<OfferController> _logger;

        /// <summary>
        /// Dependency Injection Contstructor
        /// </summary>
        /// <param name="db">Database Singleton</param>
        /// <param name="logger">Logger</param>
        public OfferController(INFTDatabaseService db, ILogger<OfferController> logger)
        {
            _db = db;
            _logger = logger;
        }


        /// <summary>
        /// Gets the Activities
        /// </summary>
        /// <returns>List of Offer records</returns>
        /// <response code="200">List of Offer records</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet()]
        [Route("GetOffers")]
        [ProducesResponseType(typeof(List<GetOfferResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetOffers()
        {
            try
            {
                var response = new List<GetOfferResponse>();

                var records = await _db.GetOffers();

                foreach (var record in records)
                {
                    var item = await _db.GetItem((int)record.ItemId);
                    var sender = await _db.GetUser((int)record.SenderId);
                    var receiver = await _db.GetUser((int)record.ReceiverId);

                    response.Add(new GetOfferResponse
                    {
                        OfferId = record.OfferId,
                        ItemName = item.Name,
                        Image = item.MediaIpfs,
                        SenderName = sender.Username,
                        ReceiverName = receiver.Username,
                        Price = record.Price,
                        CreateDate = record.CreateDate
                    });
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                var msg = $"Method: GetOffers, Exception: {ex.Message}";

                _logger.LogError(msg);

                return Problem(title: "/Offer/GetOffers", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }
    }
}