// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//
using System;
using System.Text.Json.Serialization;


namespace NFTApplication.Models.MyCollection
{
    /// <summary>
    /// My Collection View Model
    /// </summary>
    public class MyCollectionViewModel
    {
        /// <summary>Collections</summary>
        [JsonPropertyName("collections")]
        public List<GetMyCollectionResponse>? Collections { get; set; }
    }
}

