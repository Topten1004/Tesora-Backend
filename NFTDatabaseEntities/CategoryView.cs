using System;
using System.Collections.Generic;
using System.Text;

namespace NFTDatabaseEntities
{
    /// <summary>
    /// CategoryView
    /// </summary>
    public class CategoryView
    {
        /// <summary>category id pk</summary>
        public int category_id { get; set; }

        /// <summary>category title</summary>
        public string? category_title { get; set; }
    }
}
