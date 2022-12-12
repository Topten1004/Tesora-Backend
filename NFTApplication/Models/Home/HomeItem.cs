// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//
using System;
using System.Text.Json.Serialization;


namespace NFTApplication.Models.Home
{
    /// <summary>
    /// Home Item
    /// </summary>
    public class HomeItem
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
        public enum HomeItemStatuses
        {
            /// <summary>active</summary>
            active,
            /// <summary>inactive</summary>
            inactive
        }

        /// <summary>Status</summary>
        [JsonPropertyName("status")]
        public HomeItemStatuses Status { get; set; }

        /// <summary>Enable Auction?</summary>
        [JsonPropertyName("enable_auction")]
        public bool? EnableAuction { get; set; }

    }
}

