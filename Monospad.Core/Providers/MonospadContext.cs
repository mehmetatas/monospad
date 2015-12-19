using System.Runtime.Remoting.Messaging;
using Monospad.Core.Models.Database;

namespace Monospad.Core.Providers
{
    public class MonospadContext
    {
        private MonospadContext()
        {
        }

        public static MonospadContext Current
        {
            get
            {
                // TODO: Make sure consistency and thread safety of CallContext
                var ctx = CallContext.GetData("MonospadContext") as MonospadContext;
                if (ctx == null)
                {
                    ctx = new MonospadContext();
                    CallContext.SetData("MonospadContext", ctx);
                }
                return ctx;
            }
        }

        public User User => Login.User;

        public Login Login { get; set; }
    }
}
