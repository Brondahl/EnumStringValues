using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

            [Test]
            public void WithNoWeirdCachingBugs1()
            {
                EnumExtensions.ResetCaches();

                EnumExtensions.ParseToEnum<TestEnum>("Four").Should().Be(TestEnum.MultiDefinedWithPreferences);
                EnumExtensions.ParseToEnum<TestEnum>("Four").Should().Be(TestEnum.MultiDefinedWithPreferences);
                EnumExtensions.ParseToEnum<TestEnum>("4").Should().Be(TestEnum.MultiDefinedWithPreferences);
                EnumExtensions.ParseToEnum<TestEnum>("5").Should().Be(TestEnum.MultiDefinedWithMultiplePreferences);
                EnumExtensions.ParseToEnum<TestEnum>("4").Should().Be(TestEnum.MultiDefinedWithPreferences);
                EnumExtensions.ParseToEnum<TestEnum>("5").Should().Be(TestEnum.MultiDefinedWithMultiplePreferences);
                EnumExtensions.ParseToEnum<TestEnum_Secondary>("4").Should().Be(TestEnum_Secondary.SingleDefinedWithPreferences);
                EnumExtensions.ParseToEnum<TestEnum>("4").Should().Be(TestEnum.MultiDefinedWithPreferences);
                EnumExtensions.ParseToEnum<TestEnum_Secondary>("Four").Should().Be(TestEnum_Secondary.MultiDefined);
            }

            [Test]
            public void WithNoWeirdCachingBugs2()
            {
                EnumExtensions.ResetCaches();

                EnumExtensions.ParseToEnum<TestEnum_Secondary>("Four").Should().Be(TestEnum_Secondary.MultiDefined);
                EnumExtensions.ParseToEnum<TestEnum>("4").Should().Be(TestEnum.MultiDefinedWithPreferences);
                EnumExtensions.ParseToEnum<TestEnum_Secondary>("4").Should().Be(TestEnum_Secondary.SingleDefinedWithPreferences);
                EnumExtensions.ParseToEnum<TestEnum>("5").Should().Be(TestEnum.MultiDefinedWithMultiplePreferences);
                EnumExtensions.ParseToEnum<TestEnum>("4").Should().Be(TestEnum.MultiDefinedWithPreferences);
                EnumExtensions.ParseToEnum<TestEnum>("5").Should().Be(TestEnum.MultiDefinedWithMultiplePreferences);
                EnumExtensions.ParseToEnum<TestEnum>("4").Should().Be(TestEnum.MultiDefinedWithPreferences);
                EnumExtensions.ParseToEnum<TestEnum>("Four").Should().Be(TestEnum.MultiDefinedWithPreferences);
                EnumExtensions.ParseToEnum<TestEnum>("Four").Should().Be(TestEnum.MultiDefinedWithPreferences);
            }

            [Test, Repeat(50)]
            public void FasterWithCaching()
            {
              var reps = 500;

              EnumExtensions.ResetCaches();
              EnumExtensions.UseCaching = false;
              double rawTime = TimeParsingStringForEnum(TestEnum.SingleDefined, reps);

              EnumExtensions.ResetCaches();
              EnumExtensions.UseCaching = true;
              double cachedTime = TimeParsingStringForEnum(TestEnum.SingleDefined, reps);

              (cachedTime / rawTime).Should().BeLessThan(0.1f);
            }

            [Test, Repeat(10)]
            public void ButIsSlowerForLaterEnumsWhenNotCaching()
            {
                EnumExtensions.ResetCaches();
                EnumExtensions.UseCaching = false;
                var reps = 250;
                double fast = TimeParsingStringForEnum(TestEnum.SingleDefined, reps);
                double slow = TimeParsingStringForEnum(TestEnum.EnumValueWithLotsOfEnumsBeforeIt, reps);

                (slow / fast).Should().BeGreaterThan(5f);
            }

            [Test, Repeat(10)]
            public void ButIsNotSlowerForLaterEnumsWhenCachingIsActive()
            {
              EnumExtensions.ResetCaches();
              EnumExtensions.UseCaching = true;
              int reps = 500000;
              double fast = TimeParsingStringForEnum(TestEnum.SingleDefined, reps);
              double notSlow = TimeParsingStringForEnum(TestEnum.EnumValueWithLotsOfEnumsBeforeIt, reps);

              (notSlow / fast).Should().BeLessThan(1.3f);
            }

            public long TimeParsingStringForEnum(TestEnum expectedEnum, int reps)
            {
                var label = expectedEnum.GetStringValue();

                return Timer.Time(label.ParseToEnum<TestEnum>, reps);
            }
        }
}