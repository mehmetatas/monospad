using System;

namespace Monospad.Core.Providers
{
    public interface IMailProvider
    {
        void SendPasswordRecoveryMail(string email, Guid token);
    }
}
