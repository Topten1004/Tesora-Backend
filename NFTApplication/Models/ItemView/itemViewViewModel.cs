// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//
using System;
using System.Text.Json.Serialization;


namespace NFTApplication.Models.ItemView
{
    /// <summary>
    /// Item View View Model
    /// </summary>
    public class ItemViewViewModel
    {

        /// <summary>Unique Id</summary>
        [JsonPropertyName("collections")]
        public List<ItemViewCollection>? Collections { get; set; }

        /// <summary>Unique Id</summary>
        [JsonPropertyName("items")]
        public List<ItemViewItem>? Items { get; set; }

        /// <summary>Unique Id</summary>
        [JsonPropertyName("categories")]
        public List<ItemViewCategory>? Categories { get; set; }

        /// <summary>Unique Id</summary>
        [JsonPropertyName("offers")]
        public List<ItemViewOffer>? Offers { get; set; }

        /// <summary>Unique Id</summary>
        [JsonPropertyName("histories")]
        public List<ItemViewHistory>? Histories { get; set; }

        /// <summary>Unique Id</summary>
        [JsonPropertyName("prices")]
        public List<ItemViewPrice>? Prices { get; set; }

        /// <summary>Unique Id</summary>
        [JsonPropertyName("users")]
        public List<ItemViewUser>? Users { get; set; }
    }
}

