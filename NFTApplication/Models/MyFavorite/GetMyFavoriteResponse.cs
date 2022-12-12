using NFTApplication.Models.Category;
using NFTApplication.Models.Collection;

namespace NFTApplication.Models.MyFavorite
{
    /// <summary>
    /// My Favorite Response
    /// </summary>
    public class GetMyFavoriteResponse
    {
        /// <summary>Primary Key</summary>
        public int? FavoriteId { get; set; }

        /// <summary>Collection Name</summary>
        public CollectionViewItem? Collection { get; set; }

        /// <summary>Category Name</summary>
        public CategoryViewItem? Category { get; set; }

        /// <summary>Item id</summary>
        public int? ItemId { get; set; }

        /// <summary>Item Price</summary>
        public decimal? Price { get; set; }

        /// <summary>Currency</summary>
        public string? Currency { get; set; }

        /// <summary>Name of Item</summary>
        public string? Name { get; set; }

        /// <summary>NFT Image</summary>
        public string? Media { get; set; }

        /// <summary>Description of Item</summary>
        public string? Description { get; set; }

        /// <summary>Enable Auction</summary>
        public bool? EnableAuction { get; set; }

        /// <summary>Accept offer</summary>
        public bool? AcceptOffer { get; set; }
    }
}
