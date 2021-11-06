using System.Threading.Tasks;

namespace Smarthack2021.Core.CryptoAbstractions
{
    public interface ICryptoOrchestrator
    { 
        public Task<string> AddPassword(string password, int userId);

        public Task<string> GetPassword(string password);
    }
}