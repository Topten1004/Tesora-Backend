// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//
using System;
using System.Text.Json.Serialization;


namespace NFTApplication.Models.CollectionViewDetail
{
    /// <summary>
    /// Edit Collection Response
    /// </summary>
    public class EditCollectionResponse
    {
        /// <summary>Unique Id</summary>
        [JsonPropertyName("collection_id")]
        public int CollectionId { get; set; }

        /// <summary>Collection Name</summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>Description</summary>
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        /// <summary>Contract Symbol</summary>
        [JsonPropertyName("contract_symbol")]
        public string? ContractSymbol { get; set; }

        /// <summary>Contract Address</summary>
        [JsonPropertyName("contract_address")]
        public string? ContractAddress { get; set; }

        /// <summary>Banner?</summary>
        [JsonPropertyName("banner")]
        public string? Banner { get; set; }

        /// <summary>Contract Image</summary>
        [JsonPropertyName("image")]
        public string? Image { get; set; }

        /// <summary>Royalties to Author</summary>
        [JsonPropertyName("royalties")]
        public decimal Royalties { get; set; }

        /// <summary>Volume Traded</summary>
        [JsonPropertyName("volume_traded")]
        public int VolumeTraded { get; set; }

        /// <summary>Items in Collection</summary>
        [JsonPropertyName("item_count")]
        public int ItemCount { get; set; }

        /// <summary>Collection Statuses</summary>
        public enum EditCollectionResponseStatuses
        {
            /// <summary>active</summary>
            active,
            /// <summary>inactive</summary>
            inactive
        }

        /// <summary>Status</summary>
        [JsonPropertyName("status")]
        public EditCollectionResponseStatuses Status { get; set; }

        /// <summary>Author Information</summary>
        [JsonPropertyName("author")]
        public CollectionViewDetailUser Author { get; set; }

        /// <summary>Created Date</summary>
        [JsonPropertyName("create_date")]
        public DateTime CreateDate { get; set; }

    }
}

