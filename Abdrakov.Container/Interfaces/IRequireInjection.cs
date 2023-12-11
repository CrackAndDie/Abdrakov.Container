using System;
using System.Collections.Generic;
using System.Text;

namespace Abdrakov.Container.Interfaces
{
    public interface IRequireInjection
    {
        void OnInjectionsReady();
        void OnResolveReady();
    }
}
