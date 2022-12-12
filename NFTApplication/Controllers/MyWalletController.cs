// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//
using System.Numerics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using NFTWalletService;
using NFTApplication.Models.MyWallet;
using NFTApplication.Utility;
using Nethereum.Web3;
using NFTApplication.Models.MyCollection;
using FluentValidation;

namespace NFTApplication.Controllers
{

    /// <summary>
    /// MyWallet Controller
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class MyWalletController : ControllerBase
    {
        private readonly INFTWalletService _wallet;
        private readonly ILogger<MyWalletController> _logger;
        private readonly string _blockchainNode;
        private readonly string _blockchainType;
        private readonly bool _useTestWallet;
        private readonly string? _testWalletPublicAddress;
        private readonly string? _testWalletPrivateAddress;
        private readonly string _blockchainNodeAndKey;


        /// <summary>
        /// Dependency Injection Contstructor
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="wallet">Database Singleton</param>
        /// <param name="logger">Logger</param>
        public MyWalletController(IConfiguration configuration, INFTWalletService wallet, ILogger<MyWalletController> logger)
        {
            _wallet = wallet;
            _logger = logger;

            var prefix = configuration["Environment:Prefix"];
            _blockchainNode = $"{configuration[$"BlockchainNode:{prefix}Node"]}";
            _blockchainType = $"{configuration[$"BlockchainNode:{prefix}Type"]}";
            _blockchainNodeAndKey = $"{configuration[$"BlockchainNode:{prefix}Node"]}{configuration[$"BlockchainNode:{prefix}Key"]}";

            _useTestWallet = Convert.ToBoolean(configuration["TestWallet:UseTestWallet"]);
            _testWalletPublicAddress = configuration["TestWallet:PublicKey"];
            _testWalletPrivateAddress = configuration["TestWallet:PrivateKey"];
        }


        /// <summary>
        /// Get QR Code Image
        /// </summary>
        /// <returns>QR Code Image</returns>
        /// <response code="200">QR Code Image</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet()]
        [Route("GetQrCode/{address}")]
        [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetQrCode(string address)
        {
            try
            {
                var qrBox = await _wallet.GetQrCode(address);

                if (qrBox != null && qrBox.Data != null && qrBox.Type != null)
                    return File(qrBox.Data, qrBox.Type);
                else
                    return NotFound("Unable to create qr code");
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: {Method}, Exception: {Message}", "GetQrCode", ex.Message);

                return Problem(title: "/MyWallet/GetQrCode", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }


        /// <summary>
        /// MyWallet Action: Receive coins into wallet
        /// </summary>
        /// <returns>ReceiveCoinsResponse</returns>
        /// <response code="200">ReceiveCoinsResponse</response>
        /// <response code="500">Internal Server Error</response>
        [Authorize]
        [HttpGet()]
        [Route("ReceiveCoins")]
        [ProducesResponseType(typeof(ReceiveCoinsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ReceiveCoins()
        {
            try
            {
                // Get the users wallet, they have to pay to create the collection, my test wallet address
                var myAddress = _testWalletPublicAddress;
                if (!_useTestWallet)
                {
                    // Determine the logged in user
                    var masterUserId = HttpContextClaims.GetMasterUserId(HttpContext);
                    var cryptoWallet = await _wallet.GetSignature(masterUserId);
                    myAddress = cryptoWallet.Address;
                }

                if (myAddress == null)
                    throw new ArgumentException("Missing wallet address");

                var balance = await _wallet.GetBalanceForAddress(myAddress);

                var result = new ReceiveCoinsResponse
                {
                    Network = _blockchainNode,
                    Address = myAddress,
                    Balance = balance.Eth,
                    Symbol = _blockchainType,
                    QrCode = $"/api/v1/MyWallet/GetQrCode/{myAddress}"
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: {Method}, Exception: {Message}", "ReceiveCoins", ex.Message);

                return Problem(title: "/MyWallet/ReceiveCoins", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }


        /// <summary>
        /// MyWallet Action: Send coins to another wallet
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <response code="200"></response>
        /// <response code="500">Internal Server Error</response>
        [Authorize]
        [HttpPost()]
        [Route("SendCoins")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SendCoins([FromBody] SendCoinsRequest request)
        {
            try
            {
                // Validations
                var validator = new SendCoinsRequestValidator();
                await validator.ValidateAndThrowAsync(request);


                // Get the users wallet, they have to pay to create the collection, my test wallet address
                var myAddress = _testWalletPublicAddress;
                var myAccount = _testWalletPrivateAddress;
                if (!_useTestWallet)
                {
                    var masterUserId = HttpContextClaims.GetMasterUserId(HttpContext);
                    var cryptoWallet = await _wallet.GetSignature(masterUserId);
                    myAddress = cryptoWallet.Address;
                    myAccount = cryptoWallet.Value;
                }

                // Create the account object to work with
                var account = new Nethereum.Web3.Accounts.Account(myAccount);

                // Get an instance to the network node
                var web3 = new Web3(account, _blockchainNodeAndKey);

                var gas = (BigInteger)request.GasWei;

                var receipt = await web3.Eth.GetEtherTransferService().TransferEtherAndWaitForReceiptAsync(request.ToAddress, request.AmountEth, null, gas);

                return Ok($"Sent coins - {receipt.TransactionHash}");
            }
            catch (ValidationException ve)
            {
                var errors = new List<string>();

                foreach (var item in ve.Errors)
                    errors.Add(item.ErrorMessage);

                return BadRequest(errors);
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: {Method}, Exception: {Message}", "SendCoins", ex.Message);

                return Problem(title: "/MyWallet/SendCoins", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }

        /// <summary>
        /// MyWallet Query: Determine gas fee for sending coins
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <response code="200"></response>
        /// <response code="500">Internal Server Error</response>
        [Authorize]
        [HttpPost()]
        [Route("SendCoinsGas")]
        [ProducesResponseType(typeof(decimal), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SendCoinsGas([FromBody] SendCoinsRequest request)
        {
            try
            {
                // Validations
                var validator = new SendCoinsRequestValidator();
                await validator.ValidateAndThrowAsync(request);


                // Get the users wallet, they have to pay to create the collection, my test wallet address
                var myAddress = _testWalletPublicAddress;
                var myAccount = _testWalletPrivateAddress;
                if (!_useTestWallet)
                {
                    var masterUserId = HttpContextClaims.GetMasterUserId(HttpContext);
                    var cryptoWallet = await _wallet.GetSignature(masterUserId);
                    myAddress = cryptoWallet.Address;
                    myAccount = cryptoWallet.Value;
                }

                // Create the account object to work with
                var account = new Nethereum.Web3.Accounts.Account(myAccount);

                // Get an instance to the network node
                var web3 = new Web3(account, _blockchainNodeAndKey);

                var gasWei = await web3.Eth.GetEtherTransferService().EstimateGasAsync(request.ToAddress, request.AmountEth);

                return Ok((decimal)gasWei);
            }
            catch (ValidationException ve)
            {
                var errors = new List<string>();

                foreach (var item in ve.Errors)
                    errors.Add(item.ErrorMessage);

                return BadRequest(errors);
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: {Method}, Exception: {Message}", "SendCoinsGas", ex.Message);

                return Problem(title: "/MyWallet/SendCoinsGas", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }

    }
}

