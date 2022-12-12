using System;
using System.Collections.Generic;
using System.Text;

namespace NFTWalletEntities
{
    /// <summary>
    /// QR Box to hold QR Code info
    /// </summary>
    public class QrBox
    {
        /// <summary>
        /// The image data
        /// </summary>
        public byte[] Data { get; set; }

        /// <summary>
        /// The image type
        /// </summary>
        public string Type { get; set; }
    }
}
