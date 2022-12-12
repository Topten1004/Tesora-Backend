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
    public class ERC721ContractMetaData
    {
        /// <summary>
        /// Name of the item.
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>
        /// A human readable description of the item. Markdown is supported. 
        /// </summary>
        [JsonPropertyName("description")]
        public string? Description { get; set; }

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

}
