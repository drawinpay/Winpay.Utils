using Winpay.Utils.General;

namespace Winpay.Utils.Test;

/// <summary>
/// Unit tests for CmdParameterParser class
/// </summary>
[TestClass]
public class CmdArgsParserTest
{
    #region Parse(string input) Tests

    /// <summary>
    /// Test Parse with null input
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void Parse_String_NullInput_ThrowsException()
    {
        CmdArgsParser.Parse((string)null!);
    }

    /// <summary>
    /// Test Parse with empty string
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void Parse_String_EmptyString_ThrowsException()
    {
        CmdArgsParser.Parse("");
    }

    /// <summary>
    /// Test Parse with whitespace only
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void Parse_String_WhitespaceOnly_ThrowsException()
    {
        CmdArgsParser.Parse("   ");
    }

    /// <summary>
    /// Test Parse with tabs and spaces
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void Parse_String_TabsAndSpaces_ThrowsException()
    {
        CmdArgsParser.Parse("\t\t  \t");
    }

    /// <summary>
    /// Test Parse with single value without keys
    /// </summary>
    [TestMethod]
    public void Parse_String_SingleValueNoKeys_ReturnsDefaultKey()
    {
        var result = CmdArgsParser.Parse("value");

        Assert.AreEqual(1, result.Count);
        Assert.IsTrue(result.ContainsKey(""));
        CollectionAssert.AreEqual(new List<string> { "value" }, result[""]);
    }

    /// <summary>
    /// Test Parse with multiple values without keys
    /// </summary>
    [TestMethod]
    public void Parse_String_MultipleValuesNoKeys_ReturnsDefaultKey()
    {
        var result = CmdArgsParser.Parse("value1 value2 value3");

        Assert.AreEqual(1, result.Count);
        Assert.IsTrue(result.ContainsKey(""));
        CollectionAssert.AreEqual(new List<string> { "value1", "value2", "value3" }, result[""]);
    }

    /// <summary>
    /// Test Parse with single key without values
    /// </summary>
    [TestMethod]
    public void Parse_String_SingleKeyNoValues_ReturnsKeyWithEmptyList()
    {
        var result = CmdArgsParser.Parse("-key");

        Assert.AreEqual(1, result.Count);
        Assert.IsTrue(result.ContainsKey("key"));
        CollectionAssert.AreEqual(new List<string>(), result["key"]);
    }

    /// <summary>
    /// Test Parse with single key with single value
    /// </summary>
    [TestMethod]
    public void Parse_String_SingleKeySingleValue_ReturnsKeyWithValue()
    {
        var result = CmdArgsParser.Parse("-key value");

        Assert.AreEqual(1, result.Count);
        Assert.IsTrue(result.ContainsKey("key"));
        CollectionAssert.AreEqual(new List<string> { "value" }, result["key"]);
    }

    /// <summary>
    /// Test Parse with single key with multiple values
    /// </summary>
    [TestMethod]
    public void Parse_String_SingleKeyMultipleValues_ReturnsKeyWithValues()
    {
        var result = CmdArgsParser.Parse("-key value1 value2 value3");

        Assert.AreEqual(1, result.Count);
        Assert.IsTrue(result.ContainsKey("key"));
        CollectionAssert.AreEqual(new List<string> { "value1", "value2", "value3" }, result["key"]);
    }

    /// <summary>
    /// Test Parse with multiple keys each with single value
    /// </summary>
    [TestMethod]
    public void Parse_String_MultipleKeysSingleValues_ReturnsCorrectGroups()
    {
        var result = CmdArgsParser.Parse("-key1 value1 -key2 value2 -key3 value3");

        Assert.AreEqual(3, result.Count);
        CollectionAssert.AreEqual(new List<string> { "value1" }, result["key1"]);
        CollectionAssert.AreEqual(new List<string> { "value2" }, result["key2"]);
        CollectionAssert.AreEqual(new List<string> { "value3" }, result["key3"]);
    }

