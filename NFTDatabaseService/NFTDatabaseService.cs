// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//

using Microsoft.Extensions.Configuration;

using NFTDatabaseEntities;


namespace NFTDatabaseService
{
    /// <summary>
    /// NFTDatabase Service
    /// </summary>
    public class NFTDatabaseService : NFTDatabaseServiceBase, INFTDatabaseService
    {
        public NFTDatabaseService(HttpClient httpClient, IConfiguration configuration) : base(httpClient, configuration)
        {
        }

        /// <summary>
        /// Gets the Options
        /// </summary>
        /// <returns>List of Option records</returns>
        public async Task<List<Option>> GetOptions()
        {
            var apiEndPoint = "/api/v1/Option/GetOptions";

            return await MakeServiceGetCall<List<Option>>(apiEndPoint);
        }

        /// <summary>
        /// Gets a Option record based in the primary key id
        /// </summary>
        /// <param name="optionId">Primary Key</param>
        /// <returns>Option</returns>
        public async Task<Option> GetOption(int optionId)
        {
            var apiEndPoint = $"/api/v1/Option/GetOption/{optionId}";

            return await MakeServiceGetCall<Option>(apiEndPoint);
        }

        /// <summary>
        /// Add a Option record
        /// </summary>
        /// <param name="record">Option</param>
        /// <returns>Option</returns>
        public async Task PostOption(Option record)
        {
            var apiEndPoint = "/api/v1/Option/PostOption";

            await MakeServicePostCall(apiEndPoint, record);
        }

        /// <summary>
        /// Update a Option record
        /// </summary>
        /// <param name="record">Option</param>
        /// <returns></returns>
        public async Task PutOption(Option record)
        {
            var apiEndPoint = "/api/v1/Option/PutOption";

            await MakeServicePutCall(apiEndPoint, record);
        }

        /// <summary>
        /// Delete a Option record
        /// </summary>
        /// <param name="optionId">Primary Key</param>
        /// <returns></returns>
        public async Task DeleteOption(int optionId)
        {
            var apiEndPoint = $"/api/v1/Option/DeleteOption/{optionId}";

            await MakeServiceDeleteCall(apiEndPoint);
        }


        /// <summary>
        /// Gets the Roles
        /// </summary>
        /// <returns>List of Role records</returns>
        public async Task<List<Role>> GetRoles()
        {
            var apiEndPoint = "/api/v1/Role/GetRoles";

            return await MakeServiceGetCall<List<Role>>(apiEndPoint);
        }

        /// <summary>
        /// Gets a Role record based in the primary key id
        /// </summary>
        /// <param name="roleId">Primary Key</param>
        /// <returns>Role</returns>
        public async Task<Role> GetRole(int roleId)
        {
            var apiEndPoint = $"/api/v1/Role/GetRole/{roleId}";

            return await MakeServiceGetCall<Role>(apiEndPoint);
        }

        /// <summary>
        /// Add a Role record
        /// </summary>
        /// <param name="record">Role</param>
        /// <returns>Role</returns>
        public async Task PostRole(Role record)
        {
            var apiEndPoint = "/api/v1/Role/PostRole";

            await MakeServicePostCall(apiEndPoint, record);
        }

        /// <summary>
        /// Update a Role record
        /// </summary>
        /// <param name="record">Role</param>
        /// <returns></returns>
        public async Task PutRole(Role record)
        {
            var apiEndPoint = "/api/v1/Role/PutRole";

            await MakeServicePutCall(apiEndPoint, record);
        }

        /// <summary>
        /// Delete a Role record
        /// </summary>
        /// <param name="roleId">Primary Key</param>
        /// <returns></returns>
        public async Task DeleteRole(int roleId)
        {
            var apiEndPoint = $"/api/v1/Role/DeleteRole/{roleId}";

            await MakeServiceDeleteCall(apiEndPoint);
        }


        /// <summary>
        /// Gets the Histories
        /// </summary>
        /// <returns>List of History records</returns>
        public async Task<List<History>> GetHistories()
        {
            var apiEndPoint = "/api/v1/History/GetHistories";

            return await MakeServiceGetCall<List<History>>(apiEndPoint);
        }

        /// <summary>
        /// Gets a History record based in the primary key id
        /// </summary>
        /// <param name="historyId">Primary Key</param>
        /// <returns>History</returns>
        public async Task<History> GetHistory(int historyId)
        {
            var apiEndPoint = $"/api/v1/History/GetHistory/{historyId}";

            return await MakeServiceGetCall<History>(apiEndPoint);
        }

        /// <summary>
        /// Add a History record
        /// </summary>
        /// <param name="record">History</param>
        /// <returns>History</returns>
        public async Task PostHistory(History record)
        {
            var apiEndPoint = "/api/v1/History/PostHistory";

            await MakeServicePostCall(apiEndPoint, record);
        }

        /// <summary>
        /// Update a History record
        /// </summary>
        /// <param name="record">History</param>
        /// <returns></returns>
        public async Task PutHistory(History record)
        {
            var apiEndPoint = "/api/v1/History/PutHistory";

            await MakeServicePutCall(apiEndPoint, record);
        }

