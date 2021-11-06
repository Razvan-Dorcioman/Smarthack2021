using System;

namespace Smarthack2021.Core.BusinessObject
{
    public class PasswordObject
    {
        public Guid Id { get; set; }
        
        public string Username { get; set; }
        
        public string EncryptedPassword { get; set; }
    }
}