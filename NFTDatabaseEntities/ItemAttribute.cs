// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//

namespace NFTDatabaseEntities
{
    /// <summary>
    /// Item
    /// </summary>
    public class ItemAttribute   {
        /// <summary>Primary Key</summary>
        public int ItemAttributeId { get; set; }

        /// <summary>Item Id (FK)</summary>
        public int ItemId { get; set; }

        /// <summary>Item Types</summary>
        public enum ItemTypes
        {
            /// <summary>General Property</summary>
            GeneralProperty,
            /// <summary>Specific Property</summary>
            SpecificProperty,
            /// <summary>Bost Percent</summary>
            BoostPercent,
            /// <summary>Bost Value</summary>
            BoostValue,
            /// <summary>Ranking</summary>
            Ranking,
            /// <summary>Statistic, optional use Max Value</summary>
            Statistic
        }

        /// <summary>Item Type</summary>
        public ItemTypes ItemType { get; set; }

        /// <summary>Title</summary>
        public string Title { get; set; }

        /// <summary>Value</summary>
        public string Value { get; set; }

        /// <summary>Maximun Value for Statistics</summary>
        public string? MaxValue { get; set; }
    }
}
