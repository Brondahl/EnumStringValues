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

            [Test]
            public void WithNoWeirdCachingBugs()
            {
              EnumExtensions.ResetCaches();

              TestEnum.SingleDefined.GetStringValue().Should().Be("1");
              TestEnum_Secondary.SingleDefined.GetStringValue().Should().Be("3");
              TestEnum.SingleDefined.GetStringValue().Should().Be("1");
              TestEnum_Secondary.SingleDefined.GetStringValue().Should().Be("3");
              TestEnum.MultiDefined.GetStringValue().Should().Be("3");
              TestEnum_Secondary.SingleDefined.GetStringValue().Should().Be("3");
              TestEnum_Secondary.MultiDefined.GetStringValue().Should().Be("1");
            }

            [Test]
            public void WithNoWeirdCachingBugs2()
            {
              EnumExtensions.ResetCaches();

              TestEnum_Secondary.SingleDefined.GetStringValue().Should().Be("3");
              TestEnum.SingleDefined.GetStringValue().Should().Be("1");
              TestEnum_Secondary.SingleDefined.GetStringValue().Should().Be("3");
              TestEnum.SingleDefined.GetStringValue().Should().Be("1");
              TestEnum_Secondary.SingleDefined.GetStringValue().Should().Be("3");
              TestEnum_Secondary.MultiDefined.GetStringValue().Should().Be("1");
              TestEnum.MultiDefined.GetStringValue().Should().Be("3");
            }

            [Test]
            public void WithNoWeirdCachingBugs3()
            {
              EnumExtensions.ResetCaches();

              TestEnum_Secondary.MultiDefined.GetStringValue().Should().Be("1");
              TestEnum_Secondary.SingleDefined.GetStringValue().Should().Be("3");
              TestEnum.MultiDefined.GetStringValue().Should().Be("3");
              TestEnum_Secondary.SingleDefined.GetStringValue().Should().Be("3");
              TestEnum.SingleDefined.GetStringValue().Should().Be("1");
              TestEnum_Secondary.SingleDefined.GetStringValue().Should().Be("3");
              TestEnum.SingleDefined.GetStringValue().Should().Be("1");
            }
        }
}