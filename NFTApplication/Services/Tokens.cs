// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//
using System;
using System.Text.Json.Serialization;


namespace NFTApplication.Services
{
    /// <summary>
    /// Token Response from Identity Server
    /// </summary>
    public class Tokens
    {
        /// <summary>Identity Token</summary>
        [JsonPropertyName("id_token")]
        public string? IdToken { get; set; }

        /// <summary>Access Token</summary>
        [JsonPropertyName("access_token")]
        public string? AccessToken { get; set; }

        /// <summary>Expires In</summary>
        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }

        /// <summary>Token Type</summary>
        [JsonPropertyName("token_type")]
        public string? TokenType { get; set; }

        /// <summary>Scope</summary>
        [JsonPropertyName("scope")]
        public string? Scope { get; set; }
    }
}
