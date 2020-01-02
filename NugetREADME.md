EnumStringValues
================

Library to allow conversion between an Enum Value and a string, in both directions.
Implemented as an Attribute to be applied to Enum fields to define a string, and methods to extract the defined string given the enum or provide the matching given a string.
Enum name is registered as a default stringValue everywhere.
All operations are cached, to save on reflection overheads. But this can be disabled, with `EnumExtensions.Behaviour.UseCaching`, if desired.

Breaking Change Log (3.0 -> 4.0)
----------------------------------------------
- .NET 3.5 support is entirely dropped. Please use the last 3.2.* build.
- There are 2 changes which change behaviour of the library, though do not cause compile-time errors. They are:
   - Caching.
      - This is now active by default, which will change the CPU vs RAM profile of EnumStringValues. See docs below for how to disable caching, if desired.
   - Use of Enum Literal Name.
      - The literal name is now always included by default, which might affect behaviour in some edge cases.
      - Again, this behaviour is controllable, see docs below for how to adjust this behaviour, if desired.

Breaking Change Log (2.0 -> 3.0)
--------------------------------
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


/* Enable Caching. */
EnumExtensions.Behaviour.UseCaching = true;
("EnumWithoutAnyCustomStringValue").ParseToEnum<exampleEnum>()
               // Does Work
("EnumWithoutAnyCustomStringValue").ParseToEnum<exampleEnum>()
               // Does not do Work, as result is already cached.
EnumExtensions.Behaviour.UseCaching = false;
("EnumWithoutAnyCustomStringValue").ParseToEnum<exampleEnum>()
               // Returns to doing Work again


/* Modify behavior with regard underlying enum name. */
EnumExtensions.Behaviour.ShouldIncludeUnderlyingName = UnderlyingNameUsed.Never;
("EnumWithoutAnyCustomStringValue").ParseToEnum<exampleEnum>()               // Fails
 exampleEnum.EnumWithoutAnyCustomStringValue.GetStringValue()                // returns null
("EnumWithAStringValueDefined").ParseToEnum<exampleEnum>()                   // Fails
 exampleEnum.EnumWithAStringValueDefined.GetAllStringValues()                // returns only "AValue"

EnumExtensions.Behaviour.ShouldIncludeUnderlyingName = UnderlyingNameUsed.IfNoOverrideGiven;
("EnumWithoutAnyCustomStringValue").ParseToEnum<exampleEnum>()               // Suceeds
 exampleEnum.EnumWithoutAnyCustomStringValue.GetStringValue()                // returns "EnumWithoutAnyCustomStringValue"
("EnumWithAStringValueDefined").ParseToEnum<exampleEnum>()                   // Fails
 exampleEnum.EnumWithAStringValueDefined.GetAllStringValues()                // returns only "AValue"

EnumExtensions.Behaviour.ShouldIncludeUnderlyingName = UnderlyingNameUsed.Always;
("EnumWithoutAnyCustomStringValue").ParseToEnum<exampleEnum>()               // Suceeds
 exampleEnum.EnumWithoutAnyCustomStringValue.GetStringValue()                // returns "EnumWithoutAnyCustomStringValue"
("EnumWithAStringValueDefined").ParseToEnum<exampleEnum>()                   // Suceeds
 exampleEnum.EnumWithAStringValueDefined.GetAllStringValues()                // returns only "AValue" and "EnumWithAStringValueDefined"
```
