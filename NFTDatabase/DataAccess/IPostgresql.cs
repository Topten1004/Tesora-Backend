// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//

using NFTDatabaseEntities;


namespace NFTDatabase.DataAccess
{
    /// <summary>
    /// PostgreSql Interface
    /// </summary>
    public interface IPostgreSql
    {
        /// <summary></summary>
        Task CreateOption(Option record);
        /// <summary></summary>
        Task<List<Option>> RetrieveOptions();
        /// <summary></summary>
        Task<Option?> RetrieveOption(int optionId);
        /// <summary></summary>
        Task UpdateOption(Option record);
        /// <summary></summary>
        Task DeleteOption(int optionId);

        /// <summary></summary>
        Task CreateRole(Role record);
        /// <summary></summary>
        Task<List<Role>> RetrieveRoles();
        /// <summary></summary>
        Task<Role?> RetrieveRole(int roleId);
        /// <summary></summary>
        Task UpdateRole(Role record);
        /// <summary></summary>
        Task DeleteRole(int roleId);

        /// <summary></summary>
        Task CreateHistory(History record);
        /// <summary></summary>
        Task<List<History>> RetrieveHistories();
        /// <summary></summary>
        Task<History?> RetrieveHistory(int historyId);
        /// <summary></summary>
        Task UpdateHistory(History record);
        /// <summary></summary>
        Task DeleteHistory(int historyId);

        /// <summary></summary>
        Task<bool> CreateFavourite(Favourite record);
        /// <summary></summary>
        Task<List<Favourite>> RetrieveFavourites(int userId);
        /// <summary></summary>
        Task<Favourite?> RetrieveFavourite(int favouriteId);
        /// <summary></summary>
        Task UpdateFavourite(Favourite record);
        /// <summary></summary>
        Task DeleteFavourite(int favouriteId);
        /// <summary></summary>
        Task RemoveFavourite(int userId, int itemId);
        /// <summary></summary>
        Task<List<FavoriteCollectionItemCategory>> RetrieveMyFavorites(int userId);

        /// <summary></summary>
        Task CreateOffer(Offer record);
        /// <summary></summary>
        Task<List<Offer>> RetrieveOffers();
        /// <summary></summary>
        Task<Offer?> RetrieveOffer(int offerId);
        /// <summary></summary>
        Task UpdateOffer(Offer record);
        /// <summary></summary>
        Task DeleteOffer(int offerId);
        /// <summary></summary>
        Task DeletePastOffers(int itemId, int currentOwnerId);
        /// <summary></summary>
        Task<List<OfferUserCollectionItemCategory>> RetrieveMyOffers(int userId);
        /// <summary></summary>
        Task<OfferUserCollectionItemCategory?> RetrieveMyOffer(int userId, int offerId);
        /// <summary></summary>
        Task AcceptMyOffer(int userId, int offerId, string txHash);

        /// <summary></summary>
        Task CreateAuction(Auction record);
        /// <summary></summary>
        Task<Auction?> RetrieveAuction(int auctionId);
        /// <summary></summary>
        Task<List<Auction>> RetrieveAuctionsByItemId(int itemId);
        /// <summary></summary>
        Task UpdateAuction(Auction record);
        /// <summary></summary>
        Task DeleteAuction(int auctionId);
        /// <summary></summary>
        Task<List<AuctionUserCollectionItemCategory>> RetrieveMyAuctions(int userId);
        /// <summary></summary>
        Task<AuctionUserCollectionItemCategory?> RetrieveMyAuction(int userId, int auctionId);
        /// <summary></summary>
        Task AcceptMyAuction(int userId, int auctionId, string txHash);



        /// <summary></summary>
        Task<bool> CategoryExists(int categoryId);
        /// <summary></summary>
        Task CreateCategory(Category record);
        /// <summary></summary>
        Task<List<Category>> RetrieveCategories();
        /// <summary></summary>
        Task<Category?> RetrieveCategory(int categoryId);
        /// <summary></summary>
        Task<ImageBox?> RetrieveCategoryImage(int categoryId);
        /// <summary></summary>
        Task UpdateCategory(Category record);
        /// <summary></summary>
        Task DeleteCategory(int categoryId);
        /// <summary></summary>
        Task<int?> GetCategoryIdByTitle(string title);

        /// <summary></summary>
        Task<bool> CollectionNameExists(string name);
        /// <summary></summary>
        Task CreateCollection(Collection record);
        /// <summary></summary>
        Task<List<Collection>> RetrieveCollections();
        /// <summary></summary>
        Task<ImageBox?> RetrieveCollectionBanner(int collectionId);
        /// <summary></summary>
        Task<ImageBox?> RetrieveCollectionImage(int collectionId);
        /// <summary></summary>
        Task<List<Collection>> RetrieveMyCollections(int authorId);
        /// <summary></summary>
        Task<Collection?> RetrieveCollection(int collectionId);
        /// <summary></summary>
        Task<Collection?> RetrieveCollectionByName(string name);
        /// <summary></summary>
        Task UpdateCollection(Collection record);
        /// <summary></summary>
        Task DeleteCollection(int collectionId);
        /// <summary></summary>
        Task<List<Collection>> GetTrendingCollections();

