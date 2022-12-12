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
    public class CollectionController : ControllerBase
    {
        private readonly INFTDatabaseService _db;
        private readonly ILogger<CollectionController> _logger;

        /// <summary>
        /// Dependency Injection Contstructor
        /// </summary>
        /// <param name="db">Database Singleton</param>
        /// <param name="logger">Logger</param>
        public CollectionController(INFTDatabaseService db, ILogger<CollectionController> logger)
        {
            _db = db;
            _logger = logger;
        }


        /// <summary>
        /// Gets the Collections
        /// </summary>
        /// <returns>List of Collection records</returns>
        /// <response code="200">List of Collection records</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet()]
        [Route("GetCollections")]
        [ProducesResponseType(typeof(List<GetCollectionResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCollections()
        {
            try
            {
                var response = new List<GetCollectionResponse>();
                var embedImage = false;

                var records = await _db.GetCollections();

                foreach (var record in records)
                {
                    var bannerBox = await _db.GetCollectionBanner(record.CollectionId);
                    var collectionBox = await _db.GetCollectionImage(record.CollectionId);

                    response.Add(new GetCollectionResponse
                    {
                        CollectionId = record.CollectionId,
                        Name = record.Name,
                        Description = record.Description,
                        Banner = embedImage ? $"data:{bannerBox.Type}:base64, {Convert.ToBase64String(bannerBox.Data)}"
                                                   : $"/api/v1/Collection/GetCollectionBanner/{record.CollectionId}",
                        CollectionImage = embedImage ? $"data:{collectionBox.Type}:base64, {Convert.ToBase64String(collectionBox.Data)}"
                                                   : $"/api/v1/Collection/GetCollectionImage/{record.CollectionId}",
                        Royalties = record.Royalties,
                        Status = record.Status.ToString(),

                    });
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                var msg = $"Method: GetCollections, Exception: {ex.Message}";

                _logger.LogError(msg);

                return Problem(title: "/Collection/GetCollections", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }


        /// <summary>
        /// Gets a Collection record based in the primary key id
        /// </summary>
        /// <param name="CollectionId">Primary Key</param>
        /// <returns>Collection</returns>
        /// <response code="200">Collection</response>
        /// <response code="404">Record not found</response>
        [HttpGet()]
        [Route("GetCollection/{CollectionId:int}")]
        [ProducesResponseType(typeof(GetCollectionResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCollection(int CollectionId)
        {
            try
            {
                var record = await _db.GetCollection(CollectionId);
                var embedImage = false;

                var bannerBox = await _db.GetCollectionBanner(CollectionId);
                var collectionBox = await _db.GetCollectionImage(CollectionId);

                var response = new GetCollectionResponse
                {
                    CollectionId = record.CollectionId,
                    Name = record.Name,
                    Description = record.Description,
                    Banner = embedImage ? $"data:{bannerBox.Type}:base64, {Convert.ToBase64String(bannerBox.Data)}"
                                                   : $"/api/v1/Collection/GetCollectionBanner/{record.CollectionId}",
                    CollectionImage = embedImage ? $"data:{collectionBox.Type}:base64, {Convert.ToBase64String(collectionBox.Data)}"
                                                   : $"/api/v1/Collection/GetCollectionImage/{record.CollectionId}",
                    Royalties = record.Royalties,
                    Status = record.Status.ToString(),
                };


                return Ok(response);
            }
            catch (Exception ex)
            {
                var msg = $"Method: GetCollection, Exception: {ex.Message}";

                _logger.LogError(msg);

                return Problem(title: "/Collection/GetCollection", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
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


        /// <summary>
        /// Get Banner for the collection
        /// </summary>
        /// <returns>Collection Banner</returns>
        /// <response code="200">Collection Banner</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet()]
        [Route("GetCollectionBanner/{collectionId:int}")]
        [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCollectionBanner(int collectionId)
        {
            try
            {
                var image = await _db.GetCollectionBanner(collectionId);

                if (image != null && image.Data != null && image.Type != null)
                    return File(image.Data, image.Type);
                else
                    return NotFound("Collection does not have a Banner");
            }
            catch (Exception ex)
            {
                var msg = $"Method: CollectionBanner, Exception: {ex.Message}";

                _logger.LogError(msg);

                return Problem(title: "/Collection/GetCollectionBanner", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }


    }
}