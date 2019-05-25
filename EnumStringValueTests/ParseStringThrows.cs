using System;
using NUnit.Framework;
using EnumStringValues;
using FluentAssertions;

namespace EnumStringValueTests
{
    [TestFixture]
    public class ParseStringThrows
    {
        [Test]
        public void WhenStringIsNull()
        {
            Action parseAttempt = () => EnumExtensions.ParseToEnum<TestEnum>(null);
            parseAttempt.ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void WhenStringIsUnmatched()
        {
            Action parseAttempt = () => EnumExtensions.ParseToEnum<TestEnum>("InvalidStringValue");
            parseAttempt.ShouldThrow<UnmatchedStringValueException>();
        }

        [Test]
        public void WhenInvokedAsAnExtensionMethodOnString()
        {
            Action parseAttempt = () => "InvalidStringValue".ParseToEnum<TestEnum>();
            parseAttempt.ShouldThrow<UnmatchedStringValueException>();
        }

        [Test]
        public void WhenInvokedAsAnExtensionMethodOnList()
        {
            Action parseAttempt = () => new[] { "1", "InvalidStringValue" }.ParseToEnumList<TestEnum>();
            parseAttempt.ShouldThrow<UnmatchedStringValueException>();
        }

        [Test]
        public void AnExceptionWithTheExpectedText()
        {
            Action parseAttempt = () => EnumExtensions.ParseToEnum<TestEnum>("InvalidStringValue");
            parseAttempt.ShouldThrow<Exception>().WithMessage("String does not match to any value of the specified Enum. Attempted to Parse InvalidStringValue into an Enum of type TestEnum.");
        }
    }
}