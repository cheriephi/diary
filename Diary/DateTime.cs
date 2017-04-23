using System;

namespace Diary
{
    /// <summary>
    /// Handles generic date time functionality.
    /// </summary>
    /// <see cref="Date">For more detailed design explanations</see>
    public class DateTime : IComparable
    {
        private long mTotalMinutes;

        private const long MINUTESINHOUR = 60;
        private const long MINUTESINDAY = MINUTESINHOUR * 24;

        #region Constructors
        /// <summary>
        /// Creates arbitrary default datetime value.
        /// </summary>
        public DateTime() : this(new Date(), 0, 0) {}
        
        /// <summary>
        /// Initializes DateTime based on inputs. Handles overflow / underflows.
        /// For example, if more than 24 hours in a day are passed in; this increases the date.
        /// If a negative time value is passed in, it gets adjusted into the equivalent positive value.
        /// </summary>
        /// <param name="date"></param>
        /// <param name="hours"></param>
        /// <param name="minutes"></param>
        public DateTime(Date date, int hours, int minutes)
        {
            mTotalMinutes = GetTotalMinutes(date, hours, minutes);
        }

        /// <summary>
        /// Initializes DateTime based on inputs.
        /// </summary>
        /// <param name="dateTime"></param>
        public DateTime(DateTime dateTime) : this(dateTime.GetDate(), dateTime.GetHours(), dateTime.GetMinutes()) {}
        #endregion

        #region Accessors
        /// <summary>
        /// Returns the calendar date.
        /// </summary>
        /// <returns></returns>
        public Date GetDate()
        {
            var julianNumber = (int)(mTotalMinutes / MINUTESINDAY);

            int day = 0;
            int month = 0;
            int year = 0;
            Date.FromJulianNumber(julianNumber, ref day, ref month, ref year);

            return new Date(day, (Date.Month)month, year);
        }

        /// <summary>
        /// Returns the hours portion of the time. 
        /// </summary>
        /// <returns></returns>
        public int GetHours()
        {
            var minutes = mTotalMinutes % MINUTESINDAY;
            return (int)(minutes / MINUTESINHOUR);
        }

        /// <summary>
        /// Returns the associated number of Julian minutes based on the inputs
        /// </summary>
        /// <param name="date"></param>
        /// <param name="hours"></param>
        /// <param name="minutes"></param>
        /// <returns></returns>
        private long GetTotalMinutes(Date date, int hours, int minutes)
        {
            var julianNumber = (long)(Date.ToJulianNumber(date.GetDay(), (int)date.GetMonth(), date.GetYear()));

            var julianMinutes = (julianNumber * MINUTESINDAY);

            return julianMinutes + ((long)hours * MINUTESINHOUR) + (long)minutes;
        }

        /// <summary>
        /// Returns the minutes portion of the time.
        /// </summary>
        /// <returns></returns>
        public int GetMinutes()
        {
            return (int)(mTotalMinutes % MINUTESINHOUR);
        }
        #endregion

        #region Comparisons
        /// <summary>
        /// Returns how the current DateTime sorts in comparison to the input compareDate
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        /// <see cref="Date.CompareTo(object)">For further explanation</see>
        public int CompareTo(object dateTime)
        {
            if (!(dateTime is DateTime))
            {
                throw new ArgumentException("Object is not a DateTime");
            }
            var compare = dateTime as DateTime;

            var compareMinutes = GetTotalMinutes(compare.GetDate(), compare.GetHours(), compare.GetMinutes());

            int result = 0;
            if (mTotalMinutes > compareMinutes)
            {
                result = 1;
            }
            else if (mTotalMinutes < compareMinutes)
            {
                result = -1;
            }

            return result;
        }

        /// <summary>
        /// Returns whether the current DateTime is between the input start and end DateTimes, using inclusive logic.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public Boolean IsBetween(DateTime start, DateTime end)
        {
            var startMinutes = GetTotalMinutes(start.GetDate(), start.GetHours(), start.GetMinutes());
            var endMinutes = GetTotalMinutes(end.GetDate(), end.GetHours(), end.GetMinutes());

            Boolean isBetween = (mTotalMinutes >= startMinutes && mTotalMinutes <= endMinutes);

            return isBetween;
        }
        #endregion
    }
}
