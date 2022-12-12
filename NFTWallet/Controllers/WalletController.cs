// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Nethereum.Web3;
using Nethereum.HdWallet;
using NBitcoin;

using NFTWallet.DataAccess;
using NFTWallet.Models;
using NFTWallet.Engine;
using NFTWalletEntities;
using SkiaSharp.QrCode.Image;

namespace NFTWallet.Controllers
{
    /// <summary>
    /// Wallet Controller
    /// </summary>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class WalletController : Controller
    {
        private readonly IPostgreSql _db;
        private readonly ILogger<WalletController> _logger;
        private readonly string _blockchainNodeAndKey;

        /// <summary>
        /// Dependency Injection Contstructor
        /// </summary>
        /// <param name="db">Database Singleton</param>
        /// <param name="logger">Logger</param>
        /// <param name="configuration"></param>
        public WalletController(IPostgreSql db, ILogger<WalletController> logger, IConfiguration configuration)
        {
            _db = db;
            _logger = logger;

            var prefix = configuration["Environment:Prefix"];
            _blockchainNodeAndKey = $"{configuration[$"BlockchainNode:{prefix}Node"]}{configuration[$"BlockchainNode:{prefix}Key"]}";
        }

        /// <summary>
        /// Gets a QR Code for an address
        /// </summary>
        /// <param name="address">Address</param>
        /// <param name="coin"></param>
        /// <returns>QR Code</returns>
        /// <response code="200">QR Code</response>
        /// <response code="404">Record not found</response>
        [HttpGet()]
        [Route("GetQrCode/{address}/{coin?}")]
        [ProducesResponseType(typeof(QrBox), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public IActionResult GetQrCode(string address, string coin = "eth")
        {
            try
            {
                var result = new QrBox
                {
                    Data = QRCode.GenerateCode(300, 300, address, coin),
                    Type = "image/png"
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                var msg = $"Method: GetQrCode, Exception: {ex.Message}";

                _logger.LogError(msg);

                return Problem(title: "/Wallet/GetQrCode", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }


        /// <summary>
        /// Gets a Wallet
        /// </summary>
        /// <returns>GetWalletResponse</returns>
        /// <response code="200">GetWalletResponse</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet()]
        [Route("GetWallet/{masterUserId}")]
        [ProducesResponseType(typeof(GetWalletResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetWallet(string masterUserId)
        {
            try
            {
                if (await _db.WalletExists(masterUserId) == false)
                    await CreateWallet(masterUserId);

                var record = await _db.RetrieveWalletCore(masterUserId);

                if (record == null)
                    throw new PostgreSql.RecordNotFound("MasterUserId not Found");

                var wallet = new Wallet(record.Topic, record.Value);

                var depositAddress = wallet.GetAddresses().First();

                var response = new GetWalletResponse
                {
                    DepositAddress = depositAddress,
                };

                return Ok(response);
            }
            catch (PostgreSql.RecordNotFound ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                var msg = $"Method: GetWallet, Exception: {ex.Message}";

                _logger.LogError(msg);

                return Problem(title: "/Wallet/GetWallet", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }


        /// <summary>
        /// Get Signature
        /// </summary>
        /// <returns>GetSignatureResponse</returns>
        /// <response code="200">GetSignatureResponse</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet()]
        [Route("GetSignature/{masterUserId}")]
        [ProducesResponseType(typeof(GetSignatureResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSignature(string masterUserId)
        {
            try
            {
                if (await _db.WalletExists(masterUserId) == false)
                    await CreateWallet(masterUserId);

                var record = await _db.RetrieveWalletCore(masterUserId);

                if (record == null)
                    throw new PostgreSql.RecordNotFound("MasterUserId not Found");

                var wallet = new Wallet(record.Topic, record.Value);

                var account = wallet.GetAccount(0);

                var response = new GetSignatureResponse
                {
                    Address = account.Address,
                    Key = account.PublicKey,
                    Value = account.PrivateKey
                };

                return Ok(response);
            }
            catch (PostgreSql.RecordNotFound ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                var msg = $"Method: GetSignature, Exception: {ex.Message}";

                _logger.LogError(msg);

                return Problem(title: "/Wallet/GetSignature", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Get Balance
        /// </summary>
        /// <returns>GetBalanceResponse</returns>
        /// <response code="200">GetBalanceResponse</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet()]
        [Route("GetBalance/{masterUserId}")]
        [ProducesResponseType(typeof(GetBalanceResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBalance(string masterUserId)
        {
            try
            {
                if (await _db.WalletExists(masterUserId) == false)
                    await CreateWallet(masterUserId);

                var record = await _db.RetrieveWalletCore(masterUserId);

                if (record == null)
                    throw new PostgreSql.RecordNotFound("MasterUserId not Found");

                var wallet = new Wallet(record.Topic, record.Value);

                var account = wallet.GetAccount(0);

                // Get an instance to the network node
                var web3 = new Web3(_blockchainNodeAndKey);

                var wei = await web3.Eth.GetBalance.SendRequestAsync(account.Address);
                var eth = Web3.Convert.FromWei(wei);

                var response = new GetBalanceResponse
                {
                    Wei = new HexBigInt { HexValue = wei.HexValue, Value = new HexBitIntValue { IsPowerOfTwo = wei.Value.IsPowerOfTwo, IsEven = wei.Value.IsEven, IsOne = wei.Value.IsOne, IsZero = wei.Value.IsZero, Sign = wei.Value.Sign } },
                    Eth = eth
                };

                return Ok(response);
            }
            catch (PostgreSql.RecordNotFound ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                var msg = $"Method: GetBalance, Exception: {ex.Message}";

                _logger.LogError(msg);

                return Problem(title: "/Wallet/GetBalance", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }


        /// <summary>
        /// Get Balance for Address
        /// </summary>
        /// <returns>GetBalanceResponse</returns>
        /// <response code="200">GetBalanceResponse</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet()]
        [Route("GetBalanceForAddress/{address}")]
        [ProducesResponseType(typeof(GetBalanceResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBalanceForAddress(string address)
        {
            try
            {
                // Get an instance to the network node
                var web3 = new Web3(_blockchainNodeAndKey);

                var wei = await web3.Eth.GetBalance.SendRequestAsync(address);
                var eth = Web3.Convert.FromWei(wei);

                var response = new GetBalanceResponse
                {
                    Wei = new HexBigInt { HexValue = wei.HexValue, Value = new HexBitIntValue { IsPowerOfTwo = wei.Value.IsPowerOfTwo, IsEven = wei.Value.IsEven, IsOne = wei.Value.IsOne, IsZero = wei.Value.IsZero, Sign = wei.Value.Sign } },
                    Eth = eth
                };

                return Ok(response);
            }
            catch (PostgreSql.RecordNotFound ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                var msg = $"Method: GetBalanceForAddress, Exception: {ex.Message}";

                _logger.LogError(msg);

                return Problem(title: "/Wallet/GetBalanceForAddress", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }


        /// <summary>
        /// Add a Wallet for user
        /// </summary>
        /// <param name="request">PostWalletRequest</param>
        /// <returns>PostWalletResponse</returns>
        /// <response code="200">PostWalletResponse</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost()]
        [Route("PostWallet")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostWallet(PostWalletRequest request)
        {
            try
            {
                var msg = "Wallet Exists";

                if (await _db.WalletExists(request.MasterUserId) == false)
                {
                    await CreateWallet(request.MasterUserId);

                    msg = "Wallet Created";
                }

                return Ok(msg);
            }
            catch (Exception ex)
            {
                var msg = $"Method: PostCategory, Exception: {ex.Message}";

                _logger.LogError(msg);

                return Problem(title: "/Wallet/PostWallet", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }


        /// <summary>
        /// Create Wallet
        /// </summary>
        /// <param name="masterUserId"></param>
        /// <returns></returns>
        private async Task CreateWallet(string masterUserId)
        {
            var mnemonic = new Mnemonic(Wordlist.English, WordCount.Twelve);
            var password = Security.GeneratePassword();

            var words = string.Join(" ", mnemonic.Words);

            var record = new WalletCore
            {
                MasterUserId = masterUserId,
                Topic = words,
                Value = password
            };

            await _db.CreateWalletCore(record);
        }
    }
}
