using System;
using System.Collections.Generic;
using System.Text;

namespace NFTDatabaseEntities
{
    /// <summary>
    /// Favorite Collection Item Category Information
    /// </summary>
    public class FavoriteCollectionItemCategory
    {
        /// <summary>Favourite Informante</summary>
        public Favourite? Favorite { get; set; }

        /// <summary>Collection Information</summary>
        public Collection? Collection { get; set; }

        /// <summary>Item Information</summary>
        public Item? Item { get; set; }

        /// <summary>Category Information</summary>
        public Category? Category { get; set; }
    }

}
