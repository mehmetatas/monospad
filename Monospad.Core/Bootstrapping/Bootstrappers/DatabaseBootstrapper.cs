using DummyOrm.Db;
using TagKid.Framework.IoC;
using Monospad.Core.Models.Database;

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
            return Db.Setup(new MonospadDbProvider())

                // Tables
                .Table<User>()
                .Table<Note>()
                .Table<Login>()

                // Build
                .BuildFactory();
        }
    }
}