using System.Diagnostics;

namespace Abdrakov.Container.Tests
{
    internal class ConstructorInjectionsTest
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

        private class NormalClass
        {
            private InjectedClass _testClass;
            private int _a;
            private string _b;

            public NormalClass(InjectedClass testClass, int a, string b = "awd")
            {
                _testClass = testClass;
                _a = a;
                _b = b;
            }

            internal void Test()
            {
                _testClass.A = _a;
                _testClass.B = _b;
                Debug.Assert(_testClass.A == _a);
                Debug.Assert(_testClass.B == _b);
            }
        }
    }
}