        /// <summary>
        /// Delete a History record
        /// </summary>
        /// <param name="historyId">Primary Key</param>
        /// <returns></returns>
        public async Task DeleteHistory(int historyId)
        {
            var apiEndPoint = $"/api/v1/History/DeleteHistory/{historyId}";

            await MakeServiceDeleteCall(apiEndPoint);
        }


        /// <summary>
        /// Gets the Favourites
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>List of Favourite records</returns>
        public async Task<List<Favourite>> GetFavourites(int userId)
        {
            var apiEndPoint = $"/api/v1/Favourite/GetFavourites/{userId}";

            return await MakeServiceGetCall<List<Favourite>>(apiEndPoint);
        }

        /// <summary>
        /// Gets a Favourite record based in the primary key id
        /// </summary>
        /// <param name="favouriteId">Primary Key</param>
        /// <returns>Favourite</returns>
        public async Task<Favourite> GetFavourite(int favouriteId)
        {
            var apiEndPoint = $"/api/v1/Favourite/GetFavourite/{favouriteId}";

            return await MakeServiceGetCall<Favourite>(apiEndPoint);
        }

        /// <summary>
        /// Add a Favourite record
        /// </summary>
        /// <param name="record">Favourite</param>
        /// <returns>Favourite</returns>
        public async Task PostFavourite(Favourite record)
        {
            var apiEndPoint = "/api/v1/Favourite/PostFavourite";

            await MakeServicePostCall(apiEndPoint, record);
        }

        /// <summary>
        /// Update a Favourite record
        /// </summary>
        /// <param name="record">Favourite</param>
        /// <returns></returns>
        public async Task PutFavourite(Favourite record)
        {
            var apiEndPoint = "/api/v1/Favourite/PutFavourite";

            await MakeServicePutCall(apiEndPoint, record);
        }

        /// <summary>
        /// Delete a Favourite record
        /// </summary>
        /// <param name="favouriteId">Primary Key</param>
        /// <returns></returns>
        public async Task DeleteFavourite(int favouriteId)
        {
            var apiEndPoint = $"/api/v1/Favourite/DeleteFavourite/{favouriteId}";

            await MakeServiceDeleteCall(apiEndPoint);
        }

        /// <summary>
        /// Remove a Favourite record
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <param name="itemId">Item Id</param>
        /// <returns></returns>
        public async Task RemoveFavourite(int userId, int itemId)
        {
            var apiEndPoint = $"/api/v1/Favourite/RemoveFavourite/{userId}/{itemId}";

            await MakeServiceDeleteCall(apiEndPoint);
        }

        /// <summary>
        /// Get My Favorites
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<FavoriteCollectionItemCategory>> GetMyFavorites(int userId)
        {
            var apiEndPoint = $"/api/v1/Favourite/GetMyFavorites/{userId}";

           return await MakeServiceGetCall<List<FavoriteCollectionItemCategory>>(apiEndPoint);

        }

        /// <summary>
        /// Gets the Offers
        /// </summary>
        /// <returns>List of Offer records</returns>
        public async Task<List<Offer>> GetOffers()
        {
            var apiEndPoint = "/api/v1/Offer/GetOffers";

            return await MakeServiceGetCall<List<Offer>>(apiEndPoint);
        }

        /// <summary>
        /// Gets my offers
        /// </summary>
        /// <returns>List of my offers</returns>
        public async Task<List<OfferUserCollectionItemCategory>> GetMyOffers(int userId)
        {
            var apiEndPoint = $"/api/v1/Offer/GetMyOffers/{userId}";

            return await MakeServiceGetCall<List<OfferUserCollectionItemCategory>>(apiEndPoint);
        }

        /// <summary>
        /// Gets my offer
        /// </summary>
        /// <returns>my offer</returns>
        public async Task<OfferUserCollectionItemCategory> GetMyOffer(int userId, int offerId)
        {
            var apiEndPoint = $"/api/v1/Offer/GetMyOffer/{userId}/{offerId}";

            return await MakeServiceGetCall<OfferUserCollectionItemCategory>(apiEndPoint);
        }

        /// <summary>
        /// Accept My Offer
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public async Task AcceptMyOffer(OfferAccept record)
        {
            var apiEndPoint = "/api/v1/Offer/AcceptMyOffer";

            await MakeServicePutCall(apiEndPoint, record);
        }


        /// <summary>
        /// Gets a Offer record based in the primary key id
        /// </summary>
        /// <param name="offerId">Primary Key</param>
        /// <returns>Offer</returns>
        public async Task<Offer> GetOffer(int offerId)
        {
            var apiEndPoint = $"/api/v1/Offer/GetOffer/{offerId}";

            return await MakeServiceGetCall<Offer>(apiEndPoint);
        }

        /// <summary>
        /// Add a Offer record
        /// </summary>
        /// <param name="record">Offer</param>
        /// <returns>Offer</returns>
        public async Task PostOffer(Offer record)
        {
            var apiEndPoint = "/api/v1/Offer/PostOffer";

            await MakeServicePostCall(apiEndPoint, record);
        }

