using System.Diagnostics;

namespace Abdrakov.Container.Tests
{
    internal class InheritedInjectionsTest
    {
        internal static void Run()
        {
            AbdrakovContainer container = new AbdrakovContainer();
            container.RegisterType(typeof(InjectedClass), typeof(InjectedClass), true);
            container.RegisterType(typeof(NormalClass), typeof(NormalClass), true);

            var nc = container.Resolve(typeof(NormalClass)) as NormalClass;
            nc.Test();
        }

        private class InjectedClass
        {
            internal int A { get; set; }
            internal string B { get; set; }
        }

        private class BaseClass
        {
            [Injection]
            protected InjectedClass TestClass { get; set; }
        }

        private class NormalClass : BaseClass
        {
            internal void Test()
            {
                TestClass.A = 1;
                TestClass.B = "awd";
                Debug.Assert(TestClass.A == 1);
                Debug.Assert(TestClass.B == "awd");
            }
        }
    }
}
