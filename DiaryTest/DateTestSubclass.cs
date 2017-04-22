using Diary;
using System;

namespace DiaryTest
{
    /// <summary>
    /// A test specific subclass to modify the System Under Test (SUT) to improve its testability
    /// </summary>
    class DateTestSubclass : Date
    {
        public DateTestSubclass() : base() {}

        public DateTestSubclass(int day, Month month, int year) : base(day, month, year) {}

        /// <summary>
        /// Formats the input date elements as a string
        /// </summary>
        /// <returns>yyyy-mm-dd</returns>
        public static string ToString(int day, int month, int year)
        {
            return String.Format("{0}-{1}-{2}", year.ToString("0000"), month.ToString("00"), day.ToString("00"));
        }

        /// <summary>
        /// Returns the Date's identifying properties to support meaningful equality checks and debugging
        /// </summary>
        /// <returns>yyyy-mm-dd</returns>
        public override string ToString()
        {
            return ToString(GetDay(), (int)GetMonth(), GetYear());
        }
    }
}
