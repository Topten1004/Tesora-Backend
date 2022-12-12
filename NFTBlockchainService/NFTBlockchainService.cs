// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//

using Microsoft.Extensions.Configuration;

using NFTBlockchainEntities;


namespace NFTBlockchainService
{
    /// <summary>
    /// NFTDatabase Service
    /// </summary>
    public class NFTBlockchainService : NFTBlockchainServiceBase, INFTBlockchainService
    {
        public NFTBlockchainService(HttpClient httpClient, IConfiguration configuration) : base(httpClient, configuration)
        {
        }

        /// <summary>
        /// Gets the Options
        /// </summary>
        /// <returns>List of Option records</returns>
        public async Task<List<Erc720Token>> GetErc720Tokens()
        {
            var apiEndPoint = "/api/v1/Option/GetOptions";

            return await MakeServiceGetCall<List<Erc720Token>>(apiEndPoint);
        }

        /// <summary>
        /// Gets a Option record based in the primary key id
        /// </summary>
        /// <param name="optionId">Primary Key</param>
        /// <returns>Option</returns>
        public async Task<Erc720Token> GetErc720Token(int optionId)
        {
            var apiEndPoint = $"/api/v1/Option/GetOption/{optionId}";

            return await MakeServiceGetCall<Erc720Token>(apiEndPoint);
        }

        /// <summary>
        /// Add a Option record
        /// </summary>
        /// <param name="record">Option</param>
        /// <returns>Option</returns>
        public async Task PostErc720Token(Erc720Token record)
        {
            var apiEndPoint = "/api/v1/Option/PostOption";

            await MakeServicePostCall(apiEndPoint, record);
        }

        /// <summary>
        /// Update a Option record
        /// </summary>
        /// <param name="record">Option</param>
        /// <returns></returns>
        public async Task PutErc720Token(Erc720Token record)
        {
            var apiEndPoint = "/api/v1/Option/PutOption";

            await MakeServicePutCall(apiEndPoint, record);
        }

        /// <summary>
        /// Delete a Option record
        /// </summary>
        /// <param name="optionId">Primary Key</param>
        /// <returns></returns>
        public async Task DeleteErc720Token(int optionId)
        {
            var apiEndPoint = $"/api/v1/Option/DeleteOption/{optionId}";

            await MakeServiceDeleteCall(apiEndPoint);
        }




    }
}
