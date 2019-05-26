using EnumStringValues;

namespace EnumStringValueTests
{
    public enum TestEnum
    {
        Unlabelled,

        [StringValue("1")]
        SingleDefined,

        [StringValue("2", true)]
        SingleDefinedWithPreferences,

        [StringValue("3"), StringValue("Three")]
        MultiDefined,

        [StringValue("4", true), StringValue("Four")]
        MultiDefinedWithPreferences,

        [StringValue("5", true), StringValue("Five", true)]
        MultiDefinedWithMultiplePreferences,

        [StringValue("01")]Spacer1,
        [StringValue("02")]Spacer2,
        [StringValue("03")]Spacer3,
        [StringValue("04")]Spacer4,
        [StringValue("05")]Spacer5,
        [StringValue("06")]Spacer6,
        [StringValue("07")]Spacer7,
        [StringValue("08")]Spacer8,
        [StringValue("09")]Spacer9,
        [StringValue("10")]Spacer10,
        [StringValue("11")]Spacer11,
        [StringValue("12")]Spacer12,
        [StringValue("13")]Spacer13,
        [StringValue("14")]Spacer14,
        [StringValue("15")]Spacer15,
        [StringValue("16")]Spacer16,
        [StringValue("17")]Spacer17,
        [StringValue("18")]Spacer18,
        [StringValue("19")]Spacer19,
        [StringValue("20")]Spacer20,

        [StringValue("Slow")]
        EnumValueWithLotsOfEnumsBeforeIt,
    }

    public enum TestEnum_Secondary
    {
        Unlabelled,

        [StringValue("3")]
        SingleDefined,

        [StringValue("4", true)]
        SingleDefinedWithPreferences,

        [StringValue("1"), StringValue("Four")]
        MultiDefined,

        [StringValue("5", true), StringValue("Three")]
        MultiDefinedWithPreferences,

        [StringValue("2", true), StringValue("Five", true)]
        MultiDefinedWithMultiplePreferences,
    }
}