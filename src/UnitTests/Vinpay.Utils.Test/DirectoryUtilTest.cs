using Vinpay.Utils.IO;
using Vinpay.Utils.Reflection;

namespace Vinpay.Utils.Test;

[TestClass]
public class DirectoryUtilTest
{
    [TestMethod]
    public void DirCopyToRecursivelyTest()
    {
        string destinationDir = Path.Combine(Environment.GetEnvironmentVariable("Temp")!, $"DirTest_{DateTime.Now.ToString("yyyyMMddHHmmss")}");

        try
        {
            string currentDir = AssemblyUtil.GetCurrentAssemblyDir();
            string sourceDir = Path.Combine(currentDir, "Resources", "DirectoryTest");
            DirectoryUtil.CopyTo(sourceDir, destinationDir, true);

            DirectoryInfo dirInfo = new DirectoryInfo(destinationDir);
            Assert.IsTrue(dirInfo.Exists);

            var files = dirInfo.GetFiles();
            Assert.AreEqual(1, files.Length);
            Assert.AreEqual("file1.bin", files[0].Name);

            var subDirs = dirInfo.GetDirectories();
            Assert.AreEqual(1, subDirs.Length);
            Assert.AreEqual("SubDir", subDirs[0].Name);

            var subFiles = subDirs[0].GetFiles();
            Assert.AreEqual(1, subFiles.Length);
            Assert.AreEqual("file2.bin", subFiles[0].Name);

            var subSubDirs = subDirs[0].GetDirectories();
            Assert.AreEqual(0, subSubDirs.Length);
        }
        finally
        {
            Directory.Delete(destinationDir, true);
        }
    }

    [TestMethod]
    public void DirInfoCopyToRecursivelyTest()
    {
        string destinationDir = Path.Combine(Environment.GetEnvironmentVariable("Temp")!, $"DirInfoTest_{DateTime.Now.ToString("yyyyMMddHHmmss")}");

        try
        {
            string currentDir = AssemblyUtil.GetCurrentAssemblyDir();
            string sourceDir = Path.Combine(currentDir, "Resources", "DirectoryTest");
            DirectoryInfo sourceDirInfo = new DirectoryInfo(sourceDir);
            sourceDirInfo.CopyTo(destinationDir, true);

            DirectoryInfo dirInfo = new DirectoryInfo(destinationDir);
            Assert.IsTrue(dirInfo.Exists);

            var files = dirInfo.GetFiles();
            Assert.AreEqual(1, files.Length);
            Assert.AreEqual("file1.bin", files[0].Name);

            var subDirs = dirInfo.GetDirectories();
            Assert.AreEqual(1, subDirs.Length);
            Assert.AreEqual("SubDir", subDirs[0].Name);

            var subFiles = subDirs[0].GetFiles();
            Assert.AreEqual(1, subFiles.Length);
            Assert.AreEqual("file2.bin", subFiles[0].Name);

            var subSubDirs = subDirs[0].GetDirectories();
            Assert.AreEqual(0, subSubDirs.Length);
        }
        finally
        {
            Directory.Delete(destinationDir, true);
        }
    }

    [TestMethod]
    public void DirCopyToNotRecursivelyTest()
    {
        string destinationDir = Path.Combine(Environment.GetEnvironmentVariable("Temp")!, $"DirTest_NoRecursive_{DateTime.Now.ToString("yyyyMMddHHmmss")}");

        try
        {
            string currentDir = AssemblyUtil.GetCurrentAssemblyDir();
            string sourceDir = Path.Combine(currentDir, "Resources", "DirectoryTest");
            DirectoryUtil.CopyTo(sourceDir, destinationDir, false);

            DirectoryInfo dirInfo = new DirectoryInfo(destinationDir);
            Assert.IsTrue(dirInfo.Exists);

            var files = dirInfo.GetFiles();
            Assert.AreEqual(1, files.Length);
            Assert.AreEqual("file1.bin", files[0].Name);

            var subDirs = dirInfo.GetDirectories();
            Assert.AreEqual(0, subDirs.Length);
        }
        finally
        {
            Directory.Delete(destinationDir, true);
        }
    }

    [TestMethod]
    public void DirInfoCopyToNotRecursivelyTest()
    {
        string destinationDir = Path.Combine(Environment.GetEnvironmentVariable("Temp")!, $"DirInfoTest_NoRecursive_{DateTime.Now.ToString("yyyyMMddHHmmss")}");

        try
        {
            string currentDir = AssemblyUtil.GetCurrentAssemblyDir();
            string sourceDir = Path.Combine(currentDir, "Resources", "DirectoryTest");
            DirectoryInfo sourceDirInfo = new DirectoryInfo(sourceDir);
            sourceDirInfo.CopyTo(destinationDir, false);

            DirectoryInfo dirInfo = new DirectoryInfo(destinationDir);
            Assert.IsTrue(dirInfo.Exists);

            var files = dirInfo.GetFiles();
            Assert.AreEqual(1, files.Length);
            Assert.AreEqual("file1.bin", files[0].Name);

            var subDirs = dirInfo.GetDirectories();
            Assert.AreEqual(0, subDirs.Length);
        }
        finally
        {
            Directory.Delete(destinationDir, true);
        }
    }
}