    /// <summary>
    /// Test Parse with multiple keys with varying number of values
    /// </summary>
    [TestMethod]
    public void Parse_String_MultipleKeysVaryingValues_ReturnsCorrectGroups()
    {
        var result = CmdArgsParser.Parse("-key1 value1 -key2 value2 value3 -key3");

        Assert.AreEqual(3, result.Count);
        CollectionAssert.AreEqual(new List<string> { "value1" }, result["key1"]);
        CollectionAssert.AreEqual(new List<string> { "value2", "value3" }, result["key2"]);
        CollectionAssert.AreEqual(new List<string>(), result["key3"]);
    }

    /// <summary>
    /// Test Parse with default values before first key
    /// </summary>
    [TestMethod]
    public void Parse_String_DefaultValuesBeforeKey_ReturnsDefaultKeyAndKey()
    {
        var result = CmdArgsParser.Parse("default1 default2 -key value");

        Assert.AreEqual(2, result.Count);
        CollectionAssert.AreEqual(new List<string> { "default1", "default2" }, result[""]);
        CollectionAssert.AreEqual(new List<string> { "value" }, result["key"]);
    }

    /// <summary>
    /// Test Parse with default values after key
    /// </summary>
    [TestMethod]
    public void Parse_String_DefaultValuesAfterKey_ReturnsKeyAndDefaultKey()
    {
        var result = CmdArgsParser.Parse("-key value default1 default2");

        Assert.AreEqual(1, result.Count);
        CollectionAssert.AreEqual(new List<string> { "value", "default1", "default2" }, result["key"]);
        Assert.IsFalse(result.ContainsKey(""));
    }

    /// <summary>
    /// Test Parse with default values before and after keys
    /// </summary>
    [TestMethod]
    public void Parse_String_DefaultValuesBeforeAndAfterKeys_ReturnsCorrectGroups()
    {
        var result = CmdArgsParser.Parse("default1 -key1 value1 default2 -key2 value2 default3");

        Assert.AreEqual(3, result.Count);
        CollectionAssert.AreEqual(new List<string> { "default1" }, result[""]);
        CollectionAssert.AreEqual(new List<string> { "value1", "default2" }, result["key1"]);
        CollectionAssert.AreEqual(new List<string> { "value2", "default3" }, result["key2"]);
    }

    /// <summary>
    /// Test Parse with complex command line
    /// </summary>
    [TestMethod]
    public void Parse_String_ComplexCommandLine_ReturnsCorrectGroups()
    {
        var result = CmdArgsParser.Parse("input.txt output.txt -config debug verbose -file file1.txt file2.txt -mode fast");

        Assert.AreEqual(4, result.Count);
        CollectionAssert.AreEqual(new List<string> { "input.txt", "output.txt" }, result[""]);
        CollectionAssert.AreEqual(new List<string> { "debug", "verbose" }, result["config"]);
        CollectionAssert.AreEqual(new List<string> { "file1.txt", "file2.txt" }, result["file"]);
        CollectionAssert.AreEqual(new List<string> { "fast" }, result["mode"]);
    }

    /// <summary>
    /// Test Parse with extra spaces
    /// </summary>
    [TestMethod]
    public void Parse_String_ExtraSpaces_ReturnsCorrectGroups()
    {
        var result = CmdArgsParser.Parse("  default1   default2   -key   value1   value2  ");

        Assert.AreEqual(2, result.Count);
        CollectionAssert.AreEqual(new List<string> { "default1", "default2" }, result[""]);
        CollectionAssert.AreEqual(new List<string> { "value1", "value2" }, result["key"]);
    }

    /// <summary>
    /// Test Parse with key starting with single dash
    /// </summary>
    [TestMethod]
    public void Parse_String_SingleDashKey_RemovesDashFromKey()
    {
        var result = CmdArgsParser.Parse("-key value");

        Assert.AreEqual(1, result.Count);
        Assert.IsTrue(result.ContainsKey("key"));
    }

    /// <summary>
    /// Test Parse with key starting with double dash
    /// </summary>
    [TestMethod]
    public void Parse_String_DoubleDashKey_RemovesFirstDashFromKey()
    {
        var result = CmdArgsParser.Parse("--key value");

        Assert.AreEqual(1, result.Count);
        Assert.IsTrue(result.ContainsKey("-key"));
        CollectionAssert.AreEqual(new List<string> { "value" }, result["-key"]);
    }

