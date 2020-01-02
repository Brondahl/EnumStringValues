﻿using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
// ReSharper disable RedundantTypeArgumentsOfMethod
#pragma warning disable IDE0034 // Simplify 'default' expression


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
    ///==========================================================================
    /// Class : EnumExtensions.Behaviour
    ///
    /// <summary>
    ///   Static class containing Behaviour configuration for EnumExtensions.
    /// </summary>
    ///==========================================================================
    public static class Behaviour
    {
      /// <summary>
      /// Enum to list possible behaviours
      /// </summary>
      public enum UnderlyingNameUsed
      {
  #pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        Never,
        IfNoOverrideGiven,
        Always,
  #pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
      }

      /// <summary>
      /// Controls whether Caching should be used. Defaults to false.
      /// </summary>
      public static UnderlyingNameUsed ShouldIncludeUnderlyingName
      {
        get => shouldIncludeUnderlyingName;
        set { shouldIncludeUnderlyingName = value; if(UseCaching) { ResetCaches(); } }
      }
      private static UnderlyingNameUsed shouldIncludeUnderlyingName = UnderlyingNameUsed.Always;


      /// <summary>
      /// Controls whether Caching should be used. Defaults to false.
      /// </summary>
      public static bool UseCaching
      {
        get => useCaching;
        set { useCaching = value; if (value) { ResetCaches(); } }
      }
      private static bool useCaching = true;


      static Behaviour()
      {
        ResetCaches();
      }

      /// <summary>
      /// Method for use in testing.
      /// Needs to be public to be used in the test project.
      /// Not a problem to expose to the user, but never valuable (as long as the tests pass :) )
      /// So won't be documented and will be hidden from Intellisense.
      /// </summary>
      [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
      public static void ResetCaches()
      {
        enumValuesDictionary = new ConcurrentDictionary<Type, IEnumerable>();
        enumStringValuesDictionary = new ConcurrentDictionary<Enum, List<StringValueAttribute>>();
        parsedEnumStringsDictionaryByType = new ConcurrentDictionary<Type, ConcurrentDictionary<string, Enum>>();
      }
    }

    /// <summary> Cache for <see cref="EnumerateValues{TEnumType}"/> </summary>
    private static ConcurrentDictionary<Type, IEnumerable> enumValuesDictionary;

    /// <summary> Returns an IEnumerable{T} of the possible values in the enum </summary>
    public static IEnumerable<TEnumType> EnumerateValues<TEnumType>() where TEnumType : System.Enum
    {
      var enumTypeObject = typeof(TEnumType);
      IEnumerable values;

      if (Behaviour.UseCaching)
      {
        values = enumValuesDictionary.GetOrAdd(enumTypeObject, Enum.GetValues);
      }
      else
      {
        values = Enum.GetValues(enumTypeObject);
      }

      return values.Cast<TEnumType>();
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
    private static IEnumerable<StringValueAttribute> GetStringValuesWithPreferences(this Enum enumValue)
    {
      if (Behaviour.UseCaching)
      {
        return enumStringValuesDictionary.GetOrAdd(enumValue, GetStringValuesWithPreferences_Uncached);
      }
      else
      {
        return GetStringValuesWithPreferences_Uncached(enumValue);
      }
    }

    private static List<StringValueAttribute> GetStringValuesWithPreferences_Uncached(this Enum enumValue)
    {
      List<StringValueAttribute> stringValueAttributes =
        enumValue
          .GetType()
          .GetField(enumValue.ToString())
          .GetCustomAttributes(typeof(StringValueAttribute), false)
          .Cast<StringValueAttribute>()
          .ToList();

      if (
        Behaviour.ShouldIncludeUnderlyingName == Behaviour.UnderlyingNameUsed.Always ||
        (!stringValueAttributes.Any() && Behaviour.ShouldIncludeUnderlyingName != Behaviour.UnderlyingNameUsed.Never)
      )
      {
        stringValueAttributes.Add(new StringValueAttribute(enumValue.ToString(), PreferenceLevel.Low));
      }

      return stringValueAttributes;
    }

    /// <summary> Cache for <see cref="GetStringValuesWithPreferences"/> </summary>
    private static ConcurrentDictionary<Enum, List<StringValueAttribute>> enumStringValuesDictionary;

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
    public static TEnumType ParseToEnum<TEnumType>(this string stringValue) where TEnumType : System.Enum
    {
      // ReSharper disable once RedundantTypeArgumentsOfMethod
      if (TryParseStringValueToEnum<TEnumType>(stringValue, out TEnumType lRet))
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
    public static List<TEnumType> ParseToEnumList<TEnumType>(this IEnumerable<string> stringValueCollection) where TEnumType : System.Enum
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
    public static bool TryParseStringValueToEnum<TEnumType>(this string stringValue, out TEnumType parsedValue) where TEnumType : System.Enum
    {
      if (stringValue == null)
      {
        throw new ArgumentNullException(nameof(stringValue), "Input string may not be null.");
      }

      var lowerStringValue = stringValue.ToLower();
      if (!Behaviour.UseCaching)
      {
        return TryParseStringValueToEnum_Uncached(lowerStringValue, out parsedValue);
      }

      return TryParseStringValueToEnum_ViaCache(lowerStringValue, out parsedValue);
    }

    /// <remarks>
    /// This is a little more complex than one might hope, because we also need to cache the knowledge of whether the parse succeeded or not.
    /// We're doing that by storing `null`, if the answer is 'No'. And decoding that, specifically.
    /// </remarks>
    private static bool TryParseStringValueToEnum_ViaCache<TEnumType>(string lowerStringValue, out TEnumType parsedValue) where TEnumType : System.Enum
    {
      var enumTypeObject = typeof(TEnumType);

      var typeAppropriateDictionary = parsedEnumStringsDictionaryByType.GetOrAdd(
        enumTypeObject, 
        (x) => BuildCacheDictionaryForParseStringValue<TEnumType>()
      );

      if (typeAppropriateDictionary.TryGetValue(lowerStringValue, out var cachedValue))
      {
        parsedValue = (TEnumType)cachedValue;
        return true;
      }
      else
      {
        parsedValue = default(TEnumType);
        return false;
      }
    }

    /// <summary> Cache for <see cref="TryParseStringValueToEnum{TEnumType}"/> </summary>
    private static ConcurrentDictionary<Type, ConcurrentDictionary<string, Enum>> parsedEnumStringsDictionaryByType;


    private static ConcurrentDictionary<string, Enum> BuildCacheDictionaryForParseStringValue<TEnumType>() where TEnumType : System.Enum
    {
      var dict = new ConcurrentDictionary<string, Enum>();

      foreach (var enumValue in EnumerateValues<TEnumType>())
      {
        foreach (var enumString in GetStringValues<TEnumType>(enumValue))
        {
            // Add to the dictionary, just overwriting if the string is already present.
            // This overwrite is legitimate, because we've declared parsing a duplicate string definition to be `undefined behaviour`.
            dict.AddOrUpdate(enumString.ToLower(), enumValue, ((repeatedString, previousEnumValue) => enumValue));
        }
      }

      return dict;
    }


    private static bool TryParseStringValueToEnum_Uncached<TEnumType>(this string lowerStringValue, out TEnumType parsedValue) where TEnumType : System.Enum
    {
      foreach (var enumValue in EnumerateValues<TEnumType>())
      {
        var enumStrings = GetStringValues<TEnumType>(enumValue).Select(text => text.ToLower());

        if (enumStrings.Contains(lowerStringValue))
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
    private static IEnumerable<string> GetStringValues<TEnumType>(TEnumType enumValue) where TEnumType : System.Enum
    {
      return (enumValue as Enum).GetAllStringValues();
    }
  }
}
#pragma warning restore IDE0034 // Simplify 'default' expression
