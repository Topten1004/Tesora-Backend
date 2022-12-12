namespace NFTBlockchain.Models
{
    /// <summary>
    /// Basic information about an NFT
    /// </summary>
    public class GetNftsForOwnerResponse
    {
        /// <summary>
        /// Collection Name
        /// </summary>
        public string? CollectionName { get; set; }
        
        /// <summary>
        /// Token Name
        /// </summary>
        public string? TokenName { get; set; }
        
        /// <summary>
        /// URI to token
        /// </summary>
        public string? TokenIpfs { get; set; }
    }
}
