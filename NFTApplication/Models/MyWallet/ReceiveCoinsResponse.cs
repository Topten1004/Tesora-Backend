// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//

namespace NFTApplication.Models.MyWallet
{
    /// <summary>
    /// Wallet View Mode
    /// </summary>
    public class ReceiveCoinsResponse
    {
        /// <summary>The current network</summary>
        public string Network { get; set; }
        /// <summary>Wallet Address</summary>
        public string Address { get; set; }
        /// <summary>Balance</summary>
        public decimal Balance { get; set; }
        /// <summary>Symbol</summary>
        public string Symbol { get; set; }
        /// <summary>QR Code</summary>
        public string QrCode { get; set; }
    }
}
