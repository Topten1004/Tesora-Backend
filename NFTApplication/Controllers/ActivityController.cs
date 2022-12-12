// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using NFTDatabaseService;
using NFTApplication.Models.Activity;


namespace NFTApplication.Controllers
{

    /// <summary>
    /// Activity Page Controller
    /// </summary>
    /// [MyAuth]
    [Authorize]
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
        /// Gets the Activity View Model
        /// </summary>
        /// <returns>Activity View Model</returns>
        /// <response code="200">Activity View Model</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet()]
        [Route("GetActivityViewModel")]
        [ProducesResponseType(typeof(ActivityViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetActivityViewModel()
        {
            try
            {
                var histroy = await _db.GetHistories();

                var result = new ActivityViewModel
                {
                    Histories = new List<ActivityHistory>
                    {
                        new ActivityHistory
                        {
                            HistoryId = 0,
                            Item = new ActivityItem
                            {
                                ItemId = 0,
                                Name = "Item Name",
                                Media = "?"
                            },
                            Collection = new ActivityCollection
                            {
                                CollectionId = 0,
                                Name = "Collection Name",
                                Image = null
                            },
                            From = new ActivityUser
                            {
                                UserId = 0,
                                UserName = "From User",
                                PublicKey = "public key value"
                            },
                            To = new ActivityUser
                            {
                                UserId = 1,
                                UserName = "To User",
                                PublicKey = "public key value"
                            },
                            HistoryType = ActivityHistory.ActiveHistoryTypes.bid,
                            TransactionHash = "transaction hash value",
                            IsValid = true,
                            Price = 10.00m,
                            CreateDate = DateTime.UtcNow
                         }
                     }
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                var msg = $"Method: GetActivityViewModel, Exception: {ex.Message}";

                _logger.LogError(msg);

                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }

        }

    }
}

