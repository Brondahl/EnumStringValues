using NUnit.Framework;
using EnumStringValues;

namespace EnumStringValueTests
{
  [TestFixture(true)]
  [TestFixture(false)]
  public abstract class EnumStringValueTestBase
  {
    private readonly bool cachingActiveByDefault;
    protected EnumStringValueTestBase(bool cachingActiveByDefault)
    {
      this.cachingActiveByDefault = cachingActiveByDefault;
    }

    [SetUp]
    public void ResetCaching()
    {
      EnumExtensions.Behaviour.ResetCaches();
      EnumExtensions.Behaviour.UseCaching = cachingActiveByDefault;
    }
  }
}