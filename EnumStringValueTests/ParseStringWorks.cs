using NUnit.Framework;
using EnumStringValues;
using FluentAssertions;

namespace EnumStringValueTests
{
    public partial class EnumStringValueTest
    {
        [TestFixture]
        public class ParseStringWorks
        {
            [Test]
            public void InDefaultCase()
            {
                EnumExtensions.ParseStringValueToEnum<TestEnum>("Unlabelled").Should().Be(TestEnum.Unlabelled);
            }

            [Test]
            public void InSingleValueCase()
            {
                EnumExtensions.ParseStringValueToEnum<TestEnum>("1").Should().Be(TestEnum.SingleDefined);
            }

            [Test]
            public void InSingleValueWithPreferencesCase()
            {
                EnumExtensions.ParseStringValueToEnum<TestEnum>("2").Should().Be(TestEnum.SingleDefinedWithPreferences);
            }

            [Test]
            public void InMultiDefinedCases()
            {
                EnumExtensions.ParseStringValueToEnum<TestEnum>("3").Should().Be(TestEnum.MultiDefined);
            }

            [Test]
            public void InMultiDefinedCasesWithPreferences()
            {
                EnumExtensions.ParseStringValueToEnum<TestEnum>("4").Should().Be(TestEnum.MultiDefinedWithPreferences);
                EnumExtensions.ParseStringValueToEnum<TestEnum>("Four").Should().Be(TestEnum.MultiDefinedWithPreferences);
                EnumExtensions.ParseStringValueToEnum<TestEnum>("5").Should().Be(TestEnum.MultiDefinedWithMultiplePreferences);
                EnumExtensions.ParseStringValueToEnum<TestEnum>("Five").Should().Be(TestEnum.MultiDefinedWithMultiplePreferences);
            }

            [Test]
            public void AndIsCaseInsensitive()
            {
                EnumExtensions.ParseStringValueToEnum<TestEnum>("fOur").Should().Be(TestEnum.MultiDefinedWithPreferences);
            }
        }
    }
}