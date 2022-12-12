using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace NFTDatabaseService.Tests
{
    public class UserTests
    {
        private readonly IConfiguration _configuration;
        private readonly INFTDatabaseService _db;


        public UserTests(IConfiguration configuration, INFTDatabaseService db)
        {
            _configuration = configuration;
            _db = db;
        }


        [Theory]
        [InlineData("1234567890", true, "TestUser")]
        [InlineData("fbd7fae6-7650-4ff2-b4b1-5e3ddb386f23", false, "im.christopher.groe@gmail.com")]
        public async Task UserExists(string masterUserId, bool existsInTest, string user)
        {
            try
            {
                Console.WriteLine($"Testing {user}");

                var existsInDB = await _db.UserExists(masterUserId);

                Assert.True(existsInDB == existsInTest);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
        }


        [Theory]
        [InlineData("0987654321", "Chris", "McGorty", "chris@mycom.global", "TestUser" )]
        public async Task AddUser(string masterUserId, string firstName, string lastName, string email, string userName)
        {
            try
            {
                if (await _db.UserExists(masterUserId) == false)
                {
                    var record = new NFTDatabaseEntities.User
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        Email = email,
                        Username = userName,
                        Status = NFTDatabaseEntities.User.UserStatuses.active,
                        CreateDate = DateTime.UtcNow,
                        MasterUserId = masterUserId
                    };

                    await _db.PostUser(record);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        [Theory]
        [InlineData("293670b1-cf7e-4b15-ae5c-d4b6c3a9ad81")]
        public async Task GetUserMasterIdTest(string masterUserId)
        {
            try
            {
                var user = await _db.GetUserMasterId(masterUserId);

                Assert.True(user.MasterUserId == masterUserId);
                Assert.True(user.FirstName == "Chris");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
        }

        [Theory]
        [InlineData(33)]
        public async Task MyOffers(int userId)
        {
            try
            {
                var records = await _db.GetMyOffers(userId);

                Assert.NotEmpty(records);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
        }


    }
}