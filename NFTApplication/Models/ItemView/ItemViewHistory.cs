// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//
using System;
using System.Text.Json.Serialization;


namespace NFTApplication.Models.ItemView
{
    /// <summary>
    /// Item View History
    /// </summary>
    public class ItemViewHistory
    {
        /// <summary>Unique Record Id</summary>
        [JsonPropertyName("history_id")]
        public int HistoryId { get; set; }

        /// <summary>Item Id (FK)</summary>
        [JsonPropertyName("item_id")]
        public int ItemId { get; set; }

        /// <summary>Collection Id (FK)</summary>
        [JsonPropertyName("collection_id")]
        public int CollectionId { get; set; }

        /// <summary>From User Info</summary>
        [JsonPropertyName("from_user")]
        public ItemViewUser? From { get; set; }

        /// <summary>To User Info</summary>
        [JsonPropertyName("to_user")]
        public ItemViewUser? To { get; set; }

        /// <summary>Transaction Hash</summary>
        [JsonPropertyName("transaction_hash")]
        public string? TransactionHash { get; set; }

        /// <summary>Price</summary>
        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        /// <summary>Currency</summary>
        [JsonPropertyName("currency")]
        public string? Currency { get; set; }

        /// <summary>History Types</summary>
        public enum ItemViewHistoryTypes
        {
            /// <summary>created</summary>
            created,
            /// <summary>minted</summary>
            minted,
            /// <summary>bid</summary>
            bid,
            /// <summary>transfer</summary>
            transfer
        }

        /// <summary>History Type</summary>
        [JsonPropertyName("history_type")]
        public ItemViewHistoryTypes HistoryType { get; set; }

        /// <summary>Is Valid?</summary>
        [JsonPropertyName("is_valid")]
        public bool? IsValid { get; set; }

        /// <summary>Created Date</summary>
        [JsonPropertyName("create_date")]
        public DateTime CreateDate { get; set; }
    }
}

