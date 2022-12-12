// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//
using System;
using System.Text.Json.Serialization;



namespace NFTApplication.Models.Activity
{
    /// <summary>
    /// Activity History
    /// </summary>
    public class ActivityHistory
    {
        /// <summary>Unique Record Id</summary>
        [JsonPropertyName("history_id")]
        public int HistoryId { get; set; }

        /// <summary>Item Information</summary>
        [JsonPropertyName("item")]
        public ActivityItem? Item { get; set; }

        /// <summary>Collection Information</summary>
        [JsonPropertyName("collection")]
        public ActivityCollection? Collection {get;set;}

        /// <summary>User Information</summary>
        [JsonPropertyName("from")]
        public ActivityUser? From { get; set; }

        /// <summary>User Information</summary>
        [JsonPropertyName("to")]
        public ActivityUser? To { get; set; }

        /// <summary>Transaction Hash</summary>
        [JsonPropertyName("transaction_hash")]
        public string? TransactionHash { get; set; }

        /// <summary>Price</summary>
        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        /// <summary>History Types</summary>
        public enum ActiveHistoryTypes {
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
        public ActiveHistoryTypes HistoryType { get; set; }

        /// <summary>Is Valid?</summary>
        [JsonPropertyName("is_valid")]
        public bool? IsValid { get; set; }

        /// <summary>Created Date</summary>
        [JsonPropertyName("create_date")]
        public DateTime CreateDate { get; set; }

    }
}

