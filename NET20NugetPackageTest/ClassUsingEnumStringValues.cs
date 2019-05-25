using EnumStringValues;
using NUnit.Framework;
using FluentAssertions;

namespace NET20NugetPackageTest
{
  [TestFixture]
  public class ClassUsingEnumStringValues
  {
    [Test]
    public void IsFunctional()
    {
      Test.Entry.GetStringValue().Should().Be("Value");
      EnumExtensions.EnumerateValues<Test>();
    }

    public enum Test
    {
      [StringValue("Value")]
      Entry
    }
  }
}
