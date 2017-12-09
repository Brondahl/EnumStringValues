EnumStringValues
================

Library to allow conversion between an Enum Value and a string, in both directions.
Implemented as an Attribute to be applied to Enum fields to define a string, and methods to extract the defined string given the enum or provide the matching given a string.
Enum name is registered as a default stringValue everywhere.

Breaking Change in latest Release (2.0 -> 3.0)
----------------------------------------------
The Deprecated `ParseStringValueToEnum` method has been removed. Please use `ParseToEnum` instead.


Example Usage
-------------

For full Documentation, please see the GitHub page for the project.

```
/* Define Mappings. */
public enum exampleEnum
{
  EnumWithoutAnyCustomStringValue,

  [StringValue("AValue")]
  EnumWithAStringValueDefined,

  [StringValue("2", true),
   StringValue("Two")]
  EnumWithMultipleStringValuesDefinedAndOneMarkedAsPreferred
}


/* Map from Enum to string. */
using EnumStringValues.EnumExtensions;

exampleEnum.EnumWithoutAnyCustomStringValue.GetStringValue()
               // returns "EnumWithoutAnyCustomStringValue"

exampleEnum.EnumWithAStringValueDefined.GetStringValue()
               // returns "AValue"

exampleEnum.EnumWithMultipleStringValueDefinedAndOneMarkedAsPreferred.GetStringValue() 
               // returns "2"


/* Map from string to Enum. */
("EnumWithoutAnyCustomStringValue").ParseToEnum<exampleEnum>()
               // returns exampleEnum.EnumWithoutAnyCustomStringValue

("AValue").ParseToEnum<exampleEnum>()
               // returns exampleEnum.EnumWithAStringValueDefined

("2").ParseToEnum<exampleEnum>()
               // returns exampleEnum.EnumWithMultipleStringValuesDefinedAndOneMarkedAsPreferred

("Two").ParseToEnum<exampleEnum>()
               // also returns exampleEnum.EnumWithMultipleStringValuesDefinedAndOneMarkedAsPreferred
```
