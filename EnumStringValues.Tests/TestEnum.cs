namespace EnumStringValues.Tests
{
    public enum TestEnum
    {
        Unlabelled,

        [StringValue("1")]
        SingleDefined,

        [StringValue("2", true)]
        SingleDefinedWithPreferences,

        [StringValue("3"),
         StringValue("Three")]
        MultiDefined,

        [StringValue("4", true),
         StringValue("Four")]
        MultiDefinedWithPreferences,

        [StringValue("5", true),
         StringValue("Five", true)]
        MultiDefinedWithMultiplePreferences
    }
}