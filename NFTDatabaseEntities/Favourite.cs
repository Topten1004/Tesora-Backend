// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//

namespace NFTDatabaseEntities
{
    /// <summary>
    /// Favorite
    /// </summary>
    public class Favourite
    {
        /// <summary>Primary Key</summary>
        public int FavouriteId { get; set; }

        /// <summary>Item Id (FK to Items)</summary>
        public int ItemId { get; set; }

        /// <summary>User Id (FK to Users)</summary>
        public int UserId { get; set; }

        /// <summary>Created</summary>
        public DateTime CreateDate { get; set; }

    }
}
