using NUnit.Framework;
using EnumStringValues;
using FluentAssertions;

namespace EnumStringValueTests
{
        public class GetPreferredStringValuesWorks :  EnumStringValueTestBase
        {
            public GetPreferredStringValuesWorks(bool arg) : base(arg) { }

            [Test]
            public void InSingleDefinedCasesWithPreferences()
            {
                TestEnum.SingleDefinedWithPreferences.GetStringValue().Should().Be("2");
            }

            [Test]
            public void InMultiDefinedCasesWithPreferences()
            {
                TestEnum.MultiDefinedWithPreferences.GetStringValue().Should().Be("4");
            }

            [Test]
            public void InMultiDefinedMultiPreferenceCases()
            {
                TestEnum.MultiDefinedWithMultiplePreferences.GetStringValue().Should().BeOneOf("5", "Five");
            }
        }
}