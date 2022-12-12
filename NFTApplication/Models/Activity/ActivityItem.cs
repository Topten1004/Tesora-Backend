// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//
using System;
using System.Text.Json.Serialization;


namespace NFTApplication.Models.Activity
{
    /// <summary>
    /// Activity Item
    /// </summary>
    public class ActivityItem
    {
        /// <summary>Unique Record Id</summary>
        [JsonPropertyName("item_id")]
        public int ItemId { get; set; }

        /// <summary>Item Name</summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>Media?</summary>
        [JsonPropertyName("media")]
        public string? Media { get; set; }
    }
}

