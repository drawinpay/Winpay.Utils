using System.Reflection;

namespace Winpay.Utils.Reflection
{
    /// <summary>
    /// Assembly util.
    /// </summary>
    public static class AssemblyUtil
    {
        /// <summary>
        /// Get the directory path of the specific assembly.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static string GetAssemblyDir(Assembly assembly)
        {
            string assemblyDir = Path.GetDirectoryName(assembly.Location)!;
            return assemblyDir;
        }

        /// <summary>
        /// Get the directory path of the assembly of the calling method.
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentAssemblyDir()
        {
            var callingAssembly = Assembly.GetCallingAssembly();
            return GetAssemblyDir(callingAssembly);
        }

        /// <summary>
        /// The the directory path of the entry executable.
        /// </summary>
        /// <returns></returns>
        public static string GetEntryDir()
        {
            string entryLocation = Assembly.GetEntryAssembly()!.Location;
            return Path.GetDirectoryName(entryLocation)!;
        }
    }
}
