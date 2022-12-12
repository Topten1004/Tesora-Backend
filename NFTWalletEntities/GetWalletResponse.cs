// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//


namespace NFTWalletEntities
{
    /// <summary>
    ///  Get Wallet Response
    /// </summary>
    public class GetWalletResponse
    {
        /// <summary>
        /// Wallet address to transfer funds to for the deposit
        /// </summary>
        public string DepositAddress { get; set; }

        /// <summary>
        /// QRCode - Base64 encoded
        /// </summary>
        public byte[]? QRCode { get; set; }
    }
}
