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
    /// Contract Database Controller
    /// </summary>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ContractController : ControllerBase
    {
        private readonly IPostgreSql _db;
        private readonly ILogger<ContractController> _logger;

        /// <summary>
        /// Dependency Injection Contstructor
        /// </summary>
        /// <param name="db">Database Singleton</param>
        /// <param name="logger">Logger</param>
        public ContractController(IPostgreSql db, ILogger<ContractController> logger)
        {
            _db = db;
            _logger = logger;
        }


        /// <summary>
        /// Gets the Contracts
        /// </summary>
        /// <returns>List of Contract records</returns>
        /// <response code="200">List of Contract records</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet()]
        [Route("GetContracts")]
        [ProducesResponseType(typeof(List<Contract>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GeContracts()
        {
            try
            {
                var result = await _db.RetrieveContracts();

                return Ok(result);
            }
            catch (Exception ex)
            {
                var msg = $"Method: GetContracts, Exception: {ex.Message}";

                _logger.LogError(msg);

                return Problem(title: "/Contract/RetrieveContracts", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }


        /// <summary>
        /// Gets a Contract record based in the primary key id
        /// </summary>
        /// <param name="ContractId">Primary Key</param>
        /// <returns>Contract</returns>
        /// <response code="200">Contract</response>
        /// <response code="404">Record not found</response>
        [HttpGet()]
        [Route("GetContract/{ContractId:int}")]
        [ProducesResponseType(typeof(Contract), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetContract(int ContractId)
        {
            try
            {
                var result = await _db.RetrieveContract(ContractId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                var msg = $"Method: GetContract, Exception: {ex.Message}";

                _logger.LogError(msg);

                return NotFound(ex.Message);
            }

        }


        /// <summary>
        /// Gets a Contract record based on name and version
        /// </summary>
        /// <param name="name">Contract Name</param>
        /// <param name="ver">Contract Version</param>
        /// <returns>Contract</returns>
        /// <response code="200">Contract</response>
        /// <response code="404">Record not found</response>
        [HttpGet()]
        [Route("GetContractByName/{name}/{ver}")]
        [ProducesResponseType(typeof(Contract), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetContractByName(string name, string ver)
        {
            try
            {
                var result = await _db.RetrieveContractByName(name, ver);

                return Ok(result);
            }
            catch (Exception ex)
            {
                var msg = $"Method: GetContractByName, Exception: {ex.Message}";

                _logger.LogError(msg);

                return NotFound(ex.Message);
            }

        }


        /// <summary>
        /// Add a Contract record
        /// </summary>
        /// <param name="record">Contract</param>
        /// <returns>Contract</returns>
        /// <response code="200">Contract</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost()]
        [Route("PostContract")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostContract([FromBody] Contract record)
        {
            try
            {
                await _db.CreateContract(record);

                return Ok();
            }
            catch (Exception ex)
            {
                var msg = $"Method: PostContract, Exception: {ex.Message}";

                _logger.LogError(msg);

                return Problem(title: "/Contract/PostContract", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }


        /// <summary>
        /// Update a Contract record
        /// </summary>
        /// <param name="record">Contract</param>
        /// <returns></returns>
        /// <response code="200"></response>
        /// <response code="404">Not Found</response>
        [HttpPut()]
        [Route("PutContract")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutContract([FromBody] Contract record)
        {
            try
            {
                await _db.UpdateContract(record);

                return Ok();
            }
            catch (Exception ex)
            {
                var msg = $"Method: PutContract, Exception: {ex.Message}";

                _logger.LogError(msg);

                return NotFound(ex.Message);
            }

        }


        /// <summary>
        /// Delete a Contract record
        /// </summary>
        /// <param name="ContractId">Primary Key</param>
        /// <returns></returns>
        /// <response code="200"></response>
        /// <response code="404">Not Found</response>
        [HttpDelete()]
        [Route("DeleteContract/{ContractId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteContract(int ContractId)
        {
            try
            {
                await _db.DeleteContract(ContractId);

                return Ok();
            }
            catch (Exception ex)
            {
                var msg = $"Method: DeleteContract, Exception: {ex.Message}";

                _logger.LogError(msg);

                return NotFound(ex.Message);
            }

        }
    }
}

