// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//

using Microsoft.Extensions.Configuration;

using NFTWalletEntities;


namespace NFTWalletService
{
    /// <summary>
    /// NFTDatabase Service
    /// </summary>
    public class NFTWalletService : NFTWalletServiceBase, INFTWalletService
    {
        public NFTWalletService(HttpClient httpClient, IConfiguration configuration) : base(httpClient, configuration)
        {
        }

        /// <summary>
        /// Gets the wallet for a user
        /// </summary>
        /// <returns>GetWalletResponse</returns>
        public async Task<GetWalletResponse> GetWallet(string masterUserId)
        {
            var apiEndPoint = $"/api/v1/Wallet/GetWallet/{masterUserId}";

            return await MakeServiceGetCall<GetWalletResponse>(apiEndPoint);
        }

        /// <summary>
        /// Gets this signature information for a wallet
        /// </summary>
        /// <param name="masterUserId">Primary Key</param>
        /// <returns>GetSignatureResponse</returns>
        public async Task<GetSignatureResponse> GetSignature(string masterUserId)
        {
            var apiEndPoint = $"/api/v1/Wallet/GetSignature/{masterUserId}";

            return await MakeServiceGetCall<GetSignatureResponse>(apiEndPoint);
        }

        /// <summary>
        /// Gets the QR Code
        /// </summary>
        /// <param name="address">Address</param>
        /// <returns>QR Box</returns>
        public async Task<QrBox> GetQrCode(string address, string coin = "eth")
        {
            var apiEndPoint = $"/api/v1/Wallet/GetQrCode/{address}/{coin}";

            return await MakeServiceGetCall<QrBox>(apiEndPoint);
        }

        /// <summary>
        /// Gets the balance of a wallet
        /// </summary>
        /// <param name="masterUserId">Primary Key</param>
        /// <returns>GetBalanceResponse</returns>
        public async Task<GetBalanceResponse> GetBalance(string masterUserId)
        {
            var apiEndPoint = $"/api/v1/Wallet/GetBalance/{masterUserId}";

            return await MakeServiceGetCall<GetBalanceResponse>(apiEndPoint);
        }

        /// <summary>
        /// Gets the balance of a wallet
        /// </summary>
        /// <param name="address">Primary Key</param>
        /// <returns>GetBalanceResponse</returns>
        public async Task<GetBalanceResponse> GetBalanceForAddress(string address)
        {
            var apiEndPoint = $"/api/v1/Wallet/GetBalanceForAddress/{address}";

            return await MakeServiceGetCall<GetBalanceResponse>(apiEndPoint);
        }

        /// <summary>
        /// Add a Wallet
        /// </summary>
        /// <param name="record">PostWalletRequest</param>
        /// <returns>Option</returns>
        public async Task PostWallet(PostWalletRequest record)
        {
            var apiEndPoint = "/api/v1/Wallet/PostWallet";

            await MakeServicePostCall(apiEndPoint, record);
        }

    }
}
