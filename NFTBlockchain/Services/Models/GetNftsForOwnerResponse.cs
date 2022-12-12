// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//

namespace NFTBlockchain.Services.Models
{
    public class GetNftsForOwnerResponse
    {
        public List<OwnedNft>? OwnedNfts { get; set; }
        public string? PageKey { get; set; }
        public int? TotalCount { get; set; }
        public string? BlockHash { get; set; }
    }
}