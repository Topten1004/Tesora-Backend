// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json.Serialization;
using static NFTDatabaseEntities.Item;

namespace NFTApplication.Models.MyCollection
{
    /// <summary>
    /// Item Attribute
    /// </summary>
    public class AddItemAttribute
    {
        /// <summary>Item Attribute Type</summary>
        [JsonPropertyName("itemType")]
        public NFTDatabaseEntities.ItemAttribute.ItemTypes ItemType { get; set; }

        /// <summary>Item Attribute Type</summary>
        [JsonPropertyName("title")]
        public string Title { get; set; }

        /// <summary>Item Attribute Type</summary>
        [JsonPropertyName("value")]
        public string Value { get; set; }

        /// <summary>Item Attribute Type</summary>
        [JsonPropertyName("maxValue")]
        public string? MaxValue { get; set; }
    }

    /// <summary>
    /// Create Item Request
    /// </summary>
    public class ItemViewModel
    {
        /// <summary>Construction</summary>
        public ItemViewModel()
        {
            Attributes = new List<AddItemAttribute>();
        }

        /// <summary>Item Id</summary>
        [JsonPropertyName("itemId")]
        public int? ItemId { get; set; }

        /// <summary>Item Name</summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>Item Collection Id</summary>
        [JsonPropertyName("collectionId")]
        public int CollectionId { get; set; }

        /// <summary>Item Description</summary>
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        /// <summary>Price</summary>
        [JsonPropertyName("price")]
        public decimal? Price{ get; set; }

        /// <summary>Auction Reserve</summary>
        [JsonPropertyName("auctionReserve")]
        public decimal? AuctionReserve { get; set; }

        /// <summary>Currency</summary>
        [JsonPropertyName("currency")]
        public string? Currency { get; set; }

        /// <summary>External Link</summary>
        [JsonPropertyName("externalLink")]
        public string?  ExternalLink { get; set; }

        /// <summary>Item Media Image</summary>
        [JsonPropertyName("media")]
        public IFormFile? Media { get; set; }

        /// <summary>Item Thumb Image</summary>
        [JsonPropertyName("thumb")]
        public IFormFile? Thumb { get; set; }

        /// <summary>Item Category Id</summary>
        [JsonPropertyName("categoryId")]
        public int? CategoryId { get; set; }

        /// <summary>Item Attributes</summary>
        [JsonPropertyName("attributes")]
        public List<AddItemAttribute>? Attributes { get; set; }

        /// <summary>Enable Auction</summary>
        [JsonPropertyName("enableAuction")]
        public bool EnableAuction { get; set; }

        /// <summary>Accept Offer</summary>
        [JsonPropertyName("acceptOffer")]
        public bool AcceptOffer { get; set; }

        /// <summary>Status</summary>
        [JsonPropertyName("status")]
        public ItemStatuses Status { get; set; }

        /// <summary>Start Date of Auction</summary>
        [JsonPropertyName("startDate")]
        public DateTime? StartDate { get; set; }

        /// <summary>End Date of Auction</summary>
        [JsonPropertyName("endDate")]
        public DateTime? EndDate { get; set; }
    }
}
