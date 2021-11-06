using System.Security.Cryptography;
using System.Text;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;

namespace Smarthack2021.Core.CryptoGenerators
{
    public class RSAGenerator
    {

        static string FormatKey(string key, bool isPrivate = false)
        {

            string startString = "-----BEGIN RSA PUBLIC KEY-----\n";
            string endString = "\n-----END RSA PUBLIC KEY-----";

            if (isPrivate)
            {
                startString = "-----BEGIN RSA PRIVATE KEY-----\n";
                endString = "\n-----END RSA PRIVATE KEY-----";
            }

            return startString + key + endString;
        }

        public object RsaGenerator()
        {
            var random = new SecureRandom();
            var keyGenerationParameters = new KeyGenerationParameters(random, 256);
            RsaKeyPairGenerator generator = new RsaKeyPairGenerator();
            generator.Init(keyGenerationParameters);

            var keyPair = generator.GenerateKeyPair();

            var publicKey = keyPair.Public;
            var privateKey = keyPair.Private;

            byte[] publicKeyDer = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(publicKey).GetDerEncoded();
            byte[] privateKeyDer = PrivateKeyInfoFactory.CreatePrivateKeyInfo(privateKey).GetDerEncoded();

            string privateKeyStr = System.Convert.ToBase64String(privateKeyDer);
            string publicKeyStr = System.Convert.ToBase64String(publicKeyDer);

            string PrivateKey = FormatKey(privateKeyStr, true);
            string PublicKey = FormatKey(publicKeyStr);

            return new { PrivateKey, PublicKey };

        }

    }
}