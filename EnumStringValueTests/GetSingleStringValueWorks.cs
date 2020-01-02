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
                TestEnum.Unlabeled.GetStringValue().Should().Be("Unlabeled");
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
            public void ConsistentlyWhenInterleavedWithOtherCalls_DespiteCaching1()
            {
              EnumExtensions.Behaviour.ResetCaches();

              TestEnum.SingleDefined.GetStringValue().Should().Be("1");
              TestEnum_Secondary.SingleDefined.GetStringValue().Should().Be("3");
              TestEnum.SingleDefined.GetStringValue().Should().Be("1");
              TestEnum_Secondary.SingleDefined.GetStringValue().Should().Be("3");
              TestEnum.MultiDefined.GetStringValue().Should().Be("3");
              TestEnum_Secondary.SingleDefined.GetStringValue().Should().Be("3");
              TestEnum_Secondary.MultiDefined.GetStringValue().Should().Be("1");
            }

            [Test]
            public void ConsistentlyWhenInterleavedWithOtherCalls_DespiteCaching2()
            {
              EnumExtensions.Behaviour.ResetCaches();

              TestEnum_Secondary.SingleDefined.GetStringValue().Should().Be("3");
              TestEnum.SingleDefined.GetStringValue().Should().Be("1");
              TestEnum_Secondary.SingleDefined.GetStringValue().Should().Be("3");
              TestEnum.SingleDefined.GetStringValue().Should().Be("1");
              TestEnum_Secondary.SingleDefined.GetStringValue().Should().Be("3");
              TestEnum_Secondary.MultiDefined.GetStringValue().Should().Be("1");
              TestEnum.MultiDefined.GetStringValue().Should().Be("3");
            }

            [Test]
            public void ConsistentlyWhenInterleavedWithOtherCalls_DespiteCaching3()
            {
              EnumExtensions.Behaviour.ResetCaches();

              TestEnum_Secondary.MultiDefined.GetStringValue().Should().Be("1");
              TestEnum_Secondary.SingleDefined.GetStringValue().Should().Be("3");
              TestEnum.MultiDefined.GetStringValue().Should().Be("3");
              TestEnum_Secondary.SingleDefined.GetStringValue().Should().Be("3");
              TestEnum.SingleDefined.GetStringValue().Should().Be("1");
              TestEnum_Secondary.SingleDefined.GetStringValue().Should().Be("3");
              TestEnum.SingleDefined.GetStringValue().Should().Be("1");
            }

            [Test, Repeat(10)]
            public void FasterWithCaching()
            {
              var reps = 20000;

              EnumExtensions.Behaviour.ResetCaches();
              EnumExtensions.Behaviour.UseCaching = false;
              double rawTime = TimeFetchingStringForEnum(TestEnum.SingleDefined, reps);

              EnumExtensions.Behaviour.ResetCaches();
              EnumExtensions.Behaviour.UseCaching = true;
              double cachedTime = TimeFetchingStringForEnum(TestEnum.SingleDefined, reps);

              (cachedTime / rawTime).Should().BeLessThan(0.2f);
            }

            public long TimeFetchingStringForEnum(TestEnum expectedEnum, int reps)
            {
                return Timer.Time(() => expectedEnum.GetStringValue(), reps);
            }
        }
}