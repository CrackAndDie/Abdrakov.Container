using System;

namespace Abdrakov.Container.Interfaces
{
    public interface IInstanceCreator
    {
        object CreateInstance(IContainerRegistration registration, IAbdrakovContainer container, bool withInjections = true);
        void ResolveInjections(object instance, IAbdrakovContainer container, Type type = null);
        bool RequiresInjections(object instance, Type type = null);
    }
}
