using EnumStringValues;
using NUnit.Framework;

namespace EnumStringValueTests
{
    public class EnumerateEnumValuesWorks :  EnumStringValueTestBase
    {
            public EnumerateEnumValuesWorks(bool arg) : base(arg) { }

            private readonly TestEnum[] expectedValuesInTestEnum = {
                    TestEnum.Unlabelled,
                    TestEnum.SingleDefined,
                    TestEnum.SingleDefinedWithPreferences,
                    TestEnum.MultiDefined,
                    TestEnum.MultiDefinedWithPreferences,
                    TestEnum.MultiDefinedWithMultiplePreferences,
                    TestEnum.Spacer1,
                    TestEnum.Spacer2,
                    TestEnum.Spacer3,
                    TestEnum.Spacer4,
                    TestEnum.Spacer5,
                    TestEnum.Spacer6,
                    TestEnum.Spacer7,
                    TestEnum.Spacer8,
                    TestEnum.Spacer9,
                    TestEnum.Spacer10,
                    TestEnum.Spacer11,
                    TestEnum.Spacer12,
                    TestEnum.Spacer13,
                    TestEnum.Spacer14,
                    TestEnum.Spacer15,
                    TestEnum.Spacer16,
                    TestEnum.Spacer17,
                    TestEnum.Spacer18,
                    TestEnum.Spacer19,
                    TestEnum.Spacer20,
                    TestEnum.EnumValueWithLotsOfEnumsBeforeIt
                };

            private readonly TestEnum_Secondary[] expectedValuesInSecondaryEnum = {
                    TestEnum_Secondary.Unlabelled,
                    TestEnum_Secondary.SingleDefined,
                    TestEnum_Secondary.SingleDefinedWithPreferences,
                    TestEnum_Secondary.MultiDefined,
                    TestEnum_Secondary.MultiDefinedWithPreferences,
                    TestEnum_Secondary.MultiDefinedWithMultiplePreferences
                };

            [Test]
            public void InGeneral()
            {
                var enumerationReturned = EnumExtensions.EnumerateValues<TestEnum>();

              CollectionAssert.AreEquivalent(expectedValuesInTestEnum, enumerationReturned);
            }

            [Test]
            public void WithNoWeirdCachingBugs1()
            {
              EnumExtensions.ResetCaches();

              var enumeration1Returned = EnumExtensions.EnumerateValues<TestEnum>();
              var enumeration2Returned = EnumExtensions.EnumerateValues<TestEnum_Secondary>();
                var enumeration1ReturnedAgain = EnumExtensions.EnumerateValues<TestEnum>();

              CollectionAssert.AreEquivalent(expectedValuesInTestEnum, enumeration1Returned);
              CollectionAssert.AreEquivalent(expectedValuesInSecondaryEnum, enumeration2Returned);
              CollectionAssert.AreEquivalent(expectedValuesInTestEnum, enumeration1ReturnedAgain);
            }

            [Test]
            public void WithNoWeirdCachingBugs2()
            {
                EnumExtensions.ResetCaches();

                var enumeration1Returned = EnumExtensions.EnumerateValues<TestEnum_Secondary>();
                var enumeration2Returned = EnumExtensions.EnumerateValues<TestEnum>();
                var enumeration1ReturnedAgain = EnumExtensions.EnumerateValues<TestEnum_Secondary>();

                CollectionAssert.AreEquivalent(expectedValuesInSecondaryEnum, enumeration1Returned);
                CollectionAssert.AreEquivalent(expectedValuesInTestEnum, enumeration2Returned);
                CollectionAssert.AreEquivalent(expectedValuesInSecondaryEnum, enumeration1ReturnedAgain);
            }
    }
}