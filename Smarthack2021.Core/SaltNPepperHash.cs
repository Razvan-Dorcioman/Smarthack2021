using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Smarthack2021.Core.BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Security.KeyVault.Keys.Cryptography;
using Smarthack2021.Core.CryptoAbstractions;

namespace Smarthack2021.Core
{
    public class SaltNPepperHash : IPasswordHasher<User>
    {
        private IConfiguration _configuration { get; set; }
        
        private IKeyVaultService _keyVaultService { get; }

        public SaltNPepperHash(IConfiguration configuration, IKeyVaultService keyVaultService)
        {
            _configuration = configuration;
            _keyVaultService = keyVaultService;
        }

        private async Task<string> GetSalt(string salt)
        {
            var key =  await _keyVaultService.GetKey("UserSalt");

            var inputAsByteArray = Encoding.UTF8.GetBytes(salt);

            var cryptoClient = new CryptographyClient(key.Id, new DefaultAzureCredential());

            var encryptResult =  await cryptoClient.EncryptAsync(EncryptionAlgorithm.RsaOaep, inputAsByteArray);

            return Convert.ToBase64String(encryptResult.Ciphertext);
        }

        public string HashPassword(User user, string password)
        {
            var algorithm = new SHA512Managed();
            var pepper = _configuration.GetSection("Pepper").Value;

            if (string.IsNullOrEmpty(user.Salt))
                user.Salt = CreateRandomSalt();

            var cryptedSalt = GetSalt(user.Salt);

            var seasonedPassword = cryptedSalt + password + pepper;

            var hashedPassword = algorithm.ComputeHash(Encoding.ASCII.GetBytes(seasonedPassword));

            return Convert.ToBase64String(hashedPassword);
        }

        private static string CreateRandomSalt()
        {
            var _password = new char[10];
            const string charSet = "abcdefghijklmnopqursuvwxyz";
            var _random = new Random();
            int counter;

            for (counter = 0; counter < 10; counter++)
            {
                _password[counter] = charSet[_random.Next(charSet.Length - 1)];
            }

            return string.Join(null, _password);
        }

        public PasswordVerificationResult VerifyHashedPassword(User user, string hashedPassword, string providedPassword)
        {

            var hhh = HashPassword(user, providedPassword);

            if (hashedPassword.Equals(HashPassword(user, providedPassword)))
                return PasswordVerificationResult.Success;

            return PasswordVerificationResult.Failed;
        }

    }
}
