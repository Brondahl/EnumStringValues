using System;
using FluentAssertions;
using NUnit.Framework;

namespace EnumStringValues.Tests
{
    [TestFixture]
    public class ParseStringThrows
    {
        [Test]
        public void WhenStringIsNull()
        {
            Action parseAttempt = (() => EnumExtensions.ParseStringValueToEnum<TestEnum>(null));
            parseAttempt.ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void WhenStringIsUnmatched()
        {
            Action parseAttempt = (() => EnumExtensions.ParseStringValueToEnum<TestEnum>("InvalidStringValue"));
            parseAttempt.ShouldThrow<UnmatchedStringValueException>();
        }

        [Test]
        public void WhenInvokedAsAnExtensionMethodOnString()
        {
            Action parseAttempt = (() => ("InvalidStringValue").ParseToEnum<TestEnum>());
            parseAttempt.ShouldThrow<UnmatchedStringValueException>();
        }

        [Test]
        public void WhenInvokedAsAnExtensionMethodOnList()
        {
            Action parseAttempt = (() => (new[] { "1", "InvalidStringValue" }).ParseToEnumList<TestEnum>());
            parseAttempt.ShouldThrow<UnmatchedStringValueException>();
        }

        [Test]
        public void WhenTypePassedIntoTryParseIsNotAnEnum()
        {
            int x;
            Action parseAttempt = (() => EnumExtensions.TryParseStringValueToEnum<int>("IrrelevantStringValue", out x));
            parseAttempt.ShouldThrow<InvalidOperationException>();
        }

        [Test]
        public void WhenTypePassedIntoParseIsNotAnEnum()
        {
            Action parseAttempt = (() => EnumExtensions.ParseStringValueToEnum<int>("IrrelevantStringValue"));
            parseAttempt.ShouldThrow<InvalidOperationException>();
        }

        [Test]
        public void AnExceptionWithTheExpectedText()
        {
            Action parseAttempt = (() => EnumExtensions.ParseStringValueToEnum<TestEnum>("InvalidStringValue"));
            parseAttempt.ShouldThrow<Exception>().WithMessage("String does not match to any value of the specified Enum. Attempted to Parse InvalidStringValue into an Enum of type eTestEnum.");
        }
    }
}