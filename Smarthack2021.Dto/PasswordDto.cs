using System;

namespace Smarthack2021.Dto
{
    public class PasswordDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        
        public string Password { get; set; }
        
        public string Tags { get; set; }
    }
}