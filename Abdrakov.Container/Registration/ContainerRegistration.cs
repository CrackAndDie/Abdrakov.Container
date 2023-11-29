using Abdrakov.Container.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abdrakov.Container.Registration
{
    public class ContainerRegistration : IContainerRegistration
    {
        public Type RegisteredType { get; set; }

        public Type MappedToType { get; set; }

        public object Instance { get; set; }

        public Type[] InjectionMembers { get; set; }

        public RegistrationType RegistrationType { get; set; }

        public bool IsFirstResolve { get; set; } = true;

        public override string ToString()
        {
            return $"Reg: {RegisteredType}, Map: {MappedToType}, Type: {RegistrationType}";
        }
    }
}
