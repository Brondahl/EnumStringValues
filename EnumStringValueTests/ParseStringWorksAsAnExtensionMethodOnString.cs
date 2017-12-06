using NUnit.Framework;
using EnumStringValues;
using FluentAssertions;

namespace EnumStringValueTests
{
    public partial class EnumStringValueTest
    {
        [TestFixture]
        public class ParseStringWorksAsAnExtensionMethodOnString
        {
            [Test]
            public void InDefaultCase()
            {
                "Unlabelled".ParseToEnum<TestEnum>().Should().Be(TestEnum.Unlabelled);
            }

            [Test]
            public void InSingleValueCase()
            {
                "1".ParseToEnum<TestEnum>().Should().Be(TestEnum.SingleDefined);
            }

            [Test]
            public void InSingleValueWithPreferencesCase()
            {
                "2".ParseToEnum<TestEnum>().Should().Be(TestEnum.SingleDefinedWithPreferences);
            }

            [Test]
            public void InMultiDefinedCases()
            {
                "3".ParseToEnum<TestEnum>().Should().Be(TestEnum.MultiDefined);
            }

            [Test]
            public void InMultiDefinedCasesWithPreferences()
            {
                "4".ParseToEnum<TestEnum>().Should().Be(TestEnum.MultiDefinedWithPreferences);
                "Four".ParseToEnum<TestEnum>().Should().Be(TestEnum.MultiDefinedWithPreferences);
                "5".ParseToEnum<TestEnum>().Should().Be(TestEnum.MultiDefinedWithMultiplePreferences);
                "Five".ParseToEnum<TestEnum>().Should().Be(TestEnum.MultiDefinedWithMultiplePreferences);
            }

            [Test]
            public void AndIsCaseInsensitive()
            {
                "fOur".ParseToEnum<TestEnum>().Should().Be(TestEnum.MultiDefinedWithPreferences);
            }
        }
    }
}