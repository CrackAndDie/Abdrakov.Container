﻿using Abdrakov.Container.Interfaces;
using Abdrakov.Container.Registration;
using System;

namespace Abdrakov.Container.Extensions
{
    public static class ContainerRegistrationExtensions
    {
        public static object GetInstance(this IContainerRegistration registration, IAbdrakovContainer container, bool withInjections = true)
        {
            if (registration.Instance != null && registration.RegistrationType == RegistrationType.Instance)
            {
                // all the injections should be resolved in the instance on its first resolve
                if (container.InstanceCreator.RequiresInjections(registration.Instance) && withInjections)
                {
                    container.InstanceCreator.ResolveInjections(registration.Instance, container);
                }
                if (registration.Instance is IRequireInjection reqInj2) reqInj2.OnResolveReady(); // callback
                return registration.Instance;
            }

            var instance = container.InstanceCreator.CreateInstance(registration, container, withInjections);
            if (registration.RegistrationType == RegistrationType.Instance)
            {
                registration.Instance = instance;
            }
            if (instance is IRequireInjection reqInj) reqInj.OnResolveReady(); // callback
            return instance;
        }

        public static Func<IAbdrakovContainer, Type, object> GetFunc(this IContainerRegistration registration)
        {
            return registration.Instance as Func<IAbdrakovContainer, Type, object>;
        }
    }
}
