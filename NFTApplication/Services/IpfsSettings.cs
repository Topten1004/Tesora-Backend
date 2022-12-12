namespace NFTApplication.Services
{
    /// <summary>
    /// IPFS Settings
    /// </summary>
    public class IpfsSettings
    {
        /// <summary>API Endpoint</summary>
        public string? Api { get; set; }

        /// <summary>Project Id</summary>
        public string? Project { get; set; }

        /// <summary>Projet Secret Key</summary>
        public string? Key { get; set; }

        /// <summary>Dedicated Gateway</summary>
        public string? Gateway { get; set; }
        
    }
}
