using System;

namespace Monospad.Core.Models.Database
{
    public class Login
    {
        public long Id { get; set; }
        public Guid Token { get; set; }
        public User User { get; set; }
        public bool IsPasswordRecovery { get; set; }
        public DateTime ExpireDate { get; set; }
    }
}
