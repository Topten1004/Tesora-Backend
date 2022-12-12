using System.Text.Json.Serialization;

namespace NFTApplication.Models.MyCollection
{
    /// <summary>
    /// Add Collection Request
    /// </summary>
    public class UpdateCollectionRequest
    {
        /// <summary>Collection ID pk</summary>
        public int CollectionId { get; set; }

        /// <summary>Collection Name</summary>
        public string Name { get; set; }

        /// <summary>Collection Description</summary>
        public string? Description { get; set; }

        /// <summary>Contract symbol</summary>
        public string? ContractSymbol { get; set; }

        /// <summary>Banner</summary>
        public IFormFile Banner { get; set; }

        /// <summary>Collection Image</summary>
        public IFormFile CollectionImage { get; set; }

        /// <summary>Royalties paid to the author on each sale.  Entered as a percentage</summary>
        public decimal? Royalties { get; set; }

    }
}
