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

Classes, Methods, etc.
---------------------

Class: `StringValueAttribute`

An Attribute applicable to Enum fields

Methods:
```
StringValueAttribute.ctor(string, bool)
        - Defines a string value to associate with an Enum, and indicates whether it
          is preferred. Second parameter may be omitted and defaults to false.
```

Class: `EnumExtensions`

A Class defining 2 groups of methods:
Extension Methods on System.Enum, and Static helper methods.

```
Methods
    System.Enum.GetStringValue()
        - Returns the String Value associated with an Enum.
          If more than one value is defined returns the "Preferred" string value.
          If no value is defined, returns the name of the Enum.
    System.Enum.GetAllStringValues()
        - Returns an IEnumerable of all the String Values associated with an
          Enum, irrespective of preference.
    EnumExtensions.ParseToEnum<T>(this string)
        - Takes a string and an EnumType and returns the matching Enum value.
          Throws if no match can be made.
    EnumExtensions.ParseToEnumList<T>(this IEnumerable<string>)
        - Takes a collection of strings and an EnumType and returns the matching Enum values.
          Throws if any of the strings are unmatchable.
    EnumExtensions.TryParseStringValueToEnum<T>(string, out T)
        - Mirrors the int.TryParse() pattern.
          Takes a string and an EnumType and checks for a matching Enum value.
          If one exists, populates the out param and returns true. Otherwise returns false.
    EnumExtensions.EnumerateValues<T>()
        - Helper Method that returns all of the values in an EnumType.

Removed
    EnumExtensions.ParseStringValueToEnum<T>(string)
        - This was identical to ParseToEnum, but not exposed as an extension of string, and differently named.
          If you were using it, please now use ParseToEnum.
```

Exceptions and Edge Cases
-------------------------

All the Generic methods are constrained as T: struct, IConvertible, which I believe to be as close to "is an Enum" as one can get in generic Type constraints. There is a further reflection-based check in the code, so calling any of them with T as a non-Enum will throw an InvalidOperationException.

* Calling GetStringValue when no string value is defined
 * ...... will return enum Name
* Calling ParseToEnum<T>() when the string is null
 * ...... will throw an ArgumentNullException
* Calling ParseToEnum<T>() when the string doesn't match anything
 * ...... will throw an UnmatchedStringValueException()
* Calling ParseToEnumList<T>() when any of the strings are null or don't match anything
 * ...... will throw an appropriate Exception depending on the first problem hit.
* Calling GetStringValue when multiple values are defined but none are marked as preferred
 * ...... may return any of the string values.
* Calling GetStringValue when multiple values are marked as preferred
 * ...... may return any of the preferred values.
* Calling ParseToEnum<T>() when the string is defined for multiple Enums
 * ...... may return any of the Enums that it matches.

Note that in all of these 'may return any' cases, I suspect that it will always return the top-most value, but that will be dependant on .NET's implementation of various methods and is not in anyway guaranteed by this library! Frankly, any of these would be a mis-use of the library and could arguably throw instead.

If `TryParseStringValueToEnum` is called and fails, then it will populate the output variable to default(T), likely the first defined value of the Enum.

Rationale behind library
------------------------

This library was initially constructed to allow an easy way to define the string value associated with each value of an Enumeration. 
You can convert an Enum value to a string very easily, but only if the string you want is the name of that value - this library allows you to define any string and easily convert from that Enum to that string. The scenario in my case was wanting a piece of text to display to the user that wasn't forced to be CamelCase.

A natural extension of that was then to allow you to convert in the opposite direction - Given a string and an Enum Type return the enum that matches to that string. The scenario here, was reading a datafile into a program, and wanting to have one of the properties as an Enum.

Finally, it then seemed to be useful to allow you to define multiple possible strings that match to the Enum - so that it could handle the possibility that multiple different inputs should actually map to the same Enum. Once you're defining multiple strings for each enum, you then need to know which one to use when converting from enum to string, so add a property to mark one of those String Values as the '`Preferred`' string value.

TODOs
=====

Feature Requests
----------------

Please email me with any requests that occur to you.
I'll attempt to document any feature requests I receive here, along with any design thoughts or comments on whether they'll happen, etc.

 - Make the Generic "is an Enum" constraint more accurate.
  -	I don't know any way to improve on this, but any ideas are welcome.
 - Some sort of caching so that we're not reflecting all the time.
  - The mappings are compile time constants, so we should only really need to reflect on a given type once and there's no need to worry about cache invalidation etc, so should be fine.
 - Further automation of nupkg handling ... can we get post-build events to handle archiving previous versions and copying the new version into the nuget folder?
  - It would need to know the package version number, wouldn't it?

Version History
----------------

- 3.0 - Convert the project to .Net Standard 2.0
       - Remove the Obsolete `ParseStringValueToEnum` method.

