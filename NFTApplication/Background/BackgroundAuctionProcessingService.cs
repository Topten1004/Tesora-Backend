// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//
using Cronos;
using Nethereum.Web3;
using NFTApplication.Contracts.NFTCollection.ContractDefinition;
using NFTApplication.Contracts.NFTCollection;
using NFTDatabaseEntities;
using NFTDatabaseService;
using System.Numerics;
using NFTWalletService;


//                                      Allowed values    Allowed special characters   Comment
//┌───────────── second(optional)       0 - 59              * , - /
//│ ┌───────────── minute               0 - 59              * , - /
//│ │ ┌───────────── hour               0 - 23              * , - /
//│ │ │ ┌───────────── day of month      1-31               * , - / L W ?                
//│ │ │ │ ┌───────────── month           1-12 or JAN-DEC    * , - /                      
//│ │ │ │ │ ┌───────────── day of week   0-6  or SUN-SAT    * , - / # L ?                Both 0 and 7 means SUN
//│ │ │ │ │ │
//* * * * * *


namespace NFTApplication.Background
{
    /// <summary>
    /// Backgroud process
    /// </summary>
    public class AuctionProcessingBackgroundService : BackgroundService
    {
        private const string schedule = "0 0 * * *"; // every day (any day of the week, any month, any day of the month, at 12:00AM)
        private readonly CronExpression _cron;
        private readonly INFTDatabaseService _db;
        private readonly INFTWalletService _wallet;
        private readonly string _blockchainNodeAndKey;
        private readonly bool _useTestWallet;
        private readonly string? _testWalletPublicAddress;
        private readonly string? _testWalletPrivateAddress;
        private readonly ILogger<AuctionProcessingBackgroundService> _logger;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="db"></param>
        /// <param name="logger"></param>
        /// <param name="wallet"></param>
        /// <param name="configuration"></param>
        public AuctionProcessingBackgroundService(INFTDatabaseService db, INFTWalletService wallet, IConfiguration configuration, ILogger<AuctionProcessingBackgroundService> logger)
        {
            _db = db;
            _logger = logger;
            _wallet = wallet;

            _useTestWallet = Convert.ToBoolean(configuration["TestWallet:UseTestWallet"]);
            _testWalletPublicAddress = configuration["TestWallet:PublicKey"];
            _testWalletPrivateAddress = configuration["TestWallet:PrivateKey"];

            var prefix = configuration["Environment:Prefix"];
            _blockchainNodeAndKey = $"{configuration[$"BlockchainNode:{prefix}Node"]}{configuration[$"BlockchainNode:{prefix}Key"]}";

            _cron = CronExpression.Parse(schedule);
        }

        /// <summary>
        /// Execute
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug("AuctionProcessingBackgroundService is starting");

            stoppingToken.Register(() => _logger.LogDebug("AuctionProcessingBackgroundService task is stopping"));

            while (!stoppingToken.IsCancellationRequested)
            {
                var utcNow = DateTime.UtcNow;
                var nextUtc = _cron.GetNextOccurrence(utcNow);
                var delaySpan = nextUtc.Value - utcNow;

                await Task.Delay(delaySpan, stoppingToken);

                await ProcessAuctions();
            }
        }

        /// <summary>
        /// ProcessAuctions
        /// </summary>
        /// <returns></returns>
        private async Task ProcessAuctions()
        {
            try
            {
                // Get all the items that were on auction and the auction ended yesterday
                var endedAuctions = await _db.GetAuctionEndedItems();

                foreach(var item in endedAuctions)
                {
                    var sellerUser = await _db.GetUser((int)item.CurrentOwner);
                    var auctions = await _db.GetAuctionsByItemId(item.ItemId);

                    var latestAuction = auctions.LastOrDefault();

                    if ( latestAuction != null)
                    {
                        if (item.CurrentOwner != latestAuction.SenderId)
                        {
                            if (item.AuctionReserve < latestAuction.Price)
                            {
                                // Get collection for this item
                                var collection = await _db.GetCollection((int)item.CollectionId);

                                if (collection.ContractAddress == null)
                                    throw new Exception("Collection is missing the NFT contract address");

                                // Need to get the buyers wallet, they have to pay
                                var buyerAddress = _testWalletPublicAddress;
                                var buyerAccount = _testWalletPrivateAddress;

                                var sellerAddress = _testWalletPublicAddress;
                                var sellerAccount = _testWalletPrivateAddress;

                                if (!_useTestWallet)
                                {
                                    var buyer = await _db.GetUser((int)latestAuction.SenderId);
                                    var wallet = await _wallet.GetSignature(buyer.MasterUserId);

                                    buyerAddress = wallet.Address;
                                    buyerAccount = wallet.Value;

                                    var sellerWallet = await _wallet.GetSignature(sellerUser.MasterUserId);
                                    sellerAddress = sellerWallet.Address;
                                    sellerAccount = sellerWallet.Value;
                                }

                                // Token Id, we may want to switch the db to varbinary and store as a byte array
                                BigInteger tokenId = item.TokenId.Value;

                                // Price in Wei
                                BigInteger purchaseAmtWei = Web3.Convert.ToWei(latestAuction.Price.Value);

                                // Create the account object to work with
                                var account = new Nethereum.Web3.Accounts.Account(buyerAccount);
                                var ownerAccount = new Nethereum.Web3.Accounts.Account(sellerAccount);

                                // Get an instance to the network node
                                var web3 = new Web3(account, _blockchainNodeAndKey);
                                var ownerWeb3 = new Web3(ownerAccount, _blockchainNodeAndKey);

                                // Get the contract interface
                                var nftCollectionService = new NFTCollectionService(web3, collection.ContractAddress);
                                var nftCollectionApproveService = new NFTCollectionService(ownerWeb3, collection.ContractAddress);

                                // Buy the NFT
                                var buyNftParams = new BuyNFTFunction
                                {
                                    TokenId = tokenId,
                                    AmountToSend = purchaseAmtWei
                                };

                                var approveParams = new ApproveFunction
                                {
                                    To = buyerAddress,
                                    TokenId = tokenId
                                };

                                var approveReceipt = await nftCollectionApproveService.ApproveRequestAndWaitForReceiptAsync(approveParams);
                                var buyNftReceipt = await nftCollectionService.BuyNFTRequestAndWaitForReceiptAsync(buyNftParams);

                                AuctionAccept log = new AuctionAccept
                                {
                                    UserId = (int)latestAuction.SenderId,
                                    AuctionId = latestAuction.AuctionId,
                                    TransactionHash = buyNftReceipt.TransactionHash
                                };

                                await _db.AcceptMyAuction(log);
                            }
                            else
                            {
                                foreach (var auction in auctions)
                                {
                                    Offer offer = new Offer
                                    {
                                        ItemId = auction.ItemId,
                                        SenderId = auction.SenderId,
                                        ReceiverId = auction.ReceiverId,
                                        Price = auction.Price,
                                        CreateDate = auction.CreateDate
                                    };
                                    await _db.PostOffer(offer);
                                }
                            }
                        }
                    }
                    await _db.PutAuctionEndItem(item.ItemId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Auction Processing Background Service: {Message} ", ex.Message);
            }

        }
    }
}
