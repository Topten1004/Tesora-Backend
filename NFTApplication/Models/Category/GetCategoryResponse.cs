using System.Text.Json.Serialization;

namespace NFTApplication.Models.Category
{
    /// <summary>
    /// Get Category Response
    /// </summary>
    public class GetCategoryResponse
    {
        /// <summary>Category Id</summary>
        public int CategoryId { get; set; }

        /// <summary>Contract Id (FK)</summary>
        [JsonPropertyName("contractId")]
        public int ContractId { get; set; }
        /// <summary>Title</summary>
        public string Title { get; set; }

        /// <summary>Category Image</summary>
        public string CategoryImage { get; set; }

        /// <summary>Status</summary>
        public string Status { get; set; }

        /// <summary>Create Date</summary>
        public DateTime CreateDate { get; set; }
    }
}