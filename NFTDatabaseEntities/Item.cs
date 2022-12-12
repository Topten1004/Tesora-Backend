// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//

namespace NFTDatabaseEntities
{
    /// <summary>
    /// Item
    /// </summary>
    public class Item
    {
        /// <summary>Primary Key</summary>
        public int ItemId { get; set; }

        /// <summary>Name of Item</summary>
        public string Name { get; set; }

        /// <summary>Description of Item</summary>
        public string? Description { get; set; }

        /// <summary>External Link to view NFT - this is our site</summary>
        public string? ExternalLink { get; set; }

        /// <summary>Media</summary>
        public byte[]? Media { get; set; }

        /// <summary>Media IPFS</summary>
        public string? MediaIpfs { get; set; }

        /// <summary>Thumb nail image of item</summary>
        public byte[]? Thumb { get; set; }

        /// <summary>Thumb nail image IPFS</summary>
        public string? ThumbIpfs { get; set; }

        /// <summary>Has Offer</summary>
        public bool? AcceptOffer { get; set; }

        /// <summary>Unknown?</summary>
        public string? UnlockContentUrl { get; set; }

        /// <summary>Number of times the item has been viewed</summary>
        public int? ViewCount { get; set; }

        /// <summary>Number of times the item has been liked</summary>
        public int? LikeCount { get; set; }

        /// <summary>Selling Price</summary>
        public decimal? Price { get; set; }

        /// <summary>Currency</summary>
        public string? Currency { get; set; }

        /// <summary>Unique Token Id within the collection</summary>
        public int? TokenId { get; set; }

        /// <summary>Category Id (FK to Categories)</summary>
        public int? CategoryId { get; set; }

        /// <summary>Collection Id (FK to Collections)</summary>
        public int? CollectionId { get; set; }

        /// <summary>Current Owner (FK to Users)</summary>
        public int? CurrentOwner { get; set; }

        /// <summary>Author Id (FK to Users)</summary>
        public int? AuthorId { get; set; }

        /// <summary>Item Statuses</summary>
        public enum ItemStatuses {
            /// <summary>active</summary>
            active,
            /// <summary>inactive</summary>
            inactive
        }

        /// <summary>status</summary>
        public ItemStatuses Status { get; set; }

        /// <summary>Minted Date</summary>
        public DateTime? MintedDate { get; set; }

        /// <summary>Mint transaction hash</summary>
        public string? MintTrans { get; set; }

        /// <summary>Created</summary>
        public DateTime CreateDate { get; set; }

        /// <summary>Start Date of Auction</summary>
        public DateTime? StartDate { get; set; }

        /// <summary>End Date of Auction</summary>
        public DateTime? EndDate { get; set; }

        /// <summary>Is Auction Enabled?</summary>
        public bool? EnableAuction { get; set; }

        /// <summary>Item ImageType</summary>
        public string? ItemImageType { get; set; }

        /// <summary>Last Viewed</summary>
        public DateTime? LastViewed { get; set; }

        /// <summary>Auction Reservice Price</summary>
        public decimal? AuctionReserve { get; set; }
    }
}
