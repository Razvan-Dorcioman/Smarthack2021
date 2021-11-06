using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Security;

namespace Smarthack2021.Core.CryptoGenerators
{
    public class ECCGenerator
    {
        private static AsymmetricCipherKeyPair GenerateKeys() {
            var gen = new ECKeyPairGenerator ();
            var secureRandom = new SecureRandom ();
            var keyGenParam = new KeyGenerationParameters (secureRandom, 256);
            gen.Init (keyGenParam);
            return gen.GenerateKeyPair ();
        }
        
        
    }
}