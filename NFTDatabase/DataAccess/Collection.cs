// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//

using Npgsql;
using NpgsqlTypes;

using NFTDatabaseEntities;


namespace NFTDatabase.DataAccess
{
    internal partial class PostgreSql : IPostgreSql
    {
        public async Task<bool> CollectionNameExists(string name)
        {
            bool result = false;

            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "select exists(select 1 from tesora_nft.collections where name = @name)";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@name", NpgsqlDbType.Varchar).Value = name;

                    var col = await cmd.ExecuteScalarAsync();

                    if (col != null)
                        result = (bool)col;
                }
            }

            return result;
        }

        /// <summary>
        /// Create a new record in Collection
        /// </summary>
        /// <param name="record">Collection</param>
        /// <returns></returns>
        public async Task CreateCollection(Collection record)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "insert into tesora_nft.collections" +
                              " (name,description,contract_symbol,contract_address,banner,collection_image,royalties,volume_traded,item_count,status,author_id,create_date,banner_image_type,collection_image_type,chain_id)" +
                              " values" +
                              " (@name,@description,@contract_symbol,@contract_address,@banner,@collection_image,@royalties,@volume_traded,@item_count,@status,@author_id,@create_date,@banner_image_type,@collection_image_type,@chain_id)" +
                              " returning collection_id";
                int iNewId;

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {

                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@name", NpgsqlDbType.Varchar).Value = record.Name;
                    cmd.Parameters.Add("@description", NpgsqlDbType.Varchar).Value = record.Description ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@contract_symbol", NpgsqlDbType.Varchar).Value = record.ContractSymbol ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@contract_address", NpgsqlDbType.Varchar).Value = record.ContractAddress ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@banner", NpgsqlDbType.Bytea).Value = record.Banner ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@collection_image", NpgsqlDbType.Bytea).Value = record.CollectionImage ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@royalties", NpgsqlDbType.Numeric).Value = record.Royalties.GetValueOrDefault();
                    cmd.Parameters.Add("@volume_traded", NpgsqlDbType.Integer).Value = record.VolumeTraded.GetValueOrDefault();
                    cmd.Parameters.Add("@item_count", NpgsqlDbType.Integer).Value = record.ItemCount.GetValueOrDefault();
                    cmd.Parameters.Add("@status", NpgsqlDbType.Unknown).Value = record.Status.ToString();
                    cmd.Parameters.Add("@author_id", NpgsqlDbType.Integer).Value = record.AuthorId.GetValueOrDefault();
                    cmd.Parameters.Add("@create_date", NpgsqlDbType.TimestampTz).Value = DateTime.UtcNow;
                    cmd.Parameters.Add("@banner_image_type", NpgsqlDbType.Varchar).Value = record.BannerImageType ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@collection_image_type", NpgsqlDbType.Varchar).Value = record.CollectionImageType ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@chain_id", NpgsqlDbType.Integer).Value = record.ChainId;

                    var result = await cmd.ExecuteScalarAsync();

                    if (result != null)
                        iNewId = (int)result;
                    else 
                        iNewId = 0;
                }

                // Make history entry
                sSQL = "insert into tesora_nft.histories" +
                        " (item_id,collection_id,from_id,to_id,transaction_hash,price,history_type,is_valid,create_date,currency)" +
                        " values" +
                        " (@item_id,@collection_id,@from_id,@to_id,@transaction_hash,@price,@history_type,@is_valid,@create_date,@currency)";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@item_id", NpgsqlDbType.Integer).Value = -1;
                    cmd.Parameters.Add("@collection_id", NpgsqlDbType.Integer).Value = iNewId;
                    cmd.Parameters.Add("@from_id", NpgsqlDbType.Integer).Value = record.AuthorId;
                    cmd.Parameters.Add("@to_id", NpgsqlDbType.Integer).Value = record.AuthorId;
                    cmd.Parameters.Add("@transaction_hash", NpgsqlDbType.Varchar).Value = record.TransactionHash;
                    cmd.Parameters.Add("@price", NpgsqlDbType.Numeric).Value = 0;
                    cmd.Parameters.Add("@history_type", NpgsqlDbType.Unknown).Value = History.HistoryTypes.created.ToString();
                    cmd.Parameters.Add("@is_valid", NpgsqlDbType.Boolean).Value = true;
                    cmd.Parameters.Add("@create_date", NpgsqlDbType.TimestampTz).Value = DateTime.UtcNow;
                    cmd.Parameters.Add("@currency", NpgsqlDbType.Varchar).Value = record.ContractSymbol;

                    await cmd.ExecuteNonQueryAsync();
                }

            }
        }


        /// <summary>
        /// Retrieve all Collection records
        /// </summary>
        /// <returns>List of Collection records</returns>
       public async Task<List<Collection>> RetrieveCollections()
        {
            var lstCollection = new List<Collection>();

            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "select collection_id,name,description,contract_symbol,contract_address,royalties,volume_traded,item_count,status,author_id,create_date,chain_id" +
                              " from tesora_nft.collections";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            lstCollection.Add(new Collection
                            {
                                CollectionId = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Description = reader.IsDBNull(2) ? null  : (string?)reader.GetString(2),
                                ContractSymbol = reader.IsDBNull(3) ? null  : (string?)reader.GetString(3),
                                ContractAddress = reader.IsDBNull(4) ? null  : (string?)reader.GetString(4),
                                Royalties = reader.IsDBNull(5) ? null : (Decimal?)reader.GetDecimal(5),
                                VolumeTraded = reader.IsDBNull(6) ? null : (int?)reader.GetInt32(6),
                                ItemCount = reader.IsDBNull(7) ? null : (int?)reader.GetInt32(7),
                                Status = (Collection.CollectionStatuses)Enum.Parse(typeof(Collection.CollectionStatuses), reader.GetString(8)),
                                AuthorId = reader.IsDBNull(9) ? null : (int?)reader.GetInt32(9),
                                CreateDate = reader.GetDateTime(10),
                                ChainId = reader.GetInt32(11)
                            });
                        }
                    }
                }
            }

            return lstCollection;
        }

        /// <summary>
        /// Retrieve Trending Collection records
        /// </summary>
        /// <returns>List of Collection records</returns>
        public async Task<List<Collection>> GetTrendingCollections()
        {
            var lstCollection = new List<Collection>();

            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "select c.collection_id,name,description,contract_symbol,contract_address,royalties,volume_traded,item_count,status,author_id,create_date,chain_id" +
                              " from tesora_nft.collections c" +
                              " join (select distinct(collection_id), last_viewed from tesora_nft.items order by last_viewed desc limit 10) r on r.collection_id = c.collection_id";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            lstCollection.Add( new Collection
                            {
                                CollectionId = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Description = reader.IsDBNull(2) ? null : (string?)reader.GetString(2),
                                ContractSymbol = reader.IsDBNull(3) ? null : (string?)reader.GetString(3),
                                ContractAddress = reader.IsDBNull(4) ? null : (string?)reader.GetString(4),
                                Royalties = reader.IsDBNull(5) ? null : (Decimal?)reader.GetDecimal(5),
                                VolumeTraded = reader.IsDBNull(6) ? null : (int?)reader.GetInt32(6),
                                ItemCount = reader.IsDBNull(7) ? null : (int?)reader.GetInt32(7),
                                Status = (Collection.CollectionStatuses)Enum.Parse(typeof(Collection.CollectionStatuses), reader.GetString(8)),
                                AuthorId = reader.IsDBNull(9) ? null : (int?)reader.GetInt32(9),
                                CreateDate = reader.GetDateTime(10),
                                ChainId = reader.GetInt32(11),
                            });
                        }
                    }
                }
            }

            return lstCollection;
        }


        /// <summary>
        /// Retrieve all Collection records
        /// </summary>
        /// <returns>List of Collection records</returns>
        public async Task<List<Collection>> RetrieveMyCollections(int authorId)
        {
            var lstCollection = new List<Collection>();

            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "select collection_id,name,description,contract_symbol,contract_address,royalties,volume_traded,item_count,status,author_id,create_date,chain_id" +
                              " from tesora_nft.collections" +
                              " where author_id = @author_id" +
                              " order by name";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@author_id", NpgsqlDbType.Integer).Value = authorId;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            lstCollection.Add(new Collection
                            {
                                CollectionId = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Description = reader.IsDBNull(2) ? null : (string?)reader.GetString(2),
                                ContractSymbol = reader.IsDBNull(3) ? null : (string?)reader.GetString(3),
                                ContractAddress = reader.IsDBNull(4) ? null : (string?)reader.GetString(4),
                                Royalties = reader.IsDBNull(5) ? null : (Decimal?)reader.GetDecimal(5),
                                VolumeTraded = reader.IsDBNull(6) ? null : (int?)reader.GetInt32(6),
                                ItemCount = reader.IsDBNull(7) ? null : (int?)reader.GetInt32(7),
                                Status = (Collection.CollectionStatuses)Enum.Parse(typeof(Collection.CollectionStatuses), reader.GetString(8)),
                                AuthorId = reader.IsDBNull(9) ? null : (int?)reader.GetInt32(9),
                                CreateDate = reader.GetDateTime(10),
                                ChainId = reader.GetInt32(11),
                            });
                        }
                    }
                }
            }

            return lstCollection;
        }


        /// <summary>
        /// Retrieve a Category image
        /// </summary>
        /// <param name="collectionId">Primary Key</param>
        /// <returns></returns>
        public async Task<ImageBox?> RetrieveCollectionImage(int collectionId)
        {
            ImageBox? imageBox = null;

            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "select collection_image,collection_image_type from tesora_nft.collections where collection_id = @collection_id";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@collection_id", NpgsqlDbType.Integer).Value = collectionId;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            imageBox = new ImageBox
                            {
                                Data = ReadBytes(reader, 0),
                                Type = reader.GetString(1)
                            };
                        }
                    }
                }
            }

            return imageBox;
        }


        /// <summary>
        /// Retrieve a Category banner
        /// </summary>
        /// <param name="collectionId">Primary Key</param>
        /// <returns></returns>
        public async Task<ImageBox?> RetrieveCollectionBanner(int collectionId)
        {
            ImageBox? imageBox = null;

            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "select banner,banner_image_type from tesora_nft.collections where collection_id = @collection_id";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@collection_id", NpgsqlDbType.Integer).Value = collectionId;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            imageBox = new ImageBox
                            {
                                Data = ReadBytes(reader, 0),
                                Type = reader.GetString(1)
                            };
                        }
                    }
                }
            }

            return imageBox;
        }


        /// <summary>
        /// Retrieve a Collection record
        /// </summary>
        /// <param name="collectionId">Primary Key</param>
        /// <returns></returns>
        public async Task<Collection?> RetrieveCollection(int collectionId)
        {
            Collection? collection = null;

            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "select collection_id,name,description,contract_symbol,contract_address,royalties,volume_traded,item_count,status,author_id,create_date,chain_id" +
                              " from tesora_nft.collections" +
                              " where collection_id = @collection_id";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@collection_id", NpgsqlDbType.Integer).Value = collectionId;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            collection = new Collection
                            {
                                CollectionId = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Description = reader.IsDBNull(2) ? null  : (string?)reader.GetString(2),
                                ContractSymbol = reader.IsDBNull(3) ? null  : (string?)reader.GetString(3),
                                ContractAddress = reader.IsDBNull(4) ? null  : (string?)reader.GetString(4),
                                Royalties = reader.IsDBNull(5) ? null : (Decimal?)reader.GetDecimal(5),
                                VolumeTraded = reader.IsDBNull(6) ? null : (int?)reader.GetInt32(6),
                                ItemCount = reader.IsDBNull(7) ? null : (int?)reader.GetInt32(7),
                                Status = (Collection.CollectionStatuses)Enum.Parse(typeof(Collection.CollectionStatuses), reader.GetString(8)),
                                AuthorId = reader.IsDBNull(9) ? null : (int?)reader.GetInt32(9),
                                CreateDate = reader.GetDateTime(10),
                                ChainId = reader.GetInt32(11),
                            };
                        }
                    }
                }
            }

            return collection;
        }

        /// <summary>
        /// Retrieve a Collection record by name
        /// </summary>
        /// <param name="name">Name</param>
        /// <returns></returns>
        public async Task<Collection?> RetrieveCollectionByName(string name)
        {
            Collection? collection = null;

            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "select collection_id,name,description,contract_symbol,contract_address,royalties,volume_traded,item_count,status,author_id,create_date,chain_id" +
                              " from tesora_nft.collections" +
                              " where name = @name";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@name", NpgsqlDbType.Varchar).Value = name;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            collection = new Collection
                            {
                                CollectionId = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Description = reader.IsDBNull(2) ? null : (string?)reader.GetString(2),
                                ContractSymbol = reader.IsDBNull(3) ? null : (string?)reader.GetString(3),
                                ContractAddress = reader.IsDBNull(4) ? null : (string?)reader.GetString(4),
                                Royalties = reader.IsDBNull(5) ? null : (Decimal?)reader.GetDecimal(5),
                                VolumeTraded = reader.IsDBNull(6) ? null : (int?)reader.GetInt32(6),
                                ItemCount = reader.IsDBNull(7) ? null : (int?)reader.GetInt32(7),
                                Status = (Collection.CollectionStatuses)Enum.Parse(typeof(Collection.CollectionStatuses), reader.GetString(8)),
                                AuthorId = reader.IsDBNull(9) ? null : (int?)reader.GetInt32(9),
                                CreateDate = reader.GetDateTime(10),
                                ChainId = reader.GetInt32(11),
                            };
                        }
                    }
                }
            }

            return collection;
        }


        /// <summary>
        /// Update a Collection record
        /// </summary>
        /// <param name="record">Collection</param>
        /// <returns></returns>
        public async Task UpdateCollection(Collection record)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "update tesora_nft.collections set" +
                              " name = @name," +
                              " description = @description," +
                              " contract_symbol = @contract_symbol," +
                              " banner = @banner," +
                              " collection_image = @collection_image," +
                              " royalties = @royalties," +
                              " status = @status," +
                              " banner_image_type = @banner_image_type," +
                              " collection_image_type = @collection_image_type" +
                              " where collection_id = @collection_id";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {

                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@collection_id", NpgsqlDbType.Integer).Value = record.CollectionId;

                    cmd.Parameters.Add("@name", NpgsqlDbType.Varchar).Value = record.Name;
                    cmd.Parameters.Add("@description", NpgsqlDbType.Varchar).Value = record.Description ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@contract_symbol", NpgsqlDbType.Varchar).Value = record.ContractSymbol ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@banner", NpgsqlDbType.Bytea).Value = record.Banner ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@collection_image", NpgsqlDbType.Bytea).Value = record.CollectionImage ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@royalties", NpgsqlDbType.Numeric).Value = record.Royalties.GetValueOrDefault();
                    cmd.Parameters.Add("@status", NpgsqlDbType.Unknown).Value = record.Status.ToString();
                    cmd.Parameters.Add("@banner_image_type", NpgsqlDbType.Varchar).Value = record.BannerImageType ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@collection_image_type", NpgsqlDbType.Varchar).Value = record.CollectionImageType ?? (object)DBNull.Value;

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }


        /// <summary>
        /// Delete a new record in collections
        /// </summary>
        /// <param name="collectionId">Primary Key</param>
        /// <returns></returns>
        public async Task DeleteCollection(int collectionId)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "delete from tesora_nft.collections where collection_id = @collection_id";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@collection_id", NpgsqlDbType.Integer).Value = collectionId;

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }
    }
}

