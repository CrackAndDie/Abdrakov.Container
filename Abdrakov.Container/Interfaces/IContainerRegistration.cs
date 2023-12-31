﻿using Abdrakov.Container.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Abdrakov.Container.Interfaces
{
    public interface IContainerRegistration
    {
        Type RegisteredType { get; }
        Type MappedToType { get; }
        object Instance { get; set; }

        string Name { get; set; }

        ParameterInfo[] InjectionMembers { get; set; }
        RegistrationType RegistrationType { get; }
    }
}
