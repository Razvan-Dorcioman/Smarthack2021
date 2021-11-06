using System.Security.Cryptography;
using System.Text;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace Smarthack2021.Core.CryptoGenerators
{
    public class RSAGenerator
    {
        public string RsaGenerator()
        {
            // Encryption steps -----------------------------------
            var hash = new SHA256Managed();
            var randomNumber = new SecureRandom();
            var encodingParam =
                hash.ComputeHash(Encoding.UTF8.GetBytes(randomNumber.ToString() ?? string.Empty));
            
            var inputMessage = "Test Message";
            var utf8enc = new UTF8Encoding();
            // Converting the string message to byte array
            var inputBytes = utf8enc.GetBytes(inputMessage);
            
            // RSAKeyPairGenerator generates the RSA Key pair based on the random number and strength of key required
            var rsaKeyPairGnr = new RsaKeyPairGenerator();
            rsaKeyPairGnr.Init(new KeyGenerationParameters(new SecureRandom(), 1024));
            var keyPair = rsaKeyPairGnr.GenerateKeyPair();
            var publicKey = (RsaKeyParameters)keyPair.Public;
            var privateKey = (RsaKeyParameters)keyPair.Private;
            IAsymmetricBlockCipher cipher = new OaepEncoding(new RsaEngine(), new
                Sha256Digest(), encodingParam);
            cipher.Init(true, publicKey);
            var ciphered = cipher.ProcessBlock(inputBytes, 0, inputMessage.Length);
            var cipheredText = utf8enc.GetString(ciphered);
            
            // Decryption steps --------------------------------------------
            cipher.Init(false, privateKey);
            var deciphered = cipher.ProcessBlock(ciphered, 0, ciphered.Length);
            var decipheredText = utf8enc.GetString(deciphered);

            return decipheredText;
        }

    }
}