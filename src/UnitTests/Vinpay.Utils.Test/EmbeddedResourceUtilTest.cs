using System.Reflection;
using Vinpay.Utils.Reflection;

namespace Vinpay.Utils.Test;

[TestClass]
public class EmbeddedResourceUtilTest
{
    private const string EmbeddedResourcePath = "Resources.EmbeddedResourceTest.text1.txt";
    private const string EmbeddedResourceContent = "123ABC+-*/";

    private static Assembly TestAssembly => typeof(EmbeddedResourceUtilTest).Assembly;

    [TestMethod]
    public void GetEmbeddedText_WithEmbeddedResource_ReturnsExpectedText()
    {
        string result = EmbeddedResourceUtil.GetEmbeddedText(TestAssembly, EmbeddedResourcePath);

        Assert.AreEqual(EmbeddedResourceContent, result);
    }

    [TestMethod]
    public void GetEmbeddedText_WithNullEncoding_UsesUtf8ByDefault()
    {
        string result = EmbeddedResourceUtil.GetEmbeddedText(TestAssembly, EmbeddedResourcePath, null);

        Assert.AreEqual(EmbeddedResourceContent, result);
    }

    [TestMethod]
    [ExpectedException(typeof(FileNotFoundException))]
    public void GetEmbeddedText_WithMissingResource_ThrowsFileNotFoundException()
    {
        _ = EmbeddedResourceUtil.GetEmbeddedText(TestAssembly, "Resources.EmbeddedResourceTest.not-found.txt");
    }

    [TestMethod]
    [ExpectedException(typeof(FileNotFoundException))]
    public void GetEmbeddedText_WithEmptyRelativePath_ThrowsFileNotFoundException()
    {
        _ = EmbeddedResourceUtil.GetEmbeddedText(TestAssembly, string.Empty);
    }

    [TestMethod]
    public void OutputEmbeddedResource_WithEmbeddedResource_WritesExpectedFile()
    {
        string outputPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.txt");

        try
        {
            EmbeddedResourceUtil.OutputEmbeddedResource(TestAssembly, EmbeddedResourcePath, outputPath);

            Assert.IsTrue(File.Exists(outputPath));
            Assert.AreEqual(EmbeddedResourceContent, File.ReadAllText(outputPath));
        }
        finally
        {
            DeleteFileIfExists(outputPath);
        }
    }

    [TestMethod]
    public void OutputEmbeddedResource_WithExistingFile_OverwritesContent()
    {
        string outputPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.txt");

        try
        {
            File.WriteAllText(outputPath, "old-content");

            EmbeddedResourceUtil.OutputEmbeddedResource(TestAssembly, EmbeddedResourcePath, outputPath);

            Assert.AreEqual(EmbeddedResourceContent, File.ReadAllText(outputPath));
        }
        finally
        {
            DeleteFileIfExists(outputPath);
        }
    }

    [TestMethod]
    public void OutputEmbeddedResource_WithMissingResource_ThrowsNullReferenceException()
    {
        string outputPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.txt");

        try
        {
            Assert.ThrowsException<NullReferenceException>(() =>
                EmbeddedResourceUtil.OutputEmbeddedResource(TestAssembly, "Resources.EmbeddedResourceTest.not-found.txt", outputPath));
        }
        finally
        {
            DeleteFileIfExists(outputPath);
        }
    }

    private static void DeleteFileIfExists(string path)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
}