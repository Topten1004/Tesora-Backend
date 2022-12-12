using System.Text;
using System.Security.Cryptography;


namespace NFTWallet.Engine
{
    public static class Security
    {
        public static string GenerateHash(string text)
        {
            // SHA512 is disposable by inheritance.  
            using (var sha256 = SHA256.Create())
            {
                // Send a sample text to hash.  
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(text));
                
                // Get the hashed string.  
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        public static string GeneratePassword()
        {
            // Generate a password, lenght 25 characters
            var charSet = "QqWwEeRrTtYyUuIiOoPpAaSsDdFfGgHhJjKkLlZzXxCcVvBbNnMm!1@2#3$4%5^6&7*8(9)0-_=+<,>.";
            var password = new StringBuilder();
            for (int i = 0; i < charSet.Length; i++)
            {
                var randomNumber = RandomNumberGenerator.GetInt32(0, charSet.Length);
                password.Append(charSet[randomNumber]);
            }

            return password.ToString();
        }
    }
}
