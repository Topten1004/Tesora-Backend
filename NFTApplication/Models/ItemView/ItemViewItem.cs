// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//
using System;
using System.Text.Json.Serialization;


namespace NFTApplication.Models.ItemView
{
    /// <summary>
    /// Item View Item
    /// </summary>
    public class ItemViewItem
    {
        /// <summary>Unique Id</summary>
        [JsonPropertyName("item_id")]
        public int ItemId { get; set; }

        /// <summary>Item Name</summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>Item Description</summary>
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        /// <summary>Price</summary>
        [JsonPropertyName("price")]
        public decimal? Price { get; set; }

        /// <summary>Price</summary>
        [JsonPropertyName("priceDisplay")]
        public string? PriceDisplay { get; set; }

        /// <summary>Currency</summary>
        [JsonPropertyName("currency")]
        public string? Currency { get; set; }

        /// <summary>Collection Id (FK)</summary>
        [JsonPropertyName("collection_id")]
        public ItemViewCollection? Collection { get; set; }

        /// <summary>Category Id (FK)</summary>
        [JsonPropertyName("category_id")]
        public ItemViewCategory? Category { get; set; }

        /// <summary>Media</summary>
        [JsonPropertyName("media")]
        public byte[]? Media { get; set; }

        /// <summary>Media IPFS</summary>
        [JsonPropertyName("mediaIpfs")]
        public string? MediaIPFS { get; set; }

        /// <summary>External Link to view Media in our web site</summary>
        [JsonPropertyName("external_link")]
        public string? ExternalLink { get; set; }

        /// <summary>Thumbnail Image</summary>
        [JsonPropertyName("thumb")]
        public byte[]? Thumb { get; set; }

        /// <summary>Thumbnail IPFS</summary>
        [JsonPropertyName("thumbIpfs")]
        public string? ThumbIPFS { get; set; }

        /// <summary>Has Offer</summary>
        [JsonPropertyName("accept_offer")]
        public bool? AcceptOffer { get; set; }

        /// <summary>Attributes</summary>
        [JsonPropertyName("attributes")]
        public Dictionary<string, string>? Attributes { get; set; }

        /// <summary>Levels</summary>
        [JsonPropertyName("levels")]
        public Dictionary<string, string>? Levels { get; set; }

        /// <summary>Stats</summary>
        [JsonPropertyName("stats")]
        public Dictionary<string, string>? Stats { get; set; }

        /// <summary>Unlock Content Url</summary>
        [JsonPropertyName("unlock_content_url")]
        public string? UnlockContentUrl { get; set; }

        /// <summary>View Count</summary>
        [JsonPropertyName("view_count")]
        public int ViewCount { get; set; }

        /// <summary>Like Count</summary>
        [JsonPropertyName("like_count")]
        public int LikeCount { get; set; }

        /// <summary>Token Id</summary>
        [JsonPropertyName("token_id")]
        public int TokenId { get; set; }

        /// <summary>Current Owner</summary>
        [JsonPropertyName("current_owner")]
        public ItemViewUser? CurrentOwner { get; set; }

        /// <summary>Status</summary>
        [JsonPropertyName("status")]
        public NFTDatabaseEntities.Item.ItemStatuses Status { get; set; }

        /// <summary>Minted Date</summary>
        [JsonPropertyName("minted_date")]
        public DateTime MintedDate { get; set; }

        /// <summary>Created Date</summary>
        [JsonPropertyName("create_date")]
        public DateTime CreateDate { get; set; }

        /// <summary>Auction Start Date</summary>
        [JsonPropertyName("start_date")]
        public DateTime? StartDate { get; set; }

        /// <summary>Auction End Date</summary>
        [JsonPropertyName("end_date")]
        public DateTime? EndDate { get; set; }

        /// <summary>Enable Auction?</summary>
        [JsonPropertyName("enable_auction")]
        public bool? EnableAuction { get; set; }

        /// <summary>Auction Reserve</summary>
        [JsonPropertyName("auction_reserve")]
        public decimal? AuctionReserve { get; set; }

        /// <summary>List of Offers </summary>
        [JsonPropertyName("offers")]
        public List<ItemViewOffer>? Offers { get; set; }

        /// <summary>List of Auctions </summary>
        [JsonPropertyName("auctions")]
        public List<ItemViewAuction>? Auctions { get; set; }

        /// <summary>List of Historys </summary>
        [JsonPropertyName("histories")]
        public List<ItemViewHistory>? Histories { get; set; }
    }
}

