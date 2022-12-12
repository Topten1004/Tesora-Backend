// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using NFTDatabaseService;
using NFTWalletService;
using NFTApplication.Models.Profile;
using NFTApplication.Utility;
using NFTApplication.Models.MyCollection;
using Org.BouncyCastle.Asn1.Ocsp;
using NFTDatabaseEntities;

namespace NFTApplication.Controllers
{

    /// <summary>
    /// Profile Page Controller
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class MyProfileController : ControllerBase
    {
        private readonly INFTDatabaseService _db;
        private readonly INFTWalletService _wallet;
        private readonly ILogger<MyProfileController> _logger;

        /// <summary>
        /// Dependency Injection Contstructor
        /// </summary>
        /// <param name="db">Database Singleton</param>
        /// <param name="wallet"></param>
        /// <param name="logger">Logger</param>
        public MyProfileController(INFTDatabaseService db, INFTWalletService wallet, ILogger<MyProfileController> logger)
        {
            _db = db;
            _wallet = wallet;   
            _logger = logger;
        }


        /// <summary>
        /// MyProfile Page View Model
        /// </summary>
        /// <returns>Profile View Model</returns>
        /// <response code="200">Profile View Model</response>
        /// <response code="500">Internal Server Error</response>
        [Authorize]
        [HttpGet()]
        [Route("GetUser")]
        [ProducesResponseType(typeof(ProfileUserViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUser()
        {
            try
            {
                // Determine the logged in user
                var masterUserId = HttpContextClaims.GetMasterUserId(HttpContext);
                var user = await _db.GetUserMasterId(masterUserId);

                var wallet = await _wallet.GetWallet(masterUserId);

                var result = new ProfileUserViewModel
                {
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserId = user.UserId,
                    UserName = user.Username,
                    WalletAddress = wallet.DepositAddress,
                    WalletQrCode = $"/api/v1/MyWallet/GetQrCode/{wallet.DepositAddress}",
                    CreateDate = user.CreateDate,
                    UserImage = $"/api/v1/MyProfile/GetUserImage/{user.UserId}"
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: {Method}, Exception: {Message}", "GetUser", ex.Message);

                return Problem(title: "/MyProfile/GetUser", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }

        /// <summary>
        /// Profile Page View Model
        /// </summary>
        /// <returns>Profile View Model</returns>
        /// <response code="200">Profile View Model</response>
        /// <response code="500">Internal Server Error</response>
        [Authorize]
        [HttpGet()]
        [Route("GetProfile/{userId}")]
        [ProducesResponseType(typeof(ProfileUserViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetProfile(int userId)
        {
            try
            {
                // Get the user information from the database, they are the author of this collection
                var user = await _db.GetUser(userId);

                var wallet = await _wallet.GetWallet(user.MasterUserId);

                var result = new ProfileUserViewModel
                {
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserId = user.UserId,
                    UserName = user.Username,
                    WalletAddress = wallet.DepositAddress,
                    WalletQrCode = $"/api/v1/MyWallet/GetQrCode/{wallet.DepositAddress}",
                    UserImage = $"/api/v1/MyProfile/GetUserImage/{userId}"
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: {Method}, Exception: {Message}", "GetProfile", ex.Message);

                return Problem(title: "/MyProfile/GetProfile", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }

        /// <summary>
        /// Get User Image
        /// </summary>
        /// <returns>User Image</returns>
        /// <response code="200">User Image</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet()]
        [Route("GetUserImage/{UserId:int}")]
        [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserImage(int UserId)
        {
            try
            {
                // Get user info
                var imageBox = await _db.GetUserImage(UserId);

                if (imageBox != null && imageBox.Data != null && imageBox.Type != null)
                    return File(imageBox.Data, imageBox.Type);
                else
                    return NotFound("User does not have an Image");
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: {Method}, Exception: {Message}", "GetUserImage", ex.Message);

                return Problem(title: "/MyProfile/GetUserImage", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Update users image
        /// </summary>
        /// <param name="userImage"></param>
        /// <returns></returns>
        [HttpPut()]
        [Route("PutUserImage")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PutUserImage(IFormFile userImage)
        {
            try
            {
                // Get the current user
                var masterUserId = HttpContextClaims.GetMasterUserId(HttpContext);
                var user = await _db.GetUserMasterId(masterUserId);

                var request = new UserImage
                {
                    UserId = user.UserId,
                    Image = new ImageBox
                    {
                        Data = UploadFileHandler.GetFileContents(userImage),
                        Type = userImage.ContentType
                    }
                };

                await _db.PutUserImage(request);

                return Ok("Image Updated");
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: {Method}, Exception: {Message}", "PutUserImage", ex.Message);

                return Problem(title: "/MyProfile/PutUserImage", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }

    }
}

