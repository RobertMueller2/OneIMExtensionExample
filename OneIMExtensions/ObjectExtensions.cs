using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneIMExtensions
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// Poor man's Dump(), pale in comparison to LinqPad's ;)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="x"></param>
        public static void DumpObjectAsJson<T>(this T x)
        {
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(x, Newtonsoft.Json.Formatting.Indented);
            Console.WriteLine(json);
        }
    }
}
