using System.Collections.Generic;
using NUnit.Framework;
using EnumStringValues;
using FluentAssertions;
using static EnumStringValues.EnumExtensions.Behaviour;

namespace EnumStringValueTests
{
  public class GetMultipleStringValuesWorks : EnumStringValueTestBase
  {
    public GetMultipleStringValuesWorks(bool arg) : base(arg) {}

    [Test,
     TestCase(UnderlyingNameUsed.Always, true),
     TestCase(UnderlyingNameUsed.IfNoOverrideGiven, false),
     TestCase(UnderlyingNameUsed.Never, false)]
    public void InSingleDefinedCases(UnderlyingNameUsed shouldUse, bool includeLiteral)
    {
      EnumExtensions.Behaviour.ShouldIncludeUnderlyingName = shouldUse;
      var expectedCollection = new List<string>{"1"};
      if (includeLiteral)
      {
        expectedCollection.Add(nameof(TestEnum.SingleDefined));
      }

      TestEnum.SingleDefined.GetAllStringValues().Should().BeEquivalentTo(expectedCollection);
    }

    [Test,
     TestCase(UnderlyingNameUsed.Always, true),
     TestCase(UnderlyingNameUsed.IfNoOverrideGiven, false),
     TestCase(UnderlyingNameUsed.Never, false)]
    public void InMultiDefinedCases(UnderlyingNameUsed shouldUse, bool includeLiteral)
    {
      EnumExtensions.Behaviour.ShouldIncludeUnderlyingName = shouldUse;
      var expectedCollection = new List<string> { "3", "Three" };
      if (includeLiteral)
      {
        expectedCollection.Add(nameof(TestEnum.MultiDefined));
      }

      TestEnum.MultiDefined.GetAllStringValues().Should().BeEquivalentTo(expectedCollection);
    }

    [Test,
     TestCase(UnderlyingNameUsed.Always, true),
     TestCase(UnderlyingNameUsed.IfNoOverrideGiven, false),
     TestCase(UnderlyingNameUsed.Never, false)]
    public void InMultiDefinedCasesWithPreferences(UnderlyingNameUsed shouldUse, bool includeLiteral)
    {
      EnumExtensions.Behaviour.ShouldIncludeUnderlyingName = shouldUse;
      var expectedCollection = new List<string> { "4", "Four" };
      if (includeLiteral)
      {
        expectedCollection.Add(nameof(TestEnum.MultiDefinedWithPreferences));
      }

      TestEnum.MultiDefinedWithPreferences.GetAllStringValues().Should().BeEquivalentTo(expectedCollection);
    }

    [Test,
     TestCase(UnderlyingNameUsed.Always, true),
     TestCase(UnderlyingNameUsed.IfNoOverrideGiven, true),
     TestCase(UnderlyingNameUsed.Never, false)]
    public void InUnlabelledCases(UnderlyingNameUsed shouldUse, bool includeLiteral)
    {
      EnumExtensions.Behaviour.ShouldIncludeUnderlyingName = shouldUse;
      var expectedCollection = new List<string> { };
      if (includeLiteral)
      {
        expectedCollection.Add(nameof(TestEnum.Unlabeled));
      }

      TestEnum.Unlabeled.GetAllStringValues().Should().BeEquivalentTo(expectedCollection);
    }
  }
}