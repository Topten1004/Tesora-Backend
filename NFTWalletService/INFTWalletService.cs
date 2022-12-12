// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//

using NFTWalletEntities;


namespace NFTWalletService
{
    /// <summary>
    /// NFTDatabase Interface
    /// </summary>
    public interface INFTWalletService
    {
        Task<GetWalletResponse> GetWallet(string masterUserId);
        Task<GetSignatureResponse> GetSignature(string masterUserId);
        Task<QrBox> GetQrCode(string address, string coin = "eth");
        Task<GetBalanceResponse> GetBalance(string masterUserId);
        Task<GetBalanceResponse> GetBalanceForAddress(string address);
        Task PostWallet(PostWalletRequest record);
    }
}
