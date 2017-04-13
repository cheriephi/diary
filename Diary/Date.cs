using System;

namespace Diary
{
    /// <summary>
    /// Handles rudimentary date functionality
    /// </summary>
    public class Date
    {
        private int mJulianNumber; // Julian Day Number
        private int mDay;
        private int mMonth;
        private int mYear;

        #region Enums
        public enum MONTH
        {
            JANUARY,
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
        }

        public enum DayOfWeek
        {
            SUNDAY,
            MONDAY,
            TUESDAY,
            WEDNESDAY,
            THURSDAY,
            FRIDAY,
            SATURDAY
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates arbitrary default date value
        /// </summary>
        public Date() : this(1, 1, 1900) { }

        /// <summary>
        /// Validates and initializes Date
        /// </summary>
        /// <param name="day"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <remarks>Only the simplest validations currently implemented due to project scope</remarks>
        public Date(int day, int month, int year)
        {
            //Validate inputs
            if (day < 1 || day > 31)
            {
                throw new ArgumentOutOfRangeException("day", String.Format("Parameter value: <{0}>.", day));
            }
            if (month < 1 || month > 12)
            {
                throw new ArgumentOutOfRangeException("month", String.Format("Parameter value: <{0}>.", month));
            }

            //Initialize data
            this.mJulianNumber = ToJulianNumber(day, month, year);
            //Initialize the calendar fields as well to reduce the repitition in extracting this from the JulianNumber class
            //This has the down side of creating object versus function scoped fields, but adheres to the Don't Repeat Yourself principle
            FromJulianNumber(mJulianNumber, ref mDay, ref mMonth, ref mYear);
        }
        #endregion

        #region Accessors
        /// <summary>
        /// Returns the calendar day number (1 - 31).
        /// </summary>
        /// <returns></returns>
        public int GetDay()
        {
            return mDay;
        }

        /// <summary>
        /// Returns the calendar month number (1 - 12).
        /// </summary>
        /// <returns></returns>
        public int GetMonth()
        {
            return mMonth;
        }

        /// <summary>
        /// Returns the calendar year number.
        /// </summary>
        /// <returns></returns>
        public int GetYear()
        {
           return mYear;
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

        /// <summary>
        /// Converts the input calendar values to a julian number
        /// </summary>
        /// <param name="day"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns>Julian Number</returns>
        /// <see cref="https://en.wikipedia.org/wiki/Julian_day#Julian_date_calculation"/>
        public static int ToJulianNumber(int day, int month, int year)
        {
            int julianNumber = (1461 * (year + 4800 + (month - 14) / 12)) / 4 +
                (367 * (month - 2 - 12 * ((month - 14) / 12))) / 12 -
                (3 * ((year + 4900 + (month - 14) / 12) / 100)) / 4 +
                day - 32075;
            return julianNumber;
        }

        /// <summary>
        /// Converts the input julian number to a calendar value
        /// </summary>
        /// <remarks>See ToJulianNumber method</remarks>
        /// <param name="julianNumber"></param>
        /// <param name="day"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        public static void FromJulianNumber(int julianNumber, ref int day, ref int month, ref int year)
        {
            int l = julianNumber + 68569;
            int n = (4 * l) / 146097;
            l = l - (146097 * n + 3) / 4;
            int i = (4000 * (l + 1)) / 1461001;
            l = l - (1461 * i) / 4 + 31;
            int j = (80 * l) / 2447;
            day = l - (2447 * j) / 80;
            l = j / 11;
            month = j + 2 - (12 * l);
            year = 100 * (n - 49) + i + l;
        }
    }
}