        /// <summary>
        /// Update a Offer record
        /// </summary>
        /// <param name="record">Offer</param>
        /// <returns></returns>
        public async Task PutOffer(Offer record)
        {
            var apiEndPoint = "/api/v1/Offer/PutOffer";

            await MakeServicePutCall(apiEndPoint, record);
        }

        /// <summary>
        /// Delete a Offer record
        /// </summary>
        /// <param name="offerId">Primary Key</param>
        /// <returns></returns>
        public async Task DeleteOffer(int offerId)
        {
            var apiEndPoint = $"/api/v1/Offer/DeleteOffer/{offerId}";

            await MakeServiceDeleteCall(apiEndPoint);
        }

        /// <summary>
        /// Delete offers when current owner changed
        /// </summary>
        /// <param name="itemId">FK of items</param>
        /// <param name="currentOwnerId">FK of users</param>
        /// <returns></returns>
        public async Task DeletePastOffers(int itemId, int currentOwnerId)
        {
            var apiEndPoint = $"/api/v1/Offer/DeletePastOffers/{itemId}/{currentOwnerId}";

            await MakeServiceDeleteCall(apiEndPoint);
        }

        /// <summary>
        /// Gets my auctions
        /// </summary>
        /// <returns>List of my auctions</returns>
        public async Task<List<AuctionUserCollectionItemCategory>> GetMyAuctions(int userId)
        {
            var apiEndPoint = $"/api/v1/Auction/GetMyAuctions/{userId}";

            return await MakeServiceGetCall<List<AuctionUserCollectionItemCategory>>(apiEndPoint);
        }

        /// <summary>
        /// Gets my auction
        /// </summary>
        /// <returns>my auction</returns>
        public async Task<AuctionUserCollectionItemCategory> GetMyAuction(int userId, int auctionId)
        {
            var apiEndPoint = $"/api/v1/Auction/GetMyAuction/{userId}/{auctionId}";

            return await MakeServiceGetCall<AuctionUserCollectionItemCategory>(apiEndPoint);
        }

        /// <summary>
        /// Accept Auction
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public async Task AcceptMyAuction(AuctionAccept record)
        {
            var apiEndPoint = "/api/v1/Auction/AcceptMyAuction";

            await MakeServicePutCall(apiEndPoint, record);
        }


        /// <summary>
        /// Gets an Auction record based in the primary key id
        /// </summary>
        /// <param name="auctionId">Primary Key</param>
        /// <returns>Auction</returns>
        public async Task<Auction> GetAuction(int auctionId)
        {
            var apiEndPoint = $"/api/v1/Auction/GetAuction/{auctionId}";

            return await MakeServiceGetCall<Auction>(apiEndPoint);
        }

        /// <summary>
        /// Gets an Auction record based in the itemId primary key
        /// </summary>
        /// <param name="itemId">Primary Key</param>
        /// <returns>List of Auctions</returns>
        public async Task<List<Auction>> GetAuctionsByItemId(int itemId)
        {
            var apiEndPoint = $"/api/v1/Auction/GetAuctionsByItemId/{itemId}";

            return await MakeServiceGetCall<List<Auction>>(apiEndPoint);
        }

        /// <summary>
        /// Add an Auction record
        /// </summary>
        /// <param name="record">Auction</param>
        /// <returns>Auction</returns>
        public async Task PostAuction(Auction record)
        {
            var apiEndPoint = "/api/v1/Auction/PostAuction";

            await MakeServicePostCall(apiEndPoint, record);
        }

        /// <summary>
        /// Update an Auction record
        /// </summary>
        /// <param name="record">Auction</param>
        /// <returns></returns>
        public async Task PutAuction(Auction record)
        {
            var apiEndPoint = "/api/v1/Auction/PutAuction";

            await MakeServicePutCall(apiEndPoint, record);
        }

        /// <summary>
        /// Delete an Auction record
        /// </summary>
        /// <param name="auctionId">Primary Key</param>
        /// <returns></returns>
        public async Task DeleteAuction(int auctionId)
        {
            var apiEndPoint = $"/api/v1/Auction/DeleteAuction/{auctionId}";

            await MakeServiceDeleteCall(apiEndPoint);
        }

        /// <summary>
        /// Update an Auction record
        /// </summary>
        /// <param name="itemId">Foreign Key of Item</param>
        /// <returns></returns>
        public async Task PutAuctionEndItem(int itemId)
        {
            var apiEndPoint = $"/api/v1/Item/PutAuctionEndItem/{itemId}";

            await MakeServicePutCall(apiEndPoint);
        }

        /// <summary>
        /// Get auction bids for an item, sorted to highest to lowest
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public async Task<List<Auction>> GetAuctionBids(int itemId)
        {
            var apiEndPoint = $"/api/v1/Auction/GetAuctionBids/{itemId}";

            return await MakeServiceGetCall<List<Auction>>(apiEndPoint);
        }

