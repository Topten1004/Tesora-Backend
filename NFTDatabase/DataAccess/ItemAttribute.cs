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
        /// <summary>
        /// Create a new record in Item Attributes
        /// </summary>
        /// <param name="record">Item Attribute</param>
        /// <returns></returns>
        public async Task CreateItemAttribute(ItemAttribute record)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "insert into tesora_nft.item_attributes" +
                              " (item_id,type,title,value,max_value)" +
                              " values" +
                              " (@item_id,@type,@title,@value,@max_value)";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@item_id", NpgsqlDbType.Integer).Value = record.ItemId;
                    cmd.Parameters.Add("@type", NpgsqlDbType.Varchar).Value = record.ItemType.ToString();
                    cmd.Parameters.Add("@title", NpgsqlDbType.Varchar).Value = record.Title;
                    cmd.Parameters.Add("@value", NpgsqlDbType.Varchar).Value = record.Value;
                    cmd.Parameters.Add("@max_value", NpgsqlDbType.Varchar).Value = record.MaxValue ?? (object)DBNull.Value;

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }


        /// <summary>
        /// Retrieve all Item Attribute records
        /// </summary>
        /// <returns>List of Item Attribute records</returns>
        public async Task<List<ItemAttribute>> RetrieveItemAttributes()
        {
            var lstItemAttributes = new List<ItemAttribute>();

            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "select item_attribute_id,item_id,type,title,value,max_value" +
                              " from tesora_nft.item_attributes";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            lstItemAttributes.Add( new ItemAttribute
                            {
                                ItemAttributeId = reader.GetInt32(0),
                                ItemId = reader.GetInt32(1),
                                ItemType = (ItemAttribute.ItemTypes)Enum.Parse(typeof(ItemAttribute.ItemTypes), reader.GetString(3)),
                                Title = reader.GetString(4),
                                Value = reader.GetString(5),
                                MaxValue = reader.IsDBNull(6) ? null : (string?)reader.GetString(6)
                            });
                        }
                    }
                }
            }

            return lstItemAttributes;
        }


        /// <summary>
        /// Retrieve a Item Attribute record
        /// </summary>
        /// <param name="itemAttributeId">Primary Key</param>
        /// <returns></returns>
        public async Task<ItemAttribute?> RetrieveItemAttribute(int itemAttributeId)
        {
            ItemAttribute? itemAttribute = default;

            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "select item_attribute_id,item_id,type,title,value,max_value" +
                              " from tesora_nft.item_attributes" +
                              " where item_attribute_id = @item_attribute_id";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@item_attribute_id", NpgsqlDbType.Integer).Value = itemAttributeId;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            itemAttribute = new ItemAttribute
                            {
                                ItemAttributeId = reader.GetInt32(0),
                                ItemId = reader.GetInt32(1),
                                ItemType = (ItemAttribute.ItemTypes)Enum.Parse(typeof(ItemAttribute.ItemTypes), reader.GetString(3)),
                                Title = reader.GetString(4),
                                Value = reader.GetString(5),
                                MaxValue = reader.IsDBNull(6) ? null : (string?)reader.GetString(6)
                            };
                        }
                    }
                }
            }

            return itemAttribute;
        }


        /// <summary>
        /// Update a Item Attribute record
        /// </summary>
        /// <param name="record">Item Attribute</param>
        /// <returns></returns>
        public async Task UpdateItemAttribute(ItemAttribute record)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "update tesora_nft.item_attributes set" +
                              " item_id = @item_id," +
                              " type = @type," +
                              " title = @title," +
                              " value = @value," +
                              " max_value = @max_value" +
                              " where item_attribute_id = @item_attribute_id";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@item_id", NpgsqlDbType.Integer).Value = record.ItemId;
                    cmd.Parameters.Add("@type", NpgsqlDbType.Varchar).Value = record.ItemType.ToString();
                    cmd.Parameters.Add("@title", NpgsqlDbType.Varchar).Value = record.Title;
                    cmd.Parameters.Add("@value", NpgsqlDbType.Varchar).Value = record.Value;
                    cmd.Parameters.Add("@max_value", NpgsqlDbType.Varchar).Value = record.MaxValue ?? (object)DBNull.Value;

                    cmd.Parameters.Add("@item_attribute_id", NpgsqlDbType.Integer).Value = record.ItemAttributeId;

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }


        /// <summary>
        /// Delete a new record in Item Attributes
        /// </summary>
        /// <param name="itemAttributeId">Primary Key</param>
        /// <returns></returns>
        public async Task DeleteItemAttribute(int itemAttributeId)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "delete from tesora_nft.item_attributes where item_attribute_id = @item_attribute_id";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@item_attribute_id", NpgsqlDbType.Integer).Value = itemAttributeId;

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }
    }
}