    /// <summary>
    /// Test Parse with consecutive keys
    /// </summary>
    [TestMethod]
    public void Parse_String_ConsecutiveKeys_ReturnsKeysWithEmptyLists()
    {
        var result = CmdArgsParser.Parse("-key1 -key2 -key3");

        Assert.AreEqual(3, result.Count);
        CollectionAssert.AreEqual(new List<string>(), result["key1"]);
        CollectionAssert.AreEqual(new List<string>(), result["key2"]);
        CollectionAssert.AreEqual(new List<string>(), result["key3"]);
    }

    /// <summary>
    /// Test Parse with empty strings in input (should be removed by Split)
    /// </summary>
    [TestMethod]
    public void Parse_String_WithEmptyEntries_RemovesEmptyEntries()
    {
        var result = CmdArgsParser.Parse("value1  value2   -key value3");

        Assert.AreEqual(2, result.Count);
        CollectionAssert.AreEqual(new List<string> { "value1", "value2" }, result[""]);
        CollectionAssert.AreEqual(new List<string> { "value3" }, result["key"]);
    }

    #endregion

    #region Parse(string[] args) Tests

    /// <summary>
    /// Test Parse with null array
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void Parse_Array_NullInput_ThrowsException()
    {
        CmdArgsParser.Parse((string[])null!);
    }

    /// <summary>
    /// Test Parse with empty array
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void Parse_Array_EmptyArray_ThrowsException()
    {
        CmdArgsParser.Parse(Array.Empty<string>());
    }

    /// <summary>
    /// Test Parse with single element array without keys
    /// </summary>
    [TestMethod]
    public void Parse_Array_SingleElementNoKey_ReturnsDefaultKey()
    {
        var result = CmdArgsParser.Parse(new[] { "value" });

        Assert.AreEqual(1, result.Count);
        CollectionAssert.AreEqual(new List<string> { "value" }, result[""]);
    }

    /// <summary>
    /// Test Parse with multiple elements array without keys
    /// </summary>
    [TestMethod]
    public void Parse_Array_MultipleElementsNoKeys_ReturnsDefaultKey()
    {
        var result = CmdArgsParser.Parse(new[] { "value1", "value2", "value3" });

        Assert.AreEqual(1, result.Count);
        CollectionAssert.AreEqual(new List<string> { "value1", "value2", "value3" }, result[""]);
    }

    /// <summary>
    /// Test Parse with single key at start
    /// </summary>
    [TestMethod]
    public void Parse_Array_SingleKeyAtStart_ReturnsKeyWithValues()
    {
        var result = CmdArgsParser.Parse(new[] { "-key", "value1", "value2" });

        Assert.AreEqual(1, result.Count);
        CollectionAssert.AreEqual(new List<string> { "value1", "value2" }, result["key"]);
    }

    /// <summary>
    /// Test Parse with key in middle of array
    /// </summary>
    [TestMethod]
    public void Parse_Array_KeyInMiddle_ReturnsDefaultKeyAndKey()
    {
        var result = CmdArgsParser.Parse(new[] { "default1", "-key", "value1", "default2" });

        Assert.AreEqual(2, result.Count);
        CollectionAssert.AreEqual(new List<string> { "default1" }, result[""]);
        CollectionAssert.AreEqual(new List<string> { "value1", "default2" }, result["key"]);
    }

    /// <summary>
    /// Test Parse with multiple keys
    /// </summary>
    [TestMethod]
    public void Parse_Array_MultipleKeys_ReturnsCorrectGroups()
    {
        var result = CmdArgsParser.Parse(new[] { "-key1", "value1", "value2", "-key2", "value3", "-key3" });

        Assert.AreEqual(3, result.Count);
        CollectionAssert.AreEqual(new List<string> { "value1", "value2" }, result["key1"]);
        CollectionAssert.AreEqual(new List<string> { "value3" }, result["key2"]);
        CollectionAssert.AreEqual(new List<string>(), result["key3"]);
    }

    /// <summary>
    /// Test Parse with key at end of array
    /// </summary>
    [TestMethod]
    public void Parse_Array_KeyAtEnd_ReturnsCorrectGroups()
    {
        var result = CmdArgsParser.Parse(new[] { "-key1", "value1", "-key2" });

        Assert.AreEqual(2, result.Count);
        CollectionAssert.AreEqual(new List<string> { "value1" }, result["key1"]);
        CollectionAssert.AreEqual(new List<string>(), result["key2"]);
    }

