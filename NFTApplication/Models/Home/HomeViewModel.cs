// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//
using System;
using System.Text.Json.Serialization;


namespace NFTApplication.Models.Home
{
    /// <summary>
    /// Home View Model
    /// </summary>
    public class HomeViewModel
    {
        /// <summary>Trending Collections</summary>
        [JsonPropertyName("collections")]
        public List<HomeCollection>? Collections { get; set; }

        /// <summary>Categories</summary>
        [JsonPropertyName("categories")]
        public List<HomeCategory>? Categories { get; set; }
    }
}

