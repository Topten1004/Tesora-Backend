// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//

using System.Text.Json.Serialization;



namespace NFTApplication.Models.ItemView
{
    /// <summary>
    /// Create Item Attribute
    /// </summary>
    public class CreateItemAttribute
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
    public class CreateItemRequest
    {
        /// <summary>Item Name</summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>Item Description</summary>
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        /// <summary>Thumb Image</summary>
        [JsonPropertyName("thumb")]
        public IFormFile? Thumb { get; set; }

        /// <summary>Item Media Image</summary>
        [JsonPropertyName("media")]
        public IFormFile? Media { get; set; }

        /// <summary>Item Category Id</summary>
        [JsonPropertyName("categoryId")]
        public int? CategoryId { get; set; }

        /// <summary>Item Collection Id</summary>
        [JsonPropertyName("collectionId")]
        public int CollectionId { get; set; }

        /// <summary>Item Attributes</summary>
        [JsonPropertyName("attributes")]
        public List<CreateItemAttribute>? Attributes { get; set; }

        /// <summary>Item Price</summary>
        [JsonPropertyName("price")]
        public decimal? Price { get; set; }

        /// <summary>Item Status</summary>
        [JsonPropertyName("status")]
        public NFTDatabaseEntities.Item.ItemStatuses Status { get; set; }

        /// <summary>Auction Start Date</summary>
        [JsonPropertyName("startDate")]
        public DateTime? StartDate { get; set; }

        /// <summary>Auction End Date</summary>
        [JsonPropertyName("endDate")]
        public DateTime? EndDate { get; set; }

        /// <summary>Item Avaialbe for Auction</summary>
        [JsonPropertyName("enableAuction")]
        public bool? EnableAuction { get; set; }
    }
}
