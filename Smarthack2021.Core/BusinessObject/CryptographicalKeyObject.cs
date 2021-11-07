﻿using System;
using Smarthack2021.Core.Enums;

namespace Smarthack2021.Core.BusinessObject
{
    public class CryptographicalKeyObject
    { 
        public Guid Id { get; set; }
        
        public string UserName { get; set; }

        public string EncryptedPrivateKey { get; set; }

        public string EncryptedPublicKey { get; set; }
        
        public CryptoType Type { get; set; }
        
        public User User { get; set; }
    }
}