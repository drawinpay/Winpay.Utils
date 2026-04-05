using System.Reflection;
using Winpay.Utils.Reflection;

namespace Winpay.Utils.Test;

[TestClass]
public class AssemblyUtilTest
{
    [TestMethod]
    public void GetAssemblyDirTest()
    {
        string expextedResult = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
        string assemblyDir = AssemblyUtil.GetAssemblyDir(Assembly.GetExecutingAssembly());

        Assert.IsNotNull(assemblyDir);
        Assert.AreEqual(expextedResult, assemblyDir);
    }

    [TestMethod]
    public void GetCurrentAssemblyDirTest()
    {
        string assemblyDir = AssemblyUtil.GetCurrentAssemblyDir();
        string expextedResult = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;

        Assert.IsNotNull(assemblyDir);
        Assert.AreEqual(expextedResult, assemblyDir);
    }

    [TestMethod]
    public void GetEntryDirTest()
    {
        string expectedResult = Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)!;
        string entryDir = AssemblyUtil.GetEntryDir();

        Assert.IsNotNull(entryDir);
        Assert.AreEqual(expectedResult, entryDir);
    }
}
