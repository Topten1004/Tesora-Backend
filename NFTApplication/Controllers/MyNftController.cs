using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nethereum.Contracts;

namespace NFTApplication.Controllers
{
    /// <summary>
    /// NFT Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MyNftController : ControllerBase
    {

    //    public async Task GetMyOwnedNfts()
    //    {
    //        try
    //        {
    //            [Function("balanceOf", "uint256")]
    //            public class BalanceOfFunction : FunctionMessage
    //    {
    //        [Parameter("address", "_owner", 1)]
    //        public string Owner { get; set; }
    //    }




    //    public void QueryingForBalanceAtBlockNumberWorksAsExpected()
    //    {
    //        var web3 = new Nethereum.Web3.Web3("https://mainnet.infura.io/v3/7238211010344719ad14a89db874158c");

    //        string contractAddress = "0xc36442b4a4522e871399cd717abdd847ab11fe88";

    //        string accountAddress = "0x5794d36de0c21211a7906688981371132bd7c6f0";

    //        var balanceOfFunctionMessage = new BalanceOfFunction()
    //        {
    //            Owner = accountAddress,
    //        };

    //        var balanceHandler = web3.Eth.GetContractQueryHandler<BalanceOfFunction>();


    //        var k = balanceHandler.QueryAsync<BigInteger>(contractAddress, balanceOfFunctionMessage);
    //        UnityEngine.Debug.LogError(k.Result);
    //    }
    //}
    //    }
    }
}
