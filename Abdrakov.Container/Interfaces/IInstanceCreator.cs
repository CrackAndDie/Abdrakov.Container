using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abdrakov.Container.Interfaces
{
    public interface IInstanceCreator
    {
        object CreateInstance(IContainerRegistration registration, IAbdrakovContainer container);
    }
}
