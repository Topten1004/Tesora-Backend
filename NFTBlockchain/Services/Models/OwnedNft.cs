namespace NFTBlockchain.Services.Models
{
    public class OwnedNft
    {
        public Contract? Contract { get; set; }
        public Id? Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public TokenUri? TokenUri { get; set; }
        public List<Medium>? Media { get; set; }
        public Metadata? Metadata { get; set; }
        public DateTime? TimeLastUpdated { get; set; }
        public ContractMetadata? ContractMetadata { get; set; }
    }
}
