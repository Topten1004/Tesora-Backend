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
using NFTAdminApplication.Utility;

namespace NFTApplicationAdmin.Controllers
{

    /// <summary>
    /// User Database Controller
    /// </summary>
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly INFTDatabaseService _db;
        private readonly ILogger<UserController> _logger;

        /// <summary>
        /// Dependency Injection Contstructor
        /// </summary>
        /// <param name="db">Database Singleton</param>
        /// <param name="logger">Logger</param>
        public UserController(INFTDatabaseService db, ILogger<UserController> logger)
        {
            _db = db;
            _logger = logger;
        }


        /// <summary>
        /// Gets the Users
        /// </summary>
        /// <returns>List of User records</returns>
        /// <response code="200">List of User records</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet()]
        [Route("GetUsers")]
        [ProducesResponseType(typeof(List<GetUserResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var response = new List<GetUserResponse>();
                var embedImage = false;

                var records = await _db.GetUsers();

                foreach (var record in records)
                {
                    response.Add(new GetUserResponse
                    {
                        UserId = record.UserId,
                        UserName = record.Username,
                        FirstName = record.FirstName,
                        LastName = record.LastName,
                        Email = record.Email,
                        ProfileImage = embedImage ? $"data:{record.ProfileImageType}:base64, {Convert.ToBase64String(record.ProfileImage)}"
                                                   : $"/api/v1/User/ProfileImage/{record.MasterUserId}",
                        CreateDate = record.CreateDate,
                        Status = record.Status.ToString(),
                        MasterUserId = record.MasterUserId,
                    });
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                var msg = $"Method: GetUsers, Exception: {ex.Message}";

                _logger.LogError(msg);

                return Problem(title: "/User/GetUsers", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }


        /// <summary>
        /// Gets a User record based in the primary key MasterUserId
        /// </summary>
        /// <param name="MasterUserId">Primary Key</param>
        /// <returns>User</returns>
        /// <response code="200">User</response>
        /// <response code="404">Record not found</response>
        [HttpGet()]
        [Route("GetUser/{MasterUserId:Guid}")]
        [ProducesResponseType(typeof(GetUserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUser(string MasterUserId)
        {
            try
            {
                var record = await _db.GetUserMasterId(MasterUserId);
                var embedImage = false;

                var userProfile = await _db.GetUserImage(record.UserId);

                var response = new GetUserResponse
                {
                    UserId = record.UserId,
                    UserName = record.Username,
                    FirstName = record.FirstName,
                    LastName = record.LastName,
                    Email = record.Email,
                    ProfileImage = embedImage ? $"data:{userProfile.Type}:base64, {Convert.ToBase64String(userProfile.Data)}"
                                                   : $"/api/v1/User/ProfileImage/{record.MasterUserId}",
                    CreateDate = record.CreateDate,
                    Status = record.Status.ToString(),
                    MasterUserId = record.MasterUserId,
                };


                return Ok(response);
            }
            catch (Exception ex)
            {
                var msg = $"Method: GetUser, Exception: {ex.Message}";

                _logger.LogError(msg);

                return Problem(title: "/User/GetUser", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }


        /// <summary>
        /// Create a User record
        /// </summary>
        /// <param name="request">CreateUserRequest</param>
        /// <returns></returns>
        /// <response code="200">User</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost()]
        [Route("CreateUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateUser([FromForm] CreateUserRequest request)
        {
            try
            {
                if (request == null)
                    throw new Exception("Invalid request");

                var image = UploadFileHandler.GetFileContents(request.Image);

                if (image == null)
                    throw new Exception("Invalid image");

                var record = new User
                {
                    Username = request.UserName,
                    Email = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    ProfileImage = image,
                    ProfileImageType = request.Image.ContentType,
                    Status = request.Status,
                    CreateDate = DateTime.UtcNow,
                };

                await _db.PostUser(record);

                return Ok("User Created");
            }
            catch (Exception ex)
            {
                var msg = $"Method: CreateUser, Exception: {ex.Message}";

                _logger.LogError(msg);

                return Problem(title: "/User/CreateUser", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }


        /// <summary>
        /// Update a User record
        /// </summary>
        /// <param name="request">User</param>
        /// <returns></returns>
        /// <response code="200"></response>
        /// <response code="404">Not Found</response>
        [HttpPut()]
        [Route("UpdateUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateUser([FromForm] UpdateUserRequest request)
        {
            try
            {
                if (request == null)
                    throw new Exception("Invalid request");

                var image = UploadFileHandler.GetFileContents(request.Image);

                if (image == null)
                    throw new Exception("Invalid image");

                var record = new User
                {
                    UserId = request.UserId,
                    Username = request.UserName,
                    Email = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    ProfileImage = image,
                    ProfileImageType = request.Image?.ContentType,
                    Status = request.Status,
                    CreateDate = DateTime.UtcNow,
                    MasterUserId = request.MasterUserId
                };

                await _db.PutUser(record);

                return Ok("User Updated");
            }
            catch (Exception ex)
            {
                var msg = $"Method: UpdateUser, Exception: {ex.Message}";

                _logger.LogError(msg);

                return Problem(title: "/User/UpdateUser", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);

            }
        }

        /// <summary>
        /// Get Profile for the user
        /// </summary>
        /// <returns>Profile Image</returns>
        /// <response code="200">Profile Image</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet()]
        [Route("ProfileImage/{UserMasterId:Guid}")]
        [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ProfileImage(string UserMasterId)
        {
            try
            {
                var record = await _db.GetUserMasterId(UserMasterId);

                if (record != null && record.ProfileImage != null && record.ProfileImageType != null)
                {
                    return File(record.ProfileImage, record.ProfileImageType);
                }
                else
                    return NotFound("User does not have a profile");
            }
            catch (Exception ex)
            {
                var msg = $"Method: ProfileImage, Exception: {ex.Message}";

                _logger.LogError(msg);

                return Problem(title: "/User/ProfileImage", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }
    }
}