        /// <summary>
        /// Category Exists
        /// </summary>
        /// <returns>bool</returns>
        public async Task<bool> GetCategoryExists(int categoryId)
        {
            var apiEndPoint = $"/api/v1/Category/GetCategoryExists/{categoryId}";

            return await MakeServiceGetCall<bool>(apiEndPoint);
        }


        /// <summary>
        /// Gets the Categories
        /// </summary>
        /// <returns>List of Category records</returns>
        public async Task<List<Category>> GetCategories()
        {
            var apiEndPoint = "/api/v1/Category/GetCategories";

            return await MakeServiceGetCall<List<Category>>(apiEndPoint);
        }

        /// <summary>
        /// Gets a Category record based in the primary key id
        /// </summary>
        /// <param name="categoryId">Primary Key</param>
        /// <returns>Category</returns>
        public async Task<Category> GetCategory(int categoryId)
        {
            var apiEndPoint = $"/api/v1/Category/GetCategory/{categoryId}";

            return await MakeServiceGetCall<Category>(apiEndPoint);
        }

        /// <summary>
        /// Gets a Category image based in the primary key id
        /// </summary>
        /// <param name="categoryId">Primary Key</param>
        /// <returns>Category Image</returns>
        public async Task<ImageBox> GetCategoryImage(int categoryId)
        {
            var apiEndPoint = $"/api/v1/Category/GetCategoryImage/{categoryId}";

            return await MakeServiceGetCall<ImageBox>(apiEndPoint);
        }


        /// <summary>
        /// Add a Category record
        /// </summary>
        /// <param name="record">Category</param>
        /// <returns>Category</returns>
        public async Task PostCategory(Category record)
        {
            var apiEndPoint = "/api/v1/Category/PostCategory";

            await MakeServicePostCall(apiEndPoint, record);
        }

        /// <summary>
        /// Update a Category record
        /// </summary>
        /// <param name="record">Category</param>
        /// <returns></returns>
        public async Task PutCategory(Category record)
        {
            var apiEndPoint = "/api/v1/Category/PutCategory";

            await MakeServicePutCall(apiEndPoint, record);
        }

        /// <summary>
        /// Delete a Category record
        /// </summary>
        /// <param name="categoryId">Primary Key</param>
        /// <returns></returns>
        public async Task DeleteCategory(int categoryId)
        {
            var apiEndPoint = $"/api/v1/Category/DeleteCategory/{categoryId}";

            await MakeServiceDeleteCall(apiEndPoint);
        }

        /// <summary>
        /// Gets a Categor Id by Title
        /// </summary>
        /// <param name="title">Title</param>
        /// <returns>Category Id</returns>
        public async Task<int?> GetCategoryIdByTitle(string title)
        {
            var apiEndPoint = $"/api/v1/Category/GetCategoryIdByTitle/{title}";

            return await MakeServiceGetCall<int?>(apiEndPoint);
        }


        /// <summary>
        /// Gets the Contracts
        /// </summary>
        /// <returns>List of Contract records</returns>
        public async Task<List<Contract>> GetContracts()
        {
            var apiEndPoint = "/api/v1/Contract/GetContracts";

            return await MakeServiceGetCall<List<Contract>>(apiEndPoint);
        }

        /// <summary>
        /// Gets a Contract record based in the primary key id
        /// </summary>
        /// <param name="contractId">Primary Key</param>
        /// <returns>Contract</returns>
        public async Task<Contract> GetContract(int contractId)
        {
            var apiEndPoint = $"/api/v1/Contract/GetContract/{contractId}";

            return await MakeServiceGetCall<Contract>(apiEndPoint);
        }

        /// <summary>
        /// Gets a Contract record based in the primary key id
        /// </summary>
        /// <param name="contractId">Primary Key</param>
        /// <returns>Contract</returns>
        public async Task<Contract> GetContractByName(string name, string version)
        {
            var apiEndPoint = $"/api/v1/Contract/GetContractByName/{name}/{version}";

            return await MakeServiceGetCall<Contract>(apiEndPoint);
        }

        /// <summary>
        /// Add a Contract record
        /// </summary>
        /// <param name="record">Contract</param>
        /// <returns>Contract</returns>
        public async Task PostContract(Contract record)
        {
            var apiEndPoint = "/api/v1/Contract/PostContract";

            await MakeServicePostCall(apiEndPoint, record);
        }

        /// <summary>
        /// Update a Contract record
        /// </summary>
        /// <param name="record">Contract</param>
        /// <returns></returns>
        public async Task PutContract(Contract record)
        {
            var apiEndPoint = "/api/v1/Contract/PutContract";

            await MakeServicePutCall(apiEndPoint, record);
        }

        /// <summary>
        /// Delete a Contract record
        /// </summary>
        /// <param name="contractId">Primary Key</param>
        /// <returns></returns>
        public async Task DeleteContract(int contractId)
        {
            var apiEndPoint = $"/api/v1/Contract/DeleteContract/{contractId}";

            await MakeServiceDeleteCall(apiEndPoint);
        }


