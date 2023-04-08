using OneIMExtensions;
using OneIMExtensions.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypedWrapperExensions;
using VI.DB;
using VI.DB.Entities;
using VI.DB.Model;
using VI.DB.Sync;

namespace ExtensionTester
{
    internal class Tester
    {

        /// <summary>
        /// Run a test.
        /// 
        /// If you are using this, consider using git update-index --assume-unchanged
        /// </summary>
        /// <param name="session"></param>
        internal static void RunTest(ISession session)
        {
            var ce = session.Source().Get<DialogDatabase>(Query.From("DialogDatabase").SelectNone());
            ce.EditionVersion.DumpObjectAsJson();

            var pe = session.Source().Get(Query.From("Person").SelectNone());
            pe.DumpObjectAsJson();

            var p = new Person();
            p.Entity = pe;

            p.GetDepartment(session).DumpObjectAsJson();
        }
    }
}
