using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Smarthack2021.Core.BusinessObject;
using Smarthack2021.Core.LoginAbstractions;

namespace Smarthack2021.Data
{
    public class UserRepository : IUserRepository
    {
        private UserContext _userContext { get; set; }
        
        private UserManager<User> _userManager { get; set;  }
        
        public UserRepository(UserContext userContext, UserManager<User> userManager)
        {
            _userContext = userContext;
            _userManager = userManager;
        }

        public void Dispose()
        {
            _userContext?.Dispose();
        }

        public IEnumerable<PasswordObject> GetPasswords(string userId)
        {
            var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);

            return user?.Passwords;
        }

        public PasswordObject GetPasswordById(Guid passwordId, string userId)
        {
            var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return user?.Passwords?.FirstOrDefault(p => p.Id == passwordId);
        }

        public async Task<PasswordObject> AddPassword(PasswordObject password, string userId)
        {
            var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);

            if (user == null) return null;

            user.Passwords ??= new List<PasswordObject>();
           
            user.Passwords.Add(password);

            return await Save(user)? password : null;
        }

        public async Task<bool> DeletePassword(Guid passwordId, string userId)
        {
            var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);

            var passToRemove = user?.Passwords.FirstOrDefault(p => p.Id == passwordId);

            if (passToRemove == null) return false;

            user.Passwords.Remove(passToRemove);

            return await Save(user);
        }

        public IEnumerable<CryptographicalKeyObject> GetKeys(string userId)
        {
            var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);

            return user?.Keys;
        }

        public CryptographicalKeyObject GetKeysById(Guid keyId, string userId)
        {
            var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);

            return user?.Keys.FirstOrDefault(k => k.Id == keyId);
        }

        public async Task<CryptographicalKeyObject> AddKey(CryptographicalKeyObject key, string userId)
        {
            var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);

            if (user == null) return null;
            
            user.Keys ??= new List<CryptographicalKeyObject>();

            user.Keys.Add(key);

            var res = await Save(user);
            return res ? key : null;
        }

        public async Task<bool> DeleteKey(Guid keyId, string userId)
        {
            var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);

            var keyToRemove = user?.Keys.FirstOrDefault(k => k.Id == keyId);
            
            if (keyToRemove == null) return false;

            user.Keys.Remove(keyToRemove);
            
            return await Save(user);
        }
        
        public async Task<bool> Save(User user)
        {
            var resultContext = await _userContext.SaveChangesAsync();
            var result = await _userManager.UpdateAsync(user);

            return result.Succeeded && resultContext > 0;
        }
    }
}