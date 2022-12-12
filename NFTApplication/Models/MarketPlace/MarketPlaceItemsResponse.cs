

using NFTDatabaseEntities;
using static NFTDatabaseEntities.MarketPlaceResponse;


namespace NFTApplication.Models.MarketPlace
{
    public class MarketPlaceItemsResponse
    {
        public CardTypes CardType { get; set; }

        public int? ItemId { get; set; }

        public string? Name { get; set; }

        public string? Media { get; set; }

        public CollectionView? Collection { get; set; }

        public CategoryView? Category { get; set; }

        public decimal? Price { get; set; }

        public string? Currency { get; set; }

        public string? PriceDisplay { get; set; }

        public int? ViewCount { get; set; }

        public int? LikeCount { get; set; }

        public bool? AcceptOffer { get; set; }

        public bool? EnableAuction { get; set; }

        public int? ItemCount { get; set; }
    }
}
