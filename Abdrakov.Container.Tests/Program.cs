namespace Abdrakov.Container.Tests
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            InheritedInjectionsTest.Run();
            Console.WriteLine("InheritedInjectionsTest passed");
            ConstructorInjectionsTest.Run();
            Console.WriteLine("ConstructorInjectionsTest passed");
            Console.ReadKey();
        }
    }
}
