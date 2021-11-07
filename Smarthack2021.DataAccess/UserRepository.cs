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
            var passwords = _userContext.Passwords.Where(p => p.User.Id == userId);

            return passwords;
        }

        public PasswordObject GetPasswordById(Guid passwordId, string userId)
        {
            var password = _userContext.Passwords.FirstOrDefault(p => p.Id == passwordId && p.User.Id == userId);
            return password;
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
            var passToRemove = _userContext.Passwords.FirstOrDefault(u => u.Id == passwordId && u.User.Id == userId);

            // var passToRemove = user?.Passwords?.FirstOrDefault(p => p.Id == passwordId);
            //
             if (passToRemove == null) return false;

             _userContext.Passwords.Remove(passToRemove);

            //user.Passwords.Remove(passToRemove);

            return await Save();
        }

        public IEnumerable<CryptographicalKeyObject> GetKeys(string userId)
        {
            var keys = _userContext.Keys.Where(p => p.User.Id == userId);

            return keys;
        }

        public CryptographicalKeyObject GetKeyById(Guid keyId, string userId)
        {
            var key = _userContext.Keys.FirstOrDefault(k => k.User.Id == userId);

            return key;
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
            var keyToRemove = _userContext.Keys.FirstOrDefault(u => u.Id == keyId && u.User.Id == userId);

            // var passToRemove = user?.Passwords?.FirstOrDefault(p => p.Id == passwordId);
            //
            if (keyToRemove == null) return false;

            _userContext.Keys.Remove(keyToRemove);

            //user.Passwords.Remove(passToRemove);

            return await Save();
        }
        
        public async Task<bool> Save(User user = null)
        {
            var resultContext = await _userContext.SaveChangesAsync();
            
            if (user == null && resultContext > 0) return true;
            
            var result = await _userManager.UpdateAsync(user);

            return result.Succeeded && resultContext > 0;
        }
    }
}