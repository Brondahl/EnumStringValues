using System;

namespace EnumStringValues
{
  ///==========================================================================
  /// Class : StringValueAttribute
  ///
  /// <summary>
  ///   This attribute is used to represent a string value for a value in an enum.
  /// </summary>
  ///==========================================================================
  [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
  public class StringValueAttribute : Attribute
  {
    public StringValueAttribute(string value, bool preferred = false)
      : this(value, preferred ? PreferenceLevel.High : PreferenceLevel.Default) 
    {
    }

    internal StringValueAttribute(string value, PreferenceLevel preferenceLevel)
    {
      StringValue = value;
      Preference = preferenceLevel;
    }

    internal string StringValue { get; private set; }
    internal PreferenceLevel Preference { get; private set; }
  }

  internal enum PreferenceLevel
  {
    High = 0,
    Default = 1,
    Low = 2
  }

  public class UnmatchedStringValueException : Exception
  {
    public UnmatchedStringValueException(string value, Type type)
      : base(
          string.Format(
          "String does not match to any value of the specified Enum. Attempted to Parse {0} into an Enum of type {1}.",
          value, type.Name)
            )
    {}
  }
}
