using System;

using VI.DB;
using VI.DB.Auth;
using VI.DB.Entities;
using System.Reflection;
using System.IO;
using NLog;

namespace OneIMExtensions.Utils
{
    public static class Utils
    {

        private static readonly string NLogConfig = $@"<nlog autoReload=""true"" xmlns=""http://www.nlog-project.org/schemas/NLog.xsd"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
    <variable name=""appName"" value=""OneIMExtensionExample"" />
    <include file=""{ExtensionSettings.OneIMBaseDir}\globallog.config"" ignoreErrors=""true"" />
</nlog>
";

        private static readonly string NLogConfigPath = string.Format(@"{0}\nlog.config", Path.GetDirectoryName(Assembly.GetAssembly(typeof(Utils)).Location));

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
        public static Lazy<Logger> Logger = new Lazy<Logger>(() => {
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

        /// <summary>
        /// Opens a OneIM session with the given connnection string and authprops
        /// </summary>
        /// <param name="connString"></param>
        /// <param name="authProps"></param>
        /// <returns>A OneIM session</returns>
        public static ISession GetOneIMSession(string connString, IAuthProps authProps)
        {
            var connectData = DbApp.Instance.Connect(connString);
            connectData.Connection.Authenticate(authProps);

            return connectData.Connection.Session;
        }

        /// <summary>
        /// Opens a OneIM session with default DB connection String provided via project settings and 
        /// Account based system user
        /// </summary>
        /// <returns>A OneIM Session</returns>
        public static ISession GetDefaultOneIMSession() => GetOneIMSession(ExtensionSettings.OneIMDBConnString);

        /// <summary>
        /// Opens a OneIM session with the given connection string using Account Based System user auth module
        /// </summary>
        /// <param name="connString"></param>
        /// <returns>A OneIM session</returns>
        public static ISession GetOneIMSession(string connString)
        {
            IAuthProps authProps = new AuthProps("DialogUserAccountBased");
            return GetOneIMSession(connString, authProps);
        }

        /// <summary>
        /// Opens a OneIM session with the given connection string and DialogUser/Password combination
        /// </summary>
        /// <param name="connString"></param>
        /// <param name="dialogUser"></param>
        /// <param name="dialogPassword"></param>
        /// <returns>A OneIM session</returns>
        public static ISession GetOneIMSession(String connString, string dialogUser, string dialogPassword)
        {
            IAuthProps authProps = new AuthProps("DialogUser") {
                new AuthProp("User", AuthPropType.Edit, "VI.DB_USER", isMandatory: true, dialogUser),
                new AuthProp("Password", AuthPropType.Password, "VI.DB_Password", isMandatory: false, dialogPassword)
            };
            return GetOneIMSession(connString, authProps);
        }

        /// <summary>
        /// Provides an assembly resolver that includes the OneIMBaseDir defined in localsettings.props.
        /// The ExtensionTester uses these to resolve customizers. It can also be useful for REPL, LinqPad etc
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns>A resolved Assembly object</returns>
        public static Assembly ResolveOneIMBaseDirAssembly(object sender, ResolveEventArgs args) => ResolveAssembly(sender, args, ExtensionSettings.OneIMBaseDir);

        /// <summary>
        /// Provides an assembly resolver for a given assemblyDirectory
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <param name="assemblyDirectory"></param>
        /// <returns></returns>
        public static Assembly ResolveAssembly(object sender, ResolveEventArgs args, string assemblyDirectory)
        {
            string assemblyName = new AssemblyName(args.Name).Name;
            string assemblyPath = Path.Combine(assemblyDirectory, $"{assemblyName}.dll");

            return (File.Exists(assemblyPath)) ? Assembly.LoadFrom(assemblyPath) : null;
        }

        /// <summary>
        /// Sets Assembly Resolve to ResolveAssembly provided in Utils
        /// </summary>
        public static void SetAssemblyResolve()
        {
            AppDomain.CurrentDomain.AssemblyResolve += ResolveOneIMBaseDirAssembly;
        }
    }
}
