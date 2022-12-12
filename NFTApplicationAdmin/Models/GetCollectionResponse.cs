// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//

using System.Text.Json.Serialization;

namespace NFTApplicationAdmin.Models
{
    /// <summary>
    /// Get Category Response
    /// </summary>
    public class GetCollectionResponse
    {
        /// <summary>Primary Key</summary>
        public int CollectionId { get; set; }

        /// <summary>Collection Name</summary>
        public string Name { get; set; }

        /// <summary>Collection Description</summary>
        public string? Description { get; set; }

        /// <summary>Banner</summary>
        public string? Banner { get; set; }

        /// <summary>Banner Image Type</summary>
        public string? BannerImageType { get; set; }

        /// <summary>Collection Image</summary>
        public string? CollectionImage { get; set; }

        /// <summary>Collection Image Type</summary>
        public string? CollectionImageType { get; set; }

        /// <summary>Royalties paid to the author on each sale.  Entered as a percentage</summary>
        public decimal? Royalties { get; set; }

        /// <summary>Status of Collection, active or inactive</summary>
        public String Status { get; set; }

        /// <summary>Created</summary>
        public DateTime CreateDate { get; set; }
    }
}
