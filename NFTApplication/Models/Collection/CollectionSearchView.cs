using System.Text.Json.Serialization;

namespace NFTApplication.Models.Collection
{
    public class CollectionSearchView
    {
        /// <summary>Search String</summary>
        [JsonPropertyName("search_string")]
        public string? search { get; set; }

        /// <summary>Collection Only</summary>
        [JsonPropertyName("collection_only")]
        public bool? onlyCollection { get; set; }
    }
}
