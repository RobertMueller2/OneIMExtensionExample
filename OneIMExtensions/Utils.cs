using VI.DB;
using VI.DB.Auth;
using VI.DB.Entities;
using System.Reflection;

namespace OneIMExtensions
{
    public static class Utils
    {
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
        public static ISession GetDefaultOneIMSession() => GetDialogUserAccountBasedOneIMSession(ExtensionSettings.OneIMDBConnString);

        /// <summary>
        /// Opens a OneIM session with the given connection string using Account Based System user auth module
        /// </summary>
        /// <param name="connString"></param>
        /// <returns>A OneIM session</returns>
        public static ISession GetDialogUserAccountBasedOneIMSession(string connString)
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

#if NETFRAMEWORK
        /// <summary>
        /// Provides an assembly resolver that includes the OneIMBaseDir defined in localsettings.props.
        /// The ExtensionTester uses these to resolve customizers. It can also be useful for REPL, LinqPad etc
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns>A resolved Assembly object</returns>
        public static Assembly? ResolveOneIMBaseDirAssembly(object sender, ResolveEventArgs args) => ResolveAssembly(sender, args, ExtensionSettings.OneIMBaseDir);


        /// <summary>
        /// Provides an assembly resolver for a given assemblyDirectory
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <param name="assemblyDirectory"></param>
        /// <returns></returns>
        public static Assembly? ResolveAssembly(object sender, ResolveEventArgs args, string assemblyDirectory)
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

#endif

#if NET

        /// <summary>
        /// initializes
        /// </summary>
        public static void CopyBinaries() {

            var logger = NLogExtensions.GetLogger();

            var binaries = new string[] {
                "AccountBasedUserAuthenticator",
                "AE.Controls",
                "Microsoft.Win32.SystemEvents",
                "System.Drawing.Common",
                "System.ServiceModel.Primitives",
                "VI.CommonDialogs"
            }.Select(x => string.Format("{0}\\{1}.dll", ExtensionSettings.OneIMBaseDir, x))
            .Union(Directory.GetFiles(ExtensionSettings.OneIMBaseDir, "*.Customizer.dll"));

            var wd = Path.GetDirectoryName(Assembly.GetAssembly(typeof(Utils))?.Location);
            // no reason this should be null, but why not ;)
            if (wd == null) {
                return;
            }

            foreach (var fn in binaries) {
                var tfn = Path.Combine(wd, Path.GetFileName(fn));
                if (File.Exists(tfn)) {
                    continue;
                }                
                logger.Trace(String.Format("Copying {0} to {1}", fn, tfn));
                File.Copy(fn, tfn);
            }
        }
#endif

        /// <summary>
        /// Sets assembly resolver (.NET Framework). For older OneIM versions, dynamic dependencies like AccountBasedSystemUser
        /// or customizers can be loaded that way.
        /// 
        /// This does not work with .NET. Of course, for each of the potential binaries we could add a NuGet package or reference.
        /// But this would make this DLL skeleton depend somewhat on the selected modules in OneIM. Also, it does not work well
        /// with LinqPad.
        /// </summary>
        public static void Initialize() {
#if NETFRAMEWORK
            SetAssemblyResolve();
#elif NET
            CopyBinaries();
#endif
        }
    }

}
