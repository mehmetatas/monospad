using Monospad.Core.Bootstrapping;

namespace Monospad.Owin.SelfHost
{
    class Program
    {
        static void Main(string[] args)
        {
            OwinApp.SelfHost(9000);
            //SimpleApp.Start();
        }
    }
}
