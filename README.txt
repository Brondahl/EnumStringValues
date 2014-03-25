Library to allow conversion between an Enum Value and a string, in both directions.
Implemented as an Attribute to be applied to Enum fields.


============================
= Rationale behind library =
============================

This library was initially constructed to allow an easy way to define the string value associated with each value of an Enumeration. 
You can convert an Enum value to a string very easily, but only if the string you want is the name of that value - this library allows you to define any string and easily convert from that Enum to that string. The scenario in my case was wanting a piece of text to display to the user that wasn't forced to be CamelCase.

A natural extension of that was then to allow you to convert in the opposite direction - Given a string and an Enum Type return the enum that matches to that string. The scenario here, was reading a datafile into a program, and wanting to have one of the properties as an Enum.

Finally, it then seemed to be useful to allow you to define multiple possible strings that match to the Enum - so that it could handle the possibility that multiple different inputs should actually map to the same Enum.

Once you're defining multiple strings for each enum, you then need to know which one to use when converting from enum to string, so add a property to mark one of those String Values as the 'Preferred' string value.


=========
= Usage =
=========

/* Defining Mappings. */
public enum exampleEnum
{
  EnumWithoutAnyStringValue

  [StringValue("1")]
  EnumWithAStringValueDefined

  [StringValue("2", true),
   StringValue("Two")]
  EnumWithMultipleStringValueDefinedAndOneMarkedAsPreferred
}


/* Mapping from Enum to string. */
exampleEnum.EnumWithAStringValueDefined.GetStringValue()
               // returns "1"

exampleEnum.EnumWithMultipleStringValueDefinedAndOneMarkedAsPreferred,.GetStringValue() 
               // returns "4"


/* Mapping from string to Enum. */
EnumExtensions.ParseStringValueToEnumInt<exampleEnum>("1")
               // returns exampleEnum.EnumWithAStringValueDefined

EnumExtensions.ParseStringValueToEnumInt<exampleEnum>("2")
               // returns exampleEnum.EnumWithMultipleStringValueDefinedAndOneMarkedAsPreferred

EnumExtensions.ParseStringValueToEnumInt<exampleEnum>("Two")
               // also returns exampleEnum.EnumWithMultipleStringValueDefinedAndOneMarkedAsPreferred



=========================
= Errors and Edge Cases =
=========================
Further details to follow.
