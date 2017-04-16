using System;

namespace Diary
{
    /// <summary>
    /// Handles generic date functionality
    /// </summary>
    public class Date: IComparable
    {
        /// <summary>
        /// Julian Day Number
        /// </summary>
        private int mJulianNumber;

        #region Enums
        /// <summary>
        /// Calendar Months
        /// </summary>
        public enum Month
        {
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
        }

        /// <summary>
        /// Calendar days of week
        /// </summary>
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
        public Date() : this(1, Month.JANUARY, 1900) { }

        /// <summary>
        /// Validates and initializes Date
        /// </summary>
        /// <param name="day"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <remarks>Only the simplest validations currently implemented due to project scope</remarks>
        public Date(int day, Month month, int year)
        {
            //Validate inputs
            var lastDayOfMonth = GetLastDayOfMonth(month, year);
            if (day < 1 || day > lastDayOfMonth)
            {
                throw new ArgumentOutOfRangeException("day", String.Format("Parameter value: <{0}>.", day));
            }

            //Initialize data
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
            int day = 0;
            int month = 0;
            int year = 0;
            
            FromJulianNumber(mJulianNumber, ref day, ref month, ref year);

            return day;
        }

        /// <summary>
        /// Returns the calendar month.
        /// </summary>
        /// <returns></returns>
        public Month GetMonth()
        {
            int day = 0;
            int month = 0;
            int year = 0;

            FromJulianNumber(mJulianNumber, ref day, ref month, ref year);

            return (Month)month;
        }

        /// <summary>
        /// Returns the calendar year number.
        /// </summary>
        /// <returns></returns>
        public int GetYear()
        {
            int day = 0;
            int month = 0;
            int year = 0;

            FromJulianNumber(mJulianNumber, ref day, ref month, ref year);

            return year;
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
        #endregion

        #region Comparisons
        /// <summary>
        /// Returns how the current Date sorts in comparison to the input compareDate
        /// </summary>
        /// <remarks>Implements IComparable</remarks>
        /// <param name="compareDate"></param>
        /// <returns>
        /// -1 if the current date sorts before
        /// 0 for equal sort
        /// 1 if the current date sorts after
        /// </returns>
        /// <remarks>Evaluates sort order by Julian Number comparison</remarks>
        public int CompareTo(object compareDate) {
            Date compare = compareDate as Date;
            var compareJulianNumber = ToJulianNumber(compare.GetDay(), (int)compare.GetMonth(), compare.GetYear());

            int result = 0;
            if (mJulianNumber > compareJulianNumber)
            {
                result = 1;
            }
            else if (mJulianNumber < compareJulianNumber)
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
        /// <remarks>Julian Number comparisons are used</remarks>
        public Boolean IsBetween(Date start, Date end) {
            var startJulianNumber = ToJulianNumber(start.GetDay(), (int)start.GetMonth(), start.GetYear());
            var endJulianNumber = ToJulianNumber(end.GetDay(), (int)end.GetMonth(), end.GetYear());

            Boolean isBetween = (mJulianNumber >= startJulianNumber && mJulianNumber <= endJulianNumber);
   
            return isBetween;
        }
        #endregion

        #region Math
        /// <summary>
        /// Adds the specified number of days to the Date
        /// </summary>
        /// <param name="days"></param>
        public void AddDays(int days)
        {
            mJulianNumber += days;
        }

        /// <summary>
        /// Subtracts the specified number of days to the Date
        /// </summary>
        /// <param name="days"></param>
        public void SubtractDays(int days)
        {
            mJulianNumber -= days;
        }
        #endregion

        #region Date Lookups
        /// <summary>
        /// Returns whether or not the input year is a leap year
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        /// <remarks>Implemented by julian number conversion, subtracting a day from the first of March and determining if the month has 29 days</remarks>
        public static bool IsLeapYear(int year)
        {
            var firstDayofMarchJulianNumber = ToJulianNumber(1, 3, year);

            int leapYearDay = 0;
            int leapYearMonth = 0;
            int leapYearYear = 0;

            FromJulianNumber(firstDayofMarchJulianNumber - 1, ref leapYearDay, ref leapYearMonth, ref leapYearYear);

            bool isLeapYear = (leapYearDay == 29);

            return isLeapYear;
        }

        /// <summary>
        /// Returns the last calendar day number of the input month and year
        /// </summary>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        /// <remarks>Implemented via julian number conversion and subtracting a day from the first of the next month</remarks>
        public static int GetLastDayOfMonth(Month month, int year)
        {
            var firstDayOfNextMonthJulianNumber = ToJulianNumber(1, (int)month + 1, year);

            int lastDayOfMonthDay = 0;
            int lastDayOfMonthMonth = 0;
            int lastDayOfMonthYear = 0;

            FromJulianNumber(firstDayOfNextMonthJulianNumber - 1, ref lastDayOfMonthDay, ref lastDayOfMonthMonth, ref lastDayOfMonthYear);

            return lastDayOfMonthDay;
        }
        #endregion
    }
}
