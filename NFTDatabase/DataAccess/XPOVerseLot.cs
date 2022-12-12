// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//

using Npgsql;
using NpgsqlTypes;

using NFTDatabaseEntities;
using System.Net.NetworkInformation;


namespace NFTDatabase.DataAccess
{
    internal partial class PostgreSql : IPostgreSql
    {
        /// <summary>
        /// Gets the list of rings that are for sale
        /// </summary>
        /// <returns></returns>
        public async Task<List<string>> GetRingsForSale()
        {
            var lstRings = new List<string>();

            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "select distinct(ring) from tesora_nft.lots where for_sale = true and reserved_cart_id is null";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            lstRings.Add(reader.GetString(0));
                        }
                    }
                }
            }

            return lstRings;
        }


        /// <summary>
        /// Gets the list of sections that are for sale within a ring
        /// </summary>
        /// <returns></returns>
        public async Task<List<string>> GetSectionsForSale(string ring)
        {
            var lstSections = new List<string>();

            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "select distinct(section) from tesora_nft.lots where ring = @ring and for_sale = true and reserved_cart_id is null";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@ring", NpgsqlDbType.Varchar).Value = ring;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            lstSections.Add(reader.GetString(0));
                        }
                    }
                }
            }

            return lstSections;
        }


        /// <summary>
        /// Gets the list of blocks that are for sale within a ring and section
        /// </summary>
        /// <returns></returns>
        public async Task<List<string>> GetBlocksForSale(string ring, string section)
        {
            var lstBlocks = new List<string>();

            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "select distinct(block) from tesora_nft.lots where ring = @ring and section = @section and for_sale = true and reserved_cart_id is null";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@ring", NpgsqlDbType.Varchar).Value = ring;
                    cmd.Parameters.Add("@section", NpgsqlDbType.Varchar).Value = section;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            lstBlocks.Add(reader.GetString(0));
                        }
                    }
                }
            }

            return lstBlocks;
        }


        /// <summary>
        /// Gets the list of lots that are for sale within a ring, section and block
        /// </summary>
        /// <returns></returns>
        public async Task<List<string>> GetLotsForSale(string ring, string section, string block)
        {
            var lstLots = new List<string>();

            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "select distinct(lot) from tesora_nft.lots where ring = @ring and section = @section and block = @block and for_sale = true and reserved_cart_id is null";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@ring", NpgsqlDbType.Varchar).Value = ring;
                    cmd.Parameters.Add("@section", NpgsqlDbType.Varchar).Value = section;
                    cmd.Parameters.Add("@block", NpgsqlDbType.Varchar).Value = block;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            lstLots.Add(reader.GetString(0));
                        }
                    }
                }
            }

            return lstLots;
        }
    }
}

