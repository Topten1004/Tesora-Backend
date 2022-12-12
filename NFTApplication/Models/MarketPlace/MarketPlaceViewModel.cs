// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//
using System;
using System.Text.Json.Serialization;


namespace NFTApplication.Models.MarketPlace
{
    /// <summary>
    /// Market Place View Model
    /// </summary>
    public class MarketPlaceViewModel
    {
        /// <summary>Collections</summary>
        [JsonPropertyName("collections")]
        public List<MarketPlaceCollection>? Collections { get; set; }

        /// <summary>Categories</summary>
        [JsonPropertyName("categories")]
        public List<MarketPlaceCategory>? Categories { get; set; }
    }
}

