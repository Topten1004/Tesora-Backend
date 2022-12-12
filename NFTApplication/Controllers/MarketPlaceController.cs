// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using FluentValidation;


using NFTDatabaseService;
using NFTApplication.Models.MarketPlace;
using NFTDatabaseEntities;
using NFTApplication.Utility;


namespace NFTApplication.Controllers
{

    /// <summary>
    /// Market Place Page Controller
    /// </summary>
    //[Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class MarketPlaceController : ControllerBase
    {
        private readonly INFTDatabaseService _db;
        private readonly ILogger<MarketPlaceController> _logger;
        private readonly ICurrencyUtility _currencyUtility;

        /// <summary>
        /// Dependency Injection Contstructor
        /// </summary>
        /// <param name="db">Database Singleton</param>
        /// <param name="logger">Logger</param>
        /// <param name="currencyUtility"></param>
        public MarketPlaceController(INFTDatabaseService db, ILogger<MarketPlaceController> logger, ICurrencyUtility currencyUtility)
        {
            _db = db;
            _logger = logger;
            _currencyUtility = currencyUtility;
        }


        /// <summary>
        /// Gets the Market Place View Model
        /// </summary>
        /// <returns>Market Place View Model</returns>
        /// <response code="200">Market Place View Model</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet()]
        [Route("GetMarketPlaceViewModel")]
        [ProducesResponseType(typeof(MarketPlaceViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMarketPlaceViewModel()
        {
            try
            {
                bool embedImage = false;

                var collections = await _db.GetCollections();
                var lstCollections = new List<MarketPlaceCollection>();

                foreach(var collection in collections)
                {
                    var bannerImage = await _db.GetCollectionBanner(collection.CollectionId);

                    var collectionImage = await _db.GetCollectionImage(collection.CollectionId);

                    var item = new MarketPlaceCollection
                    {
                        CollectionId = collection.CollectionId,
                        Name = collection.Name,
                        ItemCount = collection.ItemCount,
                        Banner = embedImage ? $"data:{bannerImage.Type}:base64, {Convert.ToBase64String(bannerImage.Data)}"
                                                   : $"/api/v1/Collection/GetCollectionBanner/{collection.CollectionId}",
                        Image = embedImage ? $"data:{collectionImage.Type}:base64, {Convert.ToBase64String(collectionImage.Data)}"
                                                   : $"/api/v1/Collection/GetCollectionImage/{collection.CollectionId}",
                        Status = (MarketPlaceCollection.MarketPlaceCollectionStatuses)collection.Status
                    };

                    lstCollections.Add(item);
                }

                var categories = await _db.GetCategories();
                var lstCategories = new List<MarketPlaceCategory>();

                foreach(var category in categories)
                {
                    var item = new MarketPlaceCategory
                    {
                        CategoryId = category.CategoryId,
                        Title = category.Title,
                        Category = $"/api/v1/Category/GetCategoryImage/{category.CategoryId}",
                        Status = (MarketPlaceCategory.MarketPlaceCategoryStatuses)category.Status
                    };

                    lstCategories.Add(item);
                }

                var result = new MarketPlaceViewModel
                {
                    Categories = lstCategories,
                    Collections = lstCollections
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: {Method}, Exception: {Message}", "GetMarketPlace", ex.Message);

                return Problem(title: "/Marketplace/GetMarketPlace", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }


        /// <summary>
        /// Gets the Market Place Items
        /// </summary>
        /// <returns>MarketPlaceResponse</returns>
        /// <response code="200">Market Place View Model</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost()]
        [Route("GetMarketPlaceItems")]
        [ProducesResponseType(typeof(List<MarketPlaceItemsResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMarketPlaceItems(MarketPlaceItemsRequest request)
        {
            try
            {
                // Validations
                var validator = new MarketPlaceItemsRequestValidator();
                await validator.ValidateAndThrowAsync(request);

                var query = new MarketPlaceRequest
                {
                    Filter = new MarketPlaceFilter
                    {
                        PriceFilter = request.Filter.PriceFilter,
                        SaleType = request.Filter.SaleType,
                        CategoryIds = request.Filter.CategoryIds
                    },
                    Sort = new MarketPlaceSort
                    {
                        SortOrder = request.Sort.SortOrder
                    },
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize,
                    Text = request.Text,
                    TextSearchType = request.TextSearchType
                };

                var items = await _db.GetMarketPlaceItems(query);

                var response = new List<MarketPlaceItemsResponse>();
                foreach (var item in items)
                {
                    var row = new MarketPlaceItemsResponse
                    {
                        AcceptOffer = item.AcceptOffer,
                        CardType = item.CardType,
                        Collection = new CollectionView
                        {
                            collection_id = item.Collection.collection_id,
                            banner = item.Collection.banner,
                            image = item.Collection.image,
                            name = item.Collection.name
                        },
                        Currency = item.Currency,
                        EnableAuction = item.EnableAuction,
                        ItemCount = item.ItemCount,
                        ItemId = item.ItemId,
                        LikeCount = item.LikeCount,
                        Media = item.Media,
                        Name = item.Name,
                        Price = item.Price,
                        PriceDisplay = await _currencyUtility.CurrencyConversion(item.Price.Value, item.Currency, request.DisplayCurrency),
                        ViewCount = item.ViewCount
                    };

                    if (item.Category != null)
                    {
                        row.Category = new CategoryView
                        {
                            category_id = item.Category.category_id,
                            category_title = item.Category.category_title
                        };
                    }

                    response.Add(row);
                }

                return Ok(response);
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
                _logger.LogError("Method: {Method}, Exception: {Message}", "GetMarketPlaceItems", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }

        }

    }
}

