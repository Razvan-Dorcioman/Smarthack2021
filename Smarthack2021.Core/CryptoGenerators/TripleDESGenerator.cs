using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace Smarthack2021.Core.CryptoGenerators
{
    public class TripleDESGenerator
    {
        public string EncryptDES3_CBC(byte[] message)
        {
            var generator = GeneratorUtilities.GetKeyGenerator("DESEDE");

            var key = generator.GenerateKey();
            
            var strResult = System.Convert.ToBase64String(key);

            return strResult;
        }
    }
}