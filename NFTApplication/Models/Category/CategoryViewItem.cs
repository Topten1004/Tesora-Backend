using System.Text.Json.Serialization;

namespace NFTApplication.Models.Category
{
    public class CategoryViewItem
    {
        /// <summary>category id pk</summary>
        [JsonPropertyName("category_id")]
        public int? CategoryId { get; set; }

        /// <summary>category title</summary>
        [JsonPropertyName("category_title")]
        public string? Title { get; set; }

        /// <summary>category image</summary>
        [JsonPropertyName("category_image")]
        public string? Image { get; set; }
    }
}
