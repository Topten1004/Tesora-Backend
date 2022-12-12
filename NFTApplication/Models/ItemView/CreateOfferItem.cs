using System.Text.Json.Serialization;

namespace NFTApplication.Models.ItemView
{
    /// <summary>
    /// Create Offer Item
    /// </summary>
    public class CreateOfferItem
    {
        /// <summary>Item Id (FK)</summary>
        [JsonPropertyName("item_id")]
        public int ItemId { get; set; }

        /// <summary>Price</summary>
        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        /// <summary>Currency</summary>
        [JsonPropertyName("currency")]
        public string? Currency { get; set; }

    }
}
