// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//
using System.Text.Json.Serialization;


namespace NFTApplication.Services
{
    /// <summary>
    /// ERC721 Meta Data
    /// </summary>
    public class ERC721ItemMetaData
    {
        /// <summary>
        /// This is the URL to the image of the item.Can be just about any type of image (including SVGs
        /// , which will be cached into PNGs by OpenSea),
        /// and can be IPFS URLs or paths.We recommend using a 350 x 350 image.
        /// </summary>
        [JsonPropertyName("image")]
        public string? Image { get; set; }

        /// <summary>
        /// This is the URL that will appear below the asset's image on OpenSea and will allow users to leave OpenSea and view the item on your site.
        /// </summary>
        [JsonPropertyName("external_url")]
        public string? ExternalLink { get; set; }

        /// <summary>
        /// A human readable description of the item. Markdown is supported. 
        /// </summary>
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        /// <summary>
        /// Name of the item.
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }


        /// <summary>
        /// These are the attributes for the item, which will show up on the OpenSea page for the item. (see below)
        /// </summary>
        [JsonPropertyName("attributes")]
        public List<ERC721ItemMetaDataAttributes>? Attributes { get; set; }


        /// <summary>
        /// Background color of the item on OpenSea.Must be a six-character hexadecimal without a pre-pended #.
        /// </summary>
        [JsonPropertyName("background_color")]
        public string? BackgroundColor { get; set; }

        /// <summary>
        /// A URL to a multi-media attachment for the item.
        /// The file extensions GLTF, GLB, WEBM, MP4, M4V, OGV, and OGG are supported,
        /// along with the audio-only extensions MP3, WAV, and OGA.
        /// Animation_url also supports HTML pages, allowing you to build rich experiences and interactive NFTs using JavaScript canvas, WebGL, and more.
        /// Scripts and relative paths within the HTML page are now supported. However, access to browser extensions is not supported.
        /// </summary>
        [JsonPropertyName("animation_url")]
        public string? AnimationUrl { get; set; }


        /// <summary>
        /// A URL to a YouTube video.
        /// </summary>
        [JsonPropertyName("youtube_url")]
        public string? YouTubeUrl { get; set; }

        /// <summary>
        /// Seller Fee Basis Points
        /// 100, # Indicates a 1% seller fee.
        /// </summary>
        [JsonPropertyName("seller_fee_basis_points")]
        public decimal? SellerFeeBasisPoints { get; set; }

        /// <summary>
        /// Seller Reciepent Address
        /// Where seller fees will be paid to.
        /// </summary>
        [JsonPropertyName("fee_recipient")]
        public string? FeeRecipient { get; set; }
    }

    /// <summary>
    /// Attributes
    /// </summary>
    public class ERC721ItemMetaDataAttributes
    {
        /// <summary>
        /// Display Type
        /// </summary>
        [JsonPropertyName("display_type")]
        public string? DisplayType { get; set; }

        /// <summary>
        /// Trait Type
        /// </summary>
        [JsonPropertyName("trait_type")]
        public string? TraitType { get; set; }

        /// <summary>
        /// Value
        /// </summary>
        [JsonPropertyName("value")]
        public string? Value { get; set; }

        /// <summary>
        /// Number
        /// </summary>
        [JsonPropertyName("number")]
        public decimal? Number { get; set; }
    }
}
