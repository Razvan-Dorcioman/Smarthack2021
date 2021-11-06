using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Smarthack2021.Core.BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Smarthack2021.Core
{
    public class SaltNPepperHash : IPasswordHasher<User>
    {
        private IConfiguration _configuration { get; set; }

        public SaltNPepperHash(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private static string CreateSalt()
        {
            return "HardCodedSalt";
        }

        public string HashPassword(User user, string password)
        {
            var algorithm = new SHA512Managed();
            string pepper = _configuration.GetSection("Pepper").Value;

            if (string.IsNullOrEmpty(user.Salt))
                user.Salt = CreateSalt();

            var seasonedPassword = user.Salt + password + pepper;

            var hashedPassword = algorithm.ComputeHash(Encoding.ASCII.GetBytes(seasonedPassword));

            return Convert.ToBase64String(hashedPassword);
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
