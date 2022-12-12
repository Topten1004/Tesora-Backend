// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using NFTDatabase.DataAccess;
using NFTDatabaseEntities;
using System.Drawing;

namespace NFTDatabase.Controllers
{

    /// <summary>
    /// Collection Database Controller
    /// </summary>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CollectionController : ControllerBase
    {
        private readonly IPostgreSql _db;
        private readonly ILogger<CollectionController> _logger;

        /// <summary>
        /// Dependency Injection Contstructor
        /// </summary>
        /// <param name="db">Database Singleton</param>
        /// <param name="logger">Logger</param>
        public CollectionController(IPostgreSql db, ILogger<CollectionController> logger)
        {
            _db = db;
            _logger = logger;
        }


        /// <summary>
        /// Collection Name Exists?
        /// </summary>
        /// <returns>bool</returns>
        /// <response code="200">bool</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet()]
        [Route("GetCollectionNameExists/{name}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCollectionNameExists(string name)
        {
            try
            {
                return Ok(await _db.CollectionNameExists(name));
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: {Method}, Exception: {Message}", "GetCollectionNameExists", ex.Message);

                return Problem(title: "/Collection/GetCollectionNameExists", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }


        /// <summary>
        /// Gets the Collections
        /// </summary>
        /// <returns>List of Collection records</returns>
        /// <response code="200">List of Collection records</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet()]
        [Route("GetCollections")]
        [ProducesResponseType(typeof(List<Collection>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCollections()
        {
            try
            {
                var result = await _db.RetrieveCollections();

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: {Method}, Exception: {Message}", "GetCollections", ex.Message);

                return Problem(title: "/Collection/GetCollections", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }

        /// <summary>
        /// Gets the Collections
        /// </summary>
        /// <returns>List of Collection records</returns>
        /// <response code="200">List of Collection records</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet()]
        [Route("GetMyCollections/{authorId:int}")]
        [ProducesResponseType(typeof(List<Collection>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMyCollections(int authorId)
        {
            try
            {
                var result = await _db.RetrieveMyCollections(authorId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: {Method}, Exception: {Message}", "GetMyCollections", ex.Message);

                return Problem(title: "/Collection/GetMyCollections", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }


        /// <summary>
        /// Gets a Collection record based in the primary key id
        /// </summary>
        /// <param name="collectionId">Primary Key</param>
        /// <returns>Collection</returns>
        /// <response code="200">Collection</response>
        /// <response code="404">Record not found</response>
        [HttpGet()]
        [Route("GetCollection/{collectionId:int}")]
        [ProducesResponseType(typeof(Collection), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCollection(int collectionId)
        {
            try
            {
                var result = await _db.RetrieveCollection(collectionId);

                if (result == null)
                    throw new PostgreSql.RecordNotFound("Collection not found");

                return Ok(result);
            }
            catch (PostgreSql.RecordNotFound nf)
            {
                _logger.LogError("Method: {Method}, Exception: {Message}", "GetCollection", nf.Message);

                return NotFound(nf.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: {Method}, Exception: {Message}", "GetCollection", ex.Message);

                return Problem(title: "/Collection/GetCollection", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Gets a Collection record by name
        /// </summary>
        /// <param name="name">Name</param>
        /// <returns>Collection</returns>
        /// <response code="200">Collection</response>
        /// <response code="404">Record not found</response>
        [HttpGet()]
        [Route("GetCollectionByName/{name}")]
        [ProducesResponseType(typeof(Collection), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCollectionByName(string name)
        {
            try
            {
                var result = await _db.RetrieveCollectionByName(name);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: {Method}, Exception: {Message}", "GetCollection", ex.Message);

                return Problem(title: "/Collection/GetCollectionByName", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }



        /// <summary>
        /// Gets a Collection banner based in the primary key id
        /// </summary>
        /// <param name="collectionId">Primary Key</param>
        /// <returns>Collection</returns>
        /// <response code="200">Category Banner</response>
        /// <response code="404">Record not found</response>
        [HttpGet()]
        [Route("GetCollectionBanner/{collectionId:int}")]
        [ProducesResponseType(typeof(ImageBox), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCollectionBanner(int collectionId)
        {
            try
            {
                var result = await _db.RetrieveCollectionBanner(collectionId);

                if (result == null)
                    throw new PostgreSql.RecordNotFound("Collection banner not found");

                return Ok(result);
            }
            catch (PostgreSql.RecordNotFound nf)
            {
                _logger.LogError("Method: {Method}, Exception: {Message}", "GetCollectionBanner", nf.Message);

                return NotFound(nf.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: {Method}, Exception: {Message}", "GetCollectionBanner", ex.Message);

                return Problem(title: "/Collection/GetCollectionBanner", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }


        /// <summary>
        /// Gets a Collection image based in the primary key id
        /// </summary>
        /// <param name="collectionId">Primary Key</param>
        /// <returns>Category</returns>
        /// <response code="200">Collection Image</response>
        /// <response code="404">Record not found</response>
        [HttpGet()]
        [Route("GetCollectionImage/{collectionId:int}")]
        [ProducesResponseType(typeof(ImageBox), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCollectionImage(int collectionId)
        {
            try
            {
                var result = await _db.RetrieveCollectionImage(collectionId);

                if (result == null)
                    throw new PostgreSql.RecordNotFound("Collection image not found");

                return Ok(result);
            }
            catch (PostgreSql.RecordNotFound nf)
            {
                _logger.LogError("Method: {Method}, Exception: {Message}", "GetCategoryImage", nf.Message);

                return NotFound(nf.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: {Method}, Exception: {Message}", "GetCategoryImage", ex.Message);

                return Problem(title: "/Collection/GetCategoryImage", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }


        /// <summary>
        /// Add a Collection record
        /// </summary>
        /// <param name="record">Collection</param>
        /// <returns>Collection</returns>
        /// <response code="200">Collection</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost()]
        [Route("PostCollection")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostCollection([FromBody]Collection record)
        {
            try
            {
               await _db.CreateCollection(record);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: {Method}, Exception: {Message}", "PostCollection", ex.Message);

                return Problem(title: "/Collection/PostCollection", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }


        /// <summary>
        /// Update a Collection record
        /// </summary>
        /// <param name="record">Collection</param>
        /// <returns></returns>
        /// <response code="200"></response>
        /// <response code="404">Not Found</response>
        [HttpPut()]
        [Route("PutCollection")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PutCollection([FromBody]Collection record)
        {
            try
            {
               await _db.UpdateCollection(record);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: {Method}, Exception: {Message}", "PutCollection", ex.Message);

                return Problem(title: "/Collection/PutCollection", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }


        /// <summary>
        /// Delete a Collection record
        /// </summary>
        /// <param name="collectionId">Primary Key</param>
        /// <returns></returns>
        /// <response code="200"></response>
        /// <response code="404">Not Found</response>
        [HttpDelete()]
        [Route("DeleteCollection/{collectionId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteCollection(int collectionId)
        {
            try
            {
                await _db.DeleteCollection(collectionId);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: {Method}, Exception: {Message}", "DeleteCollection", ex.Message);

                return Problem(title: "/Collection/DeleteCollection", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }

        /// <summary>
        /// Gets the Trending Collections
        /// </summary>
        /// <returns>List of Collection records</returns>
        /// <response code="200">List of Collection records</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet()]
        [Route("GetTrendingCollections")]
        [ProducesResponseType(typeof(List<Collection>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTrendingCollections()
        {
            try
            {
                var result = await _db.GetTrendingCollections();

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: {Method}, Exception: {Message}", "GetTrendingCollections", ex.Message);

                return Problem(title: "/Collection/GetTrendingCollections", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }


    }
}

