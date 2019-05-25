using NUnit.Framework;
using EnumStringValues;
using FluentAssertions;

namespace EnumStringValueTests
{
        public class GetSingleStringValueWorks : EnumStringValueTestBase
        {
            public GetSingleStringValueWorks(bool arg) : base(arg) {}

            [Test]
            public void InUndefinedCases()
            {
                TestEnum.Unlabelled.GetStringValue().Should().Be("Unlabelled");
            }

            [Test]
            public void InSingleDefinedCases()
            {
                TestEnum.SingleDefined.GetStringValue().Should().Be("1");
            }

            [Test]
            public void WithPreferences()
            {
                TestEnum.SingleDefinedWithPreferences.GetStringValue().Should().Be("2");
            }

            [Test]
            public void InMultiDefinedCases()
            {
                TestEnum.MultiDefined.GetStringValue().Should().BeOneOf("3", "Three");
            }
        }
}