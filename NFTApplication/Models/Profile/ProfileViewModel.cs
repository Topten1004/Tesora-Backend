// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//
using System;
using System.Text.Json.Serialization;


namespace NFTApplication.Models.Profile
{
    /// <summary>
    /// Profile User View Model
    /// </summary>
    public class ProfileUserViewModel
    {
        /// <summary>Unique Id</summary>
        [JsonPropertyName("user_id")]
        public int UserId { get; set; }

        /// <summary>User Name</summary>
        [JsonPropertyName("user_name")]
        public string? UserName { get; set; }

        /// <summary>Email</summary>
        [JsonPropertyName("email")]
        public string? Email { get; set; }

        /// <summary>First Name</summary>
        [JsonPropertyName("first_name")]
        public string? FirstName { get; set; }

        /// <summary>Last Name</summary>
        [JsonPropertyName("last_name")]
        public string? LastName { get; set; }

        /// <summary>Wallet Address</summary>
        [JsonPropertyName("wallet_address")]
        public string? WalletAddress { get; set; }

        /// <summary>Create Date</summary>
        [JsonPropertyName("create_date")]
        public DateTime? CreateDate { get; set; }

        /// <summary>Wallet QR Code</summary>
        [JsonPropertyName("wallet_qr_code")]
        public string? WalletQrCode { get; set; }

        /// <summary>User Image URL</summary>
        [JsonPropertyName("user_image")]
        public string? UserImage { get; set; }
    }
}