        /// <summary></summary>
        Task CreateUser(User record);
        /// <summary></summary>
        Task<List<User>> RetrieveUsers();
        /// <summary></summary>
        Task<User?> RetrieveUser(int userId);
        /// <summary></summary>
        Task<User?> RetrieveUserMasterId(string masterUserId);
        /// <summary></summary>
        Task UpdateUser(User record);
        /// <summary></summary>
        Task DeleteUser(int userId);
        /// <summary></summary>
        Task<bool> UserExists(string masterUserId);
        /// <summary></summary>
        Task<ImageBox?> RetrieveUserImage(int userId);
        /// <summary></summary>
        Task UpdateUserImage(User record);


        /// <summary></summary>
        Task<int> CreateItem(Item record);
        /// <summary></summary>
        Task<List<Item>> RetrieveAllItems();
        /// <summary></summary>
        Task<List<Item>> RetrieveItems(int collectionId);
        /// <summary></summary>
        Task<List<Item>> RetrieveItemsByCategoryId(int categoryId);
        /// <summary></summary>
        Task<Item?> RetrieveItem(int itemId);
        /// <summary></summary>
        Task UpdateItem(Item record);
        /// <summary></summary>
        Task DeleteItem(int itemId);
        /// <summary></summary>
        Task<List<Item>> RetrieveMyItems(int collectionId);
        /// <summary></summary>
        Task<Item?> RetrieveMyItem(int itemId);
        /// <summary></summary>
        Task PostItemSale(ItemSale record);
        /// <summary></summary>
        Task MyItemAcceptOffer(int itemId, bool acceptOffer);
        /// <summary></summary>
        Task<List<MarketPlaceResponse>> GetMarketPlaceItems(MarketPlaceRequest request);
        /// <summary></summary>
        Task<List<Item>> RetrieveAuctionEndedItems();
        /// <summary></summary>
        Task PutAuctionEndItem(int itemId);
        /// <summary></summary>
        Task<List<Auction>> GetAuctionBids(int itemId);

        /// <summary></summary>
        Task CreateItemAttribute(ItemAttribute record);
        /// <summary></summary>
        Task<List<ItemAttribute>> RetrieveItemAttributes();
        /// <summary></summary>
        Task<ItemAttribute?> RetrieveItemAttribute(int itemAttributeId);
        /// <summary></summary>
        Task UpdateItemAttribute(ItemAttribute record);
        /// <summary></summary>
        Task DeleteItemAttribute(int itemAttributeId);

        /// <summary></summary>
        Task CreateListPrice(ListPrice record);
        /// <summary></summary>
        Task<List<ListPrice>> RetrieveListPrices();
        /// <summary></summary>
        Task<ListPrice?> RetrieveListPrice(int listPriceId);
        /// <summary></summary>
        Task UpdateListPrice(ListPrice record);
        /// <summary></summary>
        Task DeleteListPrice(int listPriceId);

        /// <summary></summary>
        Task CreateContract(Contract record);
        /// <summary></summary>
        Task<List<Contract>> RetrieveContracts();
        /// <summary></summary>
        Task<Contract?> RetrieveContract(int contractId);
        /// <summary></summary>
        Task<Contract?> RetrieveContractByName(string name, string version);
        /// <summary></summary>
        Task UpdateContract(Contract record);
        /// <summary></summary>
        Task DeleteContract(int contractId);


        /// <summary></summary>
        Task CreateCartItem(CartItem record);
        /// <summary></summary>
        Task<List<CartItem>> RetrieveCartItems(int userId);
        /// <summary></summary>
        Task<CartItem?> RetrieveCartItem(int lineId);
        /// <summary></summary>
        Task DeleteCartItem(int lineId);
        /// <summary></summary>
        Task DeleteCartItems(int userId);


        /// <summary></summary>
        Task CreateSalesOrder(Sale record);
        /// <summary></summary>
        Task UpdateSalesOrderStatus(string address, Sale.PaymentStatuses paymentStatus);

        /// <summary></summary>
        Task<List<string>> GetRingsForSale();
        /// <summary></summary>
        Task<List<string>> GetSectionsForSale(string ring);
        /// <summary></summary>
        Task<List<string>> GetBlocksForSale(string ring, string section);
        /// <summary></summary>
        Task<List<string>> GetLotsForSale(string ring, string section, string block);
    }
}

