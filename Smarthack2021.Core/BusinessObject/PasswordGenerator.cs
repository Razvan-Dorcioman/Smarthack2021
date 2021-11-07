using System;

namespace Smarthack2021.Core.BusinessObject
{
    public class PasswordGenerator
    {
        public int Length { get; set; }

        public bool UpperCase { get; set; }

        public bool Number { get; set; }

        public bool NonAlphaNumericCharacter { get; set; }
    }
}