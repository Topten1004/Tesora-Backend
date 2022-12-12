// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//
using System;
using System.Text.Json.Serialization;


namespace NFTApplication.Models.MyCollection
{
    /// <summary>
    /// My Collection 
    /// </summary>
    public class GetMyCollectionResponse
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

        /// <summary>Collection Statuses</summary>
        public enum MyCollectionCollectionStatuses
        {
            /// <summary>active</summary>
            active,
            /// <summary>inactive</summary>
            inactive
        }

        /// <summary>Royalties to Author</summary>
        [JsonPropertyName("Royalties")]
        public decimal Royalties { get; set; }

        /// <summary>Items in Collection</summary>
        [JsonPropertyName("item_count")]
        public int ItemCount { get; set; }

        /// <summary>Volume Traded in Collection</summary>
        [JsonPropertyName("volume_traded")]
        public int VolumeTraded { get; set; }

        /// <summary>Status</summary>
        [JsonPropertyName("status")]
        public MyCollectionCollectionStatuses Status { get; set; }

        /// <summary>Author</summary>
        [JsonPropertyName("author")]
        public MyCollectionUser? Author { get; set; }
    }
}

