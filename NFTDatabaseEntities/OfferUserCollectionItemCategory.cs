// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//

namespace NFTDatabaseEntities
{
    /// <summary>
    /// Item
    /// </summary>
    public class OfferUserCollectionItemCategory
    {
        /// <summary>Primary Key</summary>
        public int OfferId { get; set; }

        /// <summary>Price of Item</summary>
        public decimal? Price { get; set; }

        /// <summary>Currency</summary>
        public string? Currency { get; set; }

        /// <summary>Created</summary>
        public DateTime CreateDate { get; set; }

        /// <summary>Sender Id (FK to Users)</summary>
        public User? Sender { get; set; }

        /// <summary>Collection Information</summary>
        public Collection? Collection { get; set; }

        /// <summary>Item Information</summary>
        public Item? Item { get; set; }

        /// <summary>Category Information</summary>
        public Category? Category { get; set; }
    }
}
