// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//

using System.Text.Json.Serialization;
using NFTDatabaseEntities;


namespace NFTApplicationAdmin.Models
{
    /// <summary>
    /// Update Category Request
    /// </summary>
    public class UpdateCategoryRequest
    {
        /// <summary>Category Id</summary>
        [JsonPropertyName("category_id")]
        public int CategoryId { get; set; }

        /// <summary>Category Title</summary>
        [JsonPropertyName("title")]
        public string Title { get; set; }

        /// <summary>Category Image</summary>
        [JsonPropertyName("image")]
        public IFormFile? Image { get; set; }

        /// <summary>Category Status: active or inactive</summary>
        [JsonPropertyName("status")]
        public Category.CategoryStatuses Status { get; set; }

        /// <summary>Contract Id (FK)</summary>
        [JsonPropertyName("contractId")]
        public int ContractId { get; set; }
    }
}
