using System;
using MDMUtils.Enums;
using NUnit.Framework;

namespace MDMUtilsTests.Enums
{
  public enum eTestEnum
  {
    Unlabelled,

    [StringValue("1")]
    SingleDefined,

    [StringValue("2", true)]
    SingleDefinedWithPreferences,

    [StringValue("3"),
     StringValue("Three")]
    MultiDefined,

    [StringValue("4", true),
     StringValue("Four")]
    MultiDefinedWithPreferences,

    [StringValue("5", true),
     StringValue("Five", true)]
    MultiDefinedWithMultiplePreferences
  }

  class EnumStringValueTest
  {
    [TestFixture]
    public class EnumerateEnumValuesWorks
    {
      [Test]
      public void InGeneral()
      {
        var enumerationReturned = EnumExtensions.EnumerateValues<eTestEnum>();
        var explicitEnumeration =
          new[]
            {
              eTestEnum.Unlabelled,
              eTestEnum.SingleDefined,
              eTestEnum.SingleDefinedWithPreferences,
              eTestEnum.MultiDefined,
              eTestEnum.MultiDefinedWithPreferences,
              eTestEnum.MultiDefinedWithMultiplePreferences
            };
        CollectionAssert.AreEquivalent(explicitEnumeration, enumerationReturned);
      }
    }

    [TestFixture]
    public class GetSingleStringValueWorks
    {
      [Test]
      public void InUndefinedCases()
      {
        Assert.AreEqual(null, eTestEnum.Unlabelled.GetStringValue());
      }

      [Test]
      public void InSingleDefinedCases()
      {
        Assert.AreEqual("1", eTestEnum.SingleDefined.GetStringValue());
      }

      [Test]
      public void WithPreferences()
      {
        Assert.AreEqual("2", eTestEnum.SingleDefinedWithPreferences.GetStringValue());
      }

      [Test]
      public void InMultiDefinedCases()
      {
        CollectionAssert.Contains(new[] {"3", "Three"}, eTestEnum.MultiDefined.GetStringValue());
      }
    }

    [TestFixture]
    public class GetMultipleStringValuesWorks
    {
      [Test]
      public void InSingleDefinedCases()
      {
        CollectionAssert.AreEquivalent(new[] {"1"}, eTestEnum.SingleDefined.GetAllStringValues());
      }

      [Test]
      public void InMultiDefinedCases()
      {
        CollectionAssert.AreEquivalent(new[] {"3", "Three"}, eTestEnum.MultiDefined.GetAllStringValues());
      }

      [Test]
      public void InMultiDefinedCasesWithPreferences()
      {
        CollectionAssert.AreEquivalent(new[] {"4", "Four"}, eTestEnum.MultiDefinedWithPreferences.GetAllStringValues());
      }
    }

    [TestFixture]
    public class GetPreferredStringValuesWorks
    {
      [Test]
      public void InSingleDefinedCasesWithPreferences()
      {
        Assert.AreEqual("2", eTestEnum.SingleDefinedWithPreferences.GetStringValue());
      }

      [Test]
      public void InMultiDefinedCasesWithPreferences()
      {
        Assert.AreEqual("4", eTestEnum.MultiDefinedWithPreferences.GetStringValue());
      }

      [Test]
      public void InMultiDefinedMultiPreferenceCases()
      {
        CollectionAssert.Contains(new[] {"5", "Five"}, eTestEnum.MultiDefinedWithMultiplePreferences.GetStringValue());
      }
    }

    [TestFixture]
    public class ParseStringWorks
    {
      [Test]
      public void InSingleValueCase()
      {
        Assert.AreEqual(eTestEnum.SingleDefined, EnumExtensions.ParseStringValueToEnumInt<eTestEnum>("1"));
      }

      [Test]
      public void InSingleValueWithPreferencesCase()
      {
        Assert.AreEqual(eTestEnum.SingleDefinedWithPreferences, EnumExtensions.ParseStringValueToEnumInt<eTestEnum>("2"));
      }

      [Test]
      public void InMultiDefinedCases()
      {
        Assert.AreEqual(eTestEnum.MultiDefined, EnumExtensions.ParseStringValueToEnumInt<eTestEnum>("3"));
      }

      [Test]
      public void InMultiDefinedCasesWithPreferences()
      {
        Assert.AreEqual(eTestEnum.MultiDefinedWithPreferences, EnumExtensions.ParseStringValueToEnumInt<eTestEnum>("4"));
        Assert.AreEqual(eTestEnum.MultiDefinedWithPreferences,
          EnumExtensions.ParseStringValueToEnumInt<eTestEnum>("Four"));
        Assert.AreEqual(eTestEnum.MultiDefinedWithMultiplePreferences,
          EnumExtensions.ParseStringValueToEnumInt<eTestEnum>("5"));
        Assert.AreEqual(eTestEnum.MultiDefinedWithMultiplePreferences,
          EnumExtensions.ParseStringValueToEnumInt<eTestEnum>("Five"));
      }

      [Test]
      public void WithoutCapitalisationIssues()
      {
        Assert.AreEqual(eTestEnum.MultiDefinedWithPreferences,
          EnumExtensions.ParseStringValueToEnumInt<eTestEnum>("fOur"));
      }
    }

    [TestFixture]
    public class ParseStringThrows
    {
      [Test]
      public void WhenStringIsNull()
      {
        Assert.Throws<ArgumentNullException>(() => EnumExtensions.ParseStringValueToEnumInt<eTestEnum>(null));
      }

      [Test]
      public void WhenStringIsUnmatched()
      {
        Assert.Throws<UnmatchedStringValueException>(() => EnumExtensions.ParseStringValueToEnumInt<eTestEnum>("InvalidStringValue"));
      }

      [Test]
      public void WhenTypePassedIntoTryParseIsNotAnEnum()
      {
        int x;
        Assert.Throws<InvalidOperationException>(() => EnumExtensions.TryParseStringValueToEnumInt<int>("IrrelevantStringValue", out x));
      }

      [Test]
      public void WhenTypePassedIntoParseIsNotAnEnum()
      {
        Assert.Throws<InvalidOperationException>(() => EnumExtensions.ParseStringValueToEnumInt<int>("IrrelevantStringValue"));
      }

      [Test]
      [TestCase("InvalidStringValue", Description = "To hit catch case."),
       TestCase("1", Description = "To hit non-catch case.")]
      public void AnExceptionWithTheExpectedText(string input)
      {
        try
        {
          EnumExtensions.ParseStringValueToEnumInt<eTestEnum>(input);
        }
        catch (Exception e)
        {
          Assert.AreEqual(e.Message, "String does not match to any value of the specified Enum. Attempted to Parse InvalidStringValue into an Enum of type eTestEnum.");
        }
      }

    }
}
}