        /// <summary>
        /// Collection Name Exists
        /// </summary>
        /// <returns>bool</returns>
        public async Task<bool> GetCollectionNameExists(string name)
        {
            var apiEndPoint = $"/api/v1/Collection/GetCollectionNameExists/{name}";

            return await MakeServiceGetCall<bool>(apiEndPoint);
        }

        /// <summary>
        /// Gets the Collections
        /// </summary>
        /// <returns>List of Collection records</returns>
        public async Task<List<Collection>> GetCollections()
        {
            var apiEndPoint = "/api/v1/Collection/GetCollections";

            return await MakeServiceGetCall<List<Collection>>(apiEndPoint);
        }

        /// <summary>
        /// Gets the Trending Collections
        /// </summary>
        /// <returns>List of Collection records</returns>
        public async Task<List<Collection>> GetTrendingCollections()
        {
            var apiEndPoint = "/api/v1/Collection/GetTrendingCollections";

            return await MakeServiceGetCall<List<Collection>>(apiEndPoint);
        }

        /// <summary>
        /// Gets the Collections
        /// </summary>
        /// <returns>List of Collection records</returns>
        public async Task<List<Collection>> GetMyCollections(int authorId)
        {
            var apiEndPoint = $"/api/v1/Collection/GetMyCollections/{authorId}";

            return await MakeServiceGetCall<List<Collection>>(apiEndPoint);
        }

        /// <summary>
        /// Gets a Collection record based in the primary key id
        /// </summary>
        /// <param name="collectionId">Primary Key</param>
        /// <returns>Collection</returns>
        public async Task<Collection> GetCollection(int collectionId)
        {
            var apiEndPoint = $"/api/v1/Collection/GetCollection/{collectionId}";

            return await MakeServiceGetCall<Collection>(apiEndPoint);
        }

        /// <summary>
        /// Gets a Collection record based on name
        /// </summary>
        /// <param name="name">Name</param>
        /// <returns>Collection</returns>
        public async Task<Collection> GetCollectionByName(string name)
        {
            var apiEndPoint = $"/api/v1/Collection/GetCollectionByName/{name}";

            return await MakeServiceGetCall<Collection>(apiEndPoint);
        }

        /// <summary>
        /// Gets a Collection banner based in the primary key id
        /// </summary>
        /// <param name="collectionId">Primary Key</param>
        /// <returns>Category Image</returns>
        public async Task<ImageBox> GetCollectionBanner(int collectionId)
        {
            var apiEndPoint = $"/api/v1/Collection/GetCollectionBanner/{collectionId}";

            return await MakeServiceGetCall<ImageBox>(apiEndPoint);
        }

        /// <summary>
        /// Gets a Collection image based in the primary key id
        /// </summary>
        /// <param name="collectionId">Primary Key</param>
        /// <returns>Category Image</returns>
        public async Task<ImageBox> GetCollectionImage(int collectionId)
        {
            var apiEndPoint = $"/api/v1/Collection/GetCollectionImage/{collectionId}";

            return await MakeServiceGetCall<ImageBox>(apiEndPoint);
        }

        /// <summary>
        /// Add a Collection record
        /// </summary>
        /// <param name="record">Collection</param>
        /// <returns>Collection</returns>
        public async Task PostCollection(Collection record)
        {
            var apiEndPoint = "/api/v1/Collection/PostCollection";

            await MakeServicePostCall(apiEndPoint, record);
        }

        /// <summary>
        /// Update a Collection record
        /// </summary>
        /// <param name="record">Collection</param>
        /// <returns></returns>
        public async Task PutCollection(Collection record)
        {
            var apiEndPoint = "/api/v1/Collection/PutCollection";

            await MakeServicePutCall(apiEndPoint, record);
        }

        /// <summary>
        /// Delete a Collection record
        /// </summary>
        /// <param name="collectionId">Primary Key</param>
        /// <returns></returns>
        public async Task DeleteCollection(int collectionId)
        {
            var apiEndPoint = $"/api/v1/Collection/DeleteCollection/{collectionId}";

            await MakeServiceDeleteCall(apiEndPoint);
        }


        /// <summary>
        /// Gets the Users
        /// </summary>
        /// <returns>List of User records</returns>
        public async Task<List<User>> GetUsers()
        {
            var apiEndPoint = "/api/v1/User/GetUsers";

            return await MakeServiceGetCall<List<User>>(apiEndPoint);
        }

        /// <summary>
        /// Gets a User record based in the primary key id
        /// </summary>
        /// <param name="userId">Primary Key</param>
        /// <returns>User</returns>
        public async Task<User> GetUser(int userId)
        {
            var apiEndPoint = $"api/v1/User/GetUser/{userId}";

            return await MakeServiceGetCall<User>(apiEndPoint);
        }


        /// <summary>
        /// Gets a User record based in the master user id
        /// </summary>
        /// <param name="masterUserId">Master User Id</param>
        /// <returns>User</returns>
        public async Task<User> GetUserMasterId(string masterUserId)
        {
            var apiEndPoint = $"api/v1/User/GetUser/MasterId/{masterUserId}";

            return await MakeServiceGetCall<User>(apiEndPoint);
        }

