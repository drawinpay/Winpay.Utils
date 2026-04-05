using Winpay.Utils.IO;
using Winpay.Utils.Reflection;

namespace Winpay.Utils.Test;

[TestClass]
public class DirectoryUtilTest
{
    private static string CreateTempDirectory(string prefix)
    {
        string path = Path.Combine(Path.GetTempPath(), $"{prefix}_{Guid.NewGuid():N}");
        Directory.CreateDirectory(path);
        return path;
    }

    private static void DeleteDirectoryIfExists(string path)
    {
        if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
        }
    }

    private static string GetResourceSourceDirectory()
    {
        string currentDir = AssemblyUtil.GetCurrentAssemblyDir();
        return Path.Combine(currentDir, "Resources", "DirectoryTest");
    }

    private static void AssertDirectoryTestResourceCopiedRecursively(string destinationDir)
    {
        DirectoryInfo dirInfo = new(destinationDir);
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

    private static void AssertDirectoryTestResourceCopiedNonRecursively(string destinationDir)
    {
        DirectoryInfo dirInfo = new(destinationDir);
        Assert.IsTrue(dirInfo.Exists);

        var files = dirInfo.GetFiles();
        Assert.AreEqual(1, files.Length);
        Assert.AreEqual("file1.bin", files[0].Name);
    
        var subDirs = dirInfo.GetDirectories();
        Assert.AreEqual(0, subDirs.Length);
    }

    private static string CreateCustomSourceDirectory(string rootDir)
    {
        string sourceDir = Path.Combine(rootDir, "Source");
        string subDir = Path.Combine(sourceDir, "SubDir");

        Directory.CreateDirectory(subDir);
        File.WriteAllText(Path.Combine(sourceDir, "root.txt"), "source-root");
        File.WriteAllText(Path.Combine(subDir, "nested.txt"), "source-nested");

        return sourceDir;
    }

    [TestMethod]
    public void CopyTo_StringSource_Recursive_CopiesStructureAndContents()
    {
        string destinationDir = Path.Combine(Path.GetTempPath(), $"DirTest_{Guid.NewGuid():N}");

        try
        {
            string sourceDir = GetResourceSourceDirectory();

            DirectoryUtil.CopyTo(sourceDir, destinationDir, true);

            AssertDirectoryTestResourceCopiedRecursively(destinationDir);
        }
        finally
        {
            DeleteDirectoryIfExists(destinationDir);
        }
    }

    [TestMethod]
    public void CopyTo_DirectoryInfoSource_Recursive_CopiesStructureAndContents()
    {
        string destinationDir = Path.Combine(Path.GetTempPath(), $"DirInfoTest_{Guid.NewGuid():N}");

        try
        {
            DirectoryInfo sourceDirInfo = new(GetResourceSourceDirectory());

            sourceDirInfo.CopyTo(destinationDir, true);

            AssertDirectoryTestResourceCopiedRecursively(destinationDir);
        }
        finally
        {
            DeleteDirectoryIfExists(destinationDir);
        }
    }

    [TestMethod]
    public void CopyTo_StringSource_NonRecursive_CopiesOnlyTopLevelFiles()
    {
        string destinationDir = Path.Combine(Path.GetTempPath(), $"DirTest_NoRecursive_{Guid.NewGuid():N}");

        try
        {
            string sourceDir = GetResourceSourceDirectory();

            DirectoryUtil.CopyTo(sourceDir, destinationDir, false);

            AssertDirectoryTestResourceCopiedNonRecursively(destinationDir);
        }
        finally
        {
            DeleteDirectoryIfExists(destinationDir);
        }
    }

    [TestMethod]
    public void CopyTo_DirectoryInfoSource_NonRecursive_CopiesOnlyTopLevelFiles()
    {
        string destinationDir = Path.Combine(Path.GetTempPath(), $"DirInfoTest_NoRecursive_{Guid.NewGuid():N}");

        try
        {
            DirectoryInfo sourceDirInfo = new(GetResourceSourceDirectory());

            sourceDirInfo.CopyTo(destinationDir, false);

            AssertDirectoryTestResourceCopiedNonRecursively(destinationDir);
        }
        finally
        {
            DeleteDirectoryIfExists(destinationDir);
        }
    }

    [TestMethod]
    public void CopyTo_StringSource_WhenSourceDirectoryDoesNotExist_ThrowsDirectoryNotFoundException()
    {
        string missingSourceDir = Path.Combine(Path.GetTempPath(), $"MissingSource_{Guid.NewGuid():N}");
        string destinationDir = Path.Combine(Path.GetTempPath(), $"MissingDest_{Guid.NewGuid():N}");

        try
        {
            Assert.ThrowsException<DirectoryNotFoundException>(() => DirectoryUtil.CopyTo(missingSourceDir, destinationDir));
        }
        finally
        {
            DeleteDirectoryIfExists(destinationDir);
        }
    }

    [TestMethod]
    public void CopyTo_DirectoryInfoSource_WhenSourceDirectoryDoesNotExist_ThrowsDirectoryNotFoundException()
    {
        string missingSourceDir = Path.Combine(Path.GetTempPath(), $"MissingSourceInfo_{Guid.NewGuid():N}");
        string destinationDir = Path.Combine(Path.GetTempPath(), $"MissingDestInfo_{Guid.NewGuid():N}");

        try
        {
            DirectoryInfo sourceDirInfo = new(missingSourceDir);

            Assert.ThrowsException<DirectoryNotFoundException>(() => sourceDirInfo.CopyTo(destinationDir));
        }
        finally
        {
            DeleteDirectoryIfExists(destinationDir);
        }
    }

    [TestMethod]
    public void CopyTo_StringSource_WhenDestinationAlreadyExists_OverwritesFilesAndCopiesSubdirectories()
    {
        string testRootDir = CreateTempDirectory("DirCopyExisting");

        try
        {
            string sourceDir = CreateCustomSourceDirectory(testRootDir);
            string destinationDir = Path.Combine(testRootDir, "Destination");
            string destinationSubDir = Path.Combine(destinationDir, "SubDir");

            Directory.CreateDirectory(destinationSubDir);
            File.WriteAllText(Path.Combine(destinationDir, "root.txt"), "old-root");
            File.WriteAllText(Path.Combine(destinationSubDir, "nested.txt"), "old-nested");

            DirectoryUtil.CopyTo(sourceDir, destinationDir, true);

            Assert.AreEqual("source-root", File.ReadAllText(Path.Combine(destinationDir, "root.txt")));
            Assert.AreEqual("source-nested", File.ReadAllText(Path.Combine(destinationSubDir, "nested.txt")));
        }
        finally
        {
            DeleteDirectoryIfExists(testRootDir);
        }
    }

    [TestMethod]
    public void CopyTo_DirectoryInfoSource_WhenDestinationAlreadyExists_OverwritesFilesAndCopiesSubdirectories()
    {
        string testRootDir = CreateTempDirectory("DirInfoCopyExisting");

        try
        {
            string sourceDir = CreateCustomSourceDirectory(testRootDir);
            string destinationDir = Path.Combine(testRootDir, "Destination");
            string destinationSubDir = Path.Combine(destinationDir, "SubDir");

            Directory.CreateDirectory(destinationSubDir);
            File.WriteAllText(Path.Combine(destinationDir, "root.txt"), "old-root");
            File.WriteAllText(Path.Combine(destinationSubDir, "nested.txt"), "old-nested");

            DirectoryInfo sourceDirInfo = new(sourceDir);
            sourceDirInfo.CopyTo(destinationDir, true);

            Assert.AreEqual("source-root", File.ReadAllText(Path.Combine(destinationDir, "root.txt")));
            Assert.AreEqual("source-nested", File.ReadAllText(Path.Combine(destinationSubDir, "nested.txt")));
        }
        finally
        {
            DeleteDirectoryIfExists(testRootDir);
        }
    }
}
