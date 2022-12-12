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
    /// User Database Controller
    /// </summary>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IPostgreSql _db;
        private readonly ILogger<UserController> _logger;

        /// <summary>
        /// Dependency Injection Contstructor
        /// </summary>
        /// <param name="db">Database Singleton</param>
        /// <param name="logger">Logger</param>
        public UserController(IPostgreSql db, ILogger<UserController> logger)
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
        [ProducesResponseType(typeof(List<User>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var result = await _db.RetrieveUsers();

                return Ok(result);
            }
            catch (Exception ex)
            {
                var msg = $"Method: GetUsers, Exception: {ex.Message}";

                _logger.LogError(msg);

                return Problem(title: "/User/GetUsers", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }


        /// <summary>
        /// Gets a User record based in the primary key id
        /// </summary>
        /// <param name="userId">Primary Key</param>
        /// <returns>User</returns>
        /// <response code="200">User</response>
        /// <response code="404">Record not found</response>
        [HttpGet()]
        [Route("GetUser/{userId:int}")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUser(int userId)
        {
            try
            {
                var result = await _db.RetrieveUser(userId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                var msg = $"Method: GetUser, Exception: {ex.Message}";

                _logger.LogError(msg);

                return Problem(title: "/User/GetUser", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }


        /// <summary>
        /// Gets a User record based in the Master Id
        /// </summary>
        /// <param name="masterUserId">Master User Id</param>
        /// <returns>User</returns>
        /// <response code="200">User</response>
        /// <response code="404">Record not found</response>
        [HttpGet()]
        [Route("GetUser/MasterId/{masterUserId}")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserMasterId(string masterUserId)
        {
            try
            {
                var result = await _db.RetrieveUserMasterId(masterUserId);

                if (result == null)
                    throw new ArgumentException("User is not found");

                return Ok(result);
            }
            catch (Exception ex)
            {
                var msg = $"Method: GetUserMasterId, Exception: {ex.Message}";

                _logger.LogError(msg);

                return Problem(title: "/User/GetUserMasterId", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }


        /// <summary>
        /// Add a User record
        /// </summary>
        /// <param name="record">User</param>
        /// <returns>User</returns>
        /// <response code="200">User</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost()]
        [Route("PostUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostUser([FromBody]User record)
        {
            try
            {
               await _db.CreateUser(record);

                return Ok();
            }
            catch (Exception ex)
            {
                var msg = $"Method: PostUser, Exception: {ex.Message}";

                _logger.LogError(msg);

                return Problem(title: "/User/PostUser", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }


        /// <summary>
        /// Update a User record
        /// </summary>
        /// <param name="record">User</param>
        /// <returns></returns>
        /// <response code="200"></response>
        /// <response code="404">Not Found</response>
        [HttpPut()]
        [Route("PutUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutUser([FromBody]User record)
        {
            try
            {
               await _db.UpdateUser(record);

                return Ok();
            }
            catch (Exception ex)
            {
                var msg = $"Method: PutUser, Exception: {ex.Message}";

                _logger.LogError(msg);

                return Problem(title: "/User/PutUser", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }


        /// <summary>
        /// Delete a User record
        /// </summary>
        /// <param name="userId">Primary Key</param>
        /// <returns></returns>
        /// <response code="200"></response>
        /// <response code="404">Not Found</response>
        [HttpDelete()]
        [Route("DeleteUser/{userId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            try
            {
                await _db.DeleteUser(userId);

                return Ok();
            }
            catch (Exception ex)
            {
                var msg = $"Method: DeleteUser, Exception: {ex.Message}";

                _logger.LogError(msg);

                return Problem(title: "/User/DeleteUser", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }

        /// <summary>
        /// User Exists
        /// </summary>
        /// <param name="masterUserId">Master User Id</param>
        /// <returns></returns>
        /// <response code="200"></response>
        /// <response code="404">Not Found</response>
        [HttpGet()]
        [Route("UserExists/{masterUserId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UserExists(string masterUserId)
        {
            try
            {
                var exists = await _db.UserExists(masterUserId);

                return Ok(new {exists});
            }
            catch (Exception ex)
            {
                var msg = $"Method: UserExists, Exception: {ex.Message}";

                _logger.LogError(msg);

                return Problem(title: "/User/UserExists", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }

        /// <summary>
        /// Gets a Users image based in the primary key id
        /// </summary>
        /// <param name="userId">Primary Key</param>
        /// <returns>Category</returns>
        /// <response code="200">User Image</response>
        /// <response code="404">Record not found</response>
        [HttpGet()]
        [Route("GetUserImage/{userId:int}")]
        [ProducesResponseType(typeof(ImageBox), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserImage(int userId)
        {
            try
            {
                var result = await _db.RetrieveUserImage(userId);

                if (result == null)
                    throw new PostgreSql.RecordNotFound("User image not found");

                return Ok(result);
            }
            catch (PostgreSql.RecordNotFound nf)
            {
                _logger.LogError("Method: {Method}, Exception: {Message}", "GetUserImage", nf.Message);

                return NotFound(nf.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: {Method}, Exception: {Message}", "GetUserImage", ex.Message);

                return Problem(title: "/User/GetUserImage", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }


        /// <summary>
        /// Update a User image
        /// </summary>
        /// <param name="userImage">Image Information</param>
        /// <returns></returns>
        /// <response code="200"></response>
        /// <response code="404">Not Found</response>
        [HttpPut()]
        [Route("PutUserImage")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutUserImage([FromBody] UserImage userImage)
        {
            try
            {
                if (userImage.UserId == null)
                    throw new ArgumentException("User Id must be suppied");
                if (userImage.Image == null)
                    throw new ArgumentException("Image must be defined");

                var record = new User
                {
                    UserId = (int)userImage.UserId,
                    ProfileImage = userImage.Image.Data,
                    ProfileImageType = userImage.Image.Type
                };

                await _db.UpdateUserImage(record);

                return Ok("User Image Updated");
            }
            catch (Exception ex)
            {
                var msg = $"Method: PutUserImage, Exception: {ex.Message}";

                _logger.LogError(msg);

                return Problem(title: "/User/PutUserImage", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }


    }
}

