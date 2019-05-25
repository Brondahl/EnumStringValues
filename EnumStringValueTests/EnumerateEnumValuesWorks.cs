using NUnit.Framework;
using EnumStringValues;

namespace EnumStringValueTests
{
    public class EnumerateEnumValuesWorks :  EnumStringValueTestBase
    {
            public EnumerateEnumValuesWorks(bool arg) : base(arg) { }

            [Test]
            public void InGeneral()
            {
                var enumerationReturned = EnumExtensions.EnumerateValues<TestEnum>();
                var explicitEnumeration = new[]
                {
                    TestEnum.Unlabelled,
                    TestEnum.SingleDefined,
                    TestEnum.SingleDefinedWithPreferences,
                    TestEnum.MultiDefined,
                    TestEnum.MultiDefinedWithPreferences,
                    TestEnum.MultiDefinedWithMultiplePreferences
                };

                CollectionAssert.AreEquivalent(explicitEnumeration, enumerationReturned);
            }
    }
}