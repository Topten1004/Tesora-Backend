// <copyright company="MyCOM Global LTD"7 author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//
using System;
using System.Text.Json.Serialization;


namespace NFTApplication.Models.CollectionViewDetail
{
    /// <summary>
    /// Create Collection request
    /// </summary>
    public class CreateCollectionRequest
    {
        /// <summary>Collection Name</summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>Description</summary>
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        /// <summary>Royalties to Author</summary>
        [JsonPropertyName("Royalties")]
        public decimal Royalties { get; set; }

        /// <summary>Contract Symbol</summary>
        [JsonPropertyName("symbol")]
        public string? Symbol { get; set; }

        /// <summary>Banner?</summary>
        [JsonPropertyName("banner")]
        public IFormFile? Banner { get; set; }

        /// <summary>Collection Image</summary>
        [JsonPropertyName("image")]
        public IFormFile? Image { get; set; }
    }
}

