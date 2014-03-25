using System;

namespace MDMUtils.Enums
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
    public StringValueAttribute(string value)
    {
      StringValue = value;
    }

    public StringValueAttribute(string value, bool preferred)
      :this (value)
    {
      Preferred = preferred;
    }

    public string StringValue { get; protected set; }
    public bool Preferred { get; protected set; }
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
