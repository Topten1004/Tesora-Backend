// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//

namespace NFTDatabaseEntities
{
    /// <summary>
    /// Cart 
    /// </summary>
    public class SaleItem
    {
        /// <summary>Primary key</summary>
        public int LineId { get; set; }

        /// <summary>Foreign key</summary>
        public int SaleId { get; set; }

        /// <summary>Ring</summary>
        public string Ring { get; set; }

        /// <summary>Section</summary>
        public string Section { get; set; }

        /// <summary>Block</summary>
        public string Block { get; set; }

        /// <summary>Lot</summary>
        public string Lot { get; set; }

        /// <summary>USD Price</summary>
        public decimal Price { get; set; }

        /// <summary>BTC Price</summary>
        public string Currency { get; set; }

        /// <summary>Price Expiration date</summary>
        public DateTime Expiration { get; set; }
    }
}
