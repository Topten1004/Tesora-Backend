// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using NFTDatabaseEntities;


namespace NFTApplicationAdmin.Models
{
    /// <summary>
    /// Create User Request
    /// </summary>
    public class CreateUserRequest
    {
        /// <summary>User Name</summary>
        [JsonPropertyName("UserName")]
        public string UserName { get; set; } = String.Empty;

        /// <summary>First Name</summary>
        [JsonPropertyName("FirstName")]
        public string FirstName { get; set; } = String.Empty;

        /// <summary>Last Name</summary>
        [JsonPropertyName("LastName")]
        public string LastName { get; set; } = String.Empty;

        /// <summary>Email</summary>
        [JsonPropertyName("Email")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; } = String.Empty;

        /// <summary>Profile Image</summary>
        [JsonPropertyName("image")]
        public IFormFile Image { get; set; }

        /// <summary>User Status: active or inactive</summary>
        [JsonPropertyName("status")]
        public User.UserStatuses Status { get; set; }

    }
}
