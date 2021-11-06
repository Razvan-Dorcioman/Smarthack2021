using System.Threading;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Security.KeyVault.Keys.Cryptography;
using Microsoft.AspNetCore.Identity;
using Smarthack2021.Core.BusinessObject;
using Smarthack2021.Core.CryptoAbstractions;

namespace Smarthack2021.Core
{
    public class CryptoOrchestrator : ICryptoOrchestrator
    {
        private IRSAEncryption _rsaEncryption { get; set; }
        
        private IKeyVaultService _keyVaultService { get; }
        
        public CryptoOrchestrator(IRSAEncryption rsaEncryption, IKeyVaultService keyVaultService)
        {
            _rsaEncryption = rsaEncryption;
            _keyVaultService = keyVaultService;
        }

        public async Task<string> AddPassword(string password, int userId)
        {
            var key =  await _keyVaultService.GetKey("UserPasswords");
            
            var encryptedPassword = await _rsaEncryption.RsaEncrypt(password, key);
            
            
            
            return encryptedPassword;
        }
        
        public async Task<string> GetPassword(string password)
        {
            var key =  await _keyVaultService.GetKey("UserPasswords");

            var decryptedPassword = await _rsaEncryption.RsaDecrypt(password, key);
            
            return decryptedPassword;
        }
    }
}