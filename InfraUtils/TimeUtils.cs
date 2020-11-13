using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace InfraUtils
{
    public static class TimeUtils
    {
        public enum TimeSpanFormat
        {
            FullFormat,
            BiggestValue
        }
        
        public static TimeSpan CalcTimeLeft(DateTime? expiryDate)
        {
            return expiryDate.HasValue ? (expiryDate.Value - DateTime.Now) : TimeSpan.MaxValue;
        }
        
        public static string GetElapsedTimeString(Stopwatch stopWatch)
        {
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = string.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            return elapsedTime;
        }
        
        public static string TimeSpanToString(TimeSpan timeSpan, TimeSpanFormat timeSpanFormat = TimeSpanFormat.FullFormat)
        {
            if (timeSpan == TimeSpan.Zero)
            {
                return "0 seconds";
            }

            int years = timeSpan.Days / 365;
            int days = timeSpan.Days - (years * 365);
            int months = days / 30;
            days = days - 30 * months;
            
            List<string> elements = new List<string>();
            AddElement(years, "year", elements);
            AddElement(months, "month", elements);
            AddElement(days, "day", elements);
            AddElement(timeSpan.Hours, "hour", elements);
            AddElement(timeSpan.Minutes, "minute", elements);
            AddElement(timeSpan.Seconds, "second", elements);
            
            if (timeSpanFormat == TimeSpanFormat.FullFormat)
            {
                elements = FormatStringsListAppropriateToTimeRemainning(elements, timeSpan);
                if (elements?.Count > 2)
                {
                    string res = Utils.ListAsString(elements.GetRange(0, elements.Count - 1));
                    res = $"{res} and {elements.Last()}";
                    return res;
                }
                return Utils.ListAsString(elements, " and ");
            }
            return elements.FirstOrDefault();
        }

        private static List<string> FormatStringsListAppropriateToTimeRemainning(List<string> elements, TimeSpan timeSpan)
        {
            List<string> results = new List<string>();

            int years = timeSpan.Days / 365;
            int days = timeSpan.Days - (years * 365);
            int months = days / 30;
            days = days - 30 * months;

            int hours = timeSpan.Hours;
            int minutes = timeSpan.Minutes;
            int seconds = timeSpan.Seconds;

            if (years < 1 && timeSpan.Days < 1)
            {
                AddElement(hours, "hour", results);
                AddElement(minutes, "minute", results);
                AddElement(seconds, "second", results);
            }

            if (years < 1 && timeSpan.Days < 30 && timeSpan.Days >= 1)
            {
                AddElement(days, "day", results);
                AddElement(hours, "hour", results);
                AddElement(minutes, "minute", results);
            }

            if (years < 1 && timeSpan.Days >= 30)
            {
                AddElement(months, "month", results);
                AddElement(days, "day", results);
                AddElement(hours, "hour", results);
            }
            if (years >= 1)
            {
                AddElement(years, "year", results);
                AddElement(months, "month", results);
                AddElement(days, "day", results);
            }

            return results;
        }

        private static void AddElement(int value, string name, List<string> elements)
        {
            if (value > 0)
            {
                elements.Add(string.Format("{0} {1}{2}", value, name, value > 1 ? "s" : ""));
            }
        }
    }
}