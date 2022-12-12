// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//
using Microsoft.AspNetCore.Mvc;

using NFTDatabaseService;
using NFTApplication.Models.Home;


namespace NFTApplication.Controllers
{

    /// <summary>
    /// Home Page Controller
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly INFTDatabaseService _db;
        private readonly ILogger<HomeController> _logger;

        /// <summary>
        /// Dependency Injection Contstructor
        /// </summary>
        /// <param name="db">Database Singleton</param>
        /// <param name="logger">Logger</param>
        public HomeController(INFTDatabaseService db, ILogger<HomeController> logger)
        {
            _db = db;
            _logger = logger;
        }

        /// <summary>
        /// Server Health Check
        /// </summary>
        /// <returns>Home View Model</returns>
        /// <response code="200">Home View Model</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet()]
        [Route("/")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetHealthCheck()
        {
            return Ok("Server responsive");
        }

        /// <summary>
        /// Get Now Utc time
        /// </summary>
        /// <returns>UTC time</returns>
        /// <response code="200">Now time</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet()]
        [Route("GetNowTime")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public IActionResult GetNowTime()
        {
            try
            {
                return Ok(DateTime.UtcNow);
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: {GetNowTime}, Exception: {Message}", "GetNowTime", ex.Message);

                return Problem(title: "/Home/GetNowTime", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Gets the Home View Model
        /// </summary>
        /// <returns>Home View Model</returns>
        /// <response code="200">Home View Model</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet()]
        [Route("GetHomeViewModel")]
        [ProducesResponseType(typeof(HomeViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetHomeViewModel()
        {
            try
            {
                var collections = await _db.GetTrendingCollections();
                var lstCollections = new List<HomeCollection>();
                foreach(var collection in collections)
                {
                    lstCollections.Add(new HomeCollection
                    {
                        CollectionId = collection.CollectionId,
                        CollectionImage = collection.CollectionImageIpfs,
                        Name = collection.Name
                    });
                }

                var categories = await _db.GetCategories();
                var lstCategories = new List<HomeCategory>();
                foreach(var category in categories)
                {
                    lstCategories.Add(new HomeCategory
                    {
                        CategoryId = category.CategoryId,
                        CategoryImage = $"/api/v1/Category/GetCategoryImage/{category.CategoryId}",
                        Title = category.Title,
                        CreateDate = category.CreateDate,
                        Status = category.Status.ToString()
                    });
                }

                var result = new HomeViewModel
                {
                    Collections = lstCollections,
                    Categories = lstCategories
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: {Method}, Exception: {Message}", "GetHomeViewModel", ex.Message);

                return Problem(title: "/Home/GetHomeViewModel", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }


    }
}

