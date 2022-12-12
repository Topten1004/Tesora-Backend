// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//

namespace NFTDatabaseEntities
{
    /// <summary>
    /// MarketPlace Search Action
    /// </summary>
    public class MarketPlaceRequest
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
        /// Text Searh Types
        /// </summary>
        public enum TextSearchTypes
        {
            /// <summary>Global Search</summary>
            Global,
            /// <summary>Collections Only</summary>
            CollectionOnly
        }

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
    }

    /// <summary>
    /// MarketPlace Sorting
    /// </summary>
    public class MarketPlaceSort
    {
        /// <summary>
        /// Sort order types
        /// </summary>
        public enum SortOrderTypes
        {
            /// <summary>Recent added items</summary>
            MostRecent,
            /// <summary>Most viewed items</summary>
            MostViewed,
            /// <summary>Most liked items</summary>
            MostLiked
        }

        /// <summary>
        /// Sort setting, will default to recent
        /// </summary>
        public SortOrderTypes? SortOrder { get; set; } = SortOrderTypes.MostRecent;
    }

    /// <summary>
    /// MarketPlace Filer
    /// </summary>
    public class MarketPlaceFilter
    {
        /// <summary>
        /// Filter Types
        /// </summary>
        public enum PriceFilterTypes
        {
            /// <summary>Any</summary>
            Any,
            /// <summary>Greater or equal to 1 ETH</summary>
            GteOne,
            /// <summary>Greater or equal to 10 ETH</summary>
            GteTen,
            /// <summary>Greater or equal to 100 ETH</summary>
            GteOneHunderd,
            /// <summary>Greater or equal to 1000 ETH</summary>
            GteOneThousand
        }

        /// <summary>
        /// Price Filter ?
        /// </summary>
        public PriceFilterTypes? PriceFilter { get; set; } = PriceFilterTypes.Any;

        /// <summary>
        /// Sale Types
        /// </summary>
        public enum MarketPlaceSaleTypes
        {
            /// <summary>All Sale Methods</summary>
            All,
            /// <summary>Allow Offers Only</summary>
            AllowsOffers,
            /// <summary>On Auction Only</summary>
            OnAuction
        }

        /// <summary>
        /// Sale Type 
        /// </summary>
        public MarketPlaceSaleTypes SaleType { get; set; } = MarketPlaceSaleTypes.All;


        /// <summary>
        /// Filter on categories, if null will include all
        /// </summary>
        public List<int>? CategoryIds { get; set; }

    }
}

