using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Security.KeyVault.Keys;
using Azure.Security.KeyVault.Keys.Cryptography;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.OpenSsl;
using Smarthack2021.Core.CryptoAbstractions;

namespace Smarthack2021.Core.CryptoAlgorithms
{
    public class RSAEncryption : IRSAEncryption
    {
        public async Task<string> RsaEncrypt(string clearText, KeyVaultKey key)
        {
            var inputAsByteArray = Encoding.UTF8.GetBytes(clearText);

            var cryptoClient = new CryptographyClient(key.Id, new DefaultAzureCredential());

            var encryptResult =  await cryptoClient.EncryptAsync(EncryptionAlgorithm.RsaOaep, inputAsByteArray);

            return Convert.ToBase64String(encryptResult.Ciphertext);
        }
        
        // Decryption:
        public async Task<string> RsaDecrypt(string base64Input, KeyVaultKey key)
        {
            var inputAsByteArray = Convert.FromBase64String(base64Input);
            
            var cryptoClient = new CryptographyClient(key.Id, new DefaultAzureCredential());

            var decryptResult = await cryptoClient.DecryptAsync(EncryptionAlgorithm.RsaOaep, inputAsByteArray);

            return Encoding.Default.GetString(decryptResult.Plaintext);
        }
    }
}