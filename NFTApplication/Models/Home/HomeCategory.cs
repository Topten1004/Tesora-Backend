// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//
using System;
using System.Text.Json.Serialization;



namespace NFTApplication.Models.Home
{
    /// <summary>
    /// Home Category
    /// </summary>
    public class HomeCategory
    {
        /// <summary>Unique Id</summary>
        [JsonPropertyName("category_id")]
        public int CategoryId { get; set; }

        /// <summary>Cateogory Title</summary>
        [JsonPropertyName("title")]
        public string Title { get; set; }

        /// <summary>Category Image</summary>
        [JsonPropertyName("category_image")]
        public string CategoryImage { get; set; }

        /// <summary>Category Status: active or inactive</summary>
        [JsonPropertyName("status")]
        public string Status { get; set; }

        /// <summary>Created Date</summary>
        [JsonPropertyName("create_date")]
        public DateTime CreateDate { get; set; }
    }
}

