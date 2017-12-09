using EnumStringValues;
using NUnit.Framework;

namespace NET35NugetPackageTest
{
  [TestFixture]
  public class ClassUsingEnumStringValues
  {
    [Test]
    public void IsFunctional()
    {
      //.NET 3.5 doesn't support Fluent Assertions, apparently.
      Assert.That(Test.Entry.GetStringValue(), Is.EqualTo("Value"));
    }

    public enum Test
    {
      [StringValue("Value")]
      Entry
    }
  }
}
