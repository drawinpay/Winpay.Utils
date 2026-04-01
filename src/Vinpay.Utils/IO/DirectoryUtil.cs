namespace Vinpay.Utils.IO
{
    /// <summary>
    /// A util class to deal with directory issues.
    /// </summary>
    public static class DirectoryUtil
    {
        /// <summary>
        /// Copy a directory to a specific destination directory path.
        /// </summary>
        /// <param name="sourceDir">The source directory path.</param>
        /// <param name="destinationDir">The destination directory path.</param>
        /// <param name="recursive">Is copy sub directory recursively</param>
        /// <exception cref="DirectoryNotFoundException"></exception>
        public static void CopyTo(this string sourceDir, string destinationDir, bool recursive = true)
        {
            if (!Directory.Exists(sourceDir))
            {
                throw new DirectoryNotFoundException($"Source directory not found: {sourceDir}");
            }

            var sourceDirInfo = new DirectoryInfo(sourceDir);
            sourceDirInfo.CopyTo(destinationDir, recursive);
        }
        
        /// <summary>
        /// Copy a directory to a specific destination directory path.
        /// </summary>
        /// <param name="sourceDir">The source directory info.</param>
        /// <param name="destinationDir">The destination directory path.</param>
        /// <param name="recursive">Is copy sub directory recursively</param>
        /// <exception cref="DirectoryNotFoundException"></exception>
        public static void CopyTo(this DirectoryInfo sourceDir, string destinationDir, bool recursive = true)
        {
            if (!sourceDir.Exists)
            {
                throw new DirectoryNotFoundException($"Source directory not found: {sourceDir.FullName}");
            }

            if (!Directory.Exists(destinationDir))
            {
                Directory.CreateDirectory(destinationDir);
            }

            foreach (FileInfo file in sourceDir.GetFiles())
            {
                string targetFilePath = Path.Combine(destinationDir, file.Name);
                file.CopyTo(targetFilePath, true);
            }

            if (!recursive) return;

            DirectoryInfo[] subDirs = sourceDir.GetDirectories();
            foreach (DirectoryInfo subDir in subDirs)
            {
                string subDestinationDir = Path.Combine(destinationDir, subDir.Name);
                subDir.CopyTo(subDestinationDir, recursive);
            }
        }
    }
}
