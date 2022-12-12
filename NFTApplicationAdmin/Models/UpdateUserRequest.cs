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
    /// Update User Request
    /// </summary>
    public class UpdateUserRequest
    {
        /// <summary>User Id</summary>
        [JsonPropertyName("UserId")]
        public int UserId { get; set; }

        /// <summary>User Name</summary>
        [Required(ErrorMessage = "Field can't be empty")]
        [JsonPropertyName("UserName")]
        public string UserName { get; set; } = String.Empty;

        /// <summary>First Name</summary>
        [Required(ErrorMessage = "Field can't be empty")]
        [JsonPropertyName("FirstName")]
        public string FirstName { get; set; } = String.Empty;

        /// <summary>Last Name</summary>
        [Required(ErrorMessage = "Field can't be empty")]
        [JsonPropertyName("LastName")]
        public string LastName { get; set; } = String.Empty;

        /// <summary>Email</summary>
        [Required(ErrorMessage = "Field can't be empty")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        [JsonPropertyName("Email")]
        public string Email { get; set; } = String.Empty;

        /// <summary>Profile Image</summary>
        [JsonPropertyName("image")]
        public IFormFile? Image { get; set; }

        /// <summary>User Status: active or inactive</summary>
        [JsonPropertyName("status")]
        public User.UserStatuses Status { get; set; }

        /// <summary>Master User Id</summary>
        [Required(ErrorMessage = "Field can't be empty")]
        [JsonPropertyName("MasterUserId")]
        public string? MasterUserId { get; set; }
    }
}
