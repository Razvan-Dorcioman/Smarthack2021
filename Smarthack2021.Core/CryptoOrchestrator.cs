using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Security.KeyVault.Keys.Cryptography;
using Microsoft.AspNetCore.Identity;
using Smarthack2021.Core.BusinessObject;
using Smarthack2021.Core.CryptoAbstractions;
using Smarthack2021.Core.Enums;
using Smarthack2021.Core.LoginAbstractions;

namespace Smarthack2021.Core
{
    public class CryptoOrchestrator : ICryptoOrchestrator
    {
        private IRSAEncryption _rsaEncryption { get; set; }
        
        private IKeyVaultService _keyVaultService { get; }
        
        private IUserRepository _userRepository { get; set; }

        private const string LOWER_CASE = "abcdefghijklmnopqursuvwxyz";
        private const string UPPER_CASE = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string NUMBERS = "123456789";
        private const string SPECIALS = @"!@£$%^&*()#€";
        
        public CryptoOrchestrator(IRSAEncryption rsaEncryption, IKeyVaultService keyVaultService, IUserRepository userRepository)
        {
            _rsaEncryption = rsaEncryption;
            _keyVaultService = keyVaultService;
            _userRepository = userRepository;
        }

        public async Task<PasswordObject> AddPassword(string username, string password, string userId)
        {
            var key =  await _keyVaultService.GetKey("UserPasswords");
            
            var encryptedPassword = await _rsaEncryption.RsaEncrypt(password, key);

            var passwordDb = new PasswordObject
            {
                Username = username,
                EncryptedPassword = encryptedPassword
            };

            var result = await _userRepository.AddPassword(passwordDb, userId);
            
            return result;
        }
        
        public async Task<PasswordObject> GetPassword(Guid passwordId, string userId)
        {
            var key =  await _keyVaultService.GetKey("UserPasswords");

            var password = _userRepository.GetPasswordById(passwordId, userId);

            if (password?.EncryptedPassword == null) return null;
            
            var decryptedPassword = await _rsaEncryption.RsaDecrypt(password.EncryptedPassword, key);

            password.EncryptedPassword = decryptedPassword;

            return password;
        }

        public async Task<List<PasswordObject>> GetAllPasswords(string userId)
        {
            var passwords = _userRepository.GetPasswords(userId);

            var passwordObjects = passwords.ToList();
            return !passwordObjects.Any() ? null : passwordObjects.ToList();
        }

        public async Task<bool> DeletePassword(Guid passwordId, string userId)
        {
            var result = await _userRepository.DeletePassword(passwordId, userId);

            return result;
        }

        public async Task<string> GeneratePassword(PasswordGenerator passwordProps, string userId)
        {
            var generatePassword = GeneratePassword(passwordProps.UpperCase, passwordProps.Number,
                passwordProps.NonAlphaNumericCharacter, passwordProps.Length);

            return generatePassword;
        }

        private string GeneratePassword(bool useUppercase, bool useNumbers, bool useSpecial,
            int passwordSize)
        {
            var _password = new char[passwordSize];
            var charSet = LOWER_CASE;
            var _random = new Random();
            int counter;

            // Build up the character set to choose from
            if (useUppercase) charSet += UPPER_CASE;

            if (useNumbers) charSet += NUMBERS;

            if (useSpecial) charSet += SPECIALS;

            for (counter = 0; counter < passwordSize; counter++)
            {
                _password[counter] = charSet[_random.Next(charSet.Length - 1)];
            }

            return string.Join(null, _password);
        }

        public async Task<CryptographicalKeyObject> AddCryptoKey(string username, string publicKey, string? privateKey,string userId, CryptoType type)
        {
            //TODO: Check if userKey is correct type
            var vaultKey = await _keyVaultService.GetKey("UserKeys");

            var encryptedPublicKey = await _rsaEncryption.RsaEncrypt(publicKey, vaultKey);
            var encryptedPrivateKey = string.IsNullOrEmpty(privateKey) ? string.Empty : await _rsaEncryption.RsaEncrypt(privateKey, vaultKey);

            var cryptoKeyDb = new CryptographicalKeyObject
            {
                UserName = username,
                EncryptedPublicKey = encryptedPublicKey,
                EncryptedPrivateKey = encryptedPrivateKey,
                Type = type
            };

            var result = await _userRepository.AddKey(cryptoKeyDb, userId);

            return result;
        }
        
        public async Task<CryptographicalKeyObject> GetCryptographicalKey(Guid keyId, string userId)
        {
            var vaultKey = await _keyVaultService.GetKey("UserKeys");

            var userKey = _userRepository.GetKeyById(keyId, userId);

            if (userKey == null) return null;

            var decryptedPublicKey = await _rsaEncryption.RsaDecrypt(userKey.EncryptedPublicKey, vaultKey);
            var decryptedPrivateKey = string.IsNullOrEmpty(userKey?.EncryptedPrivateKey) ? string.Empty : await _rsaEncryption.RsaDecrypt(userKey.EncryptedPrivateKey, vaultKey);

            userKey.EncryptedPrivateKey = decryptedPrivateKey;
            userKey.EncryptedPublicKey = decryptedPublicKey;

            return userKey;
        }

    }
}