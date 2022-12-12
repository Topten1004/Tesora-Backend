using System.Text.Json.Serialization;


namespace NFTApplication.Services
{
    /// <summary>
    /// Identity Server User Information
    /// </summary>
    public class UserInfo
    {
        /// <summary>First Name</summary>
        [JsonPropertyName("first_name")]
        public string? FirstName { get; set; }

        /// <summary>Last Name</summary>
        [JsonPropertyName("last_name")]
        public string? LastName { get; set; }

        /// <summary>Email</summary>
        [JsonPropertyName("email")]
        public string? Email { get; set; }

        /// <summary>User Id</summary>
        [JsonPropertyName("user_id")]
        public string? UserId { get; set; }

        /// <summary>User Name</summary>
        [JsonPropertyName("user_name")]
        public string? UserName { get; set; }

        /// <summary>Country Code</summary>
        [JsonPropertyName("country_code")]
        public string? CountryCode { get; set; }
    }
}
