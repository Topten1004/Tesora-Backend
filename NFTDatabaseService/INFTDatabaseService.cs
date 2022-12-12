// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//

using NFTDatabaseEntities;


namespace NFTDatabaseService
{
    /// <summary>
    /// NFTDatabase Interface
    /// </summary>
    public interface INFTDatabaseService
    {
        /// <summary>Get Options</summary>
        Task<List<Option>> GetOptions();
        /// <summary>Get Option</summary>
        Task<Option> GetOption(int optionId);
        /// <summary>Post Option</summary>
        Task PostOption(Option record);
        /// <summary>Put Option</summary>
        Task PutOption(Option record);
        /// <summary>Delete Option</summary>
        Task DeleteOption(int optionId);

        /// <summary>Get Roles</summary>
        Task<List<Role>> GetRoles();
        /// <summary>Get Role</summary>
        Task<Role> GetRole(int roleId);
        /// <summary>Post Role</summary>
        Task PostRole(Role record);
        /// <summary>Put Role</summary>
        Task PutRole(Role record);
        /// <summary>Delete Role</summary>
        Task DeleteRole(int roleId);

        /// <summary>Get Histories</summary>
        Task<List<History>> GetHistories();
        /// <summary>Get History</summary>
        Task<History> GetHistory(int historyId);
        /// <summary>Post History</summary>
        Task PostHistory(History record);
        /// <summary>Put History</summary>
        Task PutHistory(History record);
        /// <summary>Delete History</summary>
        Task DeleteHistory(int historyId);

        /// <summary>Get Favourites</summary>
        Task<List<Favourite>> GetFavourites(int userId);
        /// <summary>Get Favourite</summary>
        Task<Favourite> GetFavourite(int favouriteId);
        /// <summary>Post Favourite</summary>
        Task PostFavourite(Favourite record);
        /// <summary>Put Favourite</summary>
        Task PutFavourite(Favourite record);
        /// <summary>Delete Favourite</summary>
        Task DeleteFavourite(int favouriteId);
        /// <summary>Remove Favourite</summary>
        Task RemoveFavourite(int userId, int itemId);
        /// <summary>Get My Favorites</summary>
        Task<List<FavoriteCollectionItemCategory>> GetMyFavorites(int userId);

        /// <summary>Get Offers</summary>
        Task<List<Offer>> GetOffers();
        /// <summary>Get Offer</summary>
        Task<List<OfferUserCollectionItemCategory>> GetMyOffers(int userId);
        /// <summary>Get My Offer</summary>
        Task<OfferUserCollectionItemCategory> GetMyOffer(int userId, int offerId);
        /// <summary>Get Offer</summary>
        Task<Offer> GetOffer(int offerId);
        /// <summary>Accept My Offer</summary>
        Task AcceptMyOffer(OfferAccept record);
        /// <summary>Post Offer</summary>
        Task PostOffer(Offer record);
        /// <summary>Put Offer</summary>
        Task PutOffer(Offer record);
        /// <summary>Delete Offer</summary>
        Task DeleteOffer(int offerId);
        /// <summary>Delete Past Offer</summary>
        Task DeletePastOffers(int itemId, int currentOwnerId);

        /// <summary>Get Auction</summary>
        Task<Auction> GetAuction(int auctionId);
        /// <summary>Get Auctions by itemId</summary>
        Task<List<Auction>> GetAuctionsByItemId(int itemId);
        /// <summary>Post Auction</summary>
        Task PostAuction(Auction record);
        /// <summary>Put Auction</summary>
        Task PutAuction(Auction record);
        /// <summary>Delete Auction</summary>
        Task DeleteAuction(int auctionId);
        /// <summary>Accept My Auction</summary>
        Task AcceptMyAuction(AuctionAccept record);

        /// <summary>Get Categorys</summary>
        Task<bool> GetCategoryExists(int categoryId);
        /// <summary>Get Categorys</summary>
        Task<List<Category>> GetCategories();
        /// <summary>Get Category</summary>
        Task<Category> GetCategory(int categoryId);
        /// <summary>Get Category Image</summary>
        Task<ImageBox> GetCategoryImage(int categoryId);
        /// <summary>Post Category</summary>
        Task PostCategory(Category record);
        /// <summary>Put Category</summary>
        Task PutCategory(Category record);
        /// <summary>Delete Category</summary>
        Task DeleteCategory(int categoryId);
        /// <summary>Delete Category</summary>
        Task<int?> GetCategoryIdByTitle(string title);

        /// <summary>Get Contracts</summary>
        Task<List<Contract>> GetContracts();
        /// <summary>Get Contract</summary>
        Task<Contract> GetContract(int ContractId);
        /// <summary>Get Contract by Name</summary>
        Task<Contract> GetContractByName(string name, string version);
        /// <summary>Post Contract</summary>
        Task PostContract(Contract record);
        /// <summary>Put Contract</summary>
        Task PutContract(Contract record);
        /// <summary>Delete Contract</summary>
        Task DeleteContract(int ContractId);