        /// <summary>
        /// Add a User record
        /// </summary>
        /// <param name="record">User</param>
        /// <returns>User</returns>
        public async Task PostUser(User record)
        {
            var apiEndPoint = "/api/v1/User/PostUser";

            await MakeServicePostCall(apiEndPoint, record);
        }

        /// <summary>
        /// Update a User record
        /// </summary>
        /// <param name="record">User</param>
        /// <returns></returns>
        public async Task PutUser(User record)
        {
            var apiEndPoint = "/api/v1/User/PutUser";

            await MakeServicePutCall(apiEndPoint, record);
        }

        /// <summary>
        /// Delete a User record
        /// </summary>
        /// <param name="userId">Primary Key</param>
        /// <returns></returns>
        public async Task DeleteUser(int userId)
        {
            var apiEndPoint = $"/api/v1/User/DeleteUser/{userId}";

            await MakeServiceDeleteCall(apiEndPoint);
        }

        /// <summary>
        /// User Exists
        /// </summary>
        /// <param name="masterUserId">Master User Id</param>
        /// <returns></returns>
        public async Task<bool> UserExists(string masterUserId)
        {
            var apiEndPoint = $"/api/v1/User/UserExists/{masterUserId}";

            var result = await MakeServiceGetCall<BooleanResult>(apiEndPoint);

            return result.Exists;
        }

        /// <summary>
        /// Gets a User image
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>User</returns>
        public async Task<ImageBox> GetUserImage(int userId)
        {
            var apiEndPoint = $"api/v1/User/GetUserImage/{userId}";

            return await MakeServiceGetCall<ImageBox>(apiEndPoint);
        }

        /// <summary>
        /// Update a User Image
        /// </summary>
        /// <param name="userImage">User Image</param>
        /// <returns></returns>
        public async Task PutUserImage(UserImage userImage)
        {
            var apiEndPoint = "/api/v1/User/PutUserImage";

            await MakeServicePutCall(apiEndPoint, userImage);
        }


        /// <summary>
        /// Gets the All Items
        /// </summary>
        /// <returns>List of Item records</returns>
        public async Task<List<Item>> GetAllItems()
        {
            var apiEndPoint = $"/api/v1/Item/GetAllItems";

            return await MakeServiceGetCall<List<Item>>(apiEndPoint);
        }

        /// <summary>
        /// Gets the Items
        /// </summary>
        /// <returns>List of Item records</returns>
        public async Task<List<Item>> GetItems(int collectionId)
        {
            var apiEndPoint = $"/api/v1/Item/GetItems/{collectionId}";

            return await MakeServiceGetCall<List<Item>>(apiEndPoint);
        }

        /// <summary>
        /// Gets the Items
        /// </summary>
        /// <returns>List of Item records</returns>
        public async Task<List<Item>> GetItemsByCategoryId(int categoryId)
        {
            var apiEndPoint = $"/api/v1/Item/GetItemsByCategoryId/{categoryId}";

            return await MakeServiceGetCall<List<Item>>(apiEndPoint);
        }

        
        /// <summary>
        /// Gets the Items where the auction has ended
        /// </summary>
        /// <returns>List of Item records</returns>
        public async Task<List<Item>> GetAuctionEndedItems()
        {
            var apiEndPoint = $"/api/v1/Item/GetAuctionEndedItems";

            return await MakeServiceGetCall<List<Item>>(apiEndPoint);
        }


        /// <summary>
        /// Gets my Items
        /// </summary>
        /// <returns>List of Item records</returns>
        public async Task<List<Item>> GetMyItems(int collectionId)
        {
            var apiEndPoint = $"/api/v1/Item/GetMyItems/{collectionId}";

            return await MakeServiceGetCall<List<Item>>(apiEndPoint);
        }

        /// <summary>
        /// Gets a Item record based in the primary key id
        /// </summary>
        /// <param name="itemId">Primary Key</param>
        /// <returns>Item</returns>
        public async Task<Item> GetItem(int itemId)
        {
            var apiEndPoint = $"/api/v1/Item/GetItem/{itemId}";

            return await MakeServiceGetCall<Item>(apiEndPoint);
        }

        /// <summary>
        /// Gets a Item record based in the primary key id
        /// </summary>
        /// <param name="itemId">Primary Key</param>
        /// <returns>Item</returns>
        public async Task<Item> GetMyItem(int itemId)
        {
            var apiEndPoint = $"/api/v1/Item/GetMyItem/{itemId}";

            return await MakeServiceGetCall<Item>(apiEndPoint);
        }

        /// <summary>
        /// Add a Item record
        /// </summary>
        /// <param name="record">Item</param>
        /// <returns>Item</returns>
        public async Task<int> PostItem(Item record)
        {
            var apiEndPoint = "/api/v1/Item/PostItem";

            return await MakeServicePostCall<int>(apiEndPoint, record);
        }

        /// <summary>
        /// Post item for sale
        /// </summary>
        /// <param name="record">ItemSale</param>
        /// <returns></returns>
        public async Task PostItemSale(ItemSale record)
        {
            var apiEndPoint = "/api/v1/Item/PostItemSale";

            await MakeServicePostCall(apiEndPoint, record);
        }

