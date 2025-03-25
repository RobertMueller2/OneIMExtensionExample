using OneIMExtensions;
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
            Utils.Initialize();
            var logger = NLogExtensions.Logger.Value;            

            Tester.RunTest();

            Console.Write("Press any key to continue... ");
            Console.ReadKey();
        }
    }
}
