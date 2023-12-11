using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abdrakov.Container.Interfaces
{
    public interface IAbdrakovContainer : IDisposable
    {
        IList<IContainerRegistration> Registrations { get; }

        IInstanceCreator InstanceCreator { get; }

        void SetInstanceCreator(IInstanceCreator creator);

        IAbdrakovContainer RegisterType(Type registeredType, Type mappedToType, bool isSingleton = false);
        IAbdrakovContainer RegisterType(Type registeredType, Type mappedToType, string name, bool isSingleton = false);
        IAbdrakovContainer RegisterInstance(Type type, object instance);
        IAbdrakovContainer RegisterInstance(Type type, object instance, string name);
        IAbdrakovContainer RegisterFactory(Type type, Func<IAbdrakovContainer, Type, object> factory);

        bool IsRegistered(Type type);
        bool IsRegistered(Type type, string name);

        object Resolve(Type type);
        object Resolve(Type type, bool withInjections);
        object Resolve(Type type, string name, bool withInjections);
        void ResolveInjections(Type type);
    }
}
