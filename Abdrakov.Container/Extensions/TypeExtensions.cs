using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Abdrakov.Container.Extensions
{
    public static class TypeExtensions
    {
        public static ConstructorInfo GetNormalConstructor(this Type mappedToType)
        {
            var parametrizedConstructor = mappedToType.GetTypeInfo().DeclaredConstructors.FirstOrDefault(x => x.GetParameters().Length > 0);
            // check if it exists
            parametrizedConstructor = parametrizedConstructor ?? mappedToType.GetTypeInfo().DeclaredConstructors.FirstOrDefault();
            return parametrizedConstructor;
        }
    }
}
