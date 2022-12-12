// <copyright company="Chris McGorty" author="Chris McGorty">
//     Copyright (c) 2020 All Rights Reserved
// </copyright>
using System;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;


namespace NFTWallet.Engine
{
    /// <summary>
    /// RSA Interface
    /// </summary>
    public interface IRSA
    {
        /// <summary>Encrypt a string</summary>
        /// <param name="plainText"></param>
        /// <returns>string</returns>
        string EncryptString(string plainText);

        /// <summary>Decrypt a string</summary>
        /// <param name="encryptedText"></param>
        /// <returns>string</returns>
        string DecryptString(string encryptedText);        
    }

    internal class RSA : IRSA
    {
        readonly string privateKey = "<?xml version=\"1.0\" encoding=\"utf-16\"?>" +
                                     "<RSAParameters xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">" +
                                     "<D>lrohGU294lptpk33kM7U8zkFA+E+NfIZfc9HwdObXntn4kQWeuIigDWdAsAOsEmAuCQWWQx4EdvosJOZSLlh28Sf7ets/qeeHia9tOPkwirQRsSnWCTBka30mGu+UNSe3wkjCMktf1z8MvuQ29y1lHH0ofixHWywdTtHEsyiGCOtcVvwdVXNg77EVgav19m9soX+CQtHJk7Ovtwevk/zDqxBy/nA1vka7T6N2C3OR4CJrD7sw1j0AFpuwDdApBeZzkZM8lWvGhRAgM/t++HqZnOS4OtSPSgHSIlU2K9RrmCSnqZJritSTTcWcxd+KvE9CLYXn1nAX9NaRxVr0NAb+Q==</D>" +
                                     "<DP>zYWo7dEK4J3Olenz994Zrvmt/ul4Kc1YU9ouz63ubmqZEMskTWNnkM1tzVvGEvFMmFaLjgxw/zJXqvsolGk9RdxQol6URqFvYwVGAbxKbE4nODocXBBRj4aX5P8C/mDSaIzmt8DwimQf50uRbnPuu/wmeYplOg1MpjnWMgEOdTk=</DP>" +
                                     "<DQ>X+BJWjw2cbUXwpBdQdxWOxhMlgXmkjDL6PS8oB4TU0uRxiosqvUyoyxFRBnV1zOmlMuNQYmHDcz1HO3Ow5xrF2e7cSMsK/6D0zxHkPlZbKi84DKMePQ6y8ma30DWjlM1HRalrqF0SP9cr1e3OEHt0s+hyDV2wGx0UAWAx7fHNxU=</DQ>" +
                                     "<Exponent>AQAB</Exponent>" +
                                     "<InverseQ>z1M9Sq5Ry2U6ZRN3UvMGz/05gJo2XS9ZMzPS1HavVK3qy7rSJRtOK8vSmNR0WnPdCJYZx1JtJap73LIKioEtj3/7eRdC4b7M4I79vOUKzPDR5ufFAlUAl1v4uzQoOxJZqZfq7QIYxdovRp5GN1wwDm7HQo1aQdE1sXnkRB1gtmA=</InverseQ>" +
                                     "<Modulus>pzqMqB0EEMgwVYxhl8Fo3KK+1ONYVC6PcyLcCua0R44ZfX/O5tuTYwLtIczHvVQ6bWO/82ReMXC7+C7ZJOjt3RAK5ct4GYmjMbIPQObhKSBVFr1G07wNg8pAvZS9mXB6rk6f9jTZHEaspIvX/My8pgBeZAfiNa2YiJWNsxlbWiHDa5QbfjRbHmvlKrhg2rw7o7WxdwMXC1z9eOOwFtgWabj2hRhVqsBEk3WfnoYpISgr7ThukyZMocwO9dyKIDRGbdVEVsbLaFJNbw5TdLZy3mQ8LQqTKvtV++PZ6dg03e4NZGTSEiDuguqv42fRIai05ZzpABgojIpw0+7GeP7pwQ==</Modulus>" +
                                     "<P>0sLFELAYjc58UlQYrsmbpuzniQLErgg+jwgaMGiXPihP1yBGV95/6abf0iX9A7sZXvspHhNBgC5+igiieMn2B7KJ45Jt172wUbxzdT/uusRL2LeDnzUjjfLb2bNYhE0rikKOhwpuouip49A4484PmymfJcUP9fKZe5Rf/kxuhR8=</P>" +
                                     "<Q>yx+0vsi1Hva6EVBSgqtwmCDE4ZUmN8XbGt5DzS0yefl0WpoM62A57s+GUUKAvsATqq3F8mJ8S5LZ7lBv5iPW6wUJ42u+Yiv99k1Aelrz+9jRrjLI/uWZN1WkaW4IK2ERsiPwfEKxavf0EoH7h+msQZasCJm4co/e3jvSwjfm1R8=</Q>" +
                                     "</RSAParameters>";

        readonly string publicKey = "<?xml version=\"1.0\" encoding=\"utf-16\"?>" +
                                    "<RSAParameters xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">" +
                                    "<Exponent>AQAB</Exponent>" +
                                    "<Modulus>pzqMqB0EEMgwVYxhl8Fo3KK+1ONYVC6PcyLcCua0R44ZfX/O5tuTYwLtIczHvVQ6bWO/82ReMXC7+C7ZJOjt3RAK5ct4GYmjMbIPQObhKSBVFr1G07wNg8pAvZS9mXB6rk6f9jTZHEaspIvX/My8pgBeZAfiNa2YiJWNsxlbWiHDa5QbfjRbHmvlKrhg2rw7o7WxdwMXC1z9eOOwFtgWabj2hRhVqsBEk3WfnoYpISgr7ThukyZMocwO9dyKIDRGbdVEVsbLaFJNbw5TdLZy3mQ8LQqTKvtV++PZ6dg03e4NZGTSEiDuguqv42fRIai05ZzpABgojIpw0+7GeP7pwQ==</Modulus>" +
                                    "</RSAParameters>";

        public string EncryptString(string plainText)
        {
            //get a stream from the string
            var sr = new StringReader(publicKey); // pubKeyString

            //we need a deserializer
            var xs = new XmlSerializer(typeof(RSAParameters));

            //get the object back from the stream
            var csp = new RSACryptoServiceProvider();

            csp.ImportParameters((RSAParameters)xs.Deserialize(sr));
            var bytesPlainTextData = Encoding.ASCII.GetBytes(plainText);

            //apply pkcs#1.5 padding and encrypt our data 
            var bytesCipherText = csp.Encrypt(bytesPlainTextData, false);

            //we might want a string representation of our cypher text... base64 will do
            string encryptedText = Convert.ToBase64String(bytesCipherText);

            return encryptedText;
        }


        public string DecryptString(string encryptedText)
        {
            //we want to decrypt, therefore we need a csp and load our private key
            var csp = new RSACryptoServiceProvider();

            //get a stream from the string
            var sr = new StringReader(privateKey);  // privKeyString

            //we need a deserializer
            var xs = new XmlSerializer(typeof(RSAParameters));

            //get the object back from the stream
            var privKey = (RSAParameters)xs.Deserialize(sr);
            csp.ImportParameters(privKey);

            var bytesCipherText = Convert.FromBase64String(encryptedText);

            //decrypt and strip pkcs#1.5 padding
            var bytesPlainTextData = csp.Decrypt(bytesCipherText, false);

            return Encoding.ASCII.GetString(bytesPlainTextData);
        }

    }

}
