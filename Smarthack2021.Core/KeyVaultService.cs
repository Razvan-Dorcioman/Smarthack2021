using System;
using System.Threading.Tasks;
using Azure;
using Azure.Identity;
using Azure.Security.KeyVault.Keys;
using Smarthack2021.Core.CryptoAbstractions;

namespace Smarthack2021.Core
{
    public class KeyVaultService : IKeyVaultService
    {
        private KeyClient _client { get; set; }

        public KeyVaultService()
        {
            _client = new KeyClient(new Uri("https://smarthack2021keys.vault.azure.net/"), new DefaultAzureCredential());;
        }

        public async Task<KeyVaultKey> GetKey(string keyName)
        {
            var key = await _client.GetKeyAsync(keyName);

            return key.Value;
        }
    }
}