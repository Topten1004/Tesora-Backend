// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//
using System;
using System.Text.Json.Serialization;



namespace NFTApplication.Models.CollectionViewDetail
{
    /// <summary>
    /// Collection View Detail View Model
    /// </summary>
    public class CollectionViewDetailViewModel
    {
        /// <summary>Collections Information</summary>
        [JsonPropertyName("collection")]
        public List<CollectionViewDetailCollection>? Collections { get; set; }

        /// <summary>Items in Collection</summary>
        [JsonPropertyName("items")]
        public List<CollectionViewDetailItem>? Items { get; set; }

        /// <summary>Categories?</summary>
        [JsonPropertyName("categories")]
        public List<CollectionViewDetailCategory>? Categories { get; set; }
    }
}

