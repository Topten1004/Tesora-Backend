// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//
using System;
using System.Text.Json.Serialization;


namespace NFTApplication.Models.MarketPlace
{
    /// <summary>
    /// Market Place Item
    /// </summary>
    public class MarketPlaceItem
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
        public enum MarketPlaceItemStatuses
        {
            /// <summary>active</summary>
            active,
            /// <summary>inactive</summary>
            inactive
        }

        /// <summary>Status</summary>
        [JsonPropertyName("status")]
        public MarketPlaceItemStatuses Status { get; set; }

        /// <summary>Enable Auction?</summary>
        [JsonPropertyName("enable_auction")]
        public bool? EnableAuction { get; set; }
    }
}

