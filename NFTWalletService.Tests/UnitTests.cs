using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;


namespace NFTWalletService.Tests
{
    public class MyTests
    {
        private readonly INFTWalletService _wallet;

        public MyTests(INFTWalletService wallet)
        {
            _wallet = wallet;
        }


        [Theory]
        [InlineData("293670b1-cf7e-4b15-ae5c-d4b6c3a9ad81")]
        [InlineData("fbd7fae6-7650-4ff2-b4b1-5e3ddb386f23")]
        public async Task WalletBalance(string masterUserId)
        {
            try
            {
                var balance = await _wallet.GetBalance(masterUserId);

                Assert.True(balance != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
        }


        [Theory]
        [InlineData("0xEefD87A0Bb0fF9e82C52EE02e44f1d02A1418Cc6")]
        [InlineData("0xC7D43b7dB972DcFdab0Ef11BAE6A05AA406caEBC")]
        public async Task WalletBalanceForAddress(string address)
        {
            try
            {
                var balance = await _wallet.GetBalanceForAddress(address);

                Assert.True(balance != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
        }

    }
}