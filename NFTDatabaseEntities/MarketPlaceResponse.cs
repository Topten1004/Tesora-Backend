// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//

using Microsoft.Extensions.Primitives;
using System.Text.Json.Serialization;

namespace NFTDatabaseEntities
{
    /// <summary>
    /// Market
    /// </summary>
    public class MarketPlaceResponse
    {
        /// <summary>
        /// Card Types
        /// </summary>
        public enum CardTypes
        {
            /// <summary>Item Information Provided</summary>
            Item,
            /// <summary>Collection Information Provided</summary>
            Collection
        }

        /// <summary>
        /// Card Type - Determines the information that is provided
        /// </summary>
        public CardTypes CardType { get; set; }

        /// <summary>Primary Key</summary>
        public int? ItemId { get; set; }

        /// <summary>Item Name</summary>
        public string? Name { get; set; }

        /// <summary>Image of item</summary>
        public string? Media { get; set; }

        /// <summary>Collection Item</summary>
        public CollectionView? Collection { get; set; }

        /// <summary>Category Item</summary>
        public CategoryView? Category { get; set; }

        /// <summary>Selling Price</summary>
        public decimal? Price { get; set; }

        /// <summary>Currency</summary>
        public string? Currency { get; set; }

        /// <summary>View Count</summary>
        public int? ViewCount { get; set; }

        /// <summary>Like Count</summary>
        public int? LikeCount { get; set; }

        /// <summary>Accept Offers</summary>
        public bool? AcceptOffer { get; set; }

        /// <summary>Currently on auction</summary>
        public bool? EnableAuction { get; set; }

        /// <summary>Item Count</summary>
        public int? ItemCount { get; set; }
    }
}
