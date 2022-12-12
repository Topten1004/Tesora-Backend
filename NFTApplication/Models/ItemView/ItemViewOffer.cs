// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//
using System;
using System.Text.Json.Serialization;


namespace NFTApplication.Models.ItemView
{
    /// <summary>
    /// Item View Offer
    /// </summary>
    public class ItemViewOffer
    {
        /// <summary>Unique Id</summary>
        [JsonPropertyName("offer_id")]
        public int OfferId { get; set; }

        /// <summary>Item Id (FK)</summary>
        [JsonPropertyName("item_id")]
        public int ItemId { get; set; }

        /// <summary>Sender User Info</summary>
        [JsonPropertyName("sender")]
        public ItemViewUser Sender { get; set; }

        /// <summary>Receiver User Info</summary>
        [JsonPropertyName("receiver")]
        public ItemViewUser Receiver { get; set; }

        /// <summary>Price</summary>
        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        /// <summary>Price Display</summary>
        [JsonPropertyName("priceDisplay")]
        public string PriceDisplay { get; set; }

        /// <summary>Currency</summary>
        [JsonPropertyName("currency")]
        public string? Currency { get; set; }

        /// <summary>Created Date</summary>
        [JsonPropertyName("create_date")]
        public DateTime CreateDate { get; set; }
    }
}

