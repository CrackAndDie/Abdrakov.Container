using Abdrakov.Container.Interfaces;
using Prism.Ioc;
using Prism.Ioc.Internals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Abdrakov.Container.AvaloniaPrismAdapter
{
    public class AbdrakovContainerExtension : IContainerExtension<IAbdrakovContainer>, IContainerInfo
    {
        private AbdrakovScopedProvider _currentScope;

        /// <summary>
        /// The instance of the wrapped container
        /// </summary>
        public IAbdrakovContainer Instance { get; }

#if !ContainerExtensions
        /// <summary>
        /// Constructs a default <see cref="AbdrakovContainerExtension" />
        /// </summary>
        public AbdrakovContainerExtension()
            : this(new AbdrakovContainer())
        {
        }

        /// <summary>
        /// Constructs a <see cref="AbdrakovContainerExtension" /> with the specified <see cref="IAbdrakovContainer" />
        /// </summary>
        /// <param name="container"></param>
        public AbdrakovContainerExtension(IAbdrakovContainer container)
        {
            Instance = container;
            Instance.RegisterInstance(typeof(IAbdrakovContainer), Instance);
            Instance.RegisterInstance(this.GetType(), this);
            Instance.RegisterInstance(typeof(IContainerExtension), this);
            Instance.RegisterInstance(typeof(IContainerProvider), this);
            // ExceptionExtensions.RegisterFrameworkExceptionType(typeof(ResolutionFailedException));
        }
#endif

        public IScopedProvider CurrentScope => _currentScope;

        public virtual IScopedProvider CreateScope()
        {
            return CreateScopeInternal();
        }

        public void FinalizeExtension()
        {

        }

        [Obsolete("Abdrakov.Container does not provide registrations/resolves by string name")]
        public Type GetRegistrationType(string key)
        {
            throw new NotImplementedException("Abdrakov.Container does not provide registrations/resolves by string name");
        }

        public Type GetRegistrationType(Type serviceType)
        {
            var matchingRegistration = Instance.Registrations.Where(x => x.RegisteredType == serviceType).FirstOrDefault();
            return matchingRegistration?.MappedToType;
        }

        public bool IsRegistered(Type type)
        {
            return Instance.IsRegistered(type);
        }

        [Obsolete("Abdrakov.Container does not provide registrations/resolves by string name")]
        public bool IsRegistered(Type type, string name)
        {
            return Instance.IsRegistered(type);
        }

        public IContainerRegistry Register(Type from, Type to)
        {
            Instance.RegisterType(from, to);
            return this;
        }

        [Obsolete("Abdrakov.Container does not provide registrations/resolves by string name")]
        public IContainerRegistry Register(Type from, Type to, string name)
        {
            return Register(from, to);
        }

        public IContainerRegistry Register(Type type, Func<object> factoryMethod)
        {
            Instance.RegisterFactory(type, (c, t) => { return factoryMethod?.Invoke(); });
            return this;
        }

        public IContainerRegistry Register(Type type, Func<IContainerProvider, object> factoryMethod)
        {
            Instance.RegisterFactory(type, (c, t) => { return factoryMethod?.Invoke(this); });
            return this;
        }

        public IContainerRegistry RegisterInstance(Type type, object instance)
        {
            Instance.RegisterInstance(type, instance);
            return this;
        }

        [Obsolete("Abdrakov.Container does not provide registrations/resolves by string name")]
        public IContainerRegistry RegisterInstance(Type type, object instance, string name)
        {
            Instance.RegisterInstance(type, instance);
            return this;
        }

        [Obsolete("Abdrakov.Container does not provide difficult registrations")]
        public IContainerRegistry RegisterMany(Type type, params Type[] serviceTypes)
        {
            throw new NotImplementedException();
        }

        [Obsolete("Abdrakov.Container does not provide difficult registrations")]
        public IContainerRegistry RegisterManySingleton(Type type, params Type[] serviceTypes)
        {
            throw new NotImplementedException();
        }

        [Obsolete("Abdrakov.Container does not provide difficult registrations")]
        public IContainerRegistry RegisterScoped(Type from, Type to)
        {
            return Register(from, to);
        }

        [Obsolete("Abdrakov.Container does not provide difficult registrations")]
        public IContainerRegistry RegisterScoped(Type type, Func<object> factoryMethod)
        {
            return Register(type, factoryMethod);
        }

        [Obsolete("Abdrakov.Container does not provide difficult registrations")]
        public IContainerRegistry RegisterScoped(Type type, Func<IContainerProvider, object> factoryMethod)
        {
            return Register(type, factoryMethod);
        }

        public IContainerRegistry RegisterSingleton(Type from, Type to)
        {
            Instance.RegisterType(from, to, isSingleton: true);
            return this;
        }

        [Obsolete("Abdrakov.Container does not provide registrations/resolves by string name")]
        public IContainerRegistry RegisterSingleton(Type from, Type to, string name)
        {
            Instance.RegisterType(from, to, isSingleton: true);
            return this;
        }

        [Obsolete("Abdrakov.Container does not provide difficult registrations")]
        public IContainerRegistry RegisterSingleton(Type type, Func<object> factoryMethod)
        {
            return Register(type, factoryMethod);
        }

        [Obsolete("Abdrakov.Container does not provide difficult registrations")]
        public IContainerRegistry RegisterSingleton(Type type, Func<IContainerProvider, object> factoryMethod)
        {
            return Register(type, factoryMethod);
        }

        public object Resolve(Type type)
        {
            return Instance.Resolve(type);
        }

        [Obsolete("Abdrakov.Container does not provide difficult registrations")]
        public object Resolve(Type type, params (Type Type, object Instance)[] parameters)
        {
            return Instance.Resolve(type);
        }

        [Obsolete("Abdrakov.Container does not provide registrations/resolves by string name")]
        public object Resolve(Type type, string name)
        {
            return Instance.Resolve(type);
        }

        [Obsolete("Abdrakov.Container does not provide registrations/resolves by string name")]
        public object Resolve(Type type, string name, params (Type Type, object Instance)[] parameters)
        {
            return Instance.Resolve(type);
        }

        /// <summary>
        /// Creates a new Scope and provides the updated ServiceProvider
        /// </summary>
        /// <returns>A child <see cref="IUnityContainer" />.</returns>
        /// <remarks>
        /// This should be called by custom implementations that Implement IServiceScopeFactory
        /// </remarks>
        protected IScopedProvider CreateScopeInternal()
        {
            var child = Instance;
            _currentScope = new AbdrakovScopedProvider(child);
            return _currentScope;
        }

        private class AbdrakovScopedProvider : IScopedProvider
        {
            public AbdrakovScopedProvider(IAbdrakovContainer container)
            {
                Container = container;
            }

            public IAbdrakovContainer Container { get; private set; }
            public bool IsAttached { get; set; }
            public IScopedProvider CurrentScope => this;

            public IScopedProvider CreateScope() => this;

            public void Dispose()
            {
                Container.Dispose();
                Container = null;
            }

            public object Resolve(Type type) =>
                Resolve(type, Array.Empty<(Type, object)>());

            public object Resolve(Type type, string name) =>
                Resolve(type, name, Array.Empty<(Type, object)>());

            public object Resolve(Type type, params (Type Type, object Instance)[] parameters)
            {
                try
                {
                    // var overrides = parameters.Select(p => new DependencyOverride(p.Type, p.Instance)).ToArray();
                    return Container.Resolve(type);
                }
                catch (Exception ex)
                {
                    throw new ContainerResolutionException(type, ex);
                }
            }

            public object Resolve(Type type, string name, params (Type Type, object Instance)[] parameters)
            {
                try
                {
                    // Unity will simply return a new object() for unregistered Views
                    if (!Container.IsRegistered(type))
                        throw new KeyNotFoundException($"No registered type {type.Name} with the key {name}.");

                    // var overrides = parameters.Select(p => new DependencyOverride(p.Type, p.Instance)).ToArray();
                    return Container.Resolve(type);
                }
                catch (Exception ex)
                {
                    throw new ContainerResolutionException(type, name, ex);
                }
            }
        }
    }
}
