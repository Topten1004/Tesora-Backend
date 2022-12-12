// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NFTApplication.Models.Category;
using NFTApplication.Models.Collection;
using NFTApplication.Models.MyFavorite;
using NFTApplication.Models.MyOffer;
using NFTApplication.Utility;
using NFTDatabaseEntities;
using NFTDatabaseService;

namespace NFTApplication.Controllers
{
    /// <summary>
    /// My Favorite Controller
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class MyFavoriteController : ControllerBase
    {
        private readonly INFTDatabaseService _db;
        private readonly ILogger<MyOfferController> _logger;

        public MyFavoriteController(INFTDatabaseService db, ILogger<MyOfferController> logger)
        {
            _db = db;
            _logger = logger;
        }

        /// <summary>
        /// Get My Favorites
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        [Route("GetMyFavorites")]
        [ProducesResponseType(typeof(List<GetMyOfferResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMyFavorites()
        {
            try
            {
                // Get the current user
                var masterUserId = HttpContextClaims.GetMasterUserId(HttpContext);
                var user = await _db.GetUserMasterId(masterUserId);

                // Get the collections for the user
                var favorites = await _db.GetMyFavorites(user.UserId);
                var embedImage = false;
                var result = new List<GetMyFavoriteResponse>();

                if (favorites != null)
                {
                    foreach (var favorite in favorites)
                    {
                        var item = await _db.GetItem(favorite.Item.ItemId);
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

                        result.Add(new GetMyFavoriteResponse
                        {
                            FavoriteId = favorite.Favorite?.FavouriteId,
                            Collection = collectionView,
                            Category = categoryView,
                            ItemId = favorite.Item?.ItemId,
                            Name = favorite.Item?.Name,
                            Price = favorite.Item?.Price,
                            Currency = favorite.Item?.Currency,
                            Media = favorite.Item?.MediaIpfs,
                            EnableAuction = favorite.Item?.EnableAuction,
                            AcceptOffer = favorite.Item?.AcceptOffer,
                        });
                    }
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: {Method}, Exception: {Message}", "GetMyFavorites", ex.Message);

                return Problem(title: "/MyOffer/GetMyFavorites", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }


    }
}
