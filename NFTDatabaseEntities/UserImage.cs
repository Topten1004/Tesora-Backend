using System;
using System.Collections.Generic;
using System.Text;

namespace NFTDatabaseEntities
{
    /// <summary>
    /// User Image Update
    /// </summary>
    public class UserImage
    {
        /// <summary>
        /// User Id
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// Image Information
        /// </summary>
        public ImageBox? Image { get; set; }
    }
}
