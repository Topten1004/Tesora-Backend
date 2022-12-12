// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//

namespace NFTDatabaseEntities
{
    /// <summary>
    /// List Price
    /// </summary>
    public class ListPrice
    {
        /// <summary>Primary Key</summary>
        public int ListPriceId { get; set; }

        /// <summary>Item Id (FK to Items)</summary>
        public int? ItemId { get; set; }

        /// <summary>Price of Item</summary>
        public decimal? Price { get; set; }

        /// <summary>Currency</summary>
        public string? Currency { get; set; }

        /// <summary>User Id (FK to Users)</summary>
        public int? UserId { get; set; }

        /// <summary>Created</summary>
        public DateTime CreateDate { get; set; }

    }
}
