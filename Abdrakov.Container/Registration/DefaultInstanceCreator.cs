using Abdrakov.Container.Extensions;
using Abdrakov.Container.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Abdrakov.Container.Registration
{
    public class DefaultInstanceCreator : IInstanceCreator
    {
        public object CreateInstance(IContainerRegistration registration, IAbdrakovContainer container, bool withInjections = true)
        {
            var instance = registration.MappedToType.GetNormalConstructor()?.Invoke(registration.InjectionMembers?.Select(x => container.Resolve(x)).ToArray());
            if (withInjections && RequiresInjections(instance))
                ResolveInjections(instance, container);
            return instance;
        }

        public void ResolveInjections(object instance, IAbdrakovContainer container)
        {
            // fields
            foreach (var f in instance.GetType().GetTypeInfo().DeclaredFields)
            {
                var attrs = f.GetCustomAttributes(typeof(InjectionAttribute), false);
                if (attrs.Length > 0)
                {
                    // recursion problem solver
                    if (f.GetValue(instance) != null)
                        continue;
                    var dep = container.Resolve(f.FieldType, false);
                    f.SetValue(instance, dep);
                    container.ResolveInjections(f.FieldType);
                }
            }
            // properties
            foreach (var p in instance.GetType().GetTypeInfo().DeclaredProperties)
            {
                var attrs = p.GetCustomAttributes(typeof(InjectionAttribute), false);
                if (attrs.Length > 0)
                {
                    // recursion problem solver
                    if (p.GetValue(instance) != null)
                        continue;
                    var dep = container.Resolve(p.PropertyType, false);
                    p.SetValue(instance, dep, null);
                    container.ResolveInjections(p.PropertyType);
                }
            }
            // callback
            if (instance is IRequireInjection reqInj)
            {
                reqInj.OnInjectionsReady();
            }
        }

        public bool RequiresInjections(object instance)
        {
            // fields
            foreach (var f in instance.GetType().GetTypeInfo().DeclaredFields)
            {
                var attrs = f.GetCustomAttributes(typeof(InjectionAttribute), false);
                if (attrs.Length > 0)
                {
                    if (f.GetValue(instance) == null)
                        return true;
                }
            }
            // properties
            foreach (var p in instance.GetType().GetTypeInfo().DeclaredProperties)
            {
                var attrs = p.GetCustomAttributes(typeof(InjectionAttribute), false);
                if (attrs.Length > 0)
                {
                    if (p.GetValue(instance) == null)
                        return true;
                }
            }
            return false;
        }
    }
}
