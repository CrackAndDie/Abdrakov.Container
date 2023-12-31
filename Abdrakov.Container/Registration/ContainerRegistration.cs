using Abdrakov.Container.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Abdrakov.Container.Registration
{
    public class ContainerRegistration : IContainerRegistration
    {
        public Type RegisteredType { get; set; }

        public Type MappedToType { get; set; }

        public object Instance { get; set; }

        public string Name { get; set; }

        public ParameterInfo[] InjectionMembers { get; set; }

        public RegistrationType RegistrationType { get; set; }

        public override string ToString()
        {
            return $"Reg: {RegisteredType}, Map: {MappedToType}, Type: {RegistrationType}";
        }
    }
}
