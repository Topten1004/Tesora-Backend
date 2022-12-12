// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//

namespace NFTWalletEntities
{
    /// <summary>
    /// Post Wallet Request
    /// </summary>
    public class PostWalletRequest
    {
        /// <summary>
        /// User Id
        /// </summary>
        public string MasterUserId { get; set; }

        /// <summary>
        /// Currency Code
        /// </summary>
        public string Currency { get; set; }
    }
}
