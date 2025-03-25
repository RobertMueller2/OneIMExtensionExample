using NLog;
using System.Reflection;

namespace OneIMExtensions {
    public static class NLogExtensions {
        private static readonly string NLogConfig = $@"<nlog autoReload=""true"" xmlns=""http://www.nlog-project.org/schemas/NLog.xsd"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
    <variable name=""appName"" value=""OneIMExtensionExample"" />
    <include file=""{ExtensionSettings.OneIMBaseDir}\{ExtensionSettings.NLogFilename}"" ignoreErrors=""true"" />
</nlog>
";

        private static readonly string NLogConfigPath = string.Format(@"{0}\nlog.config", Path.GetDirectoryName(Assembly.GetAssembly(typeof(NLogExtensions))?.Location));

        /// <summary>
        /// Prepares an NLog.config that sets appName to OneIMExtensionExample and includes OneIMBaseDir's globallog.config
        /// </summary>
        public static void PrepareNLogConfig() {
            Console.WriteLine($"Writing nlog.config to {NLogConfigPath}");
            File.WriteAllText(NLogConfigPath, NLogConfig);
        }

        /// <summary>
        /// Prepares NLog.config and provides a logger. This requires a prepared nlog.config in the same directory as the dll.
        /// This is needed so an appName can be used with the default globallog.config.
        /// If this is no good, you can always use NLog's GetLogger directly, e.g. with a dedicated config.
        /// </summary>
        public static Lazy<Logger> Logger = new(() => {
            PrepareNLogConfig();
            var logger = LogManager.GetLogger("OneIMExtensionExample");
            Console.WriteLine($"Loading nlog.config from {NLogConfigPath}");
            LogManager.Configuration = new NLog.Config.XmlLoggingConfiguration(NLogConfigPath);
            return logger;
        });

        /// <summary>
        /// Gets a logger
        /// </summary>
        /// <returns></returns>
        public static Logger GetLogger() => Logger.Value;
    }
}
