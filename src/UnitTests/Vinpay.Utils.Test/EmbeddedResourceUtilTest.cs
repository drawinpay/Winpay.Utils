using Vinpay.Utils.Reflection;

namespace Vinpay.Utils.Test;

[TestClass]
public class EmbeddedResourceUtilTest
{
    private const string EmbeddedResourcePath = "Resources.EmbeddedResourceTest.text1.txt";

    [TestMethod]
    public void GetEmbeddedText_WithEmbeddedResource_ReturnsExpectedText()
    {
        var assembly = typeof(EmbeddedResourceUtilTest).Assembly;

        string result = EmbeddedResourceUtil.GetEmbeddedText(assembly, EmbeddedResourcePath);

        Assert.AreEqual("123ABC+-*/", result);
    }

    [TestMethod]
    public void GetEmbeddedText_WithMissingResource_ThrowsFileNotFoundException()
    {
        var assembly = typeof(EmbeddedResourceUtilTest).Assembly;

        Assert.ThrowsException<FileNotFoundException>(() =>
            EmbeddedResourceUtil.GetEmbeddedText(assembly, "Resources.EmbeddedResourceTest.not-found.txt"));
    }

    [TestMethod]
    public void OutputEmbeddedResource_WithEmbeddedResource_WritesExpectedFile()
    {
        var assembly = typeof(EmbeddedResourceUtilTest).Assembly;
        string outputPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.txt");

        try
        {
            EmbeddedResourceUtil.OutputEmbeddedResource(assembly, EmbeddedResourcePath, outputPath);

            Assert.IsTrue(File.Exists(outputPath));
            Assert.AreEqual("123ABC+-*/", File.ReadAllText(outputPath));
        }
        finally
        {
            if (File.Exists(outputPath))
            {
                File.Delete(outputPath);
            }
        }
    }
}