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
    /// Favourite Database Controller
    /// </summary>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class FavouriteController : ControllerBase
    {
        private readonly IPostgreSql _db;
        private readonly ILogger<FavouriteController> _logger;

        /// <summary>
        /// Dependency Injection Contstructor
        /// </summary>
        /// <param name="db">Database Singleton</param>
        /// <param name="logger">Logger</param>
        public FavouriteController(IPostgreSql db, ILogger<FavouriteController> logger)
        {
            _db = db;
            _logger = logger;
        }


        /// <summary>
        /// Gets the Favourites
        /// </summary>
        /// <returns>List of Favourite records</returns>
        /// <response code="200">List of Favourite records</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet()]
        [Route("GetFavourites/{userId:int}")]
        [ProducesResponseType(typeof(List<Favourite>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetFavourites(int userId)
        {
            try
            {
                var result = await _db.RetrieveFavourites(userId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                var msg = $"Method: GetFavourites, Exception: {ex.Message}";

                _logger.LogError(msg);

                return Problem(title: "/Favourite/GetFavourites", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }


        /// <summary>
        /// Gets a Favourite record based in the primary key id
        /// </summary>
        /// <param name="favouriteId">Primary Key</param>
        /// <returns>Favourite</returns>
        /// <response code="200">Favourite</response>
        /// <response code="404">Record not found</response>
        [HttpGet()]
        [Route("GetFavourite/{favouriteId:int}")]
        [ProducesResponseType(typeof(Favourite), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetFavourite(int favouriteId)
        {
            try
            {
                var result = await _db.RetrieveFavourite(favouriteId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                var msg = $"Method: GetFavourite, Exception: {ex.Message}";

                _logger.LogError(msg);

                return Problem(title: "/Favourite/GetFavourite", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }


        /// <summary>
        /// Add a Favourite record
        /// </summary>
        /// <param name="record">Favourite</param>
        /// <returns>Favourite</returns>
        /// <response code="200">Favourite</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost()]
        [Route("PostFavourite")]
        [ProducesResponseType(typeof(string),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostFavourite([FromBody]Favourite record)
        {
            try
            {
                var exists = await _db.CreateFavourite(record);
                var msg = exists ? "Favourite Exists" : "Favourite Added";
                
                return Ok(msg);
            }
            catch (Exception ex)
            {
                var msg = $"Method: PostFavourite, Exception: {ex.Message}";

                _logger.LogError(msg);

                return Problem(title: "/Favourite/PostFavourtie", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }


        /// <summary>
        /// Update a Favourite record
        /// </summary>
        /// <param name="record">Favourite</param>
        /// <returns></returns>
        /// <response code="200"></response>
        /// <response code="404">Not Found</response>
        [HttpPut()]
        [Route("PutFavourite")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutFavourite([FromBody]Favourite record)
        {
            try
            {
               await _db.UpdateFavourite(record);

                return Ok("Favourtie Updated");
            }
            catch (Exception ex)
            {
                var msg = $"Method: PutFavourite, Exception: {ex.Message}";

                _logger.LogError(msg);

                return Problem(title: "/Favourite/PutFavourite", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }


        /// <summary>
        /// Delete a Favourite record
        /// </summary>
        /// <param name="favouriteId">Primary Key</param>
        /// <returns></returns>
        /// <response code="200"></response>
        /// <response code="404">Not Found</response>
        [HttpDelete()]
        [Route("DeleteFavourite/{favouriteId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteFavourite(int favouriteId)
        {
            try
            {
                await _db.DeleteFavourite(favouriteId);

                return Ok("Favourite Deleted");
            }
            catch (Exception ex)
            {
                var msg = $"Method: DeleteFavourite, Exception: {ex.Message}";

                _logger.LogError(msg);

                return Problem(title: "/Favourite/DeleteFavourite", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }

        /// <summary>
        /// Remove a Favourite record
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <param name="itemId">Item Id</param>
        /// <returns></returns>
        /// <response code="200"></response>
        /// <response code="404">Not Found</response>
        [HttpDelete()]
        [Route("RemoveFavourite/{userId}/{itemId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveFavourite(int userId, int itemId)
        {
            try
            {
                await _db.RemoveFavourite(userId, itemId);

                return Ok("Favourite Removed");
            }
            catch (Exception ex)
            {
                var msg = $"Method: RemoveFavourite, Exception: {ex.Message}";

                _logger.LogError(msg);

                return Problem(title: "/Favourite/RemoveFavourite", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }


        /// <summary>
        /// Gets my favorites
        /// </summary>
        /// <returns>List of my favorites</returns>
        /// <response code="200">List of my favorites</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet()]
        [Route("GetMyFavorites/{userId::int}")]
        [ProducesResponseType(typeof(List<FavoriteCollectionItemCategory>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMyFavorites(int userId)
        {
            try
            {
                var result = await _db.RetrieveMyFavorites(userId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                var msg = $"Method: GetMyFavorites, Exception: {ex.Message}";

                _logger.LogError(msg);

                return Problem(title: "/Favourite/GetMyFavorites", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }

    }
}

