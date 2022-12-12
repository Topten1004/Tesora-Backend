// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//
using Microsoft.AspNetCore.Mvc;

using NFTDatabaseService;
using NFTDatabaseEntities;
using NFTApplicationAdmin.Models;


namespace NFTApplicationAdmin.Controllers
{

    /// <summary>
    /// Contract Database Controller
    /// </summary>
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ContractController : ControllerBase
    {
        private readonly INFTDatabaseService _db;
        private readonly ILogger<ContractController> _logger;

        /// <summary>
        /// Dependency Injection Contstructor
        /// </summary>
        /// <param name="db">Database Singleton</param>
        /// <param name="logger">Logger</param>
        public ContractController(INFTDatabaseService db, ILogger<ContractController> logger)
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
        [ProducesResponseType(typeof(List<GetContractResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetContracts()
        {
            try
            {
                var records = await _db.GetContracts();

                var response = new List<GetContractResponse>();
                foreach(var record in records)
                {
                    response.Add(new GetContractResponse
                    {
                        ContractByteCode = record.ContractByteCode,
                        ContractId = record.ContractId,
                        ContractInterface = record.ContractInterface,
                        ContractName = record.ContractName,
                        ContractVersion = record.ContractVersion,
                        CreateDate = record.CreateDate
                    });
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                var msg = $"Method: GetContracts, Exception: {ex.Message}";

                _logger.LogError(msg);

                return Problem(title: "/Contract/GetContracts", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
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
        [ProducesResponseType(typeof(GetContractResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetContract(int ContractId)
        {
            try
            {
                var record = await _db.GetContract(ContractId);
                var response = new GetContractResponse
                {
                    ContractByteCode = record.ContractByteCode,
                    ContractId = record.ContractId,
                    ContractInterface = record.ContractInterface,
                    ContractName = record.ContractName,
                    ContractVersion = record.ContractVersion,
                    CreateDate = record.CreateDate
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                var msg = $"Method: GetContract, Exception: {ex.Message}";

                _logger.LogError(msg);

                return NotFound(ex.Message);
            }

        }


        /// <summary>
        /// Add a Contract record
        /// </summary>
        /// <param name="request">Contract</param>
        /// <returns>Contract</returns>
        /// <response code="200">Contract</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost()]
        [Route("CreateContract")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateContract([FromForm] CreateContractRequest request)
        {
            try
            {
                string contractInterface;
                using (var reader = new StreamReader(request.ContractInterface.OpenReadStream()))
                {
                    contractInterface = await reader.ReadToEndAsync();
                }

                string contractByteCode;
                using (var reader = new StreamReader(request.ContractByteCode.OpenReadStream()))
                {
                    contractByteCode = await reader.ReadToEndAsync();
                }

                var record = new Contract
                {
                    ContractId = 0,
                    ContractName = request.ContractName,
                    ContractVersion = request.ContractVersion,
                    ContractInterface = contractInterface,
                    ContractByteCode = contractByteCode,
                    CreateDate = DateTime.UtcNow
                };

                await _db.PostContract(record);

                return Ok("Contract Created");
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
        /// <param name="request">Contract</param>
        /// <returns></returns>
        /// <response code="200"></response>
        /// <response code="404">Not Found</response>
        [HttpPut()]
        [Route("UpdateContract")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateContract([FromForm]UpdateContractRequest request)
        {
            try
            {
                string contractInterface;
                using (var reader = new StreamReader(request.ContractInterface.OpenReadStream()))
                {
                    contractInterface = await reader.ReadToEndAsync();
                }

                string contractByteCode;
                using (var reader = new StreamReader(request.ContractByteCode.OpenReadStream()))
                {
                    contractByteCode = await reader.ReadToEndAsync();
                }

                var record = new Contract
                {
                    ContractId = request.ContractId,
                    ContractName = request.ContractName,
                    ContractVersion = request.ContractVersion,
                    ContractInterface = contractInterface,
                    ContractByteCode = contractByteCode,
                    CreateDate = DateTime.UtcNow
                };

                await _db.PutContract(record);

                return Ok("Contract Updated");
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

                return Ok("Contract Deleted");
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

