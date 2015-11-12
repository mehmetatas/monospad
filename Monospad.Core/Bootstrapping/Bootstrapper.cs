using Monospad.Core.Bootstrapping.Bootstrappers;
using Monospad.Core.Models.Messages;
using TagKid.Framework.IoC;
using TagKid.Framework.IoC.Castle;
using TagKid.Framework.Validation;
using TagKid.Framework.WebApi;

namespace Monospad.Core.Bootstrapping
{
    public static class Bootstrapper
    {
        public static void StartApp()
        {
            DependencyContainer.Init(new CastleDependencyContainer());

            Bootstrap(
                new DependencyBootstrapper(),
                new DatabaseBootstrapper(),
                new ServiceBootstrapper());

            ValidationManager.LoadValidatorsFromAssemblyOf<SigninRequestValidator>();

            WebApi.Init();
        }

        public static void Bootstrap(params IBootstrapper[] bootstrappers)
        {
            foreach (var bootstrapper in bootstrappers)
            {
                bootstrapper.Bootstrap(DependencyContainer.Current);
            }
        }
    }
}