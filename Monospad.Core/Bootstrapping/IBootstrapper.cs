﻿using Taga.Framework.IoC;

namespace Monospad.Core.Bootstrapping
{
    public interface IBootstrapper
    {
        void Bootstrap(IDependencyContainer container);
    }
}