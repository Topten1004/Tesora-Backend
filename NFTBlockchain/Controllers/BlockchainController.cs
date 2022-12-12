using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NFTBlockchain.Services;
using NFTBlockchain.Models;
using Microsoft.AspNetCore.Authorization;

namespace NFTBlockchain.Controllers
{
    /// <summary>
    /// Blockchain Controller
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class BlockchainController : ControllerBase
    {
        private readonly IAlchemyService _alchemyService;
        private readonly ILogger<BlockchainController> _logger;

        /// <summary>
        /// Dependency Injection Contstructor
        /// </summary>
        /// <param name="logger">Logger</param>
        public BlockchainController(ILogger<BlockchainController> logger, IAlchemyService alchemyService)
        {
            _logger = logger;
            _alchemyService = alchemyService;
        }


        /// <summary>
        /// Gets the NFTs for this owner
        /// </summary>
        /// <param name="owner"></param>
        /// <returns>List of NFT records</returns>
        /// <response code="200">List of NFT records</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet()]
        [Route("GetNftsForOwner/{owner}")]
        [ProducesResponseType(typeof(List<GetNftsForOwnerResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetNftsForOwner(string owner)
        {
            try
            {
                var nfts = await _alchemyService.GetNfsForOwner(owner);

                var response = new List<GetNftsForOwnerResponse>();

                if (nfts.OwnedNfts != null)
                {
                    foreach (var nft in nfts.OwnedNfts)
                    {
                        response.Add(new GetNftsForOwnerResponse
                        {
                            CollectionName = nft.ContractMetadata?.Name,
                            TokenName = nft.Metadata?.Name,
                            TokenIpfs = nft.Metadata?.Image
                        });
                    }
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: {Method}, Exception: {Message}", "GetNftsForOwner", ex.Message);

                return Problem(title: "/Blockchain/GetNftsForOwner", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }


    }
}
