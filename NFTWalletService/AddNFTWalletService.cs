// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//
using Microsoft.Extensions.DependencyInjection;


namespace NFTWalletService
{
    public static class ServicesConfiguration
    {
        public static void AddNFTWalletService(this IServiceCollection services, string baseAddress)
        {
            services.AddHttpClient<INFTWalletService, NFTWalletService>(c =>
            {
                c.BaseAddress = new Uri(baseAddress);
                c.DefaultRequestHeaders.Add("Accept", "application/json; charset=UTF-8");
                c.DefaultRequestHeaders.Add("User-Agent", "NFTWalletService");
            });
        }

    }
}
