using System;
using System.Collections.Generic;
using System.Linq;

namespace EnumStringValues
{
  ///==========================================================================
  /// Class : EnumExtensions
  ///
  /// <summary>
  ///   Contains Enum Extension methods.
  /// </summary>
  ///==========================================================================
  public static class EnumExtensions
  {
    public static IEnumerable<TEnumType> EnumerateValues<TEnumType>()
    {
      return Enum.GetValues(typeof(TEnumType)).Cast<TEnumType>();
    }

    ///==========================================================================
    /// Public Method : GetStringValue
    /// 
    /// <summary>
    ///   Retrieves a Single StringValue associated with the enum.
    ///   Returns the preferred StringValue if multiple are defined.
    /// 
    ///   If no preferred value is specified, or if multiple preferred values are
    ///   specified, returns any arbitrary one of the values (selected from the 
    ///   preferred values in the latter case.
    ///   
    ///   Returns the Enum name, if no StringValue was specified at all.
    /// </summary>
    ///==========================================================================
    public static string GetStringValue(this Enum enumValue)
    {
      return
        enumValue
          .GetStringValuesWithPreferences()
          .OrderBy(attribute => attribute.Preference)
          .Select(attribute => attribute.StringValue)
          .FirstOrDefault();
    }

    ///==========================================================================
    /// Public Method : GetAllStringValues
    /// 
    /// <summary>
    ///   Retrieves the full Collection of StringValues associated with the
    ///   particular enum (including the Enum's name)
    ///   If no StringValues are specified, then returns ONLY the Enum's name.
    /// </summary>
    ///==========================================================================
    public static IEnumerable<string> GetAllStringValues(this Enum value)
    {
      var valuesPreferencePairs = value.GetStringValuesWithPreferences();

      return valuesPreferencePairs.Select(pair => pair.StringValue);
    }

    ///==========================================================================
    /// Method : GetStringValuesWithPreferences
    /// 
    /// <summary>
    ///   Retrieves the StringValueAttributes associated with the enum.
    ///   If no StringValues are specified, defaults to treating the EnumName as
    ///   the implicit StringValue.
    /// </summary>
    ///==========================================================================
    private static IEnumerable<StringValueAttribute> GetStringValuesWithPreferences(this Enum value)
    {
      return value
              .GetType()
              .GetField(value.ToString())
              .GetCustomAttributes(typeof (StringValueAttribute), false)
              .Cast<StringValueAttribute>()
              .DefaultIfEmpty(new StringValueAttribute(value.ToString(), PreferenceLevel.Low));
    }

    ///==========================================================================
    /// Public Extension Method on System.String: ParseToEnum
    /// 
    /// <summary>
    ///   Retrieves the Enum matching the string passed in.
    ///   Throws if no match was found.
    /// </summary>
    /// <remarks>
    ///   "TEnumType : struct, IConvertible" is the closest you can get to
    ///   "TEnumType : Enum"
    /// 
    ///   This should minimise the chances of run-time errors by catching most of
    ///   them at compile time.
    /// </remarks>
    ///==========================================================================
    public static TEnumType ParseToEnum<TEnumType>(this string stringValue) where TEnumType : struct, IConvertible
    {
        TEnumType lRet;
        // ReSharper disable once RedundantTypeArgumentsOfMethod
        if (TryParseStringValueToEnum<TEnumType>(stringValue, out lRet))
        {
          return lRet;
        }
        throw new UnmatchedStringValueException(stringValue, typeof(TEnumType));
    }

    ///==========================================================================
    /// Public Extension Method on List{String}: ParseToEnumList
    /// 
    /// <summary>
    ///   Iterates over the collection of string, calling ParseToEnum on each one.
    /// </summary>
    /// <remarks>
    ///   Will throw if ANY of the values are unmatchable.
    ///  
    ///   Calls ToList() to force immediate instantiation and thus immediate
    ///   failure if any values aren't matched.
    /// </remarks>
    ///==========================================================================
    public static List<TEnumType> ParseToEnumList<TEnumType>(this IEnumerable<string> stringValueCollection) where TEnumType : struct, IConvertible
    {
        return stringValueCollection.Select(ParseToEnum<TEnumType>).ToList();
    }

    ///==========================================================================
    /// Public Method : TryParseStringValueToEnum
    /// 
    /// <summary>
    ///   Retrieves the Enum matching the string passed in.
    ///   Returns true or false according to whether a match was found.
    /// </summary>
    /// <remarks>
    ///   "TEnumType : struct, IConvertible" is the closest you can get to
    ///   "TEnumType : Enum"
    /// 
    ///   This should minimise the chances of run-time errors by catching most of
    ///   them at compile time.
    /// </remarks>
    ///==========================================================================
    public static bool TryParseStringValueToEnum<TEnumType>(this string stringValue, out TEnumType parsedValue) where TEnumType : struct, IConvertible
    {
      Type enumType = typeof (TEnumType);
      if (!enumType.IsEnum)
      {
        throw new InvalidOperationException("Type was not an Enum type.");
      }

      if (stringValue == null)
      {
        throw new ArgumentNullException("stringValue", "Input string may not be null.");
      }

      foreach (var enumValue in EnumerateValues<TEnumType>())
      {
       // ReSharper disable once RedundantTypeArgumentsOfMethod
        var enumStrings = GetStringValues<TEnumType>(enumValue).Select(text => text.ToLower());
        var inputString = stringValue.ToLower();

        if (enumStrings.Contains(inputString))
        {
          parsedValue = enumValue;
          return true;
        }
      }
      parsedValue = default(TEnumType);
      return false;
    }


    ///==========================================================================
    /// Method : GetStringValues
    /// 
    /// <summary>
    ///   Retrieves the StringValues associated with the particular Enum provided.
    ///   Returns an empty collection if none are specified.
    /// </summary>
    /// <remarks>
    ///   "TEnumType : struct, IConvertible" is the closest you can get to
    ///   "TEnumType : Enum"
    /// 
    ///   This should minimise the chances of run-time errors by catching most of
    ///   them at compile time.
    /// </remarks>
    ///==========================================================================
    private static IEnumerable<string> GetStringValues<TEnumType>(TEnumType enumValue) where TEnumType : struct, IConvertible
    {
      // ReSharper disable once SuspiciousTypeConversion.Global
      // ReSharper disable once ExpressionIsAlwaysNull
      return (enumValue as Enum).GetAllStringValues();
    }
  }
}
