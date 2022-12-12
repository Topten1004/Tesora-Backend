// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//
using System.Text.Json.Serialization;

namespace NFTApplication.Models.MyCollection
{
    /// <summary>
    /// Add Collection Request
    /// </summary>
    public class CollectionViewModel
    {
        /// <summary>Collection Id</summary>
        [JsonPropertyName("collectionId")]
        public int? CollectionId { get; set; }

        /// <summary>Collection Name</summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>Collection Description</summary>
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        /// <summary>Contract symbol</summary>
        [JsonPropertyName("contractSymbol")]
        public string? ContractSymbol { get; set; }

        /// <summary>Banner</summary>
        [JsonPropertyName("banner")]
        public IFormFile Banner { get; set; }

        /// <summary>Collection Image</summary>
        [JsonPropertyName("collectionImage")]
        public IFormFile CollectionImage { get; set; }

        /// <summary>Royalties paid to the author on each sale.  Entered as a percentage</summary>
        [JsonPropertyName("royalities")]
        public decimal? Royalties { get; set; }
    }
}
