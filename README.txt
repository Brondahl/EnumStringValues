===========
= Summary =
===========
Library to allow conversion between an Enum Value and a string, in both directions.
Implemented as an Attribute to be applied to Enum fields to define a string, and methods to extract or match against that string.



=================
= Example Usage =
=================

/* Define Mappings. */
public enum exampleEnum
{
  EnumWithoutAnyStringValue,

  [StringValue("1")]
  EnumWithAStringValueDefined,

  [StringValue("2", true),
   StringValue("Two")]
  EnumWithMultipleStringValuesDefinedAndOneMarkedAsPreferred
}


/* Map from Enum to string. */
exampleEnum.EnumWithAStringValueDefined.GetStringValue()
               // returns "1"

exampleEnum.EnumWithMultipleStringValueDefinedAndOneMarkedAsPreferred.GetStringValue() 
               // returns "4"


/* Map from string to Enum. */
EnumExtensions.ParseStringValueToEnumInt<exampleEnum>("1")
               // returns exampleEnum.EnumWithAStringValueDefined

EnumExtensions.ParseStringValueToEnumInt<exampleEnum>("2")
               // returns exampleEnum.EnumWithMultipleStringValuesDefinedAndOneMarkedAsPreferred

EnumExtensions.ParseStringValueToEnumInt<exampleEnum>("Two")
               // also returns exampleEnum.EnumWithMultipleStringValuesDefinedAndOneMarkedAsPreferred


==========================
= Classes, Methods, etc. =
==========================

StringValueAttribute               - Attribute applicable to Enum fields
  StringValueAttribute.ctor(string, bool) - Defines a string value to associate with an Enum, and indicates whether it is preferred.
                                            Second parameter may be omitted and defaults to false.

EnumExtensions                     - Class defining 2 groups of methods: Extension Methods on System.Enum, and static helper methods.
  System.Enum.GetStringValue()            - Returns the String Value associated with an Enum.
                                            If more than one value is defined returns the "Preferred" string value.
  System.Enum.GetAllStringValues()        - Returns an IEnumerable of all the String Values associated with an Enum, irrespective of preference.
  EnumExtensions.ParseStringValueToEnum<T>(string)
                                          - Takes a string and an EnumType and returns the matching Enum value.
                                            Throws if no match can be made.
  EnumExtensions.TryParseStringValueToEnum<T>(string, out T)
                                          - Mirrors the int.TryParse() pattern.
                                            Takes a string and an EnumType and checks for a matching Enum value.
                                            If one exists, populates the out param and returns true. Otherwise returns false.
  EnumExtensions.EnumerateValues<T>()     - Helper Method that returns all of the values in an EnumType.


=============================
= Exceptions and Edge Cases =
=============================

Calling .GetStringValue when no string value is defined  ... will return null. (should maybe return enum Name?)
Calling .ParseStringValueToEnum<T>() when the string is null ... will throw an ArgumentNullException
Calling .ParseStringValueToEnum<T>() when the string doesn't match anything ... will throw an UnmatchedStringValueException()


All the Generic methods are constrained as T: struct, IConvertible, which I believe to be as close to "is an Enum" as one can get in generic Type constraints. There is a further reflection-based check in the code, so calling any of them with T as a non-Enum will throw an InvalidOperationException.


Calling .GetStringValue when more than one value is defined but none are marked as preferred ... may return any of the string values.
Calling .GetStringValue when more than one value is marked as preferred ... may return any of the preferred values.
Calling .ParseStringValueToEnum<T>() when the string is defined for multiple Enums ... may return any of the Enums that it matches.
Note that in all of these cases, I suspect that it will always return the top-most value, but that will be dependant on .NET's implementation of various things and is not in anyway guaranteed by this library! Frankly, any of these would be a mis-use of the library and could arguably throw instead.

If TryParseStringValueToEnumInt is called and fails, then it will will populate the output variable to default(T), likely the first defined value of the Enum.


============================
= Rationale behind library =
============================

This library was initially constructed to allow an easy way to define the string value associated with each value of an Enumeration. 
You can convert an Enum value to a string very easily, but only if the string you want is the name of that value - this library allows you to define any string and easily convert from that Enum to that string. The scenario in my case was wanting a piece of text to display to the user that wasn't forced to be CamelCase.

A natural extension of that was then to allow you to convert in the opposite direction - Given a string and an Enum Type return the enum that matches to that string. The scenario here, was reading a datafile into a program, and wanting to have one of the properties as an Enum.

Finally, it then seemed to be useful to allow you to define multiple possible strings that match to the Enum - so that it could handle the possibility that multiple different inputs should actually map to the same Enum.

Once you're defining multiple strings for each enum, you then need to know which one to use when converting from enum to string, so add a property to mark one of those String Values as the 'Preferred' string value.




=========
= TODOs =
=========
Reformat this README.txt so that it displays better on GitHub.

====================
= Feature Requests =
====================
Please email me with any requests that occur to you.
I'll attempt to document any feature requests I receive here, along with any design thoughts or comments on whether they'll happen, etc.

 - Make the Generic "is an Enum" constraint more accurate.
	I don't know any way to improve on this, but any ideas are welcome.
 - Return enum Name is StringValue not defined.


===================
= Version History =
===================

0.1 - Initial Upload.
0.2 - Initial Readme.
0.3 - Fix Null string handling.
0.4 - Improve Attribute constructor layout, and adjust access modifiers
0.5 - Rename Parse Methods to reflect the fact that they return Enums, not ints.
0.6 - Fix namespaces which previously related to my personal Utilities project :)
