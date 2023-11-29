using Abdrakov.Container.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abdrakov.Container.Interfaces
{
    public interface IContainerRegistration
    {
        Type RegisteredType { get; }
        Type MappedToType { get; }
        object Instance { get; set; }

        Type[] InjectionMembers { get; set; }
        RegistrationType RegistrationType { get; }
        bool IsFirstResolve { get; set; }
    }
}
