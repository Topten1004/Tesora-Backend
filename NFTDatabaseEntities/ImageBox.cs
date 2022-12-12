using System;
using System.Collections.Generic;
using System.Text;

namespace NFTDatabaseEntities
{
    /// <summary>
    /// Image Box
    /// </summary>
    public class ImageBox
    {
        /// <summary>
        /// The image data
        /// </summary>
        public byte[]? Data { get; set; }

        /// <summary>
        /// The image type
        /// </summary>
        public string Type { get; set; }
    }
}
