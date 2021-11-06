using System.Threading.Tasks;
using Azure;
using Azure.Security.KeyVault.Keys;

namespace Smarthack2021.Core.CryptoAbstractions
{
    public interface IKeyVaultService
    {
        public Task<KeyVaultKey> GetKey(string keyName);
    }
}