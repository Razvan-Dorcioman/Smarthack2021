using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Smarthack2021.Core.BusinessObject;

namespace Smarthack2021.Core.LoginAbstractions
{
    public interface IUserRepository : IDisposable
    {
        IEnumerable<PasswordObject> GetPasswords(string userId);
        
        PasswordObject GetPasswordById(Guid passwordId, string userId);
        
        Task<PasswordObject> AddPassword(PasswordObject password, string userId);
        
        Task<bool> DeletePassword(Guid passwordId, string userId);
        
        IEnumerable<CryptographicalKeyObject> GetKeys(string userId);
        
        CryptographicalKeyObject GetKeyById(Guid keyId, string userId);
        
        Task<CryptographicalKeyObject> AddKey(CryptographicalKeyObject key, string userId);
        
        Task<bool> DeleteKey(Guid keyId, string userId);

        Task<bool> Save(User user);
    }
}