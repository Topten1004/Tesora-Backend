// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//


// See: https://docs.alchemy.com/reference/get_-apikey-getownersfortoken


using NFTBlockchain.Services.Models;


namespace NFTBlockchain.Services
{
    /// <summary>
    /// Acchemy Class
    /// </summary>
    public class AlchemyService : AlchemyServiceBase, IAlchemyService
    {
        private readonly string _apiKey;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="configuration"></param>
        public AlchemyService(HttpClient httpClient, IConfiguration configuration) : base(httpClient)
        {
            _apiKey = configuration["Alchemy:ApiKey"];
        }

        /// <summary>
        /// GetNftsForOwner
        ///  owner
        ///  pageKey
        ///  pageSize
        ///  contractAddresses[]
        ///  withMetadata
        ///  tokenUriTimeoutInMs
        ///  filters[]
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public async Task<GetNftsForOwnerResponse> GetNfsForOwner(string owner, string? pageKey = null, int? pageSize = null, bool? withMetadata = null)
        {
            var endpoint = $"nft/v2/{_apiKey}/getNFTs?owner={owner}";
            if (pageKey != null)
                endpoint += $"pageKey={pageKey}";
            if (pageSize != null)
                endpoint += $"pageSize={pageSize}";
            if (withMetadata != null)
                endpoint += $"withMetadata={withMetadata}";

            return await MakeServiceGetCall<GetNftsForOwnerResponse>(endpoint);
        }

        /// <summary>
        /// Get the NFT Metadata
        /// </summary>
        /// <param name="contractAddress"></param>
        /// <param name="tokenId"></param>
        /// <param name="tokenType"></param>
        /// <param name="tokenUriTimeoutInMs"></param>
        /// <param name="refreshCache"></param>
        /// <returns></returns>
        public async Task<GetNftMetadataResponse> GetNftMetadata(string contractAddress, string tokenId, string? tokenType = null, int? tokenUriTimeoutInMs = null, bool? refreshCache = null)
        {
            var endpoint = $"nft/v2/{_apiKey}/getNFTMetadata?contractAddress={contractAddress}&tokenId={tokenId}";
            if (tokenType != null)
                endpoint += $"&tokenType={tokenType}";
            if (tokenUriTimeoutInMs != null)
                endpoint += $"&tokenUriTimeoutInMs={tokenUriTimeoutInMs}";
            if (refreshCache != null)
                endpoint += $"&refreshCache={refreshCache}";

            return await MakeServiceGetCall<GetNftMetadataResponse>(endpoint);
        }


        /// <summary>
        /// Get the Contract Metadata
        /// </summary>
        /// <param name="contractAddress"></param>
        /// <returns></returns>
        public async Task<GetContractMetadataResponse> GetContractMetadata(string contractAddress)
        {
            var endpoint = $"nft/v2/{_apiKey}/getContractMetadata?contractAddress={contractAddress}";

            return await MakeServiceGetCall<GetContractMetadataResponse>(endpoint);
        }
    }
}
