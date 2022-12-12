
// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//
using Microsoft.Extensions.DependencyInjection;


namespace NFTDatabaseService
{
    public static class ServicesConfiguration
    {
        public static void AddNFTDatabaseService(this IServiceCollection services, string baseAddress)
        {
            services.AddHttpClient<INFTDatabaseService, NFTDatabaseService>(c =>
            {
                c.BaseAddress = new Uri(baseAddress);
                c.DefaultRequestHeaders.Add("Accept", "application/json; charset=UTF-8");
                c.DefaultRequestHeaders.Add("User-Agent", "NFTDatabase");
            });
        }

    }
}
