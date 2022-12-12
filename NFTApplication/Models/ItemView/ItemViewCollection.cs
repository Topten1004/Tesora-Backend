// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//
using System;
using System.Text.Json.Serialization;



namespace NFTApplication.Models.ItemView
{
    /// <summary>
    /// Item View Collection
    /// </summary>
    public class ItemViewCollection
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
        public byte[]? Image { get; set; }

        /// <summary>Collection Statuses</summary>
        public enum ItemViewCollectionStatuses
        {
            /// <summary>active</summary>
            active,
            /// <summary>inactive</summary>
            inactive
        }

        /// <summary>Status</summary>
        [JsonPropertyName("status")]
        public ItemViewCollectionStatuses Status { get; set; }

        /// <summary>Author Information</summary>
        [JsonPropertyName("author")]
        public ItemViewUser? Author { get; set; }

        /// <summary>Created Date</summary>
        [JsonPropertyName("create_date")]
        public DateTime CreateDate { get; set; }

        /// <summary>Chain Id</summary>
        [JsonPropertyName("chain_id")]
        public int ChainId { get; set; }
    }
}

