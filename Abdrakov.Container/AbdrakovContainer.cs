using Abdrakov.Container.Extensions;
using Abdrakov.Container.Interfaces;
using Abdrakov.Container.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Abdrakov.Container
{
    public class AbdrakovContainer : IAbdrakovContainer
    {
        public IList<IContainerRegistration> Registrations { get; set; } = new List<IContainerRegistration>();

        public IInstanceCreator InstanceCreator { get; set; } = new DefaultInstanceCreator();

        public void SetInstanceCreator(IInstanceCreator creator)
        {
            InstanceCreator = creator;
        }

        public bool IsRegistered(Type type)
        {
            return IsRegistered(type, out IContainerRegistration _);
        }

        public bool IsRegistered(Type type, out IContainerRegistration registration)
        {
            registration = Registrations.FirstOrDefault(x => x.RegisteredType == type);
            return registration != null;
        }

        public IAbdrakovContainer RegisterFactory(Type type, Func<IAbdrakovContainer, Type, object> factory)
        {
            var registration = new ContainerRegistration()
            {
                RegisteredType = type,
                MappedToType = factory.GetType(),
                Instance = factory,
                RegistrationType = RegistrationType.Func,
            };
            AddOrReplace(registration);
            return this;
        }

        public IAbdrakovContainer RegisterInstance(Type type, object instance)
        {
            var registration = new ContainerRegistration()
            {
                RegisteredType = type,
                MappedToType = instance.GetType(),
                Instance = instance,
                RegistrationType = RegistrationType.Instance,
            };
            AddOrReplace(registration);
            return this;
        }

        public IAbdrakovContainer RegisterType(Type registeredType, Type mappedToType, bool isSingleton = false)
        {
            var parametrizedConstructor = mappedToType.GetNormalConstructor();
            Type[] injectionMembers = parametrizedConstructor.GetParameters().Select(x => x.ParameterType).ToArray();

            var registration = new ContainerRegistration()
            {
                RegisteredType = registeredType,
                MappedToType = mappedToType,
                Instance = null,
                RegistrationType = isSingleton ? RegistrationType.Instance : RegistrationType.Type,
                InjectionMembers = injectionMembers,
            };
            AddOrReplace(registration);
            return this;
        }

        public object Resolve(Type type)
        {
            return Resolve(type, true);
        }

        public object Resolve(Type type, bool withInjections)
        {
            if (IsRegistered(type, out IContainerRegistration registration))
            {
                switch (registration.RegistrationType) 
                {
                    case RegistrationType.Type:
                    case RegistrationType.Instance:
                        {
                            return registration.GetInstance(this, withInjections);
                        }
                    case RegistrationType.Func:
                        {
                            return registration.GetFunc();
                        }
                }
            }
            // try to create it by my own
            if (type.IsClass)
            {
                var parametrizedConstructor = type.GetNormalConstructor();
                Type[] injectionMembers = parametrizedConstructor.GetParameters().Select(x => x.ParameterType).ToArray();
                var tempRegistration = new ContainerRegistration()
                {
                    RegisteredType = type,
                    MappedToType = type,
                    Instance = null,
                    RegistrationType = RegistrationType.Type,
                    InjectionMembers = injectionMembers,
                };
                return tempRegistration.GetInstance(this);
            }
            return null;
        }

        public void ResolveInjections(Type type)
        {
            if (IsRegistered(type, out IContainerRegistration registration))
            {
                switch (registration.RegistrationType)
                {
                    case RegistrationType.Type:
                    case RegistrationType.Instance:
                        {
                            registration.GetInstance(this, true);
                            break;
                        }
                    case RegistrationType.Func:
                        {
                            registration.GetFunc();
                            break;
                        }
                }
            }
        }

        public void Dispose()
        {
            
        }

        private int IndexOfType(Type type)
        {
            if (IsRegistered(type))
            {
                int index = Registrations.Select(x => x.RegisteredType).ToList().IndexOf(type);
                return index;
            }
            return -1;
        }

        private void AddOrReplace(IContainerRegistration registration)
        {
            int ind;
            if ((ind = IndexOfType(registration.RegisteredType)) != -1)
            {
                Registrations[ind] = registration;
            }
            else
            {
                Registrations.Add(registration);
            }
        }
    }
}
