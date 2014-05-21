using System;
using System.Diagnostics;
using System.Linq;
using SdbBackbone.Extensions;

namespace SdbBackbone.Logging
{
    public class StopWatchLogger
    {
        private string LoggingMethod { get; set; }
        private string TimerName { get; set; }
        private Stopwatch Watch { get; set; }
        public long TotalMs { get { return Watch.ElapsedMilliseconds; } }
        private Guid WatchId { get; set; }

        public StopWatchLogger(string loggingMethodName, string timerName)
        {
            LoggingMethod = loggingMethodName;
            TimerName = timerName;
            WatchId = Guid.NewGuid();
        }

        public void Start()
        {
            //Log("*start*");
            Watch = new Stopwatch();
            Watch.Start();
        }

        public void Stop()
        {
            //Log("*stop*");
            Watch.Stop();
            Log(Watch.Elapsed.ToFriendlyDisplay(2) + " Elapsed");
        }

        public Stopwatch StartSubWatch()
        {
            var watch = new Stopwatch();
            watch.Start();
            return watch;
        }

        public void StopSubWatch(Stopwatch watch, string message)
        {
            watch.Stop();
          //  Logger.Info("            {0} Elapsed: {1}, child of process WID:{2}".FormatWith(message, watch.Elapsed.ToFriendlyDisplay(2), WatchId), Logger.Category.Default);
        }

        public void Log(string logEntry)
        {
         //   Logger.Info(string.Format("{1} {0}-{4} : ({2}) - WID:{3}", LoggingMethod, logEntry, DateTime.UtcNow, WatchId, TimerName), Logger.Category.Default);
        }
    }

    public static class TimeSpanExtensions
    {
        private enum TimeSpanElement
        {
            Millisecond,
            Second,
            Minute,
            Hour,
            Day
        }

        public static string ToFriendlyDisplay(this TimeSpan timeSpan, int maxNrOfElements)
        {
            maxNrOfElements = Math.Max(Math.Min(maxNrOfElements, 5), 1);
            var parts = new[]
                        {
                            Tuple.Create(TimeSpanElement.Day, timeSpan.Days),
                            Tuple.Create(TimeSpanElement.Hour, timeSpan.Hours),
                            Tuple.Create(TimeSpanElement.Minute, timeSpan.Minutes),
                            Tuple.Create(TimeSpanElement.Second, timeSpan.Seconds),
                            Tuple.Create(TimeSpanElement.Millisecond, timeSpan.Milliseconds)
                        }
                                        .SkipWhile(i => i.Item2 <= 0)
                                        .Take(maxNrOfElements);

            return string.Join(", ", parts.Select(p => string.Format("{0} {1}{2}", p.Item2, p.Item1, p.Item2 > 1 ? "s" : string.Empty)));
        }
    }
}