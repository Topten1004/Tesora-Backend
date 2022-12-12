// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//


using NFTDatabaseEntities;
using static NFTDatabaseEntities.MarketPlaceRequest;

namespace NFTApplication.Models.MarketPlace
{
    /// <summary>
    /// MarketPlace Search Action
    /// </summary>
    public class MarketPlaceItemsRequest
    {
        /// <summary>
        /// Sort Options
        /// </summary>
        public MarketPlaceSort? Sort { get; set; }

        /// <summary>
        /// Filter Options
        /// </summary>
        public MarketPlaceFilter? Filter { get; set; }

        /// <summary>
        /// Text Search Options
        /// </summary>
        public string? Text { get; set; }

        /// <summary>
        /// What type of search is to be performed with the Text
        /// </summary>
        public TextSearchTypes TextSearchType { get; set; } = TextSearchTypes.Global;

        /// <summary>
        /// Number of items to be returned
        /// </summary>
        public int PageSize { get; set; } = 25;

        /// <summary>
        /// Page number of items
        /// NOTE: Page numbering starts at 0
        ///       Page number 1 with Page size of 25 will bring back records 26 to 50
        /// </summary>
        public int PageNumber { get; set; } = 0;

        /// <summary>
        /// Display Currency of the items
        /// </summary>
        public string DisplayCurrency { get; set; }
    }

}

