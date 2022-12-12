// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//
using System;
using System.Text.Json.Serialization;


namespace NFTApplication.Models.ItemView
{
    /// <summary>
    /// Item View Price
    /// </summary>
    public class ItemViewPrice
    {
        /// <summary>Unique Id</summary>
        [JsonPropertyName("price_id")]
        public int PriceId { get; set; }

        /// <summary>Item Id (FK)</summary>
        [JsonPropertyName("item_id")]
        public int ItemId { get; set; }

        /// <summary>Item Price</summary>
        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        /// <summary>User Id (FK)</summary>
        [JsonPropertyName("user_id")]
        public int UserId { get; set; }

        /// <summary>Created Date</summary>
        [JsonPropertyName("create_date")]
        public DateTime CreateDate { get; set; }
    }
}

