using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace NFTWalletService.Tests
{
    public class Startup
    {
        public void ConfigureHost(IHostBuilder hostBuilder)
        {
            hostBuilder
                .ConfigureHostConfiguration(builder =>
                {
                    builder.AddJsonFile("appsettings.json", true);
                });
        }

        public void ConfigureServices(IServiceCollection services, HostBuilderContext hostBuilderContext)
        {
            var configuration = hostBuilderContext.Configuration;

            var env = configuration["Environment:Prefix"];
            var baseAddress = configuration[$"NFTWalletService:{env}BaseUrl"];

            services.AddNFTWalletService(baseAddress);
        }

    }
}
