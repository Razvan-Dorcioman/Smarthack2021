using System;
using System.Threading.Tasks;
using Azure;
using Azure.Identity;
using Azure.Security.KeyVault.Keys;
using Microsoft.Extensions.Azure;
using Smarthack2021.Core.CryptoAbstractions;

namespace Smarthack2021.Core
{
    public class KeyVaultService : IKeyVaultService
    {
        private KeyClient _client { get; set; }

        public KeyVaultService()
        {
            var creds = new EnvironmentCredential();
            _client = new KeyClient(new Uri("https://smarthack2021key.vault.azure.net/"), creds);;
        }

        public async Task<KeyVaultKey> GetKey(string keyName)
        {
            var key = await _client.GetKeyAsync(keyName);
            
            return key.Value;
        }
    }
}