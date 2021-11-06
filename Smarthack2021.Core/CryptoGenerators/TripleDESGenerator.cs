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

            var result = generator.GenerateKey();
            
            var strResult = System.Text.Encoding.Default.GetString(result);

            return strResult;
        }
    }
}