using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;

namespace Smarthack2021.Core.CryptoGenerators
{
    public class AESGenerator
    {
        public string Generate()
        {
            var generator = GeneratorUtilities.GetKeyGenerator("AES256");

            var result = generator.GenerateKey(); // 256 bit key
            
            var strResult = System.Text.Encoding.Default.GetString(result);

            return strResult;
        }
    }
}