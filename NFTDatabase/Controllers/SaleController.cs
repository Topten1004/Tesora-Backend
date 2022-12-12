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
    /// Cart Database Controller
    /// </summary>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class SaleController : ControllerBase
    {
        private readonly IPostgreSql _db;
        private readonly ILogger<SaleController> _logger;

        /// <summary>
        /// Dependency Injection Contstructor
        /// </summary>
        /// <param name="db">Database Singleton</param>
        /// <param name="logger">Logger</param>
        public SaleController(IPostgreSql db, ILogger<SaleController> logger)
        {
            _db = db;
            _logger = logger;
        }



        /// <summary>
        /// Add a Sale Order record
        /// </summary>
        /// <param name="record">Sale</param>
        /// <returns>Cart</returns>
        /// <response code="200">Sale</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost()]
        [Route("PostSaleOrder")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostSaleOrder([FromBody] Sale record)
        {
            try
            {
                await _db.CreateSalesOrder(record);

                return Ok("Sale order added");
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: {Method}, Exception: {Message}", "PostSaleOrder", ex.Message);

                return Problem(title: "/Cart/PostSaleOrder", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }


        /// <summary>
        /// Update sales order status
        /// </summary>
        /// <param name="address">Crypto deposit address1</param>
        /// <param name="paymentStatus"></param>
        /// <returns></returns>
        /// <response code="200"></response>
        /// <response code="404">Not Found</response>
        [HttpPut()]
        [Route("UpdateSalesOrderStatus/{address}/{paymentStatus}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateSalesOrderStatus(string address, string paymentStatus)
        {
            try
            {
                var status = (Sale.PaymentStatuses)Enum.Parse(typeof(Sale.PaymentStatuses), paymentStatus);

                await _db.UpdateSalesOrderStatus(address, status);

                return Ok("Sales order status updated");
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: {Method}, Exception: {Message}", "UpdateSalesOrderStatus", ex.Message);

                return Problem(title: "/Cart/UpdateSalesOrderStatus", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }


    }
}

