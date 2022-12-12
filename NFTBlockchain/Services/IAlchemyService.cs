// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//

using NFTBlockchain.Services.Models;

namespace NFTBlockchain.Services
{
    public interface IAlchemyService
    {
        Task<GetNftsForOwnerResponse> GetNfsForOwner(string owner, string? pageKey = null, int? pageSize = null, bool? withMetadata = null);
        Task<GetNftMetadataResponse> GetNftMetadata(string contractAddress, string tokenId, string? tokenType = null, int? tokenUriTimeoutInMs = null, bool? refreshCache = null);
        Task<GetContractMetadataResponse> GetContractMetadata(string contractAddress);
    }
}
