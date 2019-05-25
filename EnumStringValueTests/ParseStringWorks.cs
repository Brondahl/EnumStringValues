using NUnit.Framework;
using EnumStringValues;
using FluentAssertions;

namespace EnumStringValueTests
{
        public class ParseStringWorks :  EnumStringValueTestBase
    {
            public ParseStringWorks(bool arg) : base(arg) { }

            [Test]
            public void InDefaultCase()
            {
                EnumExtensions.ParseToEnum<TestEnum>("Unlabelled").Should().Be(TestEnum.Unlabelled);
            }

            [Test]
            public void InSingleValueCase()
            {
                EnumExtensions.ParseToEnum<TestEnum>("1").Should().Be(TestEnum.SingleDefined);
            }

            [Test]
            public void InSingleValueWithPreferencesCase()
            {
                EnumExtensions.ParseToEnum<TestEnum>("2").Should().Be(TestEnum.SingleDefinedWithPreferences);
            }

            [Test]
            public void InMultiDefinedCases()
            {
                EnumExtensions.ParseToEnum<TestEnum>("3").Should().Be(TestEnum.MultiDefined);
            }

            [Test]
            public void InMultiDefinedCasesWithPreferences()
            {
                EnumExtensions.ParseToEnum<TestEnum>("4").Should().Be(TestEnum.MultiDefinedWithPreferences);
                EnumExtensions.ParseToEnum<TestEnum>("Four").Should().Be(TestEnum.MultiDefinedWithPreferences);
                EnumExtensions.ParseToEnum<TestEnum>("5").Should().Be(TestEnum.MultiDefinedWithMultiplePreferences);
                EnumExtensions.ParseToEnum<TestEnum>("Five").Should().Be(TestEnum.MultiDefinedWithMultiplePreferences);
            }

            [Test]
            public void AndIsCaseInsensitive()
            {
                EnumExtensions.ParseToEnum<TestEnum>("fOur").Should().Be(TestEnum.MultiDefinedWithPreferences);
            }
        }
}