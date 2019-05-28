using System;
using System.Diagnostics;

namespace EnumStringValueTests
{
  class Timer
  {
    public static long Time<T1,T2>(Func<T1,T2> act, T1 input, int reps)
    {
      return Time((Action)(() => act(input)), reps);
    }

    public static long Time<T>(Func<T> act, int reps)
    {
      return Time((Action)(() => act()), reps);
    }

    public static long Time(Action act, int reps)
    {
      var stopwatch = new Stopwatch();
      stopwatch.Start();
      for (int i = 0; i < reps; i++)
      {
        act();
      }
      stopwatch.Stop();
      return stopwatch.ElapsedTicks;
    }
  }
}
