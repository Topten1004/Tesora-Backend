// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using NFTDatabaseService;
using NFTDatabaseEntities;
using NFTApplication.Models.ItemView;
using NFTApplication.Utility;
using NFTApplication.Models.Collection;
using NFTApplication.Models.Category;
using System;

namespace NFTApplication.Controllers
{

    /// <summary>
    /// Item Page Controller
    /// </summary>
    //[Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ItemViewController : ControllerBase
    {
        private readonly INFTDatabaseService _db;
        private readonly ILogger<ItemViewController> _logger;
        private readonly ICurrencyUtility _currencyUtility;

        /// <summary>
        /// Dependency Injection Contstructor
        /// </summary>
        /// <param name="db">Database Singleton</param>
        /// <param name="logger">Logger</param>
        /// <param name="currencyUtility"></param>
        public ItemViewController(INFTDatabaseService db, ILogger<ItemViewController> logger, ICurrencyUtility currencyUtility)
        {
            _db = db;
            _logger = logger;
            _currencyUtility = currencyUtility; 
        }

        /// <summary>
        /// Gets the Item View View Model
        /// </summary>
        /// <returns>Item View View Model</returns>
        /// <response code="200">Item View View Model</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet()]
        [Route("GetItemViewItem/{itemId}/{displayCurrency?}")]
        [ProducesResponseType(typeof(ItemViewViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetItemViewItem(int itemId, string displayCurrency = "ETH")
        {
            try
            {
                var embedImage = false;

                var item = await _db.GetItem(itemId);

                var user = await _db.GetUser((int)item.CurrentOwner);

                var currentOwner = new ItemViewUser
                {
                    UserId = user.UserId,
                    UserName = user.Username
                };

                var collection = await _db.GetCollection((int)item.CollectionId);
                var authorUser = await _db.GetUser((int)collection.AuthorId);
                var collectionBox = await _db.GetCollectionImage((int)collection.CollectionId);
                var bannerBox = await _db.GetCollectionBanner((int)collection.CollectionId);

                var currentAuthor = new ItemViewUser
                {
                    UserId = authorUser.UserId,
                    UserName = authorUser.Username
                };

                var collectionView = new ItemViewCollection
                {
                    CollectionId = collection.CollectionId,
                    Name = collection.Name,
                    Description = collection.Description,
                    ContractSymbol = collection.ContractSymbol,
                    ContractAddress = collection.ContractAddress,
                    Banner = embedImage ? $"data:{bannerBox.Type}:base64, {Convert.ToBase64String(bannerBox.Data)}"
                                                   : $"/api/v1/Collection/GetCollectionBanner/{collection.CollectionId}",
                    Status = (ItemViewCollection.ItemViewCollectionStatuses)collection.Status,
                    Author = currentAuthor,
                    ChainId = collection.ChainId,
                    CreateDate = collection.CreateDate,
                };

                var category = new Category();
                var categoryBox = new ImageBox();

                if(item.CategoryId != null)
                {
                    category = await _db.GetCategory((int)item.CategoryId);
                    categoryBox = await _db.GetCategoryImage((int)item.CategoryId);
                }

                var categoryView = new ItemViewCategory
                {
                    CategoryId = category.CategoryId,
                    CategoryImage = embedImage ? $"data:{categoryBox.Type}:base64, {Convert.ToBase64String(categoryBox.Data)}"
                                                   : $"/api/v1/Category/GetCategoryImage/{category.CategoryId}",
                    Title = category.Title,
                    CreateDate = category.CreateDate,
                    Status = category.Status.ToString()
                };

                var offers = await _db.GetOffers();

                var lstOffers = new List<Offer>();
                var lstAuctions = new List<Auction>();

                lstOffers = offers.FindAll(x => x.ItemId == itemId);

                if (DateTime.UtcNow < item.EndDate && item.EnableAuction == true)
                    lstAuctions = await _db.GetAuctionsByItemId(itemId);
                
                var Auctions = new List<ItemViewAuction>();
                var Offers = new List<ItemViewOffer>();

                foreach (Auction? auction in lstAuctions)
                {
                    var senderUser = await _db.GetUser((int)auction.SenderId);
                    var receiverUser = await _db.GetUser((int)auction.ReceiverId);

                    var sender = new ItemViewUser
                    {
                        UserId = senderUser.UserId,
                        UserName = senderUser.Username
                    };

                    var receiver = new ItemViewUser
                    {
                        UserId = receiverUser.UserId,
                        UserName = receiverUser.Username
                    };

                    var temp = new ItemViewAuction
                    {
                        AuctionId = auction.AuctionId,
                        ItemId = (int)auction.ItemId,
                        Sender = sender,
                        Receiver = receiver,
                        Price = (decimal)auction.Price,
                        PriceDisplay = await _currencyUtility.CurrencyConversion(auction.Price.Value, auction.Currency, displayCurrency),
                        Currency = auction.Currency,
                        CreateDate = auction.CreateDate
                    };

                    Auctions.Add(temp);
                }

                foreach (Offer? offer in lstOffers)
                {
                    var senderUser = await _db.GetUser((int)offer.SenderId);
                    var receiverUser = await _db.GetUser((int)offer.ReceiverId);

                    var sender = new ItemViewUser
                    {
                        UserId = senderUser.UserId,
                        UserName = senderUser.Username
                    };

                    var receiver = new ItemViewUser
                    {
                        UserId = receiverUser.UserId,
                        UserName = receiverUser.Username
                    };

                    var temp = new ItemViewOffer
                    {
                        OfferId = offer.OfferId,
                        ItemId = (int)offer.ItemId,
                        Sender = sender,
                        Receiver = receiver,
                        Price = (decimal)offer.Price,
                        PriceDisplay = await _currencyUtility.CurrencyConversion(offer.Price.Value, offer.Currency, displayCurrency),
                        Currency = offer.Currency,
                        CreateDate = offer.CreateDate
                    };

                    Offers.Add(temp);
                }

                var histories = await _db.GetHistories();
                var lstHistories = histories.FindAll(x => x.ItemId == itemId);

                var Historys = new List<ItemViewHistory>();

                foreach (History? history in lstHistories)
                {
                    var fromUser = await _db.GetUser((int)history.FromId);
                    var from = new ItemViewUser
                    {
                        UserId = fromUser.UserId,
                        UserName = fromUser.Username
                    };

                    var toUser = await _db.GetUser((int)history.ToId);
                    var to = new ItemViewUser
                    {
                        UserId = toUser.UserId,
                        UserName = toUser.Username
                    };

                    var temp = new ItemViewHistory
                    {
                        HistoryId = history.HistoryId,
                        ItemId = (int)history.ItemId,
                        CollectionId = (int)history.CollectionId,
                        From = from,
                        To = to,
                        TransactionHash = history.TransactionHash,
                        Price = (decimal)history.Price,
                        Currency = history.Currency,
                        HistoryType = (ItemViewHistory.ItemViewHistoryTypes)history.HistoryType,
                        IsValid = history.IsValid,
                        CreateDate = history.CreateDate,
                    };

                    Historys.Add(temp);
                }

                var result = new ItemViewItem
                {
                    ItemId = item.ItemId,
                    Name = item.Name,
                    Description = item.Description,
                    Price = item.Price,
                    PriceDisplay = await _currencyUtility.CurrencyConversion(item.Price.Value, item.Currency, displayCurrency),
                    Currency = item.Currency,
                    Collection = collectionView,
                    Category = categoryView,
                    MediaIPFS = item.MediaIpfs,
                    ExternalLink = item.ExternalLink,
                    ThumbIPFS = item.ThumbIpfs,
                    AcceptOffer = item.AcceptOffer,
                    UnlockContentUrl = item.UnlockContentUrl,
                    ViewCount = (int)item.ViewCount,
                    LikeCount = (int)item.LikeCount,
                    TokenId = (int)item.TokenId,
                    MintedDate = (DateTime)item.MintedDate,
                    StartDate = item.StartDate,
                    EndDate = item.EndDate,
                    CreateDate = item.CreateDate,
                    Status = item.Status,
                    CurrentOwner = currentOwner,
                    EnableAuction = item.EnableAuction,
                    AuctionReserve = item.AuctionReserve,
                    Offers = Offers,
                    Auctions = Auctions,
                    Histories = Historys
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                var msg = $"Method: GetItemViewItem, Exception: {ex.Message}";

                _logger.LogError(msg);

                return Problem(title: "/ItemView/GetItemViewItem", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }


        /// <summary>
        /// Gets the Items
        /// </summary>
        /// <returns>List of Item records</returns>
        /// <response code="200">List of Item records</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet()]
        [Route("GetItems/{collectionId::int}/{displayCurrency?}")]
        [ProducesResponseType(typeof(List<GetItemResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetItems(int collectionId, string displayCurrency = "ETH")
        {
            try
            {
                var response = new List<GetItemResponse>();

                var records = await _db.GetItems(collectionId);

                foreach (var record in records)
                {
                    var collection = await _db.GetCollection((int)record.CollectionId);

                    var collectionItem = new CollectionViewItem
                    {
                        CollectionId = collection.CollectionId,
                        Name = collection.Name
                    };

                    var categoryItem = new CategoryViewItem();

                    if (record.CategoryId != null)
                    {
                        var category = await _db.GetCategory((int)record.CategoryId);

                        categoryItem.CategoryId = category.CategoryId;
                        categoryItem.Title = category.Title;
                    }

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
                        PriceDisplay = await _currencyUtility.CurrencyConversion(record.Price.Value, record.Currency, displayCurrency),
                        Currency = record.Currency,
                        Status = record.Status.ToString(),
                        CreateDate = record.CreateDate,
                        AcceptOffer = record.AcceptOffer,
                        EnableAuction = record.EnableAuction
                    });
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                var msg = $"Method: GetItems, Exception: {ex.Message}";

                _logger.LogError(msg);

                return Problem(title: "/Item/GetItems", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Gets the Items
        /// </summary>
        /// <returns>List of Item records</returns>
        /// <response code="200">List of Item records</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet()]
        [Route("GetItemsByCategoryId/{categoryId::int}/{displayCurrency?}")]
        [ProducesResponseType(typeof(List<GetItemResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetItemsByCategoryId(int categoryId, string displayCurrency = "ETH")
        {
            try
            {
                var response = new List<GetItemResponse>();

                var records = await _db.GetItemsByCategoryId(categoryId);

                foreach (var record in records)
                {
                    var collection = await _db.GetCollection((int)record.CollectionId);

                    var collectionItem = new CollectionViewItem
                    {
                        CollectionId = collection.CollectionId,
                        Name = collection.Name
                    };

                    var categoryItem = new CategoryViewItem();

                    if (record.CategoryId != null)
                    {
                        var category = await _db.GetCategory((int)record.CategoryId);

                        categoryItem.CategoryId = category.CategoryId;
                        categoryItem.Title = category.Title;
                    }

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
                        PriceDisplay = await _currencyUtility.CurrencyConversion(record.Price.Value, record.Currency, displayCurrency),
                        Currency = record.Currency,
                        Status = record.Status.ToString(),
                        CreateDate = record.CreateDate,
                        AcceptOffer = record.AcceptOffer,
                        EnableAuction = record.EnableAuction
                    });
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                var msg = $"Method: GetItems, Exception: {ex.Message}";

                _logger.LogError(msg);

                return Problem(title: "/Item/GetItems", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Add a Favourite to an Item
        /// </summary>
        /// <param name="itemId">Item Id</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost()]
        [Route("AddFavourite/{itemId::int}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddFavourite(int itemId)
        {
            try
            {
                // Get the current user
                var masterUserId = HttpContextClaims.GetMasterUserId(HttpContext);
                var user = await _db.GetUserMasterId(masterUserId);

                var record = new Favourite
                {
                    FavouriteId = 0,
                    ItemId = itemId,
                    UserId = user.UserId,
                    CreateDate = DateTime.UtcNow
                };

                await _db.PostFavourite(record);

                return Ok("Favorite Added to Item");
            }
            catch (Exception ex)
            {
                var msg = $"Method: AddFavourite, Exception: {ex.Message}";

                _logger.LogError(msg);

                return Problem(title: "/ItemView/AddFavorite", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }


        /// <summary>
        /// Remove a Favourite from an Item
        /// </summary>
        /// <param name="itemId">Item Id</param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete()]
        [Route("RemoveFavourite/{itemId::int}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveFavourite(int itemId)
        {
            try
            {
                // Get the current user
                var masterUserId = HttpContextClaims.GetMasterUserId(HttpContext);
                var user = await _db.GetUserMasterId(masterUserId);

                await _db.RemoveFavourite(user.UserId, itemId);

                return Ok("Favorite Removed for Item");
            }
            catch (Exception ex)
            {
                var msg = $"Method: RemoveFavourite, Exception: {ex.Message}";

                _logger.LogError(msg);

                return Problem(title: "/ItemView/RemoveFavorite", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

    }
}

