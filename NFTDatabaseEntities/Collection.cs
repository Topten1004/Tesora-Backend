// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//

namespace NFTDatabaseEntities
{
    /// <summary>
    /// Collection
    /// </summary>
    public class Collection
    {
        /// <summary>Primary Key</summary>
        public int CollectionId { get; set; }

        /// <summary>Collection Name</summary>
        public string Name { get; set; }

        /// <summary>Collection Description</summary>
        public string? Description { get; set; }

        /// <summary>Contract symbol</summary>
        public string? ContractSymbol { get; set; }

        /// <summary>Contract Address</summary>
        public string? ContractAddress { get; set; }

        /// <summary>Banner</summary>
        public byte[]? Banner { get; set; }

        /// <summary>Banner Image Type</summary>
        public string? BannerImageType { get; set; }

        /// <summary>Collection Image</summary>
        public byte[]? CollectionImage { get; set; }

        /// <summary>Collection Image Type</summary>
        public string? CollectionImageType { get; set; }

        /// <summary>Collection Image IPFS</summary>
        public string? CollectionImageIpfs { get; set; }

        /// <summary>Royalties paid to the author on each sale.  Entered as a percentage</summary>
        public decimal? Royalties { get; set; }

        /// <summary>Number of times the contract has traded</summary>
        public int? VolumeTraded { get; set; }

        /// <summary>Number of items in the collection</summary>
        public int? ItemCount { get; set; }

        /// <summary>
        /// Collection Statuses
        /// </summary>
        public enum CollectionStatuses {
            /// <summary>active</summary>
            active,
            /// <summary>inactive</summary>
            inactive
            }

        /// <summary>Status of Collection, active or inactive</summary>
        public CollectionStatuses Status { get; set; }

        /// <summary>Author Id (FK to users)</summary>
        public int? AuthorId { get; set; }

        /// <summary>Created</summary>
        public DateTime CreateDate { get; set; }

        /// <summary>Transaction Hash</summary>
        public string? TransactionHash { get; set; }

        /// <summary>Chain Id</summary>
        public int ChainId { get; set; }

    }
}
