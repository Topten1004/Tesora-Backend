// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//
using System;
using System.Text.Json.Serialization;


namespace NFTApplication.Models.Home
{
    /// <summary>
    /// Home Collection
    /// </summary>
    public class HomeCollection
    {
        /// <summary>Collection Id (FK)</summary>
        [JsonPropertyName("collection_id")]
        public int CollectionId { get; set; }

        /// <summary>Collection Name</summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>Category Image</summary>
        [JsonPropertyName("collection_image")]
        public string? CollectionImage { get; set; }
    }
}

