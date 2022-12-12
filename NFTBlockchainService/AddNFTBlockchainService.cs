using System;
using System.Net.Http;
using System.Text;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace NFTBlockchainService
{
    public static class ServicesConfiguration
    {
        public static void AddNFTBlockchainService(this IServiceCollection services, string baseAddress)
        {
            services.AddHttpClient<INFTBlockchainService, NFTBlockchainService>(c =>
            {
                c.BaseAddress = new Uri(baseAddress);
                c.DefaultRequestHeaders.Add("Accept", "application/json; charset=UTF-8");
                c.DefaultRequestHeaders.Add("User-Agent", "MyCom");
            });
        }

    }
}
