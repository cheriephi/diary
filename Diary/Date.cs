using System;

namespace Diary
{
    /// <summary>
    /// Handles generic date functionality.
    /// </summary>
    public class Date : IComparable<Date>
    {
        /// <summary>
        /// Julian Day Number
        /// </summary>
        private int mJulianNumber;

        #region Enums
        /// <summary>
        /// Calendar Months.
        /// </summary>
        public enum Month
        {
        #pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
            JANUARY = 1,
            FEBRUARY,
            MARCH,
            APRIL,
            MAY,
            JUNE,
            JULY,
            AUGUST,
            SEPTEMBER,
            OCTOBER,
            NOVEMBER,
            DECEMBER
        #pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        }

        /// <summary>
        /// Calendar days of week.
        /// </summary>
        public enum DayOfWeek
        {
        #pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
            SUNDAY,
            MONDAY,
            TUESDAY,
            WEDNESDAY,
            THURSDAY,
            FRIDAY,
            SATURDAY
        #pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates arbitrary default date value.
        /// </summary>
        public Date() : this(1, Month.JANUARY, 1900) { }

        /// <summary>
        /// Validates and initializes Date.
        /// </summary>
        /// <param name="day"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <remarks>Only the simplest validations currently implemented due to project scope.</remarks>
        public Date(int day, Month month, int year)
        {
            // Validate inputs.
            var lastDayOfMonth = GetLastDayOfMonth(month, year);
            if (day < 1 || day > lastDayOfMonth)
            {
                throw new ArgumentOutOfRangeException("day", String.Format("Parameter value: <{0}>.", day));
            }

            // Initialize data.
            this.mJulianNumber = ToJulianNumber(day, (int)month, year);
        }
        #endregion

        #region Accessors
        /// <summary>
        /// Returns the calendar day number (1 - 31).
        /// </summary>
        /// <returns></returns>
        public int GetDay()
        {
            var dateParts = FromJulianNumber(mJulianNumber);

            return dateParts[0];
        }

        /// <summary>
        /// Returns the calendar month.
        /// </summary>
        /// <returns></returns>
        public Month GetMonth()
        {
            var dateParts = FromJulianNumber(mJulianNumber);

            return (Month)dateParts[1];
        }

        /// <summary>
        /// Returns the calendar year number.
        /// </summary>
        /// <returns></returns>
        public int GetYear()
        {
            var dateParts = FromJulianNumber(mJulianNumber);

            return dateParts[2];
        }

        /// <summary>
        /// Returns the calendar day of week.
        /// </summary>
        /// <returns></returns>
        public DayOfWeek GetDayOfWeek()
        {
            var dayOfWeekIndex = (mJulianNumber + 1) % 7;
            var dayOfWeek = (DayOfWeek)dayOfWeekIndex;
            return dayOfWeek;
        }
        #endregion

        #region Julian Conversion
        /// <summary>
        /// Converts the input calendar values to a julian number.
        /// </summary>
        /// <param name="day"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns>Julian Number</returns>
        /// <see href="https://en.wikipedia.org/wiki/Julian_day#Julian_date_calculation"/>
        private static int ToJulianNumber(int day, int month, int year)
        {
            int julianNumber = (1461 * (year + 4800 + (month - 14) / 12)) / 4 +
                (367 * (month - 2 - 12 * ((month - 14) / 12))) / 12 -
                (3 * ((year + 4900 + (month - 14) / 12) / 100)) / 4 +
                day - 32075;
            return julianNumber;
        }

        /// <summary>
        /// Converts the input julian number to a calendar value.
        /// </summary>
        /// <see cref="ToJulianNumber(int, int, int)"/>
        /// <param name="julianNumber"></param>
        /// <returns></returns>
        private static int[] FromJulianNumber(int julianNumber)
        {
            int l = julianNumber + 68569;
            int n = (4 * l) / 146097;
            l = l - (146097 * n + 3) / 4;
            int i = (4000 * (l + 1)) / 1461001;
            l = l - (1461 * i) / 4 + 31;
            int j = (80 * l) / 2447;
            int day = l - (2447 * j) / 80;
            l = j / 11;
            int month = j + 2 - (12 * l);
            int year = 100 * (n - 49) + i + l;

            var dateParts = new int[3] { day, month, year };
            return dateParts;
        }
        #endregion

        #region Comparisons
        /// <summary>
        /// Returns how the current Date sorts in comparison to the input compare Date.
        /// </summary>
        /// <remarks>Implements IComparable.</remarks>
        /// <param name="compare"></param>
        /// <returns>
        /// -1 if the current date sorts before
        /// 0 for equal sort
        /// 1 if the current date sorts after
        /// </returns>
        public int CompareTo(Date compare)
        {
            int result = 0;
            if (mJulianNumber > compare.mJulianNumber)
            {
                result = 1;
            }
            else if (mJulianNumber < compare.mJulianNumber)
            {
                result = -1;
            }

            return result;
        } 

        /// <summary>
        /// Returns whether the current date is between the input start and end dates, using inclusive logic.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public Boolean IsBetween(Date start, Date end)
        {
            var isBetween = (mJulianNumber >= start.mJulianNumber && mJulianNumber <= end.mJulianNumber);
   
            return isBetween;
        }
        #endregion

        #region Math
        /// <summary>
        /// Adds the specified number of days to the Date.
        /// </summary>
        /// <param name="days"></param>
        public void AddDays(int days)
        {
            mJulianNumber += days;
        }

        /// <summary>
        /// Subtracts the specified number of days to the Date.
        /// </summary>
        /// <param name="days"></param>
        public void SubtractDays(int days)
        {
            mJulianNumber -= days;
        }

        /// <summary>
        /// Returns the number of days until the specified date.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public int DaysUntil(Date date)
        {
            return mJulianNumber - date.mJulianNumber;
        }
        #endregion

        #region Date Lookups
        /// <summary>
        /// Returns whether or not the input year is a leap year.
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public static bool IsLeapYear(int year)
        {
            // Look up March 1 of the input year.
            var firstDayofMarch = new Date(1, Month.MARCH, year);
            // Look up the previous day to March 1.
            firstDayofMarch.mJulianNumber -= 1;
            // Return whether or not the day found is equal to 29; a leap year date.
            return (firstDayofMarch.GetDay() == 29);
        }

        /// <summary>
        /// Returns the last calendar day number of the input month and year.
        /// </summary>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public static int GetLastDayOfMonth(Month month, int year)
        {
            // Get the julian number for the first day of the next month.
            var firstDayOfNextMonthJulianNumber = ToJulianNumber(1, (int)month + 1, year);

            // Look up the julian number for the previous day.
            var dateParts = FromJulianNumber(firstDayOfNextMonthJulianNumber - 1);

            // Return the day portion of the value returned.
            return dateParts[0];
        }
        #endregion
    }
}
