using System;

namespace Monospad.Core.Models.Database
{
    public class User
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime SignupDate { get; set; }
    }
}