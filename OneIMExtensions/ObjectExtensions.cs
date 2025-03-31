#if NETFRAMEWORK
using Newtonsoft.Json;
#elif NET
using System.Text.Json;
#endif

namespace OneIMExtensions
{
    public static class ObjectExtensions
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
#if NETFRAMEWORK
        public static string ConvertObjectToJson<T>(this T x) => JsonConvert.SerializeObject(x, Formatting.Indented);
#elif NET
        public static string ConvertObjectToJson<T>(this T x) => JsonSerializer.Serialize(x, new JsonSerializerOptions { WriteIndented = true });
#endif

        /// <summary>
        /// Poor man's Dump(), pale in comparison to LinqPad's ;)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="x"></param>
        public static void DumpObjectAsJson<T>(this T x)
        {
            Console.WriteLine(ConvertObjectToJson(x));
        }
    }
}
