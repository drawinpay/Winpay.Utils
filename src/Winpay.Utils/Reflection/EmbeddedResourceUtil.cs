using System.Reflection;
using System.Text;

namespace Winpay.Utils.Reflection
{
    /// <summary>
    /// Provides utility methods for accessing and extracting embedded resources from assemblies.
    /// </summary>
    public class EmbeddedResourceUtil
    {
        /// <summary>
        /// Extracts an embedded resource from the specified assembly and writes it to a file at the given output path.
        /// </summary>
        /// <param name="assembly">The assembly containing the embedded resource.</param>
        /// <param name="relativePath">The relative path of the embedded resource within the assembly.</param>
        /// <param name="outputPath">The file path where the resource will be written.</param>
        public static void OutputEmbeddedResource(Assembly assembly, string relativePath, string outputPath)
        {
            string assemblyName = assembly.GetName().Name!;
            var resourceAbsolutePath = $"{assemblyName}.{relativePath}";
            Stream resourceStream = assembly.GetManifestResourceStream(resourceAbsolutePath)!;

            using FileStream fileStream = File.Create(outputPath);
            resourceStream.Seek(0, SeekOrigin.Begin);
            resourceStream.CopyTo(fileStream);
        }

        /// <summary>
        /// Retrieves the content of an embedded resource as a string from the specified assembly.
        /// </summary>
        /// <param name="assembly">The assembly containing the embedded resource.</param>
        /// <param name="relativePath">The relative path of the embedded resource within the assembly.</param>
        /// <param name="encoding">The character encoding to use when reading the resource. Defaults to UTF-8 if not specified.</param>
        /// <returns>The content of the embedded resource as a string.</returns>
        /// <exception cref="FileNotFoundException">Thrown when the specified embedded resource cannot be found.</exception>
        public static string GetEmbeddedText(Assembly assembly, string relativePath, Encoding? encoding = null)
        {
            // UTF8 as default encoding if not specified
            encoding ??= Encoding.UTF8;

            string assemblyName = assembly.GetName().Name!;
            Stream? stream = assembly.GetManifestResourceStream($"{assemblyName}.{relativePath}");
            if (stream == null)
            {
                throw new FileNotFoundException($"未找到嵌入资源：{relativePath}");
            }

            using (stream)
            {
                using (StreamReader reader = new StreamReader(stream, encoding))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
