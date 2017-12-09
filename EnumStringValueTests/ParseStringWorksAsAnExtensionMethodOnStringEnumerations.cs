using NUnit.Framework;
using EnumStringValues;
using FluentAssertions;

namespace EnumStringValueTests
{
    public partial class EnumStringValueTest
    {
        [TestFixture]
        public class ParseStringWorksAsAnExtensionMethodOnStringEnumerations
        {
            [Test]
            public void ForSingleElementEnumerations()
            {
                new[] { "1" }.ParseToEnumList<TestEnum>().Should().BeEquivalentTo(TestEnum.SingleDefined);
            }

            [Test]
            public void ForMultiElementEnumerations()
            {
                new[] { "1", "2" }.ParseToEnumList<TestEnum>().Should().BeEquivalentTo(TestEnum.SingleDefined, TestEnum.SingleDefinedWithPreferences);
            }

            [Test]
            public void ForDifferentStringsMappingToSameEnum()
            {
                new[] { "4", "Four" }.ParseToEnumList<TestEnum>().Should().BeEquivalentTo(TestEnum.MultiDefinedWithPreferences, TestEnum.MultiDefinedWithPreferences);
            }

            [Test]
            public void ForDuplicatedStrings()
            {
                new[] { "1", "1" }.ParseToEnumList<TestEnum>().Should().BeEquivalentTo(TestEnum.SingleDefined, TestEnum.SingleDefined);
            }
        }
    }
}