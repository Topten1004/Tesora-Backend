// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//
using System.Numerics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using Nethereum.Web3;
using FluentValidation;

using NFTDatabaseService;
using NFTWalletService;
using NFTApplication.Utility;
using NFTApplication.Models.MyOffer;
using NFTDatabaseEntities;
using NFTApplication.Contracts.NFTCollection.ContractDefinition;
using NFTApplication.Contracts.NFTCollection;
using NFTApplication.Services;
using NFTApplication.Models.Collection;
using NFTApplication.Models.Category;

namespace NFTApplication.Controllers
{

    /// <summary>
    /// My Offer Controller
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class MyOfferController : ControllerBase
    {
        private readonly INFTDatabaseService _db;
        private readonly ILogger<MyOfferController> _logger;
        private readonly INFTWalletService _wallet;
        private readonly string _blockchainNodeAndKey;
        private readonly bool _useTestWallet;
        private readonly string? _testWalletPublicAddress;
        private readonly string? _testWalletPrivateAddress;

        /// <summary>
        /// Dependency Injection Contstructor
        /// </summary>
        /// <param name="db">Database Singleton</param>
        /// <param name="wallet"></param>
        /// <param name="configuration"></param>
        /// <param name="logger">Logger</param>
        public MyOfferController(INFTDatabaseService db, INFTWalletService wallet, IConfiguration configuration, ILogger<MyOfferController> logger)
        {
            _db = db;
            _wallet = wallet;
            _logger = logger;

            _useTestWallet = Convert.ToBoolean(configuration["TestWallet:UseTestWallet"]);
            _testWalletPublicAddress = configuration["TestWallet:PublicKey"];
            _testWalletPrivateAddress = configuration["TestWallet:PrivateKey"];

            var prefix = configuration["Environment:Prefix"];
            _blockchainNodeAndKey = $"{configuration[$"BlockchainNode:{prefix}Node"]}{configuration[$"BlockchainNode:{prefix}Key"]}";
        }

        /// <summary>
        /// Seller: Gets offers
        /// </summary>
        /// <returns>List of my offers</returns>
        /// <response code="200">List of my offers</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet()]
        [Route("GetMyOffers")]
        [ProducesResponseType(typeof(List<GetMyOfferResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMyOffers()
        {
            try
            {
                // Get the current user
                var masterUserId = HttpContextClaims.GetMasterUserId(HttpContext);
                var user = await _db.GetUserMasterId(masterUserId);

                // Get the collections for the user
                var offers = await _db.GetMyOffers(user.UserId);
                var embedImage = false;
                var result = new List<GetMyOfferResponse>();

                if (offers != null)
                {
                    foreach (var offer in offers)
                    {
                        var item = await _db.GetItem(offer.Item.ItemId);
                        var category = await _db.GetCategory((int)item.CategoryId);
                        var collection = await _db.GetCollection((int)item.CollectionId);

                        var categoryBox = await _db.GetCategoryImage((int)item.CategoryId);
                        var collectionBox = await _db.GetCollectionImage((int)item.CollectionId);

                        var collectionView = new CollectionViewItem
                        {
                            CollectionId = (int)item.CollectionId,
                            Name = collection.Name,
                            Image = embedImage ? $"data:{collectionBox.Type}:base64, {Convert.ToBase64String(collectionBox.Data)}"
                                                   : $"/api/v1/Category/GetCategoryImage/{collection.CollectionId}",
                        };

                        var categoryView = new CategoryViewItem
                        {
                            CategoryId = (int)item.CategoryId,
                            Title = category.Title,
                            Image = embedImage ? $"data:{categoryBox.Type}:base64, {Convert.ToBase64String(categoryBox.Data)}"
                                                   : $"/api/v1/Category/GetCategoryImage/{category.CategoryId}",
                        };

                        result.Add(new GetMyOfferResponse
                        {
                            OfferId = offer.OfferId,
                            Collection = collectionView,
                            Category = categoryView,
                            ItemId = offer.Item.ItemId,
                            Name = offer.Item?.Name,
                            Price = offer.Price,
                            Currency = offer.Currency,
                            Bidder = offer.Sender?.Username,
                            Description = offer.Item?.Description,
                            ImageIpfs = offer.Item?.MediaIpfs
                        });
                    }
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: {Method}, Exception: {Message}", "GetMyOffers", ex.Message);

                return Problem(title: "/MyOffer/GetMyOffers", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Seller: Get a specific offer
        /// </summary>
        /// <returns>my offer</returns>
        /// <response code="200">my offer</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet()]
        [Route("GetMyOffer/{offerId::int}")]
        [ProducesResponseType(typeof(GetMyOfferResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMyOffer(int offerId)
        {
            try
            {
                // Get the current user
                var masterUserId = HttpContextClaims.GetMasterUserId(HttpContext);
                var user = await _db.GetUserMasterId(masterUserId);

                // Get the offer
                var offer = await _db.GetMyOffer(user.UserId, offerId);

                var embedImage = false;

                var item = await _db.GetItem(offer.Item.ItemId);
                var category = await _db.GetCategory((int)item.CategoryId);
                var collection = await _db.GetCollection((int)item.CollectionId);

                var categoryBox = await _db.GetCategoryImage(offer.Category.CategoryId);
                var collectionBox = await _db.GetCollectionImage(offer.Collection.CollectionId);

                var collectionView = new CollectionViewItem
                {
                    CollectionId = (int)item.CollectionId,
                    Name = collection.Name,
                    Image = embedImage ? $"data:{collectionBox.Type}:base64, {Convert.ToBase64String(collectionBox.Data)}"
                                           : $"/api/v1/Category/GetCategoryImage/{item.CollectionId}",
                };

                var categoryView = new CategoryViewItem
                {
                    CategoryId = (int)item.CategoryId,
                    Title = category.Title,
                    Image = embedImage ? $"data:{categoryBox.Type}:base64, {Convert.ToBase64String(categoryBox.Data)}"
                                           : $"/api/v1/Category/GetCategoryImage/{item.CategoryId}",
                };

                GetMyOfferResponse? result = null;
                if (offer != null)
                {
                    result = new GetMyOfferResponse
                    {
                        OfferId = offer.OfferId,
                        Collection = collectionView,
                        Category = categoryView,
                        ItemId = offer.Item.ItemId,
                        Name = offer.Item?.Name,
                        Price = offer.Price,
                        Currency = offer.Currency,
                        Bidder = offer.Sender?.Username,
                        Description = offer.Item?.Description,
                        ImageIpfs = offer.Item?.MediaIpfs
                    };
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: {Method}, Exception: {Message}", "GetMyOffer", ex.Message);

                return Problem(title: "/MyOffer/GetMyOffer", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }


        /// <summary>
        /// MyOffer Action: Accept Offer
        /// </summary>
        /// <returns></returns>
        /// <response code="200"></response>
        /// <response code="500">Internal Server Error</response>
        [HttpPut()]
        [Route("AcceptMyOffer/{offerId::int}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AcceptMyOffer(int offerId)
        {
            try
            {
                // Get the current user
                var masterUserId = HttpContextClaims.GetMasterUserId(HttpContext);
                var user = await _db.GetUserMasterId(masterUserId);

                // Get the offer
                var offer = await _db.GetMyOffer(user.UserId, offerId);
                if (offer.Collection == null)
                    throw new Exception("Collection id not set on offer");
                if (offer.Sender == null)
                    throw new Exception("Offer does not have buyer");

                // Get collection for this item
                var collection = await _db.GetCollection(offer.Collection.CollectionId);
                if (collection.ContractAddress == null)
                    throw new Exception("Collection is missing the NFT contract address");

                // Need to get the buyers wallet, they have to pay
                var buyerAddress = _testWalletPublicAddress;
                var buyerAccount = _testWalletPrivateAddress;

                var sellerAddress = _testWalletPublicAddress;
                var sellerAccount = _testWalletPrivateAddress;

                if (!_useTestWallet)
                {
                    var wallet = await _wallet.GetSignature(offer.Sender.MasterUserId);
                    buyerAddress = wallet.Address;
                    buyerAccount = wallet.Value;

                    var sellerWallet = await _wallet.GetSignature(user.MasterUserId);
                    sellerAddress = sellerWallet.Address;
                    sellerAccount = sellerWallet.Value;
                }

                // Token Id, we may want to switch the db to varbinary and store as a byte array
                BigInteger tokenId = offer.Item.TokenId.Value;

                // Price in Wei
                BigInteger purchaseAmtWei = Web3.Convert.ToWei(offer.Price.Value);

                // Create the account object to work with
                var account = new Nethereum.Web3.Accounts.Account(buyerAccount);
                var ownerAccount = new Nethereum.Web3.Accounts.Account(sellerAccount);

                // Get an instance to the network node
                var web3 = new Web3(account, _blockchainNodeAndKey);
                var ownerWeb3 = new Web3(ownerAccount, _blockchainNodeAndKey);

                // Get the contract interface
                var nftCollectionService = new NFTCollectionService(web3, collection.ContractAddress);
                var nftCollectionApproveService = new NFTCollectionService(ownerWeb3, collection.ContractAddress);

                // Buy the NFT
                var buyNftParams = new BuyNFTFunction
                {
                    TokenId = tokenId,
                    AmountToSend = purchaseAmtWei
                };

                var approveParams = new ApproveFunction
                {
                    To = buyerAddress,
                    TokenId = tokenId
                };

                var approveReceipt = await nftCollectionApproveService.ApproveRequestAndWaitForReceiptAsync(approveParams);
                var buyNftReceipt = await nftCollectionService.BuyNFTRequestAndWaitForReceiptAsync(buyNftParams);

                // Record results
                var offerAccept = new OfferAccept
                {
                    OfferId = offerId,
                    UserId = user.UserId,
                    TransactionHash = buyNftReceipt.TransactionHash
                };

                await _db.AcceptMyOffer(offerAccept);

                return Ok("Offer Accepted");
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: {Method}, Exception: {Message}", "AcceptMyOffer", ex.Message);

                return Problem(title: "/MyOffer/AcceptMyOffer", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }


        /// <summary>
        /// Delete a Offer record
        /// </summary>
        /// <param name="OfferId">Primary Key</param>
        /// <returns></returns>
        /// <response code="200"></response>
        /// <response code="404">Not Found</response>
        [HttpDelete()]
        [Route("RejectOffer/{OfferId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RejectOffer(int OfferId)
        {
            try
            {
                // Get the current user
                var masterUserId = HttpContextClaims.GetMasterUserId(HttpContext);
                var user = await _db.GetUserMasterId(masterUserId);

                await _db.DeleteOffer(OfferId);

                return Ok("Offer Deleted");
            }
            catch (Exception ex)
            {
                var msg = $"Method: RejectOffer, Exeption: {ex.Message}";

                _logger.LogError(msg);

                return Problem(title: "/MyOffer/RejectOffer", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }
    }
}