    /// <summary>
    /// Test Parse with default values only
    /// </summary>
    [TestMethod]
    public void Parse_Array_OnlyDefaultValues_ReturnsDefaultKey()
    {
        var result = CmdArgsParser.Parse(new[] { "default1", "default2", "default3" });

        Assert.AreEqual(1, result.Count);
        CollectionAssert.AreEqual(new List<string> { "default1", "default2", "default3" }, result[""]);
    }

    /// <summary>
    /// Test Parse with no default values
    /// </summary>
    [TestMethod]
    public void Parse_Array_NoDefaultValues_ReturnsKeysOnly()
    {
        var result = CmdArgsParser.Parse(new[] { "-key1", "value1", "-key2", "value2" });

        Assert.AreEqual(2, result.Count);
        Assert.IsFalse(result.ContainsKey(""));
        CollectionAssert.AreEqual(new List<string> { "value1" }, result["key1"]);
        CollectionAssert.AreEqual(new List<string> { "value2" }, result["key2"]);
    }

    /// <summary>
    /// Test Parse with key that has no values
    /// </summary>
    [TestMethod]
    public void Parse_Array_KeyNoValues_ReturnsEmptyList()
    {
        var result = CmdArgsParser.Parse(new[] { "-key1", "value1", "-key2", "-key3", "value3" });

        Assert.AreEqual(3, result.Count);
        CollectionAssert.AreEqual(new List<string> { "value1" }, result["key1"]);
        CollectionAssert.AreEqual(new List<string>(), result["key2"]);
        CollectionAssert.AreEqual(new List<string> { "value3" }, result["key3"]);
    }

    /// <summary>
    /// Test Parse with many consecutive keys
    /// </summary>
    [TestMethod]
    public void Parse_Array_ManyConsecutiveKeys_ReturnsEmptyLists()
    {
        var result = CmdArgsParser.Parse(new[] { "-key1", "-key2", "-key3", "-key4", "-key5" });

        Assert.AreEqual(5, result.Count);
        for (int i = 1; i <= 5; i++)
        {
            CollectionAssert.AreEqual(new List<string>(), result[$"key{i}"]);
        }
    }

    /// <summary>
    /// Test Parse with mixed default values and keys
    /// </summary>
    [TestMethod]
    public void Parse_Array_MixedDefaultsAndKeys_ReturnsCorrectGroups()
    {
        var result = CmdArgsParser.Parse(new[] { "d1", "-k1", "v1", "d2", "-k2", "-k3", "v2", "d3" });

        Assert.AreEqual(4, result.Count);
        CollectionAssert.AreEqual(new List<string> { "d1" }, result[""]);
        CollectionAssert.AreEqual(new List<string> { "v1", "d2" }, result["k1"]);
        CollectionAssert.AreEqual(new List<string> { "v2", "d3" }, result["k3"]);
        CollectionAssert.AreEqual(new List<string>(), result["k2"]);
    }

    /// <summary>
    /// Test Parse with double dash keys
    /// </summary>
    [TestMethod]
    public void Parse_Array_DoubleDashKeys_PreservesOneDash()
    {
        var result = CmdArgsParser.Parse(new[] { "--verbose", "--output", "file.txt" });

        Assert.AreEqual(2, result.Count);
        CollectionAssert.AreEqual(new List<string>(), result["-verbose"]);
        CollectionAssert.AreEqual(new List<string> { "file.txt" }, result["-output"]);
    }

    /// <summary>
    /// Test Parse with single and double dash keys mixed
    /// </summary>
    [TestMethod]
    public void Parse_Array_MixedDashKeys_ReturnsCorrectKeys()
    {
        var result = CmdArgsParser.Parse(new[] { "-v", "--verbose", "true", "-o", "output.txt" });

        Assert.AreEqual(3, result.Count);
        CollectionAssert.AreEqual(new List<string>(), result["v"]);
        CollectionAssert.AreEqual(new List<string> { "true" }, result["-verbose"]);
        CollectionAssert.AreEqual(new List<string> { "output.txt" }, result["o"]);
    }

