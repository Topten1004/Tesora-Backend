// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//
using System;
using System.Text.Json.Serialization;


namespace NFTApplication.Models.MyCollection
{
    /// <summary>
    /// My Collection User
    /// </summary>
    public class MyCollectionUser
    {
        /// <summary>Unique Id</summary>
        [JsonPropertyName("user_id")]
        public int UserId { get; set; }

        /// <summary>User Name</summary>
        [JsonPropertyName("user_name")]
        public string? UserName { get; set; }
    }
}

