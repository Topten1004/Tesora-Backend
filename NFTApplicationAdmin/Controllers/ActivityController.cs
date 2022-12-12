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
    /// Activity Database Controller
    /// </summary>
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ActivityController : ControllerBase
    {
        private readonly INFTDatabaseService _db;
        private readonly ILogger<ActivityController> _logger;

        /// <summary>
        /// Dependency Injection Contstructor
        /// </summary>
        /// <param name="db">Database Singleton</param>
        /// <param name="logger">Logger</param>
        public ActivityController(INFTDatabaseService db, ILogger<ActivityController> logger)
        {
            _db = db;
            _logger = logger;
        }


        /// <summary>
        /// Gets the Activities
        /// </summary>
        /// <returns>List of Activity records</returns>
        /// <response code="200">List of Activity records</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet()]
        [Route("GetActivities")]
        [ProducesResponseType(typeof(List<GetActivityResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetActivitys()
        {
            try
            {
                var response = new List<GetActivityResponse>();
                var embedImage = false;

                var histories = await _db.GetHistories();
                var records = histories.FindAll(x => x.ItemId != 0);

                foreach (var record in records)
                {
                    var collection = await _db.GetCollection((int)record.CollectionId);
                    var item = await _db.GetItem((int)record.ItemId);
                    var from = await _db.GetUser((int)record.FromId);
                    var to = await _db.GetUser((int)record.ToId);
                    
                    var collectionImage = await _db.GetCollectionImage((int)record.CollectionId);

                    response.Add(new GetActivityResponse
                    {
                        HistoryId = record.HistoryId,
                        Collection = embedImage ? $"data:{collectionImage.Type}:base64, {Convert.ToBase64String(collectionImage.Data)}"
                                                   : $"/api/v1/Activity/GetCollectionImage/{item.CollectionId}",
                        Item = item.MediaIpfs,
                        From = from.Username,
                        HistoryType = record.HistoryType.ToString(),
                        To = to.Username,
                        TransactionHash = record.TransactionHash,
                        Price = record.Price,
                        CreateDate = record.CreateDate
                    });
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                var msg = $"Method: GetActivities, Exception: {ex.Message}";

                _logger.LogError(msg);

                return Problem(title: "/Activity/GetActivitys", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Get Image for the collection
        /// </summary>
        /// <returns>Collection Image</returns>
        /// <response code="200">Collection Image</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet()]
        [Route("GetCollectionImage/{collectionId:int}")]
        [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCollectionImage(int collectionId)
        {
            try
            {
                var image = await _db.GetCollectionImage(collectionId);

                if (image != null && image.Data != null && image.Type != null)
                    return File(image.Data, image.Type);
                else
                    return NotFound("Collection does not have an Image");
            }
            catch (Exception ex)
            {
                var msg = $"Method: CollectionImage, Exception: {ex.Message}";

                _logger.LogError(msg);

                return Problem(title: "/Collection/GetCollectionImage", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }
    }
}