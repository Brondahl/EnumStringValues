using System;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace EnumStringValues.Tests
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
        eTestEnum.Unlabelled.GetStringValue().Should().BeNull();
      }

      [Test]
      public void InSingleDefinedCases()
      {
        eTestEnum.SingleDefined.GetStringValue().Should().Be("1");
      }

      [Test]
      public void WithPreferences()
      {
        eTestEnum.SingleDefinedWithPreferences.GetStringValue().Should().Be("2");
      }

      [Test]
      public void InMultiDefinedCases()
      {
        eTestEnum.MultiDefined.GetStringValue().Should().BeOneOf("3", "Three");
      }
    }

    [TestFixture]
    public class GetMultipleStringValuesWorks
    {
      [Test]
      public void InSingleDefinedCases()
      {
        eTestEnum.SingleDefined.GetAllStringValues().Should().BeEquivalentTo("1");
      }

      [Test]
      public void InMultiDefinedCases()
      {
        eTestEnum.MultiDefined.GetAllStringValues().Should().BeEquivalentTo("3", "Three");
      }

      [Test]
      public void InMultiDefinedCasesWithPreferences()
      {
        eTestEnum.MultiDefinedWithPreferences.GetAllStringValues().Should().BeEquivalentTo("4", "Four");
      }
    }

    [TestFixture]
    public class GetPreferredStringValuesWorks
    {
      [Test]
      public void InSingleDefinedCasesWithPreferences()
      {
        eTestEnum.SingleDefinedWithPreferences.GetStringValue().Should().Be("2");
      }

      [Test]
      public void InMultiDefinedCasesWithPreferences()
      {
        eTestEnum.MultiDefinedWithPreferences.GetStringValue().Should().Be("4");
      }

      [Test]
      public void InMultiDefinedMultiPreferenceCases()
      {
        eTestEnum.MultiDefinedWithMultiplePreferences.GetStringValue().Should().BeOneOf("5", "Five");
      }
    }

    [TestFixture]
    public class ParseStringWorks
    {
      [Test]
      public void InSingleValueCase()
      {
        EnumExtensions.ParseStringValueToEnum<eTestEnum>("1").Should().Be(eTestEnum.SingleDefined);
      }

      [Test]
      public void InSingleValueWithPreferencesCase()
      {
        EnumExtensions.ParseStringValueToEnum<eTestEnum>("2").Should().Be(eTestEnum.SingleDefinedWithPreferences);
      }

      [Test]
      public void InMultiDefinedCases()
      {
        EnumExtensions.ParseStringValueToEnum<eTestEnum>("3").Should().Be(eTestEnum.MultiDefined);
      }

      [Test]
      public void InMultiDefinedCasesWithPreferences()
      {
        EnumExtensions.ParseStringValueToEnum<eTestEnum>("4").Should().Be(eTestEnum.MultiDefinedWithPreferences);
        EnumExtensions.ParseStringValueToEnum<eTestEnum>("Four").Should().Be(eTestEnum.MultiDefinedWithPreferences);
        EnumExtensions.ParseStringValueToEnum<eTestEnum>("5").Should().Be(eTestEnum.MultiDefinedWithMultiplePreferences);
        EnumExtensions.ParseStringValueToEnum<eTestEnum>("Five").Should().Be(eTestEnum.MultiDefinedWithMultiplePreferences);
      }

      [Test]
      public void AndIsCaseInsensitive()
      {
        EnumExtensions.ParseStringValueToEnum<eTestEnum>("fOur").Should().Be(eTestEnum.MultiDefinedWithPreferences);
      }
    }

    [TestFixture]
    public class ParseStringThrows
    {
      [Test]
      public void WhenStringIsNull()
      {
        Action parseAttempt = (() => EnumExtensions.ParseStringValueToEnum<eTestEnum>(null));
        parseAttempt.ShouldThrow<ArgumentNullException>();
      }

      [Test]
      public void WhenStringIsUnmatched()
      {
        Action parseAttempt = (() => EnumExtensions.ParseStringValueToEnum<eTestEnum>("InvalidStringValue"));
        parseAttempt.ShouldThrow<UnmatchedStringValueException>();
      }

      [Test]
      public void WhenTypePassedIntoTryParseIsNotAnEnum()
      {
        int x;
        Action parseAttempt = (() => EnumExtensions.TryParseStringValueToEnum<int>("IrrelevantStringValue", out x));
        parseAttempt.ShouldThrow<InvalidOperationException>();
      }

      [Test]
      public void WhenTypePassedIntoParseIsNotAnEnum()
      {
        Action parseAttempt = (() => EnumExtensions.ParseStringValueToEnum<int>("IrrelevantStringValue"));
        parseAttempt.ShouldThrow<InvalidOperationException>();
      }

      [Test]
      public void AnExceptionWithTheExpectedText()
      {
        Action parseAttempt = (() => EnumExtensions.ParseStringValueToEnum<eTestEnum>("InvalidStringValue"));
        parseAttempt.ShouldThrow<Exception>().WithMessage("String does not match to any value of the specified Enum. Attempted to Parse InvalidStringValue into an Enum of type eTestEnum.");
      }
    }
  }
}