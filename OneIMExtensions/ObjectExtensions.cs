
namespace OneIMExtensions
{
    public static class ObjectExtensions
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static string ConvertObjectToJson<T>(this T x) => Newtonsoft.Json.JsonConvert.SerializeObject(x, Newtonsoft.Json.Formatting.Indented);

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
