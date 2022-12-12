namespace NFTBlockchain.Services.Models
{
    public class GetNftMetadataResponse
    {
        public List<OwnedNft>? QwnedNfts { get; set; }
        public int? TotalCount { get; set; }
        public string? BlockHash { get; set; }
    }
}