    /// <summary>
    /// Test Parse preserves order of keys
    /// </summary>
    [TestMethod]
    public void Parse_Array_KeysPreserveInsertionOrder()
    {
        var result = CmdArgsParser.Parse(new[] { "-key3", "-key1", "-key2" });

        var keys = result.Keys.ToList();
        Assert.AreEqual("key3", keys[0]);
        Assert.AreEqual("key1", keys[1]);
        Assert.AreEqual("key2", keys[2]);
    }

    /// <summary>
    /// Test Parse with values that look like keys are treated as keys
    /// </summary>
    [TestMethod]
    public void Parse_Array_ValuesLookLikeKeys_TreatedAsKeys()
    {
        var result = CmdArgsParser.Parse(new[] { "-key", "-value" });

        Assert.AreEqual(2, result.Count);
        CollectionAssert.AreEqual(new List<string>(), result["key"]);
        CollectionAssert.AreEqual(new List<string>(), result["value"]);
    }

    /// <summary>
    /// Test Parse with single dash key only (no value)
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void Parse_Array_SingleDashOnly_ThrowsException()
    {
        CmdArgsParser.Parse(new[] { "-" });
    }

    /// <summary>
    /// Test Parse with single dash key with value
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void Parse_Array_SingleDashKeyWithValue_ThrowsException()
    {
        CmdArgsParser.Parse(new[] { "-", "value" });
    }

    /// <summary>
    /// Test Parse with key followed by default and next key
    /// </summary>
    [TestMethod]
    public void Parse_Array_KeyDefaultNextKey_DefaultBelongsToFirstKey()
    {
        var result = CmdArgsParser.Parse(new[] { "-key1", "value1", "default", "-key2", "value2" });

        Assert.AreEqual(2, result.Count);
        CollectionAssert.AreEqual(new List<string> { "value1", "default" }, result["key1"]);
        CollectionAssert.AreEqual(new List<string> { "value2" }, result["key2"]);
    }

    #endregion

    #region Edge Cases and Boundary Tests



    /// <summary>
    /// Test Parse with key at last index
    /// </summary>
    [TestMethod]
    public void Parse_EdgeCase_KeyAtLastIndex_NoValuesAfter()
    {
        var result = CmdArgsParser.Parse(new[] { "value", "-key" });
        Assert.AreEqual(2, result.Count);
        CollectionAssert.AreEqual(new List<string> { "value" }, result[""]);
        CollectionAssert.AreEqual(new List<string>(), result["key"]);
    }

    /// <summary>
    /// Test Parse with empty string value
    /// </summary>
    [TestMethod]
    public void Parse_EdgeCase_EmptyStringValue_TreatedAsValid()
    {
        var result = CmdArgsParser.Parse(new[] { "-key", "", "value" });
        Assert.AreEqual(1, result.Count);
        CollectionAssert.AreEqual(new List<string> { "", "value" }, result["key"]);
    }

    /// <summary>
    /// Test Parse with special characters in values
    /// </summary>
    [TestMethod]
    public void Parse_EdgeCase_SpecialCharactersInValues_TreatedAsValues()
    {
        var result = CmdArgsParser.Parse(new[] { "-key", "value@#$%^&*()", "value2!@#" });
        Assert.AreEqual(1, result.Count);
        CollectionAssert.AreEqual(new List<string> { "value@#$%^&*()", "value2!@#" }, result["key"]);
    }

    /// <summary>
    /// Test Parse with numeric values
    /// </summary>
    [TestMethod]
    public void Parse_EdgeCase_NumericValues_TreatedAsStrings()
    {
        var result = CmdArgsParser.Parse(new[] { "-count", "123", "456.78" });
        Assert.AreEqual(1, result.Count);
        CollectionAssert.AreEqual(new List<string> { "123", "456.78" }, result["count"]);
    }

    /// <summary>
    /// Test Parse with whitespace values (array input)
    /// </summary>
    [TestMethod]
    public void Parse_EdgeCase_WhitespaceInValues_TreatedAsValid()
    {
        var result = CmdArgsParser.Parse(new[] { "-key", " ", "\t", "value" });
        Assert.AreEqual(1, result.Count);
        CollectionAssert.AreEqual(new List<string> { " ", "\t", "value" }, result["key"]);
    }

