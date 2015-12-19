using Taga.Orm.Db;
using Taga.Framework.IoC;
using Monospad.Core.Models.Database;
using Taga.Orm.Providers.SqlServer2012;

namespace Monospad.Core.Bootstrapping.Bootstrappers
{
    class DatabaseBootstrapper : IBootstrapper
    {
        public void Bootstrap(IDependencyContainer container)
        {
            var dbFactory = InitTableMappings();

            container.RegisterSingleton(dbFactory);
        }

        private static IDbFactory InitTableMappings()
        {
            return Db.Setup(SqlServer2012Provider.FromConnectionStringName("monospad"))
                // Tables
                .Table<User>()
                .Table<Note>()
                .Table<Login>()
                // Build
                .BuildFactory();
        }
    }
}