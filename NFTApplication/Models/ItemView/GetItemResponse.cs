using NFTApplication.Models.Category;
using NFTApplication.Models.Collection;
using System.Text.Json.Serialization;

namespace NFTApplication.Models.ItemView
{
    /// <summary>
    /// Get Item Response
    /// </summary>
    public class GetItemResponse
    {
        /// <summary>Primary Key</summary>
        public int ItemId { get; set; }

        /// <summary>Name of Item</summary>
        public string? Name { get; set; }

        /// <summary>Description of Item</summary>
        public string? Description { get; set; }

        /// <summary>Thumb nail image of item</summary>
        public string? Media { get; set; }

        /// <summary>Category details</summary>
        public CategoryViewItem? Category { get; set; }

        /// <summary>Collection details</summary>
        public CollectionViewItem? Collection { get; set; }

        /// <summary>Number of times the item has been liked</summary>
        public int? LikeCount { get; set; }

        /// <summary></summary>
        public decimal? Price { get; set; }

        /// <summary></summary>
        public string? PriceDisplay { get; set; }

        /// <summary></summary>
        public string? Currency { get; set; }

        /// <summary>status</summary>
        public string? Status { get; set; }

        /// <summary>Created</summary>
        public DateTime CreateDate { get; set; }

        /// <summary>Enable Auction?</summary>
        public bool? EnableAuction { get; set; }

        /// <summary>Has Offer</summary>
        public bool? AcceptOffer { get; set; }
    }
}