        /// <summary>
        /// Put MyItem Accept Offer
        /// </summary>
        /// <param name="itemId">Item</param>
        /// <param name="acceptOffer"></param>
        /// <returns></returns>
        public async Task PutMyItemAcceptOffer(int itemId, bool acceptOffer)
        {
            var record = new { itemId, acceptOffer };

            var apiEndPoint = "/api/v1/Item/PutMyItemAcceptOffer";

            await MakeServicePutCall(apiEndPoint, record);
        }

        /// <summary>
        /// Update a Item record
        /// </summary>
        /// <param name="record">Item</param>
        /// <returns></returns>
        public async Task PutItem(Item record)
        {
            var apiEndPoint = "/api/v1/Item/PutItem";

            await MakeServicePutCall(apiEndPoint, record);
        }

        /// <summary>
        /// Delete a Item record
        /// </summary>
        /// <param name="itemId">Primary Key</param>
        /// <returns></returns>
        public async Task DeleteItem(int itemId)
        {
            var apiEndPoint = $"/api/v1/Item/DeleteItem/{itemId}";

            await MakeServiceDeleteCall(apiEndPoint);
        }

        /// <summary>
        /// Get the Market Place Items
        /// </summary>
        /// <param name="record">Item</param>
        /// <returns>Item</returns>
        public async Task<List<MarketPlaceResponse>> GetMarketPlaceItems(MarketPlaceRequest request)
        {
            var apiEndPoint = "/api/v1/Item/GetMarketPlaceItems";

            return await MakeServicePostCall<List<MarketPlaceResponse>>(apiEndPoint, request);
        }


        /// <summary>
        /// Gets the Item Attributes
        /// </summary>
        /// <returns>List of Item Attribute records</returns>
        public async Task<List<ItemAttribute>> GetItemAttributes()
        {
            var apiEndPoint = "/api/v1/ItemAttribute/GetItemAttributes";

            return await MakeServiceGetCall<List<ItemAttribute>>(apiEndPoint);
        }

        /// <summary>
        /// Gets a Item Attribute record based in the primary key id
        /// </summary>
        /// <param name="itemAttributeId">Primary Key</param>
        /// <returns>Item Attribute</returns>
        public async Task<ItemAttribute> GetItemAttribute(int itemAttributeId)
        {
            var apiEndPoint = $"/api/v1/ItemAttribute/GetItemAttribute/{itemAttributeId}";

            return await MakeServiceGetCall<ItemAttribute>(apiEndPoint);
        }

        /// <summary>
        /// Add a Item Attribute record
        /// </summary>
        /// <param name="record">Item Attribute</param>
        /// <returns>Item Attribute</returns>
        public async Task PostItemAttribute(ItemAttribute record)
        {
            var apiEndPoint = "/api/v1/ItemAttribute/PostItemAttribute";

            await MakeServicePostCall(apiEndPoint, record);
        }

        /// <summary>
        /// Update a Item Attribute record
        /// </summary>
        /// <param name="record">Item Attribute</param>
        /// <returns></returns>
        public async Task PutItemAttribute(ItemAttribute record)
        {
            var apiEndPoint = "/api/v1/ItemAttribute/PutItemAttribute";

            await MakeServicePutCall(apiEndPoint, record);
        }

        /// <summary>
        /// Delete a Item Attribute record
        /// </summary>
        /// <param name="itemAttributeId">Primary Key</param>
        /// <returns></returns>
        public async Task DeleteItemAttribute(int itemAttributeId)
        {
            var apiEndPoint = $"/api/v1/ItemAttribute/DeleteItemAttribute/{itemAttributeId}";

            await MakeServiceDeleteCall(apiEndPoint);
        }


        /// <summary>
        /// Gets the ListPrices
        /// </summary>
        /// <returns>List of ListPrice records</returns>
        public async Task<List<ListPrice>> GetListPrices()
        {
            var apiEndPoint = "/api/v1/ListPrice/GetListPrices";

            return await MakeServiceGetCall<List<ListPrice>>(apiEndPoint);
        }

        /// <summary>
        /// Gets a ListPrice record based in the primary key id
        /// </summary>
        /// <param name="listPriceId">Primary Key</param>
        /// <returns>ListPrice</returns>
        public async Task<ListPrice> GetListPrice(int listPriceId)
        {
            var apiEndPoint = $"/api/v1/ListPrice/GetListPrice/{listPriceId}";

            return await MakeServiceGetCall<ListPrice>(apiEndPoint);
        }

        /// <summary>
        /// Add a ListPrice record
        /// </summary>
        /// <param name="record">ListPrice</param>
        /// <returns>ListPrice</returns>
        public async Task PostListPrice(ListPrice record)
        {
            var apiEndPoint = "/api/v1/ListPrice/PostListPrice";

            await MakeServicePostCall(apiEndPoint, record);
        }

