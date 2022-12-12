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
        /// Create a new record in Contract
        /// </summary>
        /// <param name="record">Contract</param>
        /// <returns></returns>
        public async Task CreateContract(Contract record)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "insert into tesora_nft.contracts" +
                              " (contract_name,contract_version,contract_interface,contract_byte_code,create_date)" +
                              " values" +
                              " (@contract_name,@contract_version,@contract_interface,@contract_byte_code,@create_date)";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {

                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@contract_name", NpgsqlDbType.Varchar).Value = record.ContractName;
                    cmd.Parameters.Add("@contract_version", NpgsqlDbType.Varchar).Value = record.ContractVersion;
                    cmd.Parameters.Add("@contract_interface", NpgsqlDbType.Varchar).Value = record.ContractInterface;
                    cmd.Parameters.Add("@contract_byte_code", NpgsqlDbType.Varchar).Value = record.ContractByteCode;
                    cmd.Parameters.Add("@create_date", NpgsqlDbType.TimestampTz).Value = record.CreateDate;

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }


        /// <summary>
        /// Retrieve all Contract records
        /// </summary>
        /// <returns>List of Contract records</returns>
        public async Task<List<Contract>> RetrieveContracts()
        {
            var lstContract = new List<Contract>();

            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "select contract_id,contract_name,contract_version,contract_interface,contract_byte_code,create_date" +
                              " from tesora_nft.contracts";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            lstContract.Add(new Contract
                            {
                                ContractId = reader.GetInt32(0),
                                ContractName = reader.GetString(1),
                                ContractVersion = reader.GetString(2),
                                ContractInterface = reader.GetString(3),
                                ContractByteCode = reader.GetString(4),
                                CreateDate = reader.GetDateTime(5)
                            });
                        }
                    }
                }
            }

            return lstContract;
        }


        /// <summary>
        /// Retrieve a Contract record
        /// </summary>
        /// <param name="contractId">Primary Key</param>
        /// <returns></returns>
        public async Task<Contract?> RetrieveContract(int contractId)
        {
            Contract? Contract = default;

            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "select contract_id,contract_name,contract_version,contract_interface,contract_byte_code,create_date" +
                              " from tesora_nft.contracts" +
                              " where contract_id = @contract_id";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@contract_id", NpgsqlDbType.Integer).Value = contractId;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            Contract = new Contract
                            {
                                ContractId = reader.GetInt32(0),
                                ContractName = reader.GetString(1),
                                ContractVersion = reader.GetString(2),
                                ContractInterface = reader.GetString(3),
                                ContractByteCode = reader.GetString(4),
                                CreateDate = reader.GetDateTime(5)
                            };
                        }
                    }
                }
            }

            return Contract;
        }


        /// <summary>
        /// Retrieve a Contract record by name and version
        /// </summary>
        /// <param name="name">Contract Name</param>
        /// <param name="version">Contract Version</param>
        /// <returns></returns>
        public async Task<Contract?> RetrieveContractByName(string name, string version)
        {
            Contract? Contract = default;

            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "select contract_id,contract_name,contract_version,contract_interface,contract_byte_code,create_date" +
                              " from tesora_nft.contracts" +
                              " where contract_name = @contract_name and contract_version = @contract_version";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@contract_name", NpgsqlDbType.Varchar).Value = name;
                    cmd.Parameters.Add("@contract_version", NpgsqlDbType.Varchar).Value = version;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            Contract = new Contract
                            {
                                ContractId = reader.GetInt32(0),
                                ContractName = reader.GetString(1),
                                ContractVersion = reader.GetString(2),
                                ContractInterface = reader.GetString(3),
                                ContractByteCode = reader.GetString(4),
                                CreateDate = reader.GetDateTime(5)
                            };
                        }
                    }
                }
            }

            return Contract;
        }


        /// <summary>
        /// Update a Contract record
        /// </summary>
        /// <param name="record">Contract</param>
        /// <returns></returns>
        public async Task UpdateContract(Contract record)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "update tesora_nft.contracts set" +
                              " contract_name = @contract_name," +
                              " contract_version = @contract_version," +
                              " contract_interface = @contract_interface," +
                              " contract_byte_code = @contract_byte_code," +
                              " create_date = @create_date" +
                              " where contract_id = @contract_id";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {

                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@contract_id", NpgsqlDbType.Integer).Value = record.ContractId;
                    cmd.Parameters.Add("@contract_name", NpgsqlDbType.Varchar).Value = record.ContractName;
                    cmd.Parameters.Add("@Contract_version", NpgsqlDbType.Varchar).Value = record.ContractVersion;
                    cmd.Parameters.Add("@Contract_interface", NpgsqlDbType.Varchar).Value = record.ContractInterface;
                    cmd.Parameters.Add("@Contract_byte_code", NpgsqlDbType.Varchar).Value = record.ContractByteCode;
                    cmd.Parameters.Add("@create_date", NpgsqlDbType.TimestampTz).Value = record.CreateDate;

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }


        /// <summary>
        /// Delete a new record in categories
        /// </summary>
        /// <param name="contractId"></param>
        /// <returns></returns>
        public async Task DeleteContract(int contractId)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                await conn.OpenAsync();

                string sSQL = "delete from tesora_nft.contracts where contract_id = @contract_id";

                using (var cmd = new NpgsqlCommand(sSQL, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add("@contract_id", NpgsqlDbType.Integer).Value = contractId;

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }
    }
}