        /// <summary>Collection Name Exists?</summary>
        Task<bool> GetCollectionNameExists(string name);
        /// <summary>Get Collections</summary>
        Task<List<Collection>> GetCollections();
        /// <summary>Get My Collections</summary>
        Task<List<Collection>> GetMyCollections(int authorId);
        /// <summary>Get Collection</summary>
        Task<Collection> GetCollection(int collectionId);
        /// <summary>Get Collection</summary>
        Task<Collection> GetCollectionByName(string name);
        /// <summary>Get Collection</summary>
        Task<ImageBox> GetCollectionBanner(int collectionId);
        /// <summary>Get Collection</summary>
        Task<ImageBox> GetCollectionImage(int collectionId);
        /// <summary>Post Collection</summary>
        Task PostCollection(Collection record);
        /// <summary>Put Collection</summary>
        Task PutCollection(Collection record);
        /// <summary>Delete Collection</summary>
        Task DeleteCollection(int collectionId);
        /// <summary>Delete Collection</summary>
        Task<List<Collection>> GetTrendingCollections();

        /// <summary>Get Users</summary>
        Task<List<User>> GetUsers();
        /// <summary>Get User</summary>
        Task<User> GetUser(int userId);
        /// <summary>Get User based on Master Id</summary>
        Task<User> GetUserMasterId(string masterUserId);
        /// <summary>Post User</summary>
        Task PostUser(User record);
        /// <summary>Put User</summary>
        Task PutUser(User record);
        /// <summary>Delete User</summary>
        Task DeleteUser(int userId);
        /// <summary>User Exists?</summary>
        Task<bool> UserExists(string masterUserId);
        /// <summary>Get User Image</summary>
        Task<ImageBox> GetUserImage(int userId);
        /// <summar>Update User Image</summary>
        Task PutUserImage(UserImage userImage);

        /// <summary>Get Items</summary>
        Task<List<Item>> GetItems(int collectionId);
        /// <summary>Get Items</summary>
        Task<List<Item>> GetItemsByCategoryId(int categoryId);
        /// <summary>Get Items</summary>
        Task<List<Item>> GetAuctionEndedItems();
        /// <summary>Get Items</summary>
        Task<List<Item>> GetAllItems();
        /// <summary>Get Items</summary>
        Task<List<Item>> GetMyItems(int collectionId);
        /// <summary>Get Item</summary>
        Task<Item> GetItem(int itemId);
        /// <summary>Get Item</summary>
        Task<Item> GetMyItem(int itemId);
        /// <summary>Post Item</summary>
        Task<int> PostItem(Item record);
        /// <summary>Post Item</summary>
        Task PostItemSale(ItemSale record);
        /// <summary>MyItem Accept Offer</summary>
        Task PutMyItemAcceptOffer(int itemId, bool acceptOffer);
        /// <summary>Put Item</summary>
        Task PutItem(Item record);
        /// <summary>Delete Item</summary>
        Task DeleteItem(int itemId);
        /// <summary>Get the Market Place Items</summary>
        Task<List<MarketPlaceResponse>> GetMarketPlaceItems(MarketPlaceRequest request);
        /// <summary>Item Auction Ended without a vaild bid</summary>
        Task PutAuctionEndItem(int itemId);
        /// <summary>Get auction bids, sorted highest to lowest</summary>
        Task<List<Auction>> GetAuctionBids(int itemId);

        /// <summary>Get ListPrices</summary>
        Task<List<ListPrice>> GetListPrices();
        /// <summary>Ge ListPrice</summary>
        Task<ListPrice> GetListPrice(int listPriceId);
        /// <summary>Post ListPrice</summary>
        Task PostListPrice(ListPrice record);
        /// <summary>Put ListPrice</summary>
        Task PutListPrice(ListPrice record);
        /// <summary>Delete ListPrice</summary>
        Task DeleteListPrice(int listPriceId);

        /// <summary>Get Item Attributes</summary>
        Task<List<ItemAttribute>> GetItemAttributes();
        /// <summary>Get Item Attribute</summary>
        Task<ItemAttribute> GetItemAttribute(int itemAttributeId);
        /// <summary>Post Item Attribute</summary>
        Task PostItemAttribute(ItemAttribute record);
        /// <summary>Put Item Attribute</summary>
        Task PutItemAttribute(ItemAttribute record);
        /// <summary>Delte Item Attribute</summary>
        Task DeleteItemAttribute(int itemAttributeId);

        /// <summary></summary>
        Task<List<CartItem>> GetCartItems(int userId);
        /// <summary></summary>
        Task<CartItem> GetCartItem(int lineId);
        /// <summary></summary>
        Task PostCartItem(CartItem record);
        /// <summary></summary>
        Task DeleteCartItem(int lineId);
        /// <summary></summary>
        Task DeleteCartItems(int userId);

        /// <summary></summary>
        Task PostSaleOrder(Sale record);
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
