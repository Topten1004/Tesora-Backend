// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//

using NFTApplication.Models.Category;
using NFTApplication.Models.Collection;

namespace NFTApplication.Models.MyOffer
{
    /// <summary>
    /// My Offer Response
    /// </summary>
    public class GetMyOfferResponse
    {
        /// <summary>Primary Key</summary>
        public int OfferId { get; set; }

        /// <summary>Collection Info</summary>
        public CollectionViewItem? Collection { get; set; }

        /// <summary>Category Info</summary>
        public CategoryViewItem? Category { get; set; }

        /// <summary>key of Item fk</summary>
        public int ItemId { get; set; }

        /// <summary>Name of Item</summary>
        public string? Name { get; set; }

        /// <summary>Price of Item</summary>
        public decimal? Price { get; set; }

        /// <summary>Currency</summary>
        public string? Currency { get; set; }

        /// <summary>Username for user</summary>
        public string? Bidder { get; set; }

        /// <summary>NFT Image</summary>
        public string? ImageIpfs { get; set; }

        /// <summary>Description of Item</summary>
        public string? Description { get; set; }
    }
}
