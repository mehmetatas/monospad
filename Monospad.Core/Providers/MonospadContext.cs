using Monospad.Core.Models.Database;
using TagKid.Framework.Context;

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
                var ctx = CallContext.Current["MonospadContext"] as MonospadContext;
                if (ctx == null)
                {
                    ctx = new MonospadContext();
                    CallContext.Current["MonospadContext"] = ctx;
                }
                return ctx;
            }
        }

        public User User => Login.User;

        public Login Login { get; set; }
    }
}
