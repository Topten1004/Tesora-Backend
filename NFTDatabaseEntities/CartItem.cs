// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//

namespace NFTDatabaseEntities
{
    /// <summary>
    /// Cart 
    /// </summary>
    public class CartItem
    {
        /// <summary>Primary key</summary>
        public int LineId { get; set; }

        /// <summary>Ring</summary>
        public string Ring { get; set; }

        /// <summary>Section</summary>
        public string Section { get; set; }

        /// <summary>Block</summary>
        public string Block { get; set; }

        /// <summary>Lot</summary>
        public string Lot { get; set; }

        /// <summary>USD Price</summary>
        public decimal UsdPrice { get; set; }

        /// <summary>USD Price</summary>
        public decimal? EurPrice { get; set; }

        /// <summary>BTC Price</summary>
        public decimal? BitcoinPrice { get; set; }
        
        /// <summary>ETH Price</summary>
        public decimal? EthereumPrice { get; set; }
        
        /// <summary>USDT Price</summary>
        public decimal? TetherPrice { get; set; }

        /// <summary>Price Expiration date</summary>
        public DateTime PriceExpiration { get; set; }

        /// <summary>User Id</summary>
        public int UserId { get; set; }
    }
}
