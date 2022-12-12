// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//
using System;
using System.Text.Json.Serialization;

namespace NFTApplication.Models.CollectionViewDetail
{
    /// <summary>
    /// Collection View Detail Item
    /// </summary>
    public class CollectionViewDetailItem
    {
        /// <summary>Unique Id</summary>
        [JsonPropertyName("item_id")]
        public int ItemId { get; set; }

        /// <summary>Item Name</summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>Price</summary>
        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        /// <summary>Collection Id (FK)</summary>
        [JsonPropertyName("collection_id")]
        public int CollectionId { get; set; }

        /// <summary>Category Id (FK)</summary>
        [JsonPropertyName("category_id")]
        public int CategoryId { get; set; }

        /// <summary>Media?</summary>
        [JsonPropertyName("media")]
        public string? Media { get; set; }

        /// <summary>Item Statuses</summary>
        public enum CollectionViewDetailItemStatuses
        {
            /// <summary>active</summary>
            active,
            /// <summary>inactive</summary>
            inactive
        }

        /// <summary>Status</summary>
        [JsonPropertyName("status")]
        public CollectionViewDetailItemStatuses Status { get; set; }

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
    }
}

