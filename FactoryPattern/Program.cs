using System;

namespace FactoryPattern
{
    internal class Program
    {
        private static void Main()
        {
            var durationHolder = DurationFactory.FromMilliseconds(50);

            Console.WriteLine(durationHolder.DurationInTicks);

            Console.ReadKey();
        }
    }

    public class DurationClass
    {
        public readonly long DurationInTicks;

        public DurationClass(long p_dur)
        {
            DurationInTicks = p_dur;
        }
    }

    public class DurationFactory
    {
        private const long TicksInSecond = 10000000; //or whatever
        private const long TicksInMillisecond = 10000;

        private readonly long _ticks;

        private DurationFactory(long p_ticks)
        {
            this._ticks = p_ticks;
        }

        //what we are solving by providing static methods is the "nightmare of ctor overloadings"
        //because if we have had regular non static class DurationProvider
        //we would implement two or more ctors for each type of duration we would want to create
        // ctor DurationProvider(long p_seconds), ctor DurationProvider(long p_milliseconds) and so on, but we cant!
        public static DurationClass FromSeconds(long p_seconds)
        {
            return new DurationClass(p_seconds*TicksInSecond);
        }

        public static DurationClass FromMilliseconds(long p_milliseconds)
        {
            return new DurationClass(p_milliseconds*TicksInMillisecond);
        }

        //...
    }

    public class DurationProvider
    {
        private const long ticksInSecond = 10000000; //or whatever
        private const long ticksInMillisecond = 10000;

        private readonly long _ticks;

        //!!!!!we could not do that, because we try to overload ctor with the same signature

//        public DurationProvider(long p_seconds)
//        {
//            this._ticks = p_seconds*ticksInSecond;
//        }
//
//        public DurationProvider(long p_milliseconds)
//        {
//            this._ticks = p_milliseconds*ticksInMillisecond;
//        }
    }
}