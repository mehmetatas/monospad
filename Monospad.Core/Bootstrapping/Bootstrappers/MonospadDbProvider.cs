using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DummyOrm.Providers.SqlServer2012;

namespace Monospad.Core.Bootstrapping.Bootstrappers
{
    class MonospadDbProvider : SqlServer2012Provider
    {
        public override IDbConnection CreateConnection()
        {
            return new SqlConnection(ConfigurationManager.ConnectionStrings["monospad"].ConnectionString);
        }
    }
}