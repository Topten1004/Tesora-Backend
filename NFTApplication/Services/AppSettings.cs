namespace NFTApplication.Services
{
    /// <summary>
    /// App Settings for Authentication
    /// </summary>
    public class AppSettings
    {
        /// <summary>Auth Base Url</summary>
        public string? AuthBaseUri { get; set; }

        /// <summary>Auth Client Id</summary>
        public string? AuthClientId { get; set; }

        /// <summary>Auth Client Secret</summary>
        public string? AuthClientSecret { get; set; }
    }
}
