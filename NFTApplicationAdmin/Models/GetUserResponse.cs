// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//

using System.Text.Json.Serialization;

namespace NFTApplicationAdmin.Models
{
    /// <summary>
    /// Get Category Response
    /// </summary>
    public class GetUserResponse
    {
        /// <summary>User Id</summary>
        public int UserId { get; set; }

        /// <summary>Image</summary>
        public string? ProfileImage { get; set; }

        /// <summary>User Name</summary>
        public string UserName { get; set; }

        /// <summary>First Name</summary>
        public string FirstName { get; set; }

        /// <summary>Last Name</summary>
        public string LastName { get; set; }

        /// <summary>Email</summary>
        public string Email { get; set; }

        /// <summary>Status</summary>
        public string Status { get; set; }

        /// <summary>Status</summary>
        public string MasterUserId { get; set; }

        /// <summary>Create Date</summary>
        public DateTime CreateDate { get; set; }
    }
}
