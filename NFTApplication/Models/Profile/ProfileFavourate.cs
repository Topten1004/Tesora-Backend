// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//
using System;
using System.Text.Json.Serialization;


namespace NFTApplication.Models.Profile
{
    /// <summary>
    ///  Profile Favourate
    /// </summary>
    public class ProfileFavourate
    {
        /// <summary>Unique Id</summary>
        [JsonPropertyName("favourite_id")]
        public int FavorateId { get; set; }

        /// <summary>Item Id (FK)</summary>
        [JsonPropertyName("item_id")]
        public int ItemId { get; set; }

        /// <summary>User Id (FK)</summary>
        [JsonPropertyName("user_id")]
        public int UserId { get; set; }

        /// <summary>Created Date</summary>
        [JsonPropertyName("create_date")]
        public DateTime CreateDate { get; set; }
    }
}

