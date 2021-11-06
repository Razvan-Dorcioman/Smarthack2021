﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smarthack2021.Core.BusinessObject
{
    public class User : IdentityUser
    {
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpirationTime { get; set; }

        public ICollection<PasswordObject> Passwords { get; set; }
        
        public ICollection<CryptographicalKeyObject> Keys { get; set; }

        public string Salt { get; set; }
    }
}