        /// <summary>
        /// Update a ListPrice record
        /// </summary>
        /// <param name="record">ListPrice</param>
        /// <returns></returns>
        public async Task PutListPrice(ListPrice record)
        {
            var apiEndPoint = "/api/v1/ListPrice/PutListPrice";

            await MakeServicePutCall(apiEndPoint, record);
        }

        /// <summary>
        /// Delete a ListPrice record
        /// </summary>
        /// <param name="listPriceId">Primary Key</param>
        /// <returns></returns>
        public async Task DeleteListPrice(int listPriceId)
        {
            var apiEndPoint = $"/api/v1/ListPrice/DeleteListPrice/{listPriceId}";

            await MakeServiceDeleteCall(apiEndPoint);
        }

        /// <summary>
        /// Gets a cart items based in the user 
        /// </summary>
        /// <param name="userId">Primary Key</param>
        /// <returns>ListPrice</returns>
        public async Task<List<CartItem>> GetCartItems(int userId)
        {
            var apiEndPoint = $"/api/v1/Cart/GetCartItems/{userId}";

            return await MakeServiceGetCall<List<CartItem>>(apiEndPoint);
        }

        /// <summary>
        /// Gets a ListPrice record based in the primary key id
        /// </summary>
        /// <param name="lineId">Primary Key</param>
        /// <returns>ListPrice</returns>
        public async Task<CartItem> GetCartItem(int lineId)
        {
            var apiEndPoint = $"/api/v1/Cart/GetCartItem/{lineId}";

            return await MakeServiceGetCall<CartItem>(apiEndPoint);
        }

        /// <summary>
        /// Add a cart item record
        /// </summary>
        /// <param name="record">ListPrice</param>
        /// <returns>ListPrice</returns>
        public async Task PostCartItem(CartItem record)
        {
            var apiEndPoint = "/api/v1/Cart/PostCartItem";

            await MakeServicePostCall(apiEndPoint, record);
        }

        /// <summary>
        /// Delete a cart item record
        /// </summary>
        /// <param name="lineId">Primary Key</param>
        /// <returns></returns>
        public async Task DeleteCartItem(int lineId)
        {
            var apiEndPoint = $"/api/v1/Cart/DeleteCartItem/{lineId}";

            await MakeServiceDeleteCall(apiEndPoint);
        }

        /// <summary>
        /// Delete all cart items
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns></returns>
        public async Task DeleteCartItems(int userId)
        {
            var apiEndPoint = $"/api/v1/Cart/DeleteCartItems/{userId}";

            await MakeServiceDeleteCall(apiEndPoint);
        }


        /// <summary>
        /// Add a sales order
        /// </summary>
        /// <param name="record">Sale</param>
        /// <returns>Sale</returns>
        public async Task PostSaleOrder(Sale record)
        {
            var apiEndPoint = "/api/v1/Sale/PostSaleOrder";

            await MakeServicePostCall(apiEndPoint, record);
        }

        /// <summary>
        /// Update Sales Order Status
        /// </summary>
        /// <param name="address"></param>
        /// <param name="paymentStatus"></param>
        /// <returns></returns>
        public async Task UpdateSalesOrderStatus(string address, Sale.PaymentStatuses paymentStatus)
        {
            var apiEndPoint = $"/api/v1/Sale/UpdateSalesOrderStatus/{address}/{paymentStatus}";

            await MakeServicePutCall(apiEndPoint);
        }

        /// <summary>
        /// Get Rings for Sale in XPOVerse
        /// </summary>
        /// <returns></returns>
        public async Task<List<string>> GetRingsForSale()
        {
            var apiEndPoint = "/api/v1/XPOVerseLot/GetRingsForSale";

            return await MakeServiceGetCall<List<string>>(apiEndPoint);
        }

        /// <summary>
        /// Get Sections for Sale in XPOVerse Ring
        /// </summary>
        /// <param name="ring"></param>
        /// <returns></returns>
        public async Task<List<string>> GetSectionsForSale(string ring)
        {
            var apiEndPoint = $"/api/v1/XPOVerseLot/GetSectionsForSale/{ring}";

            return await MakeServiceGetCall<List<string>>(apiEndPoint);
        }

        /// <summary>
        /// Get Blocks for Sale in XPOVerse Ring Section
        /// </summary>
        /// <param name="ring"></param>
        /// <param name="section"></param>
        /// <returns></returns>
        public async Task<List<string>> GetBlocksForSale(string ring, string section)
        {
            var apiEndPoint = $"/api/v1/XPOVerseLot/GetSectionsForSale/{ring}/{section}";

            return await MakeServiceGetCall<List<string>>(apiEndPoint);
        }

        /// <summary>
        /// Get Lots for Sale in XPOVerse Ring Section Block
        /// </summary>
        /// <param name="ring"></param>
        /// <param name="section"></param>
        /// <param name="block"></param>
        /// <returns></returns>
        public async Task<List<string>> GetLotsForSale(string ring, string section, string block)
        {
            var apiEndPoint = $"/api/v1/XPOVerseLot/GetLotsForSale/{ring}/{section}/{block}";

            return await MakeServiceGetCall<List<string>>(apiEndPoint);
        }

    }
}
