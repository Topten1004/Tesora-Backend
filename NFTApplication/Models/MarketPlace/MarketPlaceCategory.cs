// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//
using System;
using System.Text.Json.Serialization;


namespace NFTApplication.Models.MarketPlace
{
    /// <summary>
    /// Market Place Category
    /// </summary>
    public class MarketPlaceCategory
    {
        /// <summary>Unique Id</summary>
        [JsonPropertyName("category_id")]
        public int CategoryId { get; set; }

        /// <summary>Cateogory Title</summary>
        [JsonPropertyName("title")]
        public string? Title { get; set; }

        /// <summary>Category Image</summary>
        [JsonPropertyName("category_image")]
        public string? Category { get; set; }

        /// <summary>Category Statuses</summary>
        public enum MarketPlaceCategoryStatuses
        {
            /// <summary>active</summary>
            active,
            /// <summary>inactive</summary>
            inactive
        }

        /// <summary>Category Status: active or inactive</summary>
        [JsonPropertyName("status")]
        public MarketPlaceCategoryStatuses Status { get; set; }

    }
}