    /// <summary>
    /// Test Parse with long key names
    /// </summary>
    [TestMethod]
    public void Parse_EdgeCase_LongKeyName_TreatedCorrectly()
    {
        var result = CmdArgsParser.Parse(new[] { "-verylongkeyname", "value" });
        Assert.AreEqual(1, result.Count);
        CollectionAssert.AreEqual(new List<string> { "value" }, result["verylongkeyname"]);
    }

    /// <summary>
    /// Test Parse with many values for one key
    /// </summary>
    [TestMethod]
    public void Parse_EdgeCase_ManyValuesForOneKey_AllBelongToKey()
    {
        var values = Enumerable.Range(1, 100).Select(i => $"value{i}").ToArray();
        var input = new[] { "-key" }.Concat(values).ToArray();
        var result = CmdArgsParser.Parse(input);

        Assert.AreEqual(1, result.Count);
        Assert.AreEqual(100, result["key"].Count);
        for (int i = 0; i < 100; i++)
        {
            Assert.AreEqual($"value{i + 1}", result["key"][i]);
        }
    }

    /// <summary>
    /// Test Parse with many keys
    /// </summary>
    [TestMethod]
    public void Parse_EdgeCase_ManyKeys_AllProcessedCorrectly()
    {
        var input = Enumerable.Range(1, 50).Select(i => $"-key{i}").ToArray();
        var result = CmdArgsParser.Parse(input);

        Assert.AreEqual(50, result.Count);
        for (int i = 1; i <= 50; i++)
        {
            Assert.IsTrue(result.ContainsKey($"key{i}"));
            CollectionAssert.AreEqual(new List<string>(), result[$"key{i}"]);
        }
    }

    /// <summary>
    /// Test Parse with duplicate keys
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void Parse_EdgeCase_DuplicateKeys_ThrowsException()
    {
        // Dictionary.Add throws ArgumentException for duplicate keys
        CmdArgsParser.Parse(new[] { "-key", "value1", "-key", "value2" });
    }

    /// <summary>
    /// Test Parse with key name that is empty after removing dash
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void Parse_EdgeCase_KeyNameEmptyAfterDash_ThrowsException()
    {
        CmdArgsParser.Parse(new[] { "-", "value" });
    }

    /// <summary>
    /// Test Parse with triple dash key
    /// </summary>
    [TestMethod]
    public void Parse_EdgeCase_TripleDashKey_RemovesFirstDash()
    {
        var result = CmdArgsParser.Parse(new[] { "---key", "value" });
        Assert.AreEqual(1, result.Count);
        CollectionAssert.AreEqual(new List<string> { "value" }, result["--key"]);
    }

    /// <summary>
    /// Test Parse with value containing dash but not starting with dash
    /// </summary>
    [TestMethod]
    public void Parse_EdgeCase_ValueWithDashNotStartingWithDash_TreatedAsValue()
    {
        var result = CmdArgsParser.Parse(new[] { "-key", "value-with-dash", "another-value" });
        Assert.AreEqual(1, result.Count);
        CollectionAssert.AreEqual(new List<string> { "value-with-dash", "another-value" }, result["key"]);
    }

    /// <summary>
    /// Test Parse with mixed case keys
    /// </summary>
    [TestMethod]
    public void Parse_EdgeCase_MixedCaseKeys_KeysAreCaseSensitive()
    {
        var result = CmdArgsParser.Parse(new[] { "-Key", "-key", "-KEY", "value" });
        Assert.AreEqual(3, result.Count);
        CollectionAssert.AreEqual(new List<string>(), result["Key"]);
        CollectionAssert.AreEqual(new List<string>(), result["key"]);
        CollectionAssert.AreEqual(new List<string> { "value" }, result["KEY"]);
    }

    /// <summary>
    /// Test Parse with Unicode characters in key
    /// </summary>
    [TestMethod]
    public void Parse_EdgeCase_UnicodeInKey_TreatedCorrectly()
    {
        var result = CmdArgsParser.Parse(new[] { "-键名", "值", "-αβγ", "test" });
        Assert.AreEqual(2, result.Count);
        CollectionAssert.AreEqual(new List<string> { "值" }, result["键名"]);
        CollectionAssert.AreEqual(new List<string> { "test" }, result["αβγ"]);
    }

