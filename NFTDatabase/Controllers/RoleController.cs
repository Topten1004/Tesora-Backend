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
    /// Role Database Controller
    /// </summary>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly IPostgreSql _db;
        private readonly ILogger<RoleController> _logger;

        /// <summary>
        /// Dependency Injection Contstructor
        /// </summary>
        /// <param name="db">Database Singleton</param>
        /// <param name="logger">Logger</param>
        public RoleController(IPostgreSql db, ILogger<RoleController> logger)
        {
            _db = db;
            _logger = logger;
        }


        /// <summary>
        /// Gets the Roles
        /// </summary>
        /// <returns>List of Role records</returns>
        /// <response code="200">List of Role records</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet()]
        [Route("GetRoles")]
        [ProducesResponseType(typeof(List<Role>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetRoles()
        {
            try
            {
                var result = await _db.RetrieveRoles();

                return Ok(result);
            }
            catch (Exception ex)
            {
                var msg = $"Method: GetRoles, Exception: {ex.Message}";

                _logger.LogError(msg);

                return Problem(title: "/Role/GetRoles", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }


        /// <summary>
        /// Gets a Role record based in the primary key id
        /// </summary>
        /// <param name="roleId">Primary Key</param>
        /// <returns>Role</returns>
        /// <response code="200">Role</response>
        /// <response code="404">Record not found</response>
        [HttpGet()]
        [Route("GetRole/{roleId:int}")]
        [ProducesResponseType(typeof(Role), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetRole(int roleId)
        {
            try
            {
                var result = await _db.RetrieveRole(roleId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                var msg = $"Method: GetRole, Exception: {ex.Message}";

                _logger.LogError(msg);

                return NotFound(ex.Message);
            }

        }


        /// <summary>
        /// Add a Role record
        /// </summary>
        /// <param name="record">Role</param>
        /// <returns>Role</returns>
        /// <response code="200">Role</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost()]
        [Route("PostRole")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostRole([FromBody]Role record)
        {
            try
            {
               await _db.CreateRole(record);

                return Ok();
            }
            catch (Exception ex)
            {
                var msg = $"Method: PostRole, Exception: {ex.Message}";

                _logger.LogError(msg);

                return Problem(title: "/Role/PostRole", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }


        /// <summary>
        /// Update a Role record
        /// </summary>
        /// <param name="record">Role</param>
        /// <returns></returns>
        /// <response code="200"></response>
        /// <response code="404">Not Found</response>
        [HttpPut()]
        [Route("PutRole")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutRole([FromBody]Role record)
        {
            try
            {
               await _db.UpdateRole(record);

                return Ok();
            }
            catch (Exception ex)
            {
                var msg = $"Method: PutRole, Exception: {ex.Message}";

                _logger.LogError(msg);

                return NotFound(ex.Message);
            }

        }


        /// <summary>
        /// Delete a Role record
        /// </summary>
        /// <param name="roleId">Primary Key</param>
        /// <returns></returns>
        /// <response code="200"></response>
        /// <response code="404">Not Found</response>
        [HttpDelete()]
        [Route("DeleteRole/{roleId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteRole(int roleId)
        {
            try
            {
                await _db.DeleteRole(roleId);

                return Ok();
            }
            catch (Exception ex)
            {
                var msg = $"Method: DeleteRole, Exception: {ex.Message}";

                _logger.LogError(msg);

                return NotFound(ex.Message);
            }

        }
    }
}

