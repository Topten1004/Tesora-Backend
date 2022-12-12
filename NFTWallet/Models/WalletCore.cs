// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//

namespace NFTWallet.Models
{
    /// <summary>
    /// Wallet Core
    /// </summary>
    public class WalletCore
    {
        /// <summary>Master User Id</summary>
        public string MasterUserId { get; set; }

        /// <summary>Currency</summary>
        public string Topic { get; set; }

        /// <summary>Mnumonic</summary>
        public string Value { get; set; }
    }
}