    /// <summary>
    /// Test Parse with special characters in key name
    /// </summary>
    [TestMethod]
    public void Parse_EdgeCase_SpecialCharsInKey_TreatedCorrectly()
    {
        var result = CmdArgsParser.Parse(new[] { "-key_name", "value1", "-key-name", "value2", "-key.name", "value3" });
        Assert.AreEqual(3, result.Count);
        CollectionAssert.AreEqual(new List<string> { "value1" }, result["key_name"]);
        CollectionAssert.AreEqual(new List<string> { "value2" }, result["key-name"]);
        CollectionAssert.AreEqual(new List<string> { "value3" }, result["key.name"]);
    }

    /// <summary>
    /// Test Parse with very long key name (stress test)
    /// </summary>
    [TestMethod]
    public void Parse_EdgeCase_VeryLongKeyName_HandlesCorrectly()
    {
        var longKey = "k" + new string('x', 999);
        var result = CmdArgsParser.Parse(new[] { $"-{longKey}", "value" });
        Assert.AreEqual(1, result.Count);
        CollectionAssert.AreEqual(new List<string> { "value" }, result[longKey]);
    }

    #endregion

    #region Real-world Scenario Tests

    /// <summary>
    /// Test Parse with typical file copy command
    /// </summary>
    [TestMethod]
    public void Parse_RealWorld_FileCopyCommand_ReturnsCorrectGroups()
    {
        var result = CmdArgsParser.Parse("copy -source input.txt -dest output.txt -verbose");

        Assert.AreEqual(4, result.Count);
        CollectionAssert.AreEqual(new List<string> { "copy" }, result[""]);
        CollectionAssert.AreEqual(new List<string> { "input.txt" }, result["source"]);
        CollectionAssert.AreEqual(new List<string> { "output.txt" }, result["dest"]);
        CollectionAssert.AreEqual(new List<string>(), result["verbose"]);
    }

    /// <summary>
    /// Test Parse with build command
    /// </summary>
    [TestMethod]
    public void Parse_RealWorld_BuildCommand_ReturnsCorrectGroups()
    {
        var result = CmdArgsParser.Parse("build -config Release -target clean compile -output bin -parallel");

        Assert.AreEqual(5, result.Count);
        CollectionAssert.AreEqual(new List<string> { "build" }, result[""]);
        CollectionAssert.AreEqual(new List<string> { "Release" }, result["config"]);
        CollectionAssert.AreEqual(new List<string> { "clean", "compile" }, result["target"]);
        CollectionAssert.AreEqual(new List<string> { "bin" }, result["output"]);
        CollectionAssert.AreEqual(new List<string>(), result["parallel"]);
    }

    /// <summary>
    /// Test Parse with API request command
    /// </summary>
    [TestMethod]
    public void Parse_RealWorld_ApiRequestCommand_ReturnsCorrectGroups()
    {
        var result = CmdArgsParser.Parse("api -url https://example.com/api -method GET -headers Authorization:Bearer123 Content-Type:application/json");

        Assert.AreEqual(4, result.Count);
        CollectionAssert.AreEqual(new List<string> { "api" }, result[""]);
        CollectionAssert.AreEqual(new List<string> { "https://example.com/api" }, result["url"]);
        CollectionAssert.AreEqual(new List<string> { "GET" }, result["method"]);
        CollectionAssert.AreEqual(new List<string> { "Authorization:Bearer123", "Content-Type:application/json" }, result["headers"]);
    }

    /// <summary>
    /// Test Parse with data processing command
    /// </summary>
    [TestMethod]
    public void Parse_RealWorld_DataProcessingCommand_ReturnsCorrectGroups()
    {
        var result = CmdArgsParser.Parse("process -input data1.csv data2.csv -output result.csv -format csv excel -log verbose debug");

        Assert.AreEqual(5, result.Count);
        CollectionAssert.AreEqual(new List<string> { "process" }, result[""]);
        CollectionAssert.AreEqual(new List<string> { "data1.csv", "data2.csv" }, result["input"]);
        CollectionAssert.AreEqual(new List<string> { "result.csv" }, result["output"]);
        CollectionAssert.AreEqual(new List<string> { "csv", "excel" }, result["format"]);
        CollectionAssert.AreEqual(new List<string> { "verbose", "debug" }, result["log"]);
    }

    #endregion
}
