using Vinpay.Utils.IO;

namespace Vinpay.Utils.Test;

[TestClass]
public class FileNameValidatorTest
{
    [TestMethod]
    [DataRow("F/ileName", false)]
    [DataRow(@"Fi\leName", false)]
    [DataRow("Fil:eName", false)]
    [DataRow("File*Name", false)]
    [DataRow("FileN?ame", false)]
    [DataRow("FileNa\"me", false)]
    [DataRow("FileNam<e", false)]
    [DataRow("FileName>", false)]
    [DataRow("FileName|", false)]
    [DataRow("FileName", true)]
    public void IsFileLegalTest(string fileName, bool result)
    {
        bool output = fileName.IsLegal();
        Assert.AreEqual(result, output);
    }

    [TestMethod]
    [DataRow("F/ileName", "-", "F-ileName")]
    [DataRow(@"Fi\leName", "-", "Fi-leName")]
    [DataRow("Fil:eName", "-", "Fil-eName")]
    [DataRow("File*Name", "-", "File-Name")]
    [DataRow("FileN?ame", "-", "FileN-ame")]
    [DataRow("FileNa\"me", "-", "FileNa-me")]
    [DataRow("FileNam<e", "-", "FileNam-e")]
    [DataRow("FileName>", "-", "FileName-")]
    [DataRow("FileName|", "-", "FileName-")]
    [DataRow(@"/F\i:l*e?N<a>m.e|", "-", "-F-i-l-e-N-a-m.e-")]
    [DataRow("F/ileName", "-_", "F-_ileName")]
    [DataRow(@"Fi\leName", "-_", "Fi-_leName")]
    [DataRow("Fil:eName", "-_", "Fil-_eName")]
    [DataRow("File*Name", "-_", "File-_Name")]
    [DataRow("FileN?ame", "-_", "FileN-_ame")]
    [DataRow("FileNa\"me", "-_", "FileNa-_me")]
    [DataRow("FileNam<e", "-_", "FileNam-_e")]
    [DataRow("FileName>", "-_", "FileName-_")]
    [DataRow("FileName|", "-_", "FileName-_")]
    [DataRow(@"/F\i:l*e?N<a>m.e|", "-_", "-_F-_i-_l-_e-_N-_a-_m.e-_")]
    [DataRow("FileName", "", "FileName")]
    public void CorrectIllegalCharactersTest(string fileName, string alter, string result)
    {
        string output = fileName.CorrectIllegalCharacters(alter);
        Assert.AreEqual(result, output);
    }
}
