using Abdrakov.Container.Extensions;
using Abdrakov.Container.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Abdrakov.Container.Registration
{
    public class DefaultInstanceCreator : IInstanceCreator
    {
        public object CreateInstance(IContainerRegistration registration, IAbdrakovContainer container)
        {
            var instance = registration.MappedToType.GetNormalConstructor()?.Invoke(registration.InjectionMembers?.Select(x => container.Resolve(x)).ToArray());
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
                    var dep = container.Resolve(f.FieldType);
                    f.SetValue(instance, dep);
                }
            }
            // properties
            foreach (var p in instance.GetType().GetTypeInfo().DeclaredProperties)
            {
                var attrs = p.GetCustomAttributes(typeof(InjectionAttribute), false);
                if (attrs.Length > 0)
                {
                    var dep = container.Resolve(p.PropertyType);
                    p.SetValue(instance, dep, null);
                }
            }
        }
    }
}
