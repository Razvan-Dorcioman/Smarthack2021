using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;

namespace Smarthack2021.Core.CryptoGenerators
{
    public class AESGenerator
    {
        public string Generate()
        {
            var generator = GeneratorUtilities.GetKeyGenerator("AES256");

            var key = generator.GenerateKey(); // 256 bit key
            
            var strResult = System.Convert.ToBase64String(key);

            return strResult;
        }
    }
}