// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//

using NFTWallet.Models;


namespace NFTWallet.DataAccess
{
    /// <summary>
    /// PostgreSql Interface
    /// </summary>
    public interface IPostgreSql
    {
        /// <summary>Wallet Exists</summary>
        /// <param name="masterUserId"></param>
        /// <returns>Bool</returns>
        Task<bool> WalletExists(string masterUserId);

        /// <summary>Wallet Exists</summary>
        /// <param name="record"></param>
        /// <returns></returns>
        Task CreateWalletCore(WalletCore record);

        /// <summary>Wallet Exists</summary>
        /// <param name="masterUserId"></param>
        /// <returns>Wallet Core</returns>
        Task<WalletCore?> RetrieveWalletCore(string masterUserId);

        /// <summary>Wallet Exists</summary>
        /// <param name="masterUserId"></param>
        /// <returns></returns>
        Task DeleteWalletCore(string masterUserId);
    }
}

