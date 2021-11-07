using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Smarthack2021.Core.BusinessObject;
using Smarthack2021.Core.Enums;

namespace Smarthack2021.Core.CryptoAbstractions
{
    public interface ICryptoOrchestrator
    { 
        Task<PasswordObject> AddPassword(string username, string password, string userId);
        Task<PasswordObject> GetPassword(Guid passwordId, string userId);
        Task<List<PasswordObject>> GetAllPasswords(string userId);
        Task<bool> DeletePassword(Guid guid, string toString);
        Task<PasswordGenerator> GeneratePassword(PasswordGenerator map, string userId);
        Task<CryptographicalKeyObject> AddCryptoKey(string username, string publicKey, string? privateKey, string userId, CryptoType type);
        Task<CryptographicalKeyObject> GetCryptographicalKey(Guid keyId, string userId);
    }
}