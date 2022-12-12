using System.Text.Json.Serialization;

namespace NFTApplication.Models.Collection
{
    public class CollectionViewItem
    {
        /// <summary>collection id pk</summary>
        [JsonPropertyName("collection_id")]
        public int CollectionId { get; set; }

        /// <summary>collection name</summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>collection image</summary>
        [JsonPropertyName("collection_image")]
        public string? Image { get; set; }
    }
}
