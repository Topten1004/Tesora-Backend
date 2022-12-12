// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//
using System;
using System.Text.Json.Serialization;


namespace NFTApplication.Models.Activity
{
    /// <summary>
    /// Activity View Model
    /// </summary>
    public class ActivityViewModel
    {
        /// <summary>History records</summary>
        [JsonPropertyName("histories")]
        public List<ActivityHistory>? Histories { get; set; }
    }
}

