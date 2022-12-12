// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//
using System;
using System.Text.Json.Serialization;


namespace NFTApplication.Models.MarketPlace
{
    /// <summary>
    /// Market Place Collection
    /// </summary>
    public class MarketPlaceCollection
    {
        /// <summary>Unique Id</summary>
        [JsonPropertyName("collection_id")]
        public int CollectionId { get; set; }

        /// <summary>Collection Name</summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>Banner?</summary>
        [JsonPropertyName("banner")]
        public string? Banner { get; set; }

        /// <summary>Contract Image</summary>
        [JsonPropertyName("image")]
        public string? Image { get; set; }

        /// <summary>Item Count</summary>
        [JsonPropertyName("item_count")]
        public int? ItemCount { get; set; }

        /// <summary>Collection Statuses</summary>
        public enum MarketPlaceCollectionStatuses
        {
            /// <summary>active</summary>
            active,
            /// <summary>inactive</summary>
            inactive
        }

        /// <summary>Status</summary>
        [JsonPropertyName("status")]
        public MarketPlaceCollectionStatuses Status { get; set; }

    }
}

