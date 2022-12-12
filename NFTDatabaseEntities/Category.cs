// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//

namespace NFTDatabaseEntities
{
    /// <summary>
    /// Category
    /// </summary>
    public class Category
    {
        /// <summary>Primary key</summary>
        public int CategoryId { get; set; }

        /// <summary>Contract Id (FK to contract)</summary>
        public int ContractId { get; set; }

        /// <summary>Category Title</summary>
        public string Title { get; set; }

        /// <summary>Category Image</summary>
        public byte[]? CategoryImage { get; set; }

        /// <summary>Category ImageType</summary>
        public string CategoryImageType { get; set; }

        /// <summary>Category Statuses</summary>
        public enum CategoryStatuses {
            /// <summary>active</summary>
            active,
            /// <summary>inactive</summary>
            inactive
            }

        /// <summary>Category Status: active or inactive</summary>
        public CategoryStatuses Status { get; set; }

        /// <summary>Created date</summary>
        public DateTime CreateDate { get; set; }

    }
}