- 2.0  - Make the library use the existing Enum name as its default string value
       - Exposed the Parse methods as extensions on `String` and `List<string>`
       - Added a clone of the basic Parse method renamed as `ParseToEnum<T>`
       - NOTE: The old parse method (`ParseStringValueToEnum`) is now deprecated and will be removed in vNext. `ParseToEnum` is identical and should be used instead.

- 1.0  - Final cosmetic tweaks. (Project is now essentially complete and this is likely the last update in a while)

- 0.11 - Update nuget files on build
- 0.10 - Explicitly support recent major .NET versions.
- 0.9  - Created a nuget package, so committing structure for that.
- 0.8  - Upgrade to .NET 4.5.1
- 0.7  - Make nuget manage the nUnit dependency.
- 0.6  - Fix namespaces which previously related to my personal Utilities project :)
- 0.5  - Rename Parse Methods to reflect the fact that they return Enums, not ints.
- 0.4  - Improve Attribute constructor layout, and adjust access modifiers
- 0.3  - Fix Null string handling.
- 0.2  - Initial Readme.
- 0.1  - Initial Upload.

Dev Guide
=========

This section is *just* notes for myself - I don't do much remote-based git development from home, nor work much on this or on other nuget packages very often, so I usually forget most of what I've learned about getting the setup to workin-between updates. It may, or may not be valuable to anyone else who wants to contribute, but I'm not maintaining it with that in mind. Definitely feel free to add to this section if you think there are better ways to achieve the goals, but I'll be very picky about anything that removes information, for obvious reasons.

Git
---
SSH ought to be being managed by Pageant?
 - Start Pageant
 - Open Pageant UI and 'Add Key'
 - <User>\.ssh\<github ppk>
Doesn't seem to work.
 - key is registered with GH, but not getting accepted.
 $ ssh -vT git@github.com
OpenSSH_4.6p1, OpenSSL 0.9.8e 23 Feb 2007
debug1: Connecting to github.com [192.30.252.131] port 22.
debug1: Connection established.
debug1: identity file /c/Users/Brondahl/.ssh/identity type -1
debug1: identity file /c/Users/Brondahl/.ssh/id_rsa type -1
debug1: identity file /c/Users/Brondahl/.ssh/id_dsa type -1
debug1: Remote protocol version 2.0, remote software version libssh-0.6.0
debug1: no match: libssh-0.6.0
debug1: Enabling compatibility mode for protocol 2.0
debug1: Local version string SSH-2.0-OpenSSH_4.6
debug1: SSH2_MSG_KEXINIT sent
debug1: SSH2_MSG_KEXINIT received
debug1: kex: server->client aes128-cbc hmac-sha1 none
debug1: kex: client->server aes128-cbc hmac-sha1 none
debug1: sending SSH2_MSG_KEXDH_INIT
debug1: expecting SSH2_MSG_KEXDH_REPLY
debug1: Host 'github.com' is known and matches the RSA host key.
debug1: Found key in /c/Users/Brondahl/.ssh/known_hosts:1
debug1: ssh_rsa_verify: signature correct
debug1: SSH2_MSG_NEWKEYS sent
debug1: expecting SSH2_MSG_NEWKEYS
debug1: SSH2_MSG_NEWKEYS received
debug1: SSH2_MSG_SERVICE_REQUEST sent
debug1: SSH2_MSG_SERVICE_ACCEPT received
debug1: Authentications that can continue: publickey
debug1: Next authentication method: publickey
debug1: Trying private key: /c/Users/Brondahl/.ssh/identity
debug1: Trying private key: /c/Users/Brondahl/.ssh/id_rsa
debug1: Trying private key: /c/Users/Brondahl/.ssh/id_dsa
debug1: No more authentication methods to try.
Permission denied (publickey).

doesn't seem to be trying to use github key?
Tried restart the bash prompt.

Gave up and copied "id" key from work - works now (despite not registered with Pageant?)

Creating a new Nuget Package
-----
With .Net Standard, you no longer have to use a nuspec file since all the package information is added to the csproj file.

**Note:** Please make sure you have the latest version of Visual Studio 2017.

- Update the [EnumStringValues.csproj](EnumStringValues/EnumStringValues.csproj) with any new information. This can also be done in the `package` tab of the project properties
  - For example, the new: `PackageVersion`, `PackageReleaseNotes`, etc.
  - new semantic version number
  - release note tag (note no `<>` in here, use XML-escaped version instead - `&gt;`)
- Update README.
- Open your command line and navigate to the root `EnumStringValues` directory.
- Run the NuGet CLI tool included in the project: `.\nuget\nuget.exe restore`
- Use MSBuild to create the new NuGet package: `msbuild .\EnumStringValues\EnumStringValues.csproj /t:pack /p:Configuration=Release`
  - The newly created package will be dropped in the `.\EnumStringValues\bin\Release\` directory.
- Open new package with Nuget Package Explorer, explore to dll, open dll with reflector, verify updates have 'taken'.
- Packages will be stored in `root\nuget`
  - Move old package to `Old` folder
  - Move newly packed package, from the `bin\Release` folder where it was created, to `root\nuget`
  
