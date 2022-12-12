// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//

namespace NFTDatabaseEntities
{
    /// <summary>
    /// Auction
    /// </summary>
    public class Auction
    {
        /// <summary>Primary Key</summary>
        public int AuctionId { get; set; }

        /// <summary>Item Id (FK to Items)</summary>
        public int? ItemId { get; set; }

        /// <summary>Sender Id (FK to Users)</summary>
        public int? SenderId { get; set; }

        /// <summary>Receiver Id (FK to Users)</summary>
        public int? ReceiverId { get; set; }

        /// <summary>Price of Item</summary>
        public decimal? Price { get; set; }

        /// <summary>Currency</summary>
        public string? Currency { get; set; }

        /// <summary>Created</summary>
        public DateTime CreateDate { get; set; }

    }
}
