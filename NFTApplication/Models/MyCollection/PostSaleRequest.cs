// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//
using System;
using System.Text.Json.Serialization;

namespace NFTApplication.Models.MyCollection
{
    /// <summary>
    /// Post a sale
    /// </summary>
    public class PostSaleRequest
    {
        /// <summary>Item Id for the sale item</summary>
        [JsonPropertyName("item_id")]
        public int ItemId { get; set; }

        /// <summary>Sale Types</summary>
        public enum SaleTypes
        {
            /// <summary>Fixed Price</summary>
            Fixed,
            /// <summary>Auction</summary>
            Auction,
            /// <summary>Auction</summary>
            NotForSale
        }

        /// <summary>Sale Type</summary>
        [JsonPropertyName("sale_type")]
        public SaleTypes SaleType { get; set; }

        /// <summary>Fixed Price for Fixed Sale Type</summary>
        [JsonPropertyName("fixed_price")]
        public decimal? FixedPrice { get; set; }

        /// <summary>Currency of this request</summary>
        [JsonPropertyName("currency")]
        public string? Currency { get; set; }

        /// <summary>Auction Start Date</summary>
        [JsonPropertyName("auction_startdate")]
        public DateTime? AuctionStartDate { get; set; }

        /// <summary>Auction End Date</summary>
        [JsonPropertyName("auction_enddate")]
        public DateTime? AuctionEndDate { get; set; }

        /// <summary>If Auction Sale, Minimum Price Accepted when Auction Ends</summary>
        [JsonPropertyName("reserve_price")]
        public decimal? ReservePrice { get; set; }

        /// <summary>Is Accept Offer?</summary>
        [JsonPropertyName("accept_offer")]
        public bool? AcceptOffer { get; set; }
    }
}
