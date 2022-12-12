// <copyright company="Chris McGorty" author="Chris McGorty">
//     Copyright (c) 2020 All Rights Reserved
// </copyright>

namespace NFTWalletEntities
{
    /// <summary>
    /// Get Balance Response
    /// </summary>
    public class GetBalanceResponse
    {
        /// <summary>
        /// Value in Wei
        /// </summary>
        public HexBigInt Wei { get; set; }

        /// <summary>
        /// Value in Eth
        /// </summary>
        public decimal Eth { get; set; }
    }
}
