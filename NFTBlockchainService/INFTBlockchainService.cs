// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//

using NFTBlockchainEntities;


namespace NFTBlockchainService
{
    /// <summary>
    /// NFTDatabase Interface
    /// </summary>
    public interface INFTBlockchainService
    {
        Task<List<Erc720Token>> GetErc720Tokens();
        Task<Erc720Token> GetErc720Token(int Erc720TokenId);
        Task PostErc720Token(Erc720Token record);
        Task PutErc720Token(Erc720Token record);
        Task DeleteErc720Token(int Erc720TokenId);

    }
}
