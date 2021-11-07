using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using System;

namespace Smarthack2021.Core.CryptoGenerators
{
    public class ECCGenerator
    {

        private static string FormatString(string key, bool IsPrivate = false)
        {
            //string startString = "-----BEGIN EC PUBLIC KEY-----\n";
            //string endString = "\n-----END EC PUBLIC KEY-----";

            //if (IsPrivate)
            //{
            //    startString = "-----BEGIN EC PRIVATE KEY-----\n";
            //    endString = "\n-----END EC PRIVATE KEY-----";
            //}

            //return startString + key + endString;

            return key;
        }

        public object GenerateKeys() {
            var gen = new ECKeyPairGenerator();
            var secureRandom = new SecureRandom();
            var keyGenParam = new KeyGenerationParameters(secureRandom, 256);
            gen.Init(keyGenParam);

            AsymmetricCipherKeyPair keyPair = gen.GenerateKeyPair();

            var privateKey = (ECPrivateKeyParameters)keyPair.Private;
            var publicKey = (ECPublicKeyParameters)keyPair.Public;

            var privateKeyDer = PrivateKeyInfoFactory.CreatePrivateKeyInfo(privateKey).GetDerEncoded();
            var publicKeyDer = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(publicKey).GetDerEncoded();

            var privateKeyStr = Convert.ToBase64String(privateKeyDer);
            var publicKeyStr = Convert.ToBase64String(publicKeyDer);

            var PublicKey = FormatString(publicKeyStr);
            var PrivateKey = FormatString(privateKeyStr, true);

            return new { PublicKey, PrivateKey };

        }
        
        
    }
}