// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//
using System.Text.Json;
using System.Numerics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

using Nethereum.Web3;
using FluentValidation;

using NFTDatabaseService;
using NFTWalletService;
using NFTApplication.Models.MyCollection;
using NFTApplication.Services;
using NFTDatabaseEntities;
using NFTApplication.Utility;
using NFTApplication.Contracts.NFTCollection;
using NFTApplication.Contracts.NFTCollection.ContractDefinition;
using NFTApplication.Models;


namespace NFTApplication.Controllers
{

    /// <summary>
    /// My Collection Page Controller
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class MyCollectionController : ControllerBase
    {
        private readonly INFTDatabaseService _db;
        private readonly INFTWalletService _wallet;
        private readonly string _blockchainNodeAndKey;
        private readonly string _contractName;
        private readonly string _contractVersion;
        private readonly bool _useTestWallet;
        private readonly string? _testWalletPublicAddress;
        private readonly string? _testWalletPrivateAddress;
        private readonly ILogger<MyCollectionController> _logger;
        private readonly IOptions<IpfsSettings> _ipfsSettings;

        /// <summary>
        /// Dependency Injection Contstructor
        /// </summary>
        /// <param name="db">Database Singleton</param>
        /// <param name="logger">Logger</param>
        /// <param name="configuration"></param>
        /// <param name="ipfsSettings"></param>
        /// <param name="wallet"></param>
        public MyCollectionController(INFTDatabaseService db, INFTWalletService wallet, IConfiguration configuration, ILogger<MyCollectionController> logger, IOptions<IpfsSettings> ipfsSettings)
        {
            _db = db;
            _wallet = wallet;
            _logger = logger;

            _useTestWallet = Convert.ToBoolean(configuration["TestWallet:UseTestWallet"]);
            _testWalletPublicAddress = configuration["TestWallet:PublicKey"];
            _testWalletPrivateAddress = configuration["TestWallet:PrivateKey"];

            var prefix = configuration["Environment:Prefix"];
            _blockchainNodeAndKey = $"{configuration[$"BlockchainNode:{prefix}Node"]}{configuration[$"BlockchainNode:{prefix}Key"]}";
            _ipfsSettings = ipfsSettings;

            _contractName = configuration["CollectionContract:Name"];
            _contractVersion = configuration["CollectionContract:Version"];
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// Collections

        /// <summary>
        /// Seller: Gets their collections
        /// </summary>
        /// <returns>List of My Collection</returns>
        /// <response code="200">List of My Collection</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet()]
        [Route("GetMyCollections")]
        [ProducesResponseType(typeof(List<GetMyCollectionResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMyCollections()
        {
            try
            {
                // Get the current user
                var masterUserId = HttpContextClaims.GetMasterUserId(HttpContext);
                var user = await _db.GetUserMasterId(masterUserId);

                // Get the collections for the user
                var collections = await _db.GetMyCollections(user.UserId);

                // Map over the database, we can use the user info as this is their collections
                var result = new List<GetMyCollectionResponse>();
                foreach (var collection in collections)
                {
                    result.Add(new GetMyCollectionResponse
                    {
                        CollectionId = collection.CollectionId,
                        Name = collection.Name,
                        Image = $"/api/v1/Collection/GetCollectionImage/{collection.CollectionId}",
                        Banner = $"/api/v1/Collection/GetCollectionBanner/{collection.CollectionId}",
                        ItemCount = collection.ItemCount.GetValueOrDefault(),
                        Royalties = collection.Royalties.GetValueOrDefault(),
                        Status = (GetMyCollectionResponse.MyCollectionCollectionStatuses)Enum.Parse(typeof(GetMyCollectionResponse.MyCollectionCollectionStatuses), collection.Status.ToString()),
                        Author = new MyCollectionUser {
                            UserId = user.UserId,
                            UserName = $"{user.FirstName} {user.LastName}"
                        }
                    });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: {Method}, Exception: {Message}", "GetMyCollections", ex.Message);

                return Problem(title: "/MyCollection/GetMyCollections", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }

        /// <summary>
        /// Seller: Gets a collection details
        /// </summary>
        /// <param name="collectionId"></param>
        /// <returns>My Collection</returns>
        /// <response code="200">My Collection</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet()]
        [Route("GetMyCollection/{collectionId::int}")]
        [ProducesResponseType(typeof(List<GetMyCollectionResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMyCollection(int collectionId)
        {
            try
            {
                // Get the collection
                var collection = await _db.GetCollection(collectionId);
                if (collection == null)
                    throw new Exception("Invalid collection id");

                // Get the current user
                var masterUserId = HttpContextClaims.GetMasterUserId(HttpContext);
                var user = await _db.GetUserMasterId(masterUserId);

                // Make sure this collection is theirs
                if (collection.AuthorId != user.UserId)
                    throw new Exception("This collection does not belong to this user");

                var result = new GetMyCollectionResponse
                {
                    CollectionId = collection.CollectionId,
                    Name = collection.Name,
                    Image = $"/api/v1/Collection/GetCollectionImage/{collection.CollectionId}",
                    Banner = $"/api/v1/Collection/GetCollectionBanner/{collection.CollectionId}",
                    ItemCount = collection.ItemCount.GetValueOrDefault(),
                    Royalties = collection.Royalties.GetValueOrDefault(),
                    VolumeTraded = collection.VolumeTraded.GetValueOrDefault(),
                    Status = (GetMyCollectionResponse.MyCollectionCollectionStatuses)Enum.Parse(typeof(GetMyCollectionResponse.MyCollectionCollectionStatuses), collection.Status.ToString()),
                    Author = new MyCollectionUser
                    {
                        UserId = user.UserId,
                        UserName = $"{user.FirstName} {user.LastName}"
                    }
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: {Method}, Exception: {Message}", "GetMyCollection", ex.Message);

                return Problem(title: "/MyCollection/GetMyCollection", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }

        /// <summary>
        /// MyCollection Page: view model for add/edit collection form
        /// </summary>
        /// <param name="input"></param>
        /// <returns>My Collection</returns>
        /// <response code="200">My Collection</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet()]
        [Route("GetMyCollectionViewModel")]
        [ProducesResponseType(typeof(CollectionViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMyCollectionViewModel([FromQuery] CollectionViewModelParams input)
        {
            try
            {
                var viewModel = new CollectionViewModel();
                if (input.CollectionId.HasValue)
                {
                    var collection = await _db.GetCollection(input.CollectionId.Value);
                    if (collection == null)
                        throw new ArgumentException("Invalid collection id");

                    viewModel.CollectionId = input.CollectionId.Value;
                    viewModel.Name = collection.Name;
                    viewModel.Description = collection.Description;
                    viewModel.ContractSymbol = collection.ContractSymbol;
                    viewModel.Royalties = collection.Royalties;
                }

                return Ok(viewModel);
            }
            catch (ArgumentException ae)
            {
                _logger.LogError("Method: {Method}, Exception: {Message}", "GetMyCollectionAddForm", ae.Message);

                return BadRequest(ae.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: {Method}, Exception: {Message}", "GetMyCollectionAddForm", ex.Message);

                return Problem(title: "/MyCollection/GetMyCollectionAddForm", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }


        /// <summary>
        /// MyCollection Action: Adds a Collection to My Collection from the input form
        /// </summary>
        /// <returns></returns>
        /// <response code="200"></response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost()]
        [Route("AddMyCollection")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddMyCollection([FromForm] CollectionViewModel request)
        {
            try
            {
                // Validations
                var validator = new CollectionViewModelValidator(_db, false);
                await validator.ValidateAndThrowAsync(request);

                // Determine the logged in user
                var masterUserId = HttpContextClaims.GetMasterUserId(HttpContext);
                var user = await _db.GetUserMasterId(masterUserId);

                // Get the users wallet, they have to pay to create the collection, my test wallet address
                var myAddress = _testWalletPublicAddress;
                var myAccount = _testWalletPrivateAddress;
                if (!_useTestWallet)
                {
                    var wallet = await _wallet.GetSignature(masterUserId);
                    myAddress = wallet.Address;
                    myAccount = wallet.Value;
                }

                // Get the image data
                var banner = UploadFileHandler.GetFileContents(request.Banner);
                var image = UploadFileHandler.GetFileContents(request.CollectionImage);

                // Push Image to IPS
                var nftIpfsService = new NFTIpfsService(_ipfsSettings.Value.Api, _ipfsSettings.Value.Project, _ipfsSettings.Value.Key);

                var prefixName = $"{request.Name.Replace(" ", "")}";

                IPFSFileInfo? imageIpfs = null;
                if (image != null)
                {
                    var imageName = Path.GetFileNameWithoutExtension(request.CollectionImage.FileName).Replace(" ", "");
                    var imageExt = Path.GetExtension(request.CollectionImage.FileName);

                    imageIpfs = await nftIpfsService.Add(image, $"{prefixName}_{imageName}_image.{imageExt}");
                }

                // Define contract meta data
                var metaDataContract = new ERC721ContractMetaData
                {
                    Name = request.Name,
                    Image = _ipfsSettings.Value.Gateway + imageIpfs.Hash,
                    Description = request.Description,
                    ExternalLink = "https://tesora.art/collection/view/#",
                    SellerFeeBasisPoints = request.Royalties ?? 0.00m * 100.00m,
                    FeeRecipient = myAddress
                };

                // Create meta data
                var metaString = JsonSerializer.Serialize<ERC721ContractMetaData>(metaDataContract);
                var codedMetaData = $"data:application/json;base64, {Base64Helper.Base64Encode(metaString)}";
                var pubMetaData = System.Text.Encoding.UTF8.GetBytes(codedMetaData);

                //Adding the metadata to ipfs
                var metaDataName = $"{prefixName}_meta.json";
                var metadataIpfs = await nftIpfsService.Add(pubMetaData, metaDataName);

                // Get the ERC721 contract
                //var contract = await _db.GetContractByName(_contractName, _contractVersion);

                // Create the account object to work with
                var account = new Nethereum.Web3.Accounts.Account(myAccount);

                // Get an instance to the network node
                var web3 = new Web3(account, _blockchainNodeAndKey);

                var chain = await web3.Eth.ChainId.SendRequestAsync();
                int ChainId = (int)chain.Value;

                var inputParms = new NFTCollectionDeployment
                {
                    Name_ = request.Name ?? "Tesora Collection",
                    Symbol_ = request.ContractSymbol ?? "C2IT",
                    ContractURI_ = _ipfsSettings.Value.Gateway + metadataIpfs.Hash
                };

                // var estimateGas = await web3.Eth.DeployContract.EstimateGasAsync<NFTCollectionDeployment>(NFTCollectionDeploymentBase.BYTECODE, myAddress, inputParms);

                // var receipt = await web3.Eth.DeployContract.SendRequestAndWaitForReceiptAsync<NFTCollectionDeployment>(NFTCollectionDeploymentBase.BYTECODE, myAddress, estimateGas, inputParms);

                var receipt = await NFTCollectionService.DeployContractAndWaitForReceiptAsync(web3, inputParms);

                var record = new Collection
                {
                    CollectionId = 0,
                    AuthorId = user.UserId,
                    Banner = banner,
                    CollectionImage = image,
                    ContractAddress = receipt.ContractAddress,
                    ContractSymbol = request.ContractSymbol,
                    Description = request.Description,
                    ItemCount = 0,
                    Royalties = request.Royalties,
                    Name = request.Name ?? "",
                    Status = Collection.CollectionStatuses.inactive,
                    VolumeTraded = 0,
                    CreateDate = DateTime.UtcNow,
                    TransactionHash = receipt.TransactionHash,
                    BannerImageType = request.Banner?.ContentType,
                    CollectionImageType = request.CollectionImage?.ContentType,
                    CollectionImageIpfs = _ipfsSettings.Value.Gateway + imageIpfs.Hash,
                    ChainId = ChainId,
                };

                await _db.PostCollection(record);

                return Ok("Collection Created");
            }
            catch (Nethereum.JsonRpc.Client.RpcResponseException rpc)
            {
                return BadRequest(rpc.Message);
            }
            catch (ValidationException ve)
            {
                var errors = new List<string>();

                foreach (var item in ve.Errors)
                    errors.Add(item.ErrorMessage);

                return BadRequest(errors);
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: {Method}, Exception: {Message}", "AddCollection", ex.Message);

                return Problem(title: "/MyCollection/AddCollection", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }


        /// <summary>
        /// MyCollection Action: Updates a Collection in My Collection
        /// </summary>
        /// <returns></returns>
        /// <response code="200"></response>
        /// <response code="500">Internal Server Error</response>
        [HttpPut()]
        [Route("UpdateMyCollection")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateMyCollection([FromForm] UpdateCollectionRequest request)
        {
            try
            {
                // Get the images
                var banner = UploadFileHandler.GetFileContents(request.Banner);
                var image = UploadFileHandler.GetFileContents(request.CollectionImage);

                var record = new Collection
                {
                    ContractSymbol = request.ContractSymbol,
                    Banner = banner,
                    BannerImageType = request.Banner?.ContentType,
                    CollectionImage = image,
                    CollectionImageType = request.CollectionImage?.ContentType,
                    Description = request.Description,
                    Name = request.Name,
                    Royalties = request.Royalties,
                    Status = Collection.CollectionStatuses.inactive
                };

                await _db.PutCollection(record);

                return Ok("Collection Updated");

            }
            catch (Exception ex)
            {
                _logger.LogError("Method: {Method}, Exception: {Message}", "UpdateCollection", ex.Message);

                return Problem(title: "/MyCollection/UpdateCollection", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// Items

        /// <summary>
        /// Seller: Gets items associated with a collection
        /// </summary>
        /// <returns>List of my items</returns>
        /// <response code="200">List of my items</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet()]
        [Route("GetMyItems/{collectionId::int}")]
        [ProducesResponseType(typeof(List<GetMyItemResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMyItems(int collectionId)
        {
            try
            {
                // Get the current user
                var masterUserId = HttpContextClaims.GetMasterUserId(HttpContext);
                var user = await _db.GetUserMasterId(masterUserId);

                // Get the collections for the user
                var items = await _db.GetMyItems(collectionId);

                // Map over the database, we can use the user info as this is their collections
                var result = new List<GetMyItemResponse>();
                foreach (var item in items)
                {
                    result.Add(new GetMyItemResponse
                    {
                        AuthorId = item.AuthorId,
                        CategoryId = item.CategoryId,
                        CollectionId = item.CollectionId,
                        CreateDate = item.CreateDate,
                        CurrentOwner = item.CurrentOwner,
                        Description = item.Description,
                        EnableAuction = item.EnableAuction,
                        EndDate = item.EndDate,
                        ExternalLink = item.ExternalLink,
                        AcceptOffer = item.AcceptOffer,
                        ItemId = item.ItemId,
                        LastViewed = item.LastViewed,
                        LikeCount = item.LikeCount,
                        MediaIpfs = item.MediaIpfs,
                        MintedDate = item.MintedDate,
                        MintTrans = item.MintTrans,
                        Name = item.Name,
                        Price = item.Price,
                        Currency = item.Currency,
                        StartDate = item.StartDate,
                        Status = item.Status.ToString(),
                        ThumbIpfs = item.ThumbIpfs,
                        TokenId = item.TokenId,
                        UnlockContentUrl = item.UnlockContentUrl,
                        ViewCount = item.ViewCount
                    });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: {Method}, Exception: {Message}", "GetMyItems", ex.Message);

                return Problem(title: "/MyCollection/GetMyItems", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }

        /// <summary>
        /// Seller: Gets an item details
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns>My Item</returns>
        /// <response code="200">My Item</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet()]
        [Route("GetMyItem/{itemId::int}")]
        [ProducesResponseType(typeof(List<GetMyItemResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMyItem(int itemId)
        {
            try
            {
                // Get the item
                var item = await _db.GetItem(itemId);
                if (item == null)
                    throw new ArgumentException("Invalid item number");

                // Get the current user
                var masterUserId = HttpContextClaims.GetMasterUserId(HttpContext);
                var user = await _db.GetUserMasterId(masterUserId);

                // Is this item for this user?
                if (item.AuthorId != user.UserId)
                    throw new ArgumentException("This item does not belong to this user");

                var result = new GetMyItemResponse
                {
                    AuthorId = item.AuthorId,
                    CategoryId = item.CategoryId,
                    CollectionId = item.CollectionId,
                    CreateDate = item.CreateDate,
                    CurrentOwner = item.CurrentOwner,
                    Description = item.Description,
                    EnableAuction = item.EnableAuction,
                    EndDate = item.EndDate,
                    ExternalLink = item.ExternalLink,
                    AcceptOffer = item.AcceptOffer,
                    ItemId = item.ItemId,
                    LastViewed = item.LastViewed,
                    LikeCount = item.LikeCount,
                    MediaIpfs = item.MediaIpfs,
                    MintedDate = item.MintedDate,
                    MintTrans = item.MintTrans,
                    Name = item.Name,
                    Price = item.Price,
                    Currency = item.Currency,
                    StartDate = item.StartDate,
                    Status = item.Status.ToString(),
                    ThumbIpfs = item.ThumbIpfs,
                    TokenId = item.TokenId,
                    UnlockContentUrl = item.UnlockContentUrl,
                    ViewCount = item.ViewCount
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: {Method}, Exception: {Message}", "GetMyItem", ex.Message);

                return Problem(title: "/MyCollection/GetMyItem", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }

        /// <summary>
        /// MyCollection Item: view model input form to add
        /// </summary>
        /// <param name="input"></param>
        /// <returns>My Item</returns>
        /// <response code="200">View Model</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet()]
        [Route("GetMyItemViewModel")]
        [ProducesResponseType(typeof(ItemViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMyItemViewModel([FromQuery] ItemViewModelParams input)
        {
            try
            {
                // Items are tied to collections
                var viewModel = new ItemViewModel
                {
                    CollectionId = input.CollectionId,
                };

                // If edit, load data
                if (input.ItemId.HasValue)
                {
                    // Get the item
                    var item = await _db.GetItem(input.ItemId.Value);
                    if (item == null)
                        throw new ArgumentException("Invalid item number");

                    viewModel.ItemId = input.ItemId.Value;
                    viewModel.Name = item.Name;
                    viewModel.Description = item.Description;
                }

                return Ok(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: {Method}, Exception: {Message}", "GetMyItemViewModel", ex.Message);

                return Problem(title: "/MyCollection/GetMyItemViewModel", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }


        /// <summary>
        /// MyCollection Item Action: Adds a Item to My Collection from input form
        /// </summary>
        /// <returns></returns>
        /// <response code="200"></response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost()]
        [Route("AddMyItem")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddMyItem([FromForm] ItemViewModel request)
        {
            try
            {
                // Validations
                var validator = new ItemViewModelValidator(_db);
                await validator.ValidateAndThrowAsync(request);

                // Get the current user
                var masterUserId = HttpContextClaims.GetMasterUserId(HttpContext);
                var user = await _db.GetUserMasterId(masterUserId);

                // Get collection for this item
                var collection = await _db.GetCollection(request.CollectionId);
                if (collection.ContractAddress == null)
                    throw new ArgumentException("Collection is missing the NFT contract address");

                // Get uploaded images
                var thumb = UploadFileHandler.GetFileContents(request.Thumb);
                var media = UploadFileHandler.GetFileContents(request.Media);

                // Push Image to IPS
                var nftIpfsService = new NFTIpfsService(_ipfsSettings.Value.Api, _ipfsSettings.Value.Project, _ipfsSettings.Value.Key);

                var prefixName = $"{collection.Name.Replace(" ", "")}_{request.Name.Replace(" ", "")}";

                // Thumb image
                IPFSFileInfo? thumbIpfs = null;
                if (thumb != null)
                {
                    var thumbName = Path.GetFileNameWithoutExtension(request.Thumb.FileName).Replace(" ", "");
                    var thumbExt = Path.GetExtension(request.Thumb.FileName);

                    thumbIpfs = await nftIpfsService.Add(thumb, $"{prefixName}_{thumbName}_thumb.{thumbExt}");
                }

                // Image
                IPFSFileInfo? mediaIpfs = null;
                if (media != null)
                {
                    var mediaName = Path.GetFileNameWithoutExtension(request.Media.FileName).Replace(" ", "");
                    var mediaExt = Path.GetExtension(request.Media.FileName);

                    mediaIpfs = await nftIpfsService.Add(media, $"{prefixName}_{mediaName}_media.{mediaExt}");
                }

                // Create Meta Data
                // NEEDED: determine the external link to pull up this item in our website

                long unixTime = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds();

                var metaDataNFT = new ERC721ItemMetaData
                {
                    Name = request.Name,
                    Image = _ipfsSettings.Value.Gateway + mediaIpfs.Hash,
                    Description = request.Description,
                    ExternalLink = "https://tesora.art/item"
                };

                if (request.Attributes != null)
                {
                    metaDataNFT.Attributes = new List<ERC721ItemMetaDataAttributes>();

                    foreach (var attribute in request.Attributes)
                    {
                        if (attribute.ItemType == ItemAttribute.ItemTypes.GeneralProperty)
                        {
                            metaDataNFT.Attributes.Add(new ERC721ItemMetaDataAttributes
                            {
                                Value = attribute.Value
                            });
                        }
                        else if (attribute.ItemType == ItemAttribute.ItemTypes.SpecificProperty)
                        {
                            metaDataNFT.Attributes.Add(new ERC721ItemMetaDataAttributes
                            {
                                TraitType = attribute.Title,
                                Value = attribute.Value
                            });
                        }
                        else if (attribute.ItemType == ItemAttribute.ItemTypes.BoostValue)
                        {
                            metaDataNFT.Attributes.Add(new ERC721ItemMetaDataAttributes
                            {
                                DisplayType = "boost_number",
                                TraitType = attribute.Title,
                                Number = Convert.ToDecimal(attribute.Value)
                            });
                        }
                        else if (attribute.ItemType == ItemAttribute.ItemTypes.BoostPercent)
                        {
                            metaDataNFT.Attributes.Add(new ERC721ItemMetaDataAttributes
                            {
                                DisplayType = "boost_percentage",
                                TraitType = attribute.Title,
                                Number = Convert.ToDecimal(attribute.Value)
                            });
                        }
                        else if (attribute.ItemType == ItemAttribute.ItemTypes.Statistic)
                        {
                            metaDataNFT.Attributes.Add(new ERC721ItemMetaDataAttributes
                            {
                                DisplayType = "level",
                                TraitType = attribute.Title,
                                Number = Convert.ToDecimal(attribute.Value)
                            });
                        }
                        else if (attribute.ItemType == ItemAttribute.ItemTypes.Ranking)
                        {
                            metaDataNFT.Attributes.Add(new ERC721ItemMetaDataAttributes
                            {
                                DisplayType = "number",
                                TraitType = attribute.Title,
                                Number = Convert.ToDecimal(attribute.Value)
                            });
                        }
                    }
                }

                //Adding the metadata to ipfs
                var metaDataName = $"{prefixName}_meta.json";
                var metadataIpfs = await nftIpfsService.AddNftsMetadataToIpfsAsync<ERC721ItemMetaData>(metaDataNFT, metaDataName);

                // Get the users wallet, they have to pay
                var myAddress = _testWalletPublicAddress;
                var myAccount = _testWalletPrivateAddress;
                if (!_useTestWallet)
                {
                    var wallet = await _wallet.GetSignature(masterUserId);
                    myAddress = wallet.Address;
                    myAccount = wallet.Value;
                }

                // Create the account object to work with
                var account = new Nethereum.Web3.Accounts.Account(myAccount);

                // Get an instance to the network node
                var web3 = new Web3(account, _blockchainNodeAndKey);

                var nftCollectionService = new NFTCollectionService(web3, collection.ContractAddress);

                // Minting function parameters
                var inputParams = new MintFunction
                {
                    To = myAddress,
                    TokenURI_ = _ipfsSettings.Value.Gateway + metadataIpfs.Hash,
                    RoyaltyRecipient = myAddress,
                    RoyaltyValue = new BigInteger(collection.Royalties ?? 0.00m)
                };

                // Mint NFT
                var mintReceipt = await nftCollectionService.MintRequestAndWaitForReceiptAsync(inputParams);

                // Get the token id of the minted token
                var tokenId = Convert.ToInt32(mintReceipt.Logs[0]["topics"][3].ToString(), 16);

                // Add the item
                var record = new Item
                {
                    ItemId = 0,
                    Name = request.Name,
                    Price = request.Price,
                    Currency = request.Currency,
                    AuctionReserve = request.AuctionReserve,
                    CategoryId = request.CategoryId,
                    CollectionId = request.CollectionId,
                    TokenId = tokenId,
                    MintedDate = DateTime.UtcNow,
                    EnableAuction = request.EnableAuction,
                    CreateDate = DateTime.UtcNow,
                    Status = request.Status,
                    AuthorId = user.UserId,
                    CurrentOwner = user.UserId,
                    LikeCount = 0,
                    Description = request.Description,
                    ExternalLink = request.ExternalLink,
                    AcceptOffer = request.AcceptOffer,
                    StartDate = request.StartDate?.ToUniversalTime(),
                    EndDate = request.EndDate?.ToUniversalTime(),
                    Media = media,
                    MediaIpfs = _ipfsSettings.Value.Gateway + mediaIpfs.Hash,
                    MintTrans = mintReceipt.TransactionHash,
                    Thumb = thumb,
                    ItemImageType = UploadFileHandler.GetFileExtention(request.Media),
                    ThumbIpfs = thumbIpfs == null ? null : _ipfsSettings.Value.Gateway + thumbIpfs.Hash,
                    ViewCount = 0,
                };

                var iNewItemId = await _db.PostItem(record);

                // Add the attributes
                if (request.Attributes != null)
                {
                    foreach (var attribute in request.Attributes)
                    {
                        await _db.PostItemAttribute(new ItemAttribute
                        {
                            ItemId = iNewItemId,
                            ItemType = attribute.ItemType,
                            MaxValue = attribute.MaxValue,
                            Title = attribute.Title,
                            Value = attribute.Value
                        });
                    }
                }

                return Ok("Item Added");
            }
            catch (ValidationException ve)
            {
                var errors = new List<string>();

                foreach (var item in ve.Errors)
                    errors.Add(item.ErrorMessage);

                return BadRequest(errors);
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: {Method}, Exception: {Message}", "AddItem", ex.Message);

                return Problem(title: "/MyCollection/AddItem", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }


        /// <summary>
        /// MyCollection Item Action: Sets the ability to enable offers on one of their items
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="acceptOffer"></param>
        /// <returns></returns>
        /// <response code="200"></response>
        /// <response code="500">Internal Server Error</response>
        [HttpPut()]
        [Route("PutMyItemAcceptOffer")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> MyItemAcceptOffer(int itemId, bool acceptOffer)
        {
            try
            {
                await _db.PutMyItemAcceptOffer(itemId, acceptOffer);

                return Ok("Accept Offer Updated");
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: {Method}, Exception: {Message}", "MyItemAcceptOffer", ex.Message);

                return Problem(title: "/MyCollection/MyItemAcceptOffer", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }

        }


        /// <summary>
        /// MyCollection Item Action: Control the item sale, either fixed price, auction or not for sale
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost()]
        [Route("PostMyItemSale")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostSale([FromForm] PostSaleRequest request)
        {
            try
            {
                // Rules
                // - only accept offers if fixed price
                // - price is the reserve price for the auction
                var item = await _db.GetItem(request.ItemId);

                var record = new ItemSale
                {
                    ItemId = request.ItemId,
                    Price = request.FixedPrice == null? item.Price: request.FixedPrice,
                    Currency = request.Currency,
                    EnableAuction = request.SaleType == PostSaleRequest.SaleTypes.Auction,
                    StartDate = request.AuctionStartDate,
                    EndDate = request.AuctionEndDate,
                    Status = request.SaleType == PostSaleRequest.SaleTypes.NotForSale ? Item.ItemStatuses.inactive : Item.ItemStatuses.active,
                    AcceptOffer = request.AcceptOffer,
                    AuctionReserve = request.ReservePrice
                };

                await _db.PostItemSale(record);
    
                return Ok("Item Sale Posted");
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: {Method}, Exception: {Message}", "PostSale", ex.Message);

                return Problem(title: "/MyCollection/PostSale", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }
    }
}

