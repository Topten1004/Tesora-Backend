// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//

using Npgsql;
using NpgsqlTypes;

using NFTDatabaseEntities;
using System.Transactions;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;

namespace NFTDatabase.DataAccess
{
    internal partial class PostgreSql : IPostgreSql
    {
        /// <summary>
        /// Create a new record in Item
        /// </summary>
        /// <param name="record">Item</param>
        /// <returns></returns>
        public async Task<int> CreateItem(Item record)
        {
            int iNewId = -1;

            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "insert into tesora_nft.items" +
                              " (name,description,external_link,media,thumb,accept_offer,unlock_content_url,view_count,like_count,price,currency,token_id,category_id,collection_id,current_owner,author_id,status,minted_date,create_date,start_date,end_date,enable_auction,media_ipfs,thumb_ipfs,item_image_type,last_viewed,auction_reserve)" +
                              " values" +
                              " (@name,@description,@external_link,@media,@thumb,@accept_offer,@unlock_content_url,@view_count,@like_count,@price,@currency,@token_id,@category_id,@collection_id,@current_owner,@author_id,@status,@minted_date,@create_date,@start_date,@end_date,@enable_auction,@media_ipfs,@thumb_ipfs,@item_image_type,@last_viewed,@auction_reserve)" +
                              " returning item_id";
                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@name", NpgsqlDbType.Varchar).Value = record.Name;
                    cmd.Parameters.Add("@description", NpgsqlDbType.Varchar).Value = record.Description ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@external_link", NpgsqlDbType.Varchar).Value = record.ExternalLink ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@media", NpgsqlDbType.Bytea).Value = record.Media ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@thumb", NpgsqlDbType.Bytea).Value = record.Thumb ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@accept_offer", NpgsqlDbType.Boolean).Value = record.AcceptOffer ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@unlock_content_url", NpgsqlDbType.Varchar).Value = record.UnlockContentUrl ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@view_count", NpgsqlDbType.Integer).Value = record.ViewCount ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@like_count", NpgsqlDbType.Integer).Value = record.LikeCount ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@price", NpgsqlDbType.Numeric).Value = record.Price ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@currency", NpgsqlDbType.Varchar).Value = record.Currency ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@token_id", NpgsqlDbType.Integer).Value = record.TokenId ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@category_id", NpgsqlDbType.Integer).Value = record.CategoryId ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@collection_id", NpgsqlDbType.Integer).Value = record.CollectionId ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@current_owner", NpgsqlDbType.Integer).Value = record.CurrentOwner ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@author_id", NpgsqlDbType.Integer).Value = record.AuthorId ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@status", NpgsqlDbType.Unknown).Value = record.Status.ToString();
                    cmd.Parameters.Add("@minted_date", NpgsqlDbType.TimestampTz).Value = record.MintedDate ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@create_date", NpgsqlDbType.TimestampTz).Value = record.CreateDate;
                    cmd.Parameters.Add("@start_date", NpgsqlDbType.TimestampTz).Value = record.StartDate ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@end_date", NpgsqlDbType.TimestampTz).Value = record.EndDate ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@enable_auction", NpgsqlDbType.Boolean).Value = record.EnableAuction ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@media_ipfs", NpgsqlDbType.Varchar).Value = record.MediaIpfs ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@thumb_ipfs", NpgsqlDbType.Varchar).Value = record.ThumbIpfs ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@item_image_type", NpgsqlDbType.Varchar).Value = record.ItemImageType ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@last_viewed", NpgsqlDbType.TimestampTz).Value = record.LastViewed ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@auction_reserve", NpgsqlDbType.Numeric).Value = record.AuctionReserve ?? (object)DBNull.Value;

                    var result = await cmd.ExecuteScalarAsync();

                    if (result != null)
                        iNewId = (int)result;
                }

                // Make history entry
                sSQL = "insert into tesora_nft.histories" +
                        " (item_id,collection_id,from_id,to_id,transaction_hash,price,currency,history_type,is_valid,create_date)" +
                        " values" +
                        " (@item_id,@collection_id,@from_id,@to_id,@transaction_hash,@price,@currency,@history_type,@is_valid,@create_date)";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@item_id", NpgsqlDbType.Integer).Value = iNewId;
                    cmd.Parameters.Add("@collection_id", NpgsqlDbType.Integer).Value = record.CollectionId;
                    cmd.Parameters.Add("@from_id", NpgsqlDbType.Integer).Value = record.AuthorId;
                    cmd.Parameters.Add("@to_id", NpgsqlDbType.Integer).Value = record.AuthorId;
                    cmd.Parameters.Add("@transaction_hash", NpgsqlDbType.Varchar).Value = record.MintTrans;
                    cmd.Parameters.Add("@price", NpgsqlDbType.Numeric).Value = record.Price;
                    cmd.Parameters.Add("@currency", NpgsqlDbType.Varchar).Value = record.Currency;
                    cmd.Parameters.Add("@history_type", NpgsqlDbType.Unknown).Value = History.HistoryTypes.minted.ToString();
                    cmd.Parameters.Add("@is_valid", NpgsqlDbType.Boolean).Value = true;
                    cmd.Parameters.Add("@create_date", NpgsqlDbType.TimestampTz).Value = DateTime.UtcNow;

                    await cmd.ExecuteNonQueryAsync();
                }

                // Increase item count

                sSQL = "update tesora_nft.collections set item_count = item_count + 1 where collection_id = @collection_id";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@collection_id", NpgsqlDbType.Integer).Value = record.CollectionId;

                    await cmd.ExecuteNonQueryAsync();
                }
            }

            return iNewId;
        }

        /// <summary>
        /// Retrieve all Item records
        /// </summary>
        /// <returns>List of Item records</returns>
        public async Task<List<Item>> RetrieveAllItems()
        {
            var lstItem = new List<Item>();

            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();
                string sSQL = "select item_id,name,description,external_link,media,thumb,accept_offer,unlock_content_url,view_count,like_count,price,currency,token_id,category_id,collection_id,current_owner,author_id,status,minted_date,create_date,start_date,end_date,enable_auction,media_ipfs,thumb_ipfs,item_image_type,last_viewed,auction_reserve" +
                              " from tesora_nft.items";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            lstItem.Add(new Item
                            {
                                ItemId = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Description = reader.IsDBNull(2) ? null : (string?)reader.GetString(2),
                                ExternalLink = reader.IsDBNull(3) ? null : (string?)reader.GetString(3),
                                Media = ReadBytes(reader, 4),
                                Thumb = ReadBytes(reader, 5),
                                AcceptOffer = reader.IsDBNull(6) ? null : (bool?)reader.GetBoolean(6),
                                UnlockContentUrl = reader.IsDBNull(7) ? null : (string?)reader.GetString(7),
                                ViewCount = reader.IsDBNull(8) ? null : (int?)reader.GetInt32(8),
                                LikeCount = reader.IsDBNull(9) ? null : (int?)reader.GetInt32(9),
                                Price = reader.IsDBNull(10) ? null : (decimal?)reader.GetDecimal(10),
                                Currency = reader.IsDBNull(11) ? null : (string?)reader.GetString(11),
                                TokenId = reader.IsDBNull(12) ? null : (int?)reader.GetInt32(12),
                                CategoryId = reader.IsDBNull(13) ? null : (int?)reader.GetInt32(13),
                                CollectionId = reader.IsDBNull(14) ? null : (int?)reader.GetInt32(14),
                                CurrentOwner = reader.IsDBNull(15) ? null : (int?)reader.GetInt32(15),
                                AuthorId = reader.IsDBNull(16) ? null : (int?)reader.GetInt32(16),
                                Status = (Item.ItemStatuses)Enum.Parse(typeof(Item.ItemStatuses), reader.GetString(17)),
                                MintedDate = reader.IsDBNull(18) ? null : (DateTime?)reader.GetDateTime(18),
                                CreateDate = reader.GetDateTime(19),
                                StartDate = reader.IsDBNull(20) ? null : (DateTime?)reader.GetDateTime(20),
                                EndDate = reader.IsDBNull(21) ? null : (DateTime?)reader.GetDateTime(21),
                                EnableAuction = reader.IsDBNull(22) ? null : (bool?)reader.GetBoolean(22),
                                MediaIpfs = reader.IsDBNull(23) ? null : (string?)reader.GetString(23),
                                ThumbIpfs = reader.IsDBNull(24) ? null : (string?)reader.GetString(24),
                                ItemImageType = reader.IsDBNull(25) ? null : (string?)reader.GetString(25),
                                LastViewed = reader.IsDBNull(26) ? null : (DateTime?)reader.GetDateTime(26),
                                AuctionReserve = reader.IsDBNull(27) ? null : (decimal?)reader.GetDecimal(27)
                            });
                        }
                    }
                }

            }

            return lstItem;
        }

        /// <summary>
        /// Retrieve all Item records
        /// </summary>
        /// <returns>List of Item records</returns>
        public async Task<List<Item>> RetrieveItems(int collectionId)
        {
            var lstItem = new List<Item>();

            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();
                string sSQL = "select item_id,name,description,external_link,media,thumb,accept_offer,unlock_content_url,view_count,like_count,price,currency,token_id,category_id,collection_id,current_owner,author_id,status,minted_date,create_date,start_date,end_date,enable_auction,media_ipfs,thumb_ipfs,item_image_type,last_viewed,auction_reserve" +
                              " from tesora_nft.items" +
                              " where collection_id = @collection_id";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@collection_id", NpgsqlDbType.Integer).Value = collectionId;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            lstItem.Add(new Item
                            {
                                ItemId = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Description = reader.IsDBNull(2) ? null : (string?)reader.GetString(2),
                                ExternalLink = reader.IsDBNull(3) ? null : (string?)reader.GetString(3),
                                Media = ReadBytes(reader, 4),
                                Thumb = ReadBytes(reader, 5),
                                AcceptOffer = reader.IsDBNull(6) ? null : (bool?)reader.GetBoolean(6),
                                UnlockContentUrl = reader.IsDBNull(7) ? null : (string?)reader.GetString(7),
                                ViewCount = reader.IsDBNull(8) ? null : (int?)reader.GetInt32(8),
                                LikeCount = reader.IsDBNull(9) ? null : (int?)reader.GetInt32(9),
                                Price = reader.IsDBNull(10) ? null : (decimal?)reader.GetDecimal(10),
                                Currency = reader.IsDBNull(11) ? null : (string?)reader.GetString(11),
                                TokenId = reader.IsDBNull(12) ? null : (int?)reader.GetInt32(12),
                                CategoryId = reader.IsDBNull(13) ? null : (int?)reader.GetInt32(13),
                                CollectionId = reader.IsDBNull(14) ? null : (int?)reader.GetInt32(14),
                                CurrentOwner = reader.IsDBNull(15) ? null : (int?)reader.GetInt32(15),
                                AuthorId = reader.IsDBNull(16) ? null : (int?)reader.GetInt32(16),
                                Status = (Item.ItemStatuses)Enum.Parse(typeof(Item.ItemStatuses), reader.GetString(17)),
                                MintedDate = reader.IsDBNull(18) ? null : (DateTime?)reader.GetDateTime(18),
                                CreateDate = reader.GetDateTime(19),
                                StartDate = reader.IsDBNull(20) ? null : (DateTime?)reader.GetDateTime(20),
                                EndDate = reader.IsDBNull(21) ? null : (DateTime?)reader.GetDateTime(21),
                                EnableAuction = reader.IsDBNull(22) ? null : (bool?)reader.GetBoolean(22),
                                MediaIpfs = reader.IsDBNull(23) ? null : (string?)reader.GetString(23),
                                ThumbIpfs = reader.IsDBNull(24) ? null : (string?)reader.GetString(24),
                                ItemImageType = reader.IsDBNull(25) ? null : (string?)reader.GetString(25),
                                LastViewed = reader.IsDBNull(26) ? null : (DateTime?)reader.GetDateTime(26),
                                AuctionReserve = reader.IsDBNull(27) ? null : (decimal?)reader.GetDecimal(27)
                            });
                        }
                    }
                }

                sSQL = "update tesora_nft.items set view_count = view_count + 1, last_viewed = @last_viewed where collection_id = @collection_id";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@last_viewed", NpgsqlDbType.TimestampTz).Value = DateTime.UtcNow;
                    cmd.Parameters.Add("@collection_id", NpgsqlDbType.Integer).Value = collectionId;

                    await cmd.ExecuteNonQueryAsync();
                }

            }

            return lstItem;
        }


        /// <summary>
        /// Retrieve Auction Ended Items
        /// </summary>
        /// <returns></returns>
        public async Task<List<Item>> RetrieveAuctionEndedItems()
        {
            var lstItem = new List<Item>();

            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();
                string sSQL = "select item_id,name,description,external_link,media,thumb,accept_offer,unlock_content_url,view_count,like_count,price,currency,token_id,category_id,collection_id,current_owner,author_id,status,minted_date,create_date,start_date,end_date,enable_auction,media_ipfs,thumb_ipfs,item_image_type,last_viewed,auction_reserve" +
                              " from tesora_nft.items" +
                              " where enable_auction = true and end_date < '" + DateTime.UtcNow.ToString("yyyy-MM-dd") + "'::date";

                using var cmd = new NpgsqlCommand(sSQL, conn);
                cmd.CommandType = System.Data.CommandType.Text;

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    lstItem.Add(new Item
                    {
                        ItemId = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Description = reader.IsDBNull(2) ? null : (string?)reader.GetString(2),
                        ExternalLink = reader.IsDBNull(3) ? null : (string?)reader.GetString(3),
                        Media = ReadBytes(reader, 4),
                        Thumb = ReadBytes(reader, 5),
                        AcceptOffer = reader.IsDBNull(6) ? null : (bool?)reader.GetBoolean(6),
                        UnlockContentUrl = reader.IsDBNull(7) ? null : (string?)reader.GetString(7),
                        ViewCount = reader.IsDBNull(8) ? null : (int?)reader.GetInt32(8),
                        LikeCount = reader.IsDBNull(9) ? null : (int?)reader.GetInt32(9),
                        Price = reader.IsDBNull(10) ? null : (decimal?)reader.GetDecimal(10),
                        Currency = reader.IsDBNull(11) ? null : (string?)reader.GetString(11),
                        TokenId = reader.IsDBNull(12) ? null : (int?)reader.GetInt32(12),
                        CategoryId = reader.IsDBNull(13) ? null : (int?)reader.GetInt32(13),
                        CollectionId = reader.IsDBNull(14) ? null : (int?)reader.GetInt32(14),
                        CurrentOwner = reader.IsDBNull(15) ? null : (int?)reader.GetInt32(15),
                        AuthorId = reader.IsDBNull(16) ? null : (int?)reader.GetInt32(16),
                        Status = (Item.ItemStatuses)Enum.Parse(typeof(Item.ItemStatuses), reader.GetString(17)),
                        MintedDate = reader.IsDBNull(18) ? null : (DateTime?)reader.GetDateTime(18),
                        CreateDate = reader.GetDateTime(19),
                        StartDate = reader.IsDBNull(20) ? null : (DateTime?)reader.GetDateTime(20),
                        EndDate = reader.IsDBNull(21) ? null : (DateTime?)reader.GetDateTime(21),
                        EnableAuction = reader.IsDBNull(22) ? null : (bool?)reader.GetBoolean(22),
                        MediaIpfs = reader.IsDBNull(23) ? null : (string?)reader.GetString(23),
                        ThumbIpfs = reader.IsDBNull(24) ? null : (string?)reader.GetString(24),
                        ItemImageType = reader.IsDBNull(25) ? null : (string?)reader.GetString(25),
                        LastViewed = reader.IsDBNull(26) ? null : (DateTime?)reader.GetDateTime(26),
                        AuctionReserve = reader.IsDBNull(27) ? null : (decimal?)reader.GetDecimal(27)
                    });
                }

            }

            return lstItem;
        }

        /// <summary>
        /// Auction ended without any acceptable bids
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public async Task PutAuctionEndItem(int itemId)
        {

            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "update tesora_nft.items set enable_auction = @enable_auction where item_id = @item_id";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {

                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@enable_auction", NpgsqlDbType.Boolean).Value = false;
                    cmd.Parameters.Add("@item_id", NpgsqlDbType.Integer).Value = itemId;

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        /// <summary>
        /// Retrieve all Item records
        /// </summary>
        /// <returns>List of Item records</returns>
        public async Task<List<Item>> RetrieveItemsByCategoryId(int categoryId)
        {
            var lstItem = new List<Item>();

            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "select item_id,name,description,external_link,media,thumb,accept_offer,unlock_content_url,view_count,like_count,price,currency,token_id,category_id,collection_id,current_owner,author_id,status,minted_date,create_date,start_date,end_date,enable_auction,media_ipfs,thumb_ipfs,item_image_type,last_viewed,auction_reserve" +
                              " from tesora_nft.items" +
                              " where category_id = @category_id " +
                              " order by create_date desc limit 5";

                if (categoryId > 0)
                {
                    using (var cmd = new NpgsqlCommand(sSQL, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.Text;

                        cmd.Parameters.Add("@category_id", NpgsqlDbType.Integer).Value = categoryId;

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                lstItem.Add(new Item
                                {
                                    ItemId = reader.GetInt32(0),
                                    Name = reader.GetString(1),
                                    Description = reader.IsDBNull(2) ? null : (string?)reader.GetString(2),
                                    ExternalLink = reader.IsDBNull(3) ? null : (string?)reader.GetString(3),
                                    Media = ReadBytes(reader, 4),
                                    Thumb = ReadBytes(reader, 5),
                                    AcceptOffer = reader.IsDBNull(6) ? null : (bool?)reader.GetBoolean(6),
                                    UnlockContentUrl = reader.IsDBNull(7) ? null : (string?)reader.GetString(7),
                                    ViewCount = reader.IsDBNull(8) ? null : (int?)reader.GetInt32(8),
                                    LikeCount = reader.IsDBNull(9) ? null : (int?)reader.GetInt32(9),
                                    Price = reader.IsDBNull(10) ? null : (Decimal?)reader.GetDecimal(10),
                                    Currency = reader.IsDBNull(11) ? null : (string?)reader.GetString(11),
                                    TokenId = reader.IsDBNull(12) ? null : (int?)reader.GetInt32(12),
                                    CategoryId = reader.IsDBNull(13) ? null : (int?)reader.GetInt32(13),
                                    CollectionId = reader.IsDBNull(14) ? null : (int?)reader.GetInt32(14),
                                    CurrentOwner = reader.IsDBNull(15) ? null : (int?)reader.GetInt32(15),
                                    AuthorId = reader.IsDBNull(16) ? null : (int?)reader.GetInt32(16),
                                    Status = (Item.ItemStatuses)Enum.Parse(typeof(Item.ItemStatuses), reader.GetString(17)),
                                    MintedDate = reader.IsDBNull(18) ? null : (DateTime?)reader.GetDateTime(18),
                                    CreateDate = reader.GetDateTime(19),
                                    StartDate = reader.IsDBNull(20) ? null : (DateTime?)reader.GetDateTime(20),
                                    EndDate = reader.IsDBNull(21) ? null : (DateTime?)reader.GetDateTime(21),
                                    EnableAuction = reader.IsDBNull(22) ? null : (bool?)reader.GetBoolean(22),
                                    MediaIpfs = reader.IsDBNull(23) ? null : (string?)reader.GetString(23),
                                    ThumbIpfs = reader.IsDBNull(24) ? null : (string?)reader.GetString(24),
                                    ItemImageType = reader.IsDBNull(25) ? null : (string?)reader.GetString(25),
                                    LastViewed = reader.IsDBNull(26) ? null : (DateTime?)reader.GetDateTime(26),
                                    AuctionReserve = reader.IsDBNull(27) ? null : (decimal?)reader.GetDecimal(27)
                                });
                            }
                        }
                    }
                }
                else
                {
                    sSQL = "select item_id,name,description,external_link,media,thumb,accept_offer,unlock_content_url,view_count,like_count,price,currency,token_id,category_id,collection_id,current_owner,author_id,status,minted_date,create_date,start_date,end_date,enable_auction,media_ipfs,thumb_ipfs,item_image_type,last_viewed,auction_reserve" +
                              " from tesora_nft.items" +
                              " order by create_date desc limit 5";

                    using (var cmd = new NpgsqlCommand(sSQL, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.Text;

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                lstItem.Add(new Item
                                {
                                    ItemId = reader.GetInt32(0),
                                    Name = reader.GetString(1),
                                    Description = reader.IsDBNull(2) ? null : (string?)reader.GetString(2),
                                    ExternalLink = reader.IsDBNull(3) ? null : (string?)reader.GetString(3),
                                    Media = ReadBytes(reader, 4),
                                    Thumb = ReadBytes(reader, 5),
                                    AcceptOffer = reader.IsDBNull(6) ? null : (bool?)reader.GetBoolean(6),
                                    UnlockContentUrl = reader.IsDBNull(7) ? null : (string?)reader.GetString(7),
                                    ViewCount = reader.IsDBNull(8) ? null : (int?)reader.GetInt32(8),
                                    LikeCount = reader.IsDBNull(9) ? null : (int?)reader.GetInt32(9),
                                    Price = reader.IsDBNull(10) ? null : (decimal?)reader.GetDecimal(10),
                                    Currency = reader.IsDBNull(11) ? null : (string?)reader.GetString(11),
                                    TokenId = reader.IsDBNull(12) ? null : (int?)reader.GetInt32(12),
                                    CategoryId = reader.IsDBNull(13) ? null : (int?)reader.GetInt32(13),
                                    CollectionId = reader.IsDBNull(14) ? null : (int?)reader.GetInt32(14),
                                    CurrentOwner = reader.IsDBNull(15) ? null : (int?)reader.GetInt32(15),
                                    AuthorId = reader.IsDBNull(16) ? null : (int?)reader.GetInt32(16),
                                    Status = (Item.ItemStatuses)Enum.Parse(typeof(Item.ItemStatuses), reader.GetString(17)),
                                    MintedDate = reader.IsDBNull(18) ? null : (DateTime?)reader.GetDateTime(18),
                                    CreateDate = reader.GetDateTime(19),
                                    StartDate = reader.IsDBNull(20) ? null : (DateTime?)reader.GetDateTime(20),
                                    EndDate = reader.IsDBNull(21) ? null : (DateTime?)reader.GetDateTime(21),
                                    EnableAuction = reader.IsDBNull(22) ? null : (bool?)reader.GetBoolean(22),
                                    MediaIpfs = reader.IsDBNull(23) ? null : (string?)reader.GetString(23),
                                    ThumbIpfs = reader.IsDBNull(24) ? null : (string?)reader.GetString(24),
                                    ItemImageType = reader.IsDBNull(25) ? null : (string?)reader.GetString(25),
                                    LastViewed = reader.IsDBNull(26) ? null : (DateTime?)reader.GetDateTime(26),
                                    AuctionReserve = reader.IsDBNull(27) ? null : (decimal?)reader.GetDecimal(27)
                                });
                            }
                        }
                    }
                }

                sSQL = "update tesora_nft.items set view_count = view_count + 1, last_viewed = @last_viewed where category_id = @category_id";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@last_viewed", NpgsqlDbType.TimestampTz).Value = DateTime.UtcNow;
                    cmd.Parameters.Add("@category_id", NpgsqlDbType.Integer).Value = categoryId;

                    await cmd.ExecuteNonQueryAsync();
                }

            }

            return lstItem;
        }
        /// <summary>
        /// Retrieve a Item record
        /// </summary>
        /// <param name="itemId">Primary Key</param>
        /// <returns></returns>
        public async Task<Item?> RetrieveItem(int itemId)
        {
            Item? item = default;

            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();
                string sSQL = "select item_id,name,description,external_link,media,thumb,accept_offer,unlock_content_url,view_count,like_count,price,currency,token_id,category_id,collection_id,current_owner,author_id,status,minted_date,create_date,start_date,end_date,enable_auction,media_ipfs,thumb_ipfs,item_image_type,last_viewed,auction_reserve" +
                              " from tesora_nft.items" +
                              " where item_id = @item_id";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@item_id", NpgsqlDbType.Integer).Value = itemId;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            item = new Item
                            {
                                ItemId = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Description = reader.IsDBNull(2) ? null : (string?)reader.GetString(2),
                                ExternalLink = reader.IsDBNull(3) ? null : (string?)reader.GetString(3),
                                Media = ReadBytes(reader, 4),
                                Thumb = ReadBytes(reader, 5),
                                AcceptOffer = reader.IsDBNull(6) ? null : (bool?)reader.GetBoolean(6),
                                UnlockContentUrl = reader.IsDBNull(7) ? null : (string?)reader.GetString(7),
                                ViewCount = reader.IsDBNull(8) ? null : (int?)reader.GetInt32(8),
                                LikeCount = reader.IsDBNull(9) ? null : (int?)reader.GetInt32(9),
                                Price = reader.IsDBNull(10) ? null : (decimal?)reader.GetDecimal(10),
                                Currency = reader.IsDBNull(11) ? null : (string?)reader.GetString(11),
                                TokenId = reader.IsDBNull(12) ? null : (int?)reader.GetInt32(12),
                                CategoryId = reader.IsDBNull(13) ? null : (int?)reader.GetInt32(13),
                                CollectionId = reader.IsDBNull(14) ? null : (int?)reader.GetInt32(14),
                                CurrentOwner = reader.IsDBNull(15) ? null : (int?)reader.GetInt32(15),
                                AuthorId = reader.IsDBNull(16) ? null : (int?)reader.GetInt32(16),
                                Status = (Item.ItemStatuses)Enum.Parse(typeof(Item.ItemStatuses), reader.GetString(17)),
                                MintedDate = reader.IsDBNull(18) ? null : (DateTime?)reader.GetDateTime(18),
                                CreateDate = reader.GetDateTime(19),
                                StartDate = reader.IsDBNull(20) ? null : (DateTime?)reader.GetDateTime(20),
                                EndDate = reader.IsDBNull(21) ? null : (DateTime?)reader.GetDateTime(21),
                                EnableAuction = reader.IsDBNull(22) ? null : (bool?)reader.GetBoolean(22),
                                MediaIpfs = reader.IsDBNull(23) ? null : (string?)reader.GetString(23),
                                ThumbIpfs = reader.IsDBNull(24) ? null : (string?)reader.GetString(24),
                                ItemImageType = reader.IsDBNull(25) ? null : (string?)reader.GetString(25),
                                LastViewed = reader.IsDBNull(26) ? null : (DateTime?)reader.GetDateTime(26),
                                AuctionReserve = reader.IsDBNull(27) ? null : (decimal?)reader.GetDecimal(27)
                            };

                        }
                    }
                }

                sSQL = "update tesora_nft.items set view_count = view_count + 1, last_viewed = @last_viewed where item_id = @item_id";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@last_viewed", NpgsqlDbType.TimestampTz).Value = DateTime.UtcNow;
                    cmd.Parameters.Add("@item_id", NpgsqlDbType.Integer).Value = itemId;

                    await cmd.ExecuteNonQueryAsync();
                }

            }

            return item;
        }


        /// <summary>
        /// Update a Item record
        /// </summary>
        /// <param name="record">Item</param>
        /// <returns></returns>
        public async Task UpdateItem(Item record)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "update tesora_nft.items set" +
                              " name = @name," +
                              " description = @description," +
                              " external_link = @external_link," +
                              " media = @media," +
                              " thumb = @thumb," +
                              " accept_offer = @accept_offer," +
                              " unlock_content_url = @unlock_content_url," +
                              " view_count = @view_count," +
                              " like_count = @like_count," +
                              " price = @price," +
                              " currency = @currency," +
                              " token_id = @token_id," +
                              " category_id = @category_id," +
                              " collection_id = @collection_id," +
                              " current_owner = @current_owner," +
                              " author_id = @author_id," +
                              " status = @status," +
                              " minted_date = @minted_date," +
                              " create_date = @create_date," +
                              " start_date = @start_date," +
                              " end_date = @end_date," +
                              " enable_auction = @enable_auction," +
                              " media_ipfs = @media_ipfs," +
                              " thumb_ipfs = @thumb_ipfs," +
                              " item_image_type = @item_image_type," +
                              " last_viewed = @last_viewed," +
                              " auction_reserve = @auction_reserve" +
                              " where item_id = @item_id";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@item_id", NpgsqlDbType.Integer).Value = record.ItemId;
                    cmd.Parameters.Add("@name", NpgsqlDbType.Varchar).Value = record.Name;
                    cmd.Parameters.Add("@description", NpgsqlDbType.Varchar).Value = record.Description ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@external_link", NpgsqlDbType.Varchar).Value = record.ExternalLink ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@media", NpgsqlDbType.Bytea).Value = record.Media ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@thumb", NpgsqlDbType.Bytea).Value = record.Thumb ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@accept_offer", NpgsqlDbType.Boolean).Value = record.AcceptOffer ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@unlock_content_url", NpgsqlDbType.Varchar).Value = record.UnlockContentUrl ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@view_count", NpgsqlDbType.Integer).Value = record.ViewCount ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@like_count", NpgsqlDbType.Integer).Value = record.LikeCount ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@price", NpgsqlDbType.Numeric).Value = record.Price ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@currency", NpgsqlDbType.Varchar).Value = record.Currency ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@token_id", NpgsqlDbType.Integer).Value = record.TokenId ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@category_id", NpgsqlDbType.Integer).Value = record.CategoryId ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@collection_id", NpgsqlDbType.Integer).Value = record.CollectionId ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@current_owner", NpgsqlDbType.Integer).Value = record.CurrentOwner ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@author_id", NpgsqlDbType.Integer).Value = record.AuthorId ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@status", NpgsqlDbType.Unknown).Value = record.Status.ToString();
                    cmd.Parameters.Add("@minted_date", NpgsqlDbType.TimestampTz).Value = record.MintedDate ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@create_date", NpgsqlDbType.TimestampTz).Value = record.CreateDate;
                    cmd.Parameters.Add("@start_date", NpgsqlDbType.TimestampTz).Value = record.StartDate ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@end_date", NpgsqlDbType.TimestampTz).Value = record.EndDate ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@enable_auction", NpgsqlDbType.Boolean).Value = record.EnableAuction ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@media_ipfs", NpgsqlDbType.Varchar).Value = record.MediaIpfs ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@thumb_ipfs", NpgsqlDbType.Varchar).Value = record.ThumbIpfs ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@item_image_type", NpgsqlDbType.Varchar).Value = record.ItemImageType ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@last_viewed", NpgsqlDbType.TimestampTz).Value = record.LastViewed ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@auction_reserve", NpgsqlDbType.Numeric).Value = record.AuctionReserve ?? (object)DBNull.Value;

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }


        /// <summary>
        /// Delete a new record in items
        /// </summary>
        /// <param name="itemId">Primary Key</param>
        /// <returns></returns>
        public async Task DeleteItem(int itemId)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "delete from tesora_nft.items where item_id = @item_id";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@item_id", NpgsqlDbType.Integer).Value = itemId;

                    await cmd.ExecuteNonQueryAsync();
                }

                sSQL = "select collection_id" +
                              " from tesora_nft.items" +
                              " where item_id = @item_id";

                int collection_id = 0;
                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@item_id", NpgsqlDbType.Integer).Value = itemId;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            collection_id = reader.GetInt32(0);
                        }
                    }
                }

                sSQL = "update tesora_nft.items set view_count = view_count + 1, last_viewed = @last_viewed where item_id = @item_id";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@last_viewed", NpgsqlDbType.TimestampTz).Value = DateTime.UtcNow;
                    cmd.Parameters.Add("@item_id", NpgsqlDbType.Integer).Value = itemId;

                    await cmd.ExecuteNonQueryAsync();
                }
                // Increase item count

                sSQL = "update tesora_nft.collections set item_count = item_count - 1 where collection_id = @collection_id";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@collection_id", NpgsqlDbType.Integer).Value = collection_id;

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        /// <summary>
        /// Retrieve my items
        /// </summary>
        /// <param name="collectionId"></param>
        /// <returns>List of my items</returns>
        public async Task<List<Item>> RetrieveMyItems(int collectionId)
        {
            var lstItem = new List<Item>();

            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();
                string sSQL = "select item_id,name,description,external_link,media,thumb,accept_offer,unlock_content_url,view_count,like_count,price,currency,token_id,category_id,collection_id,current_owner,author_id,status,minted_date,create_date,start_date,end_date,enable_auction,media_ipfs,thumb_ipfs,item_image_type,last_viewed,auction_reserve" +
                              " from tesora_nft.items" +
                              " where collection_id = @collection_id";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@collection_id", NpgsqlDbType.Integer).Value = collectionId;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            lstItem.Add(new Item
                            {
                                ItemId = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Description = reader.IsDBNull(2) ? null : (string?)reader.GetString(2),
                                ExternalLink = reader.IsDBNull(3) ? null : (string?)reader.GetString(3),
                                Media = ReadBytes(reader, 4),
                                Thumb = ReadBytes(reader, 5),
                                AcceptOffer = reader.IsDBNull(6) ? null : (bool?)reader.GetBoolean(6),
                                UnlockContentUrl = reader.IsDBNull(7) ? null : (string?)reader.GetString(7),
                                ViewCount = reader.IsDBNull(8) ? null : (int?)reader.GetInt32(8),
                                LikeCount = reader.IsDBNull(9) ? null : (int?)reader.GetInt32(9),
                                Price = reader.IsDBNull(10) ? null : (decimal?)reader.GetDecimal(10),
                                Currency = reader.IsDBNull(11) ? null : (string?)reader.GetString(11),
                                TokenId = reader.IsDBNull(12) ? null : (int?)reader.GetInt32(12),
                                CategoryId = reader.IsDBNull(13) ? null : (int?)reader.GetInt32(13),
                                CollectionId = reader.IsDBNull(14) ? null : (int?)reader.GetInt32(14),
                                CurrentOwner = reader.IsDBNull(15) ? null : (int?)reader.GetInt32(15),
                                AuthorId = reader.IsDBNull(16) ? null : (int?)reader.GetInt32(16),
                                Status = (Item.ItemStatuses)Enum.Parse(typeof(Item.ItemStatuses), reader.GetString(17)),
                                MintedDate = reader.IsDBNull(18) ? null : (DateTime?)reader.GetDateTime(18),
                                CreateDate = reader.GetDateTime(19),
                                StartDate = reader.IsDBNull(20) ? null : (DateTime?)reader.GetDateTime(20),
                                EndDate = reader.IsDBNull(21) ? null : (DateTime?)reader.GetDateTime(21),
                                EnableAuction = reader.IsDBNull(22) ? null : (bool?)reader.GetBoolean(22),
                                MediaIpfs = reader.IsDBNull(23) ? null : (string?)reader.GetString(23),
                                ThumbIpfs = reader.IsDBNull(24) ? null : (string?)reader.GetString(24),
                                ItemImageType = reader.IsDBNull(25) ? null : (string?)reader.GetString(25),
                                LastViewed = reader.IsDBNull(26) ? null : (DateTime?)reader.GetDateTime(26),
                                AuctionReserve = reader.IsDBNull(27) ? null : (decimal?)reader.GetDecimal(27)
                            });
                        }
                    }
                }
            }

            return lstItem;
        }


        /// <summary>
        /// Retrieve a Item record
        /// </summary>
        /// <param name="itemId">Primary Key</param>
        /// <returns></returns>
        public async Task<Item?> RetrieveMyItem(int itemId)
        {
            Item? item = default;

            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();
                string sSQL = "select item_id,name,description,external_link,media,thumb,accept_offer,unlock_content_url,view_count,like_count,price,currency,token_id,category_id,collection_id,current_owner,author_id,status,minted_date,create_date,start_date,end_date,enable_auction,media_ipfs,thumb_ipfs,item_image_type,last_viewed,auction_reserve" +
                              " from tesora_nft.items" +
                              " where item_id = @item_id";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@item_id", NpgsqlDbType.Integer).Value = itemId;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            item = new Item
                            {
                                ItemId = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Description = reader.IsDBNull(2) ? null : (string?)reader.GetString(2),
                                ExternalLink = reader.IsDBNull(3) ? null : (string?)reader.GetString(3),
                                Media = ReadBytes(reader, 4),
                                Thumb = ReadBytes(reader, 5),
                                AcceptOffer = reader.IsDBNull(6) ? null : (bool?)reader.GetBoolean(6),
                                UnlockContentUrl = reader.IsDBNull(7) ? null : (string?)reader.GetString(7),
                                ViewCount = reader.IsDBNull(8) ? null : (int?)reader.GetInt32(8),
                                LikeCount = reader.IsDBNull(9) ? null : (int?)reader.GetInt32(9),
                                Price = reader.IsDBNull(10) ? null : (Decimal?)reader.GetDecimal(10),
                                Currency = reader.IsDBNull(11) ? null : (string?)reader.GetString(11),
                                TokenId = reader.IsDBNull(12) ? null : (int?)reader.GetInt32(12),
                                CategoryId = reader.IsDBNull(13) ? null : (int?)reader.GetInt32(13),
                                CollectionId = reader.IsDBNull(14) ? null : (int?)reader.GetInt32(14),
                                CurrentOwner = reader.IsDBNull(15) ? null : (int?)reader.GetInt32(15),
                                AuthorId = reader.IsDBNull(16) ? null : (int?)reader.GetInt32(16),
                                Status = (Item.ItemStatuses)Enum.Parse(typeof(Item.ItemStatuses), reader.GetString(17)),
                                MintedDate = reader.IsDBNull(18) ? null : (DateTime?)reader.GetDateTime(18),
                                CreateDate = reader.GetDateTime(19),
                                StartDate = reader.IsDBNull(20) ? null : (DateTime?)reader.GetDateTime(20),
                                EndDate = reader.IsDBNull(21) ? null : (DateTime?)reader.GetDateTime(21),
                                EnableAuction = reader.IsDBNull(22) ? null : (bool?)reader.GetBoolean(22),
                                MediaIpfs = reader.IsDBNull(23) ? null : (string?)reader.GetString(23),
                                ThumbIpfs = reader.IsDBNull(24) ? null : (string?)reader.GetString(24),
                                ItemImageType = reader.IsDBNull(25) ? null : (string?)reader.GetString(25),
                                LastViewed = reader.IsDBNull(26) ? null : (DateTime?)reader.GetDateTime(26),
                                AuctionReserve = reader.IsDBNull(27) ? null : (decimal?)reader.GetDecimal(27)
                            };

                        }
                    }
                }
            }

            return item;
        }

        /// <summary>
        /// Post item for sale
        /// </summary>
        /// <param name="record">Item</param>
        /// <returns></returns>
        public async Task PostItemSale(ItemSale record)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "update tesora_nft.items set" +
                              " price = @price," +
                              " currency = @currency," +
                              " status = @status," +
                              " start_date = @start_date," +
                              " end_date = @end_date," +
                              " accept_offer = @accept_offer," +
                              " enable_auction = @enable_auction," +
                              " auction_reserve = @auction_reserve" +
                              " where item_id = @item_id";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@item_id", NpgsqlDbType.Integer).Value = record.ItemId;

                    cmd.Parameters.Add("@price", NpgsqlDbType.Numeric).Value = record.Price ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@currency", NpgsqlDbType.Varchar).Value = record.Currency ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@status", NpgsqlDbType.Unknown).Value = record.Status.ToString();
                    cmd.Parameters.Add("@start_date", NpgsqlDbType.TimestampTz).Value = record.StartDate ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@end_date", NpgsqlDbType.TimestampTz).Value = record.EndDate ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@enable_auction", NpgsqlDbType.Boolean).Value = record.EnableAuction ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@accept_offer", NpgsqlDbType.Boolean).Value = record.AcceptOffer ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@auction_reserve", NpgsqlDbType.Numeric).Value = record.AuctionReserve ?? (object)DBNull.Value;

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        /// <summary>
        /// Update a Item record
        /// </summary>
        /// <param name="itemId">Item</param>
        /// <param name="acceptOffer"></param>
        /// <returns></returns>
        public async Task MyItemAcceptOffer(int itemId, bool acceptOffer)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "update tesora_nft.items set accept_offer = @accept_offer where item_id = @item_id";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@accept_offer", NpgsqlDbType.Boolean).Value = acceptOffer;
                    cmd.Parameters.Add("@item_id", NpgsqlDbType.Integer).Value = itemId;

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<List<MarketPlaceResponse>> GetMarketPlaceItems(MarketPlaceRequest request)
        {
            var items = new List<MarketPlaceResponse>();

            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();
                string sSQL;

                var where = "i.status = 'active'";
                if (request.Filter != null)
                {
                    if (request.Filter.PriceFilter != null)
                    {
                        where += request.Filter.PriceFilter switch
                        {
                            MarketPlaceFilter.PriceFilterTypes.GteOne => " and i.price >= 1.00",
                            MarketPlaceFilter.PriceFilterTypes.GteTen => " and i.price >= 10.00",
                            MarketPlaceFilter.PriceFilterTypes.GteOneHunderd => " and i.price >= 100.00",
                            MarketPlaceFilter.PriceFilterTypes.GteOneThousand => " and i.price >= 1000.00",
                            _ => ""
                        };
                    }

                    if (request.Filter.SaleType != MarketPlaceFilter.MarketPlaceSaleTypes.All)
                    {
                        where += request.Filter.SaleType switch
                        {
                            MarketPlaceFilter.MarketPlaceSaleTypes.AllowsOffers => " and i.enable_offer = true",
                            MarketPlaceFilter.MarketPlaceSaleTypes.OnAuction => " and i.enable_auction = true",
                            _ => ""
                        };
                    }

                    if (request.Filter.CategoryIds != null)
                    {
                        if (!request.Filter.CategoryIds.Contains(0))
                        {
                            var categoryIds = string.Join(",", request.Filter.CategoryIds);
                            where += $" and i.category_id in ({categoryIds})";
                        }
                    }
                }

                var orderby = "i.minted_date desc, i.last_viewed desc";
                if (request.Sort != null)
                {
                    if (request.Sort.SortOrder != null)
                    {
                        orderby = request.Sort.SortOrder switch
                        {
                            MarketPlaceSort.SortOrderTypes.MostRecent => "i.minted_date desc, i.last_viewed desc",
                            MarketPlaceSort.SortOrderTypes.MostViewed => "i.view_count desc, i.last_viewed desc",
                            MarketPlaceSort.SortOrderTypes.MostLiked => "i.like_count desc, i.last_viewed desc",
                            _ => "i.minted_date desc, i.last_viewed desc"
                        };
                    }
                }

                var paging = "limit 50";
                if (request.PageSize > 0)
                    paging = $"limit {request.PageSize}";

                if (request.PageNumber > 0)
                    paging += $" offset {request.PageNumber * request.PageSize}";

                // Default to items
                var cardType = MarketPlaceResponse.CardTypes.Item;

                // Text Searching?
                if (!string.IsNullOrEmpty(request.Text))
                {
                    if (request.TextSearchType == MarketPlaceRequest.TextSearchTypes.Global)
                    {
                        sSQL = "select i.item_id,c.name,i.name,t.title,i.view_count,i.like_count,i.price,i.currency,i.media_ipfs,i.enable_auction,i.collection_id,i.category_id,i.accept_offer,c.item_count" +
                               " from" +
                               " (select * from tesora_nft.items where collection_id in" +
                               $"   (select collection_id from tesora_nft.collections where text_search @@ to_tsquery(@text))" +
                               $" union" +
                               $" select * from tesora_nft.items where text_search @@ to_tsquery(@text)" +
                               " ) i" +
                               " join tesora_nft.collections c on i.collection_id = c.collection_id" +
                               " left join tesora_nft.categories t on i.category_id = t.category_id" +
                               $" where {where}" +
                               $" order by {orderby}" +
                               $" {paging}";
                    }
                    else
                    {
                        sSQL = "select null,name,null,null,null,null,null,collection_image_ipfs,null,collection_id,null,null,item_count" +
                               " from tesora_nft.collections" +
                               $" where text_search @@ to_tsquery(@text)" +
                               " order by create_date" +
                               $" {paging}";

                        // the are searching for collections only
                        cardType = MarketPlaceResponse.CardTypes.Collection;
                    }
                }
                else
                {
                    // No search, so just use items
                    if (request.TextSearchType == MarketPlaceRequest.TextSearchTypes.Global)
                    {
                        sSQL = "select i.item_id,c.name,i.name,t.title,i.view_count,i.like_count,i.price,i.currency,i.media_ipfs,i.enable_auction,i.collection_id,i.category_id,i.accept_offer,c.item_count" +
                           " from tesora_nft.items i" +
                           " join tesora_nft.collections c on i.collection_id = c.collection_id" +
                           " left join tesora_nft.categories t on i.category_id = t.category_id" +
                           $" where {where}" +
                           $" order by {orderby}" +
                           $" {paging}" ;
                    }
                    else
                    {
                        sSQL = "select null,name,null,null,null,null,null,collection_image_ipfs,null,collection_id,null,null,item_count" +
                            " from tesora_nft.collections" +
                            " order by create_date" +
                            $" {paging}";

                        // the are searching for collections only
                        cardType = MarketPlaceResponse.CardTypes.Collection;
                    }
                }

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    if (!string.IsNullOrEmpty(request.Text))
                        cmd.Parameters.Add("@text", NpgsqlDbType.Varchar).Value = request.Text;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            items.Add(new MarketPlaceResponse
                            {
                                CardType = cardType,
                                ItemId =  reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                                Name = reader.IsDBNull(2) ? null : (string?)reader.GetString(2),
                                ViewCount = reader.IsDBNull(4) ? null : (int?)reader.GetInt32(4),
                                LikeCount = reader.IsDBNull(5) ? null : (int?)reader.GetInt32(5),
                                Price = reader.IsDBNull(6) ? null : (decimal?)reader.GetDecimal(6),
                                Currency = reader.IsDBNull(7) ? null : (string?)reader.GetString(7),
                                Media = reader.IsDBNull(8) ? null : (string?)reader.GetString(8),
                                EnableAuction = reader.IsDBNull(9) ? null : (bool?)reader.GetBoolean(9),
                                AcceptOffer = reader.IsDBNull(12) ? null : (bool?)reader.GetBoolean(12),
                                ItemCount = reader.IsDBNull(13) ? null : (int?)reader.GetInt32(13),
                                Collection = new CollectionView
                                {
                                    collection_id = reader.GetInt32(10),
                                    name = reader.IsDBNull(1) ? null : (string?)reader.GetString(1),
                                    image = $"/api/v1/Collection/GetCollectionImage/{reader.GetInt32(10)}",
                                    banner = $"/api/v1/Collection/GetCollectionBanner/{reader.GetInt32(10)}",
                                },
                                Category = reader.IsDBNull(10) ? null : new CategoryView
                                {
                                    category_id = reader.IsDBNull(11) ? 0 : reader.GetInt32(11),
                                    category_title = reader.IsDBNull(3) ? null : (string?)reader.GetString(3)
                                }
                            });
                        }
                    }
                }
            }

            return items;
        }
    }
}

