// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//
using System.Numerics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using Nethereum.Web3;

using NFTDatabaseService;
using NFTWalletService;
using NFTApplication.Models.CollectionViewDetail;
using NFTApplication.Utility;
using NFTApplication.Models.ItemView;
using NFTDatabaseEntities;
using NFTApplication.Contracts.NFTCollection.ContractDefinition;
using NFTApplication.Contracts.NFTCollection;
using NFTApplication.Models.Collection;
using NFTApplication.Models.Category;


namespace NFTApplication.Controllers
{

    /// <summary>
    /// Collection Database Controller
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CollectionController : ControllerBase
    {
        private readonly INFTDatabaseService _db;
        private readonly INFTWalletService _wallet;
        private readonly string _blockchainNodeAndKey;
        private readonly string _testWalletPublicAddress;
        private readonly string _testWalletPrivateAddress;
        private readonly ILogger<CollectionController> _logger;
        private readonly bool _useTestWallet;

        /// <summary>
        /// Dependency Injection Contstructor
        /// </summary>
        /// <param name="db">Database Singleton</param>
        /// <param name="logger">Logger</param>
        /// <param name="configuration"></param>
        /// <param name="wallet"></param>
        public CollectionController(INFTDatabaseService db, INFTWalletService wallet, ILogger<CollectionController> logger, IConfiguration configuration)
        {
            _db = db;
            _wallet = wallet;
            _logger = logger;
            var prefix = configuration["Environment:Prefix"];
            _blockchainNodeAndKey = $"{configuration[$"BlockchainNode:{prefix}Node"]}{configuration[$"BlockchainNode:{prefix}Key"]}";

            _useTestWallet = Convert.ToBoolean(configuration["TestWallet:UseTestWallet"]);
            _testWalletPublicAddress = configuration["TestWallet:PublicKey"];
            _testWalletPrivateAddress = configuration["TestWallet:PrivateKey"];
        }


        /// <summary>
        /// Get Image for the collection
        /// </summary>
        /// <returns>Collection Image</returns>
        /// <response code="200">Collection Image</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet()]
        [Route("GetCollectionImage/{collectionId:int}")]
        [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCollectionImage(int collectionId)
        {
            try
            {
                var image = await _db.GetCollectionImage(collectionId);

                if (image != null && image.Data != null && image.Type != null)
                    return File(image.Data, image.Type);
                else
                    return NotFound("Collection does not have an Image");
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: {Method}, Exception: {Message}", "CollectionImage", ex.Message);

                return Problem(title: "/Collection/GetCollectionImage", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }


        /// <summary>
        /// Get Banner for the collection
        /// </summary>
        /// <returns>Collection Banner</returns>
        /// <response code="200">Collection Banner</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet()]
        [Route("GetCollectionBanner/{collectionId:int}")]
        [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCollectionBanner(int collectionId)
        {
            try
            {
                var image = await _db.GetCollectionBanner(collectionId);

                if (image != null && image.Data != null && image.Type != null)
                    return File(image.Data, image.Type);
                else
                    return NotFound("Collection does not have a Banner");
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: {Method}, Exception: {Message}", "CollectionBanner", ex.Message);

                return Problem(title: "/Collection/GetCollectionBanner", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Buy Item
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Buy Item</response>
        /// <response code="500">Internal Server Error</response>
        [Authorize]
        [HttpGet()]
        [Route("BuyItem/{itemId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> BuyItem(int itemId)
        {
            try
            {
                // Get the current user
                var masterUserId = HttpContextClaims.GetMasterUserId(HttpContext);
                var user = await _db.GetUserMasterId(masterUserId);

                // Get the offer
                var item = await _db.GetItem(itemId);

                var sellerUser = await _db.GetUser((int)item.CurrentOwner);

                if (item.CurrentOwner != user.UserId)
                {
                    // Get collection for this item
                    var collection = await _db.GetCollection((int)item.CollectionId);

                    if (collection.ContractAddress == null)
                        throw new Exception("Collection is missing the NFT contract address");

                    // Need to get the buyers wallet, they have to pay
                    var buyerAddress = _testWalletPublicAddress;
                    var buyerAccount = _testWalletPrivateAddress;

                    var sellerAddress = _testWalletPublicAddress;
                    var sellerAccount = _testWalletPrivateAddress;

                    if (!_useTestWallet)
                    {
                        var wallet = await _wallet.GetSignature(masterUserId);
                        buyerAddress = wallet.Address;
                        buyerAccount = wallet.Value;

                        var sellerWallet = await _wallet.GetSignature(sellerUser.MasterUserId);
                        sellerAddress = sellerWallet.Address;
                        sellerAccount = sellerWallet.Value;
                    }

                    // Token Id, we may want to switch the db to varbinary and store as a byte array
                    BigInteger tokenId = item.TokenId.Value;

                    // Price in Wei
                    BigInteger purchaseAmtWei = Web3.Convert.ToWei(item.Price.Value);

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

                    await _db.DeletePastOffers(item.ItemId, (int)item.CurrentOwner);

                    item.CurrentOwner = user.UserId;
                    await _db.PutItem(item);

                    var history = new History
                    {
                        ItemId = item.ItemId,
                        CollectionId = item.CollectionId,
                        FromId = item.CurrentOwner,
                        ToId = user.UserId,
                        TransactionHash = buyNftReceipt.TransactionHash,
                        Price = item.Price,
                        Currency = item.Currency,
                        HistoryType = History.HistoryTypes.transfer,
                        IsValid = true,
                        CreateDate = DateTime.UtcNow,
                    };

                    await _db.PostHistory(history);

                    return Ok("Buy Item");
                }
                else
                {
                    return BadRequest("You are owner");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: {Method}, Exception: {Message}", "BuyItem", ex.Message);

                return Problem(title: "/Collection/BuyItem", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Create Offer
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Create Offer</response>
        /// <response code="500">Internal Server Error</response>
        [Authorize]
        [HttpPost()]
        [Route("CreateOffer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateOffer([FromBody] CreateOfferItem request)
        {
            try
            {
                if (request == null)
                    throw new  ArgumentException("Invalid request");

                // Get the current user
                var masterUserId = HttpContextClaims.GetMasterUserId(HttpContext);
                var user = await _db.GetUserMasterId(masterUserId);

                // Retrieve the item
                var item = await _db.GetItem(request.ItemId);
                if (item == null)
                    throw new ArgumentException($"Item {request.ItemId} was not found");

                if (user.UserId == item.CurrentOwner)
                    return BadRequest("You are owner");

                if ( item.StartDate < DateTime.UtcNow && DateTime.UtcNow < item.EndDate && item.EnableAuction == true)
                    return BadRequest("You can't offer on Auction period");

                if (item.AcceptOffer == false)
                    return BadRequest("AcceptOffer is false"); 

                var offer = new Offer
                {
                    ItemId = request.ItemId,
                    Price = request.Price,
                    Currency = request.Currency,
                    CreateDate = DateTime.UtcNow,
                    SenderId = user.UserId,
                    ReceiverId = item.CurrentOwner
                };

                await _db.PostOffer(offer);

                return Ok("Offer Created");
            }
            catch (ArgumentException ae)
            {
                _logger.LogError("Method: {Method}, Exception: {Message}", "CreateOffer", ae.Message);

                return BadRequest(ae.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: {Method}, Exception: {Message}", "CreateOffer", ex.Message);

                return Problem(title: "/Collection/CreateOffer", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Create Auction
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Create Auction</response>
        /// <response code="500">Internal Server Error</response>
        [Authorize]
        [HttpPost()]
        [Route("CreateAuction")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateAuction([FromBody] CreateOfferItem request)
        {
            try
            {
                if (request == null)
                    throw new ArgumentException("Invalid request");

                // Get the current user
                var masterUserId = HttpContextClaims.GetMasterUserId(HttpContext);
                var user = await _db.GetUserMasterId(masterUserId);

                // Retrieve the item
                var item = await _db.GetItem(request.ItemId);
                if (item == null)
                    throw new ArgumentException($"Item {request.ItemId} was not found");

                if (item.CurrentOwner == user.UserId)
                    return BadRequest("You are owner");

                if (item.EnableAuction == true)
                {
                    if (item.EndDate != null)
                    {
                        if (DateTime.UtcNow < item.EndDate)
                        {
                            var auctions = await _db.GetAuctionsByItemId(item.ItemId);
                            var itemOffer = auctions.LastOrDefault(x => x.CreateDate < item.EndDate);

                            if (itemOffer != null)
                                if (item.AuctionReserve > request.Price)
                                    return BadRequest("Price is low");

                            var auction = new Auction
                            {
                                ItemId = request.ItemId,
                                Price = request.Price,
                                Currency = request.Currency,
                                CreateDate = DateTime.UtcNow,
                                SenderId = user.UserId,
                                ReceiverId = item.CurrentOwner
                            };

                            await _db.PostAuction(auction);

                            return Ok("Auction Created");
                        }
                        else
                            return BadRequest("Auction period is expired");
                    }
                    else
                        return BadRequest("EndDate is null");
                }
                else
                    return BadRequest("EnableAuction is not true");
            }
            catch (ArgumentException ae)
            {
                _logger.LogError("Method: {Method}, Exception: {Message}", "CreateOffer", ae.Message);

                return BadRequest(ae.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: {Method}, Exception: {Message}", "CreateOffer", ex.Message);

                return Problem(title: "/Collection/CreateOffer", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }


        /// <summary>
        /// Rescind Offer
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Rescind Offer</response>
        /// <response code="500">Internal Server Error</response>
        [Authorize]
        [HttpDelete()]
        [Route("RescindOffer/{offerId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RescindOffer(int offerId)
        {
            try
            {
                if (offerId <= 0)
                    throw new ArgumentException("Invalid offerId");

                await _db.DeleteOffer(offerId);

                return Ok("Offer rescinded");
            }
            catch (ArgumentException ae)
            {
                _logger.LogError("Method: {Method}, Exception: {Message}", "RecindOffer", ae.Message);

                return BadRequest(ae.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: {Method}, Exception: {Message}", "RecindOffer", ex.Message);

                return Problem(title: "/Collection/RescindOffer", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }


        /// <summary>
        /// Search in the marketplace
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Collections and Items</response>
        /// <response code="500">Internal Server Error</response>
        [Authorize]
        [HttpPost()]
        [Route("SearchMarketplace")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SearchMarketplace([FromBody]CollectionSearchView search)
        {
            try
            {
                if(search.onlyCollection == true)
                {
                        var collecions = await _db.GetCollections();
                        var result = collecions.FindAll(x => x.Name.Contains(search.search, StringComparison.OrdinalIgnoreCase));

                        var lstCollections = new List<CollectionViewDetailCollection>();

                        foreach (var collection in result)
                        {
                            var user = await _db.GetUser((int)collection.AuthorId);

                            var item = new CollectionViewDetailCollection
                            {
                                Author = new CollectionViewDetailUser { UserId = user.UserId, UserName = user.Username },
                                Banner = collection.Banner != null ? $"data:{collection.BannerImageType}:base64, {Convert.ToBase64String(collection.Banner)}"
                                                : $"/api/v1/Collection/GetCollectionBanner/{collection.CollectionId}",
                                CollectionId = collection.CollectionId,
                                ContractAddress = collection.ContractAddress,
                                ContractSymbol = collection.ContractSymbol,
                                Description = collection.Description,
                                Image = collection.CollectionImage != null ? $"data:{collection.CollectionImageType}:base64, {Convert.ToBase64String(collection.CollectionImage)}"
                                                : $"/api/v1/Collection/GetCollectionImage/{collection.CollectionId}",
                                ItemCount = (int)collection.ItemCount,
                                Name = collection.Name,
                                Royalties = (decimal)collection.Royalties,
                                Status = (CollectionViewDetailCollection.CollectionViewDetailCollectionStatuses)collection.Status,
                                VolumeTraded = (int)collection.VolumeTraded,
                                CreateDate = collection.CreateDate
                            };

                            lstCollections.Add(item);
                        }
   
                        return Ok(lstCollections);
                    }
                else
                {
                    var response = new List<GetItemResponse>();

                    var collections = await _db.GetCollections();
                    foreach (var collection in collections)
                    {
                        if (collection.Name.Contains(search.search, StringComparison.OrdinalIgnoreCase))
                        {
                            var items = await _db.GetItems(collection.CollectionId);

                            foreach (var record in items)
                            {
                                var collectionItem = new CollectionViewItem
                                {
                                    CollectionId = collection.CollectionId,
                                    Name = collection.Name
                                };

                                var category = await _db.GetCategory((int)record.CategoryId);

                                var categoryItem = new CategoryViewItem
                                {
                                    CategoryId = category.CategoryId,
                                    Title = category.Title
                                };

                                response.Add(new GetItemResponse
                                {
                                    ItemId = record.ItemId,
                                    Name = record.Name,
                                    Description = record.Description,
                                    Media = record.MediaIpfs,
                                    LikeCount = record.LikeCount,
                                    Collection = collectionItem,
                                    Category = categoryItem,
                                    Price = record.Price,
                                    Currency = record.Currency,
                                    Status = record.Status.ToString(),
                                    CreateDate = record.CreateDate,
                                    AcceptOffer = record.AcceptOffer,
                                    EnableAuction = record.EnableAuction
                                });
                            }
                        }
                        else
                        {
                            var items = await _db.GetItems(collection.CollectionId);

                            foreach (var record in items)
                            {
                                if(record.Name.Contains(search.search, StringComparison.OrdinalIgnoreCase))
                                {
                                    var collectionItem = new CollectionViewItem
                                    {
                                        CollectionId = collection.CollectionId,
                                        Name = collection.Name
                                    };

                                    var category = await _db.GetCategory((int)record.CategoryId);

                                    var categoryItem = new CategoryViewItem
                                    {
                                        CategoryId = category.CategoryId,
                                        Title = category.Title
                                    };

                                    response.Add(new GetItemResponse
                                    {
                                        ItemId = record.ItemId,
                                        Name = record.Name,
                                        Description = record.Description,
                                        Media = record.MediaIpfs,
                                        LikeCount = record.LikeCount,
                                        Collection = collectionItem,
                                        Category = categoryItem,
                                        Price = record.Price,
                                        Currency = record.Currency,
                                        Status = record.Status.ToString(),
                                        CreateDate = record.CreateDate,
                                        AcceptOffer = record.AcceptOffer,
                                        EnableAuction = record.EnableAuction
                                    });
                                }
                            }
                        }
                    }
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: {Method}, Exception: {Message}", "SearchMarketplace", ex.Message);

                return Problem(title: "/Collection/SearchMarketplace", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

    }
}

