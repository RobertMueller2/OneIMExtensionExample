using OneIMExtensions.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VI.DB;


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
