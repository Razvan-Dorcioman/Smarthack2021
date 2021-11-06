using System.Threading.Tasks;
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

        public async Task<string> AddPassword(string password)
        {
            var key =  await _keyVaultService.GetKey("UserPasswords");

            var publicKey = System.Text.Encoding.Default.GetString(key.Key.N);

            var encryptedPassword = _rsaEncryption.RsaEncryptWithPublic(password, publicKey);

            return encryptedPassword;
        }
    }
}