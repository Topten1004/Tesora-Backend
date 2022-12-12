namespace NFTBlockchain.Services.Models
{
    public class GetContractMetadataResponse
    {
        public string? Address { get; set; }
        public ContractMetadata? ContractMetadata { get; set; }
    }
}
