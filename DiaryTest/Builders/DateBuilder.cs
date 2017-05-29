using Diary;
using System;

namespace DiaryTest
{
    /// <summary>
    /// Builder factory pattern for Date class.
    /// </summary>
    /// <see cref="DiaryBuilder"/>
    internal class DateBuilder
    {
        private int day = 1;
        private int month = 1;
        private int year = 1900;

        internal int GetDay()
        {
            return day;
        }

        internal DateBuilder SetDay(int day)
        {
            this.day = day;
            return this;
        }

        internal int GetMonth()
        {
            return month;
        }

        internal DateBuilder SetMonth(int month)
        {
            this.month = month;
            return this;
        }

        internal int GetYear()
        {
            return year;
        }

        internal DateBuilder SetYear(int year)
        {
            this.year = year;
            return this;
        }

        internal Date Build()
        {
            var date = new Date(day, (Date.Month)month, year);
            return date;
        }

        /// <summary>
        /// Formats the input elements as a string. Supports meaningful equality checks and debugging.
        /// </summary>
        /// <returns>yyyy-mm-dd</returns>
        public override String ToString()
        {
            return String.Format("{0}-{1}-{2}", year.ToString("0000"), month.ToString("00"), day.ToString("00"));
        }
    }
}
