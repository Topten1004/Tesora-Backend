// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//

using static NFTDatabaseEntities.Item;

namespace NFTDatabaseEntities
{
    /// <summary>
    /// Item
    /// </summary>
    public class ItemSale
    {
        /// <summary>Primary Key</summary>
        public int ItemId { get; set; }

        /// <summary>Selling Price</summary>
        public decimal? Price { get; set; }

        /// <summary>Currency</summary>
        public string? Currency { get; set; }

        /// <summary>status</summary>
        public ItemStatuses Status { get; set; }

        /// <summary>Start Date of Auction</summary>
        public DateTime? StartDate { get; set; }

        /// <summary>End Date of Auction</summary>
        public DateTime? EndDate { get; set; }

        /// <summary>Is Auction Enabled?</summary>
        public bool? EnableAuction { get; set; }

        /// <summary>Is Accept Offer?</summary>
        public bool? AcceptOffer{ get; set; }

        /// <summary>Auction Reserve Price</summary>
        public decimal? AuctionReserve { get; set; }
    }
}
