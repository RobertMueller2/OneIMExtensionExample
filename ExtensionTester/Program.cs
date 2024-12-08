using OneIMExtensions.Utils;
using System;

namespace ExtensionTester
{
    class Program
    {
        /// <summary>
        /// Starts the ExtensionTester
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Utils.SetAssemblyResolve();
            var logger = Utils.Logger.Value;
            var session = Utils.GetDefaultOneIMSession();

            Tester.RunTest(session);

            Console.Write("Press any key to continue... ");
            Console.ReadKey();
        }
    }
}
