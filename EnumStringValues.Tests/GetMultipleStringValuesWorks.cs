using NUnit.Framework;
using FluentAssertions;

namespace EnumStringValues.Tests
{
    [TestFixture]
    public class GetMultipleStringValuesWorks
    {
        [Test]
        public void InSingleDefinedCases()
        {
            TestEnum.SingleDefined.GetAllStringValues().Should().BeEquivalentTo("1");
        }

        [Test]
        public void InMultiDefinedCases()
        {
            TestEnum.MultiDefined.GetAllStringValues().Should().BeEquivalentTo("3", "Three");
        }

        [Test]
        public void InMultiDefinedCasesWithPreferences()
        {
            TestEnum.MultiDefinedWithPreferences.GetAllStringValues().Should().BeEquivalentTo("4", "Four");
        }
    }
}