using NUnit.Framework;

namespace EnumStringValues.Tests
{
    [TestFixture]
    public class EnumerateEnumValuesWorks
    {
        [Test]
        public void InGeneral()
        {
            var enumerationReturned = EnumExtensions.EnumerateValues<TestEnum>();
            var explicitEnumeration =
                new[]
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