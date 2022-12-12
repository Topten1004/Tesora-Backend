// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//
using System;
using System.Text.Json.Serialization;


namespace NFTApplication.Models.Profile
{
    /// <summary>
    /// Profile Offer
    /// </summary>
    public class ProfileOffer
    {
        /// <summary>Unique Id</summary>
        [JsonPropertyName("offer_id")]
        public int OfferId { get; set; }

        /// <summary>Item Id (FK)</summary>
        [JsonPropertyName("item_id")]
        public int ItemId { get; set; }

        /// <summary>Sender Id (FK)</summary>
        [JsonPropertyName("sender_id")]
        public int SenderId { get; set; }

        /// <summary>Receiver Id (FK)</summary>
        [JsonPropertyName("receiver_id")]
        public int ReceiverId { get; set; }

        /// <summary>Price</summary>
        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        /// <summary>Created Date</summary>
        [JsonPropertyName("create_date")]
        public DateTime CreateDate { get; set; }
    }
}

