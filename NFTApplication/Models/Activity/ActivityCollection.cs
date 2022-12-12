// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//
using System;
using System.Text.Json.Serialization;


namespace NFTApplication.Models.Activity
{
    /// <summary>
    /// Activity Collection
    /// </summary>
    public class ActivityCollection
    {
        /// <summary>Collection Id (FK)</summary>
        [JsonPropertyName("collection_id")]
        public int CollectionId { get; set; }

        /// <summary>Collection Name</summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>Image for Collection</summary>
        [JsonPropertyName("image")]
        public byte[]? Image { get; set; }
    }
}

