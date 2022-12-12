// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//

namespace NFTWalletEntities
{
    /// <summary>
    /// Get Signature
    /// </summary>
    public class GetSignatureResponse
    {
        /// <summary>
        /// Wallet Address
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Key
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Value
        /// </summary>
        public string Value { get; set; }
    }
}
