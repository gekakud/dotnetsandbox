using System;

namespace ExtensionMethods
{
    internal class MyExtCheck
    {
        public MyExtCheck(string p_stringForCheck)
        {
            SomeString = p_stringForCheck;
            ShowOriginalValue();
            ApplyMyExtension();
        }

        private string SomeString { get; set; }

        private void ShowOriginalValue()
        {
            if (SomeString != null)
                Console.WriteLine("The original value is:" + SomeString);
        }

        private void ApplyMyExtension()
        {
            if (SomeString != null)
                Console.WriteLine("Number of spaces in current string is:"
                                  + SomeString.GetNumberOfSpaces());
        }
    }

    //String extension!
    public static class MyStringExtension
    {
        public static int GetNumberOfSpaces(this string p_stringToApllyOn)
        {
            var splitStrings = p_stringToApllyOn.Split(' ');
            return splitStrings.Length - 1;
        }
    }
}