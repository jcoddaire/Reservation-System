using System;
using System.Threading;

namespace Booking.Helpers
{
    //took this from https://stackoverflow.com/a/11473510
    //There is a problem with generating random numbers in tight loops.
    public static class StaticRandom
    {
        private static int seed;

        private static ThreadLocal<Random> threadLocal = new ThreadLocal<Random>
            (() => new Random(Interlocked.Increment(ref seed)));

        static StaticRandom()
        {
            seed = Environment.TickCount;
        }

        public static Random Instance { get { return threadLocal.Value; } }
    }
}
