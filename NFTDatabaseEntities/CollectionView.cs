using System;
using System.Collections.Generic;
using System.Text;

namespace NFTDatabaseEntities
{
    /// <summary>
    /// CollectionView
    /// </summary>
    public class CollectionView
    {
        /// <summary>collection id pk</summary>
        public int collection_id { get; set; }

        /// <summary>collection name</summary>
        public string? name { get; set; }

        /// <summary>collection image</summary>
        public string? image { get; set; }

        /// <summary>collection banner</summary>
        public string? banner { get; set; }
    }
}
