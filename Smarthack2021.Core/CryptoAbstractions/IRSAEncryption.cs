using System.Threading.Tasks;
using Azure.Security.KeyVault.Keys;

namespace Smarthack2021.Core.CryptoAbstractions
{
    public interface IRSAEncryption
    {
        public Task<string> RsaEncrypt(string clearText, KeyVaultKey key);

        public Task<string> RsaDecrypt(string clearText, KeyVaultKey key);
    }
}