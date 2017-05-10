using System;

namespace Diary
{
    /// <summary>
    /// Handles generic date time functionality.
    /// </summary>
    /// <see cref="Date">For more detailed design explanations.</see>
    public class DateTime : IComparable
    {
        private Date mDate;
        private int mElapsedMinutes;

        private const int MINUTESINHOUR = 60;
        private const int MINUTESINDAY = MINUTESINHOUR * 24;

        #region Constructors
        /// <summary>
        /// Creates arbitrary default datetime value.
        /// </summary>
        public DateTime() : this(new Date(), 0, 0) { }

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
            mDate = new Date(date.GetDay(), date.GetMonth(), date.GetYear());
            this.AddTime(hours, minutes);
        }

        /// <summary>
        /// Initializes DateTime based on inputs.
        /// </summary>
        /// <param name="dateTime"></param>
        public DateTime(DateTime dateTime) : this(dateTime.GetDate(), dateTime.GetHours(), dateTime.GetMinutes()) { }
        #endregion

        #region Accessors
        /// <summary>
        /// Returns the calendar date.
        /// </summary>
        /// <returns></returns>
        public Date GetDate()
        {
            return new Date(mDate.GetDay(), mDate.GetMonth(), mDate.GetYear());
        }

        /// <summary>
        /// Returns the hours portion of the time. 
        /// </summary>
        /// <returns></returns>
        public int GetHours()
        {
            return (mElapsedMinutes / MINUTESINHOUR);
        }

        /// <summary>
        /// Returns the minutes portion of the time.
        /// </summary>
        /// <returns></returns>
        public int GetMinutes()
        {
            return (mElapsedMinutes % MINUTESINHOUR);
        }
        #endregion

        #region Comparisons
        /// <summary>
        /// Returns how the current DateTime sorts in comparison to the input compareDate.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        /// <see cref="Date.CompareTo(object)">For further explanation about the CompareTo method behavior.</see>
        public int CompareTo(object dateTime)
        {
            if (!(dateTime is DateTime))
            {
                throw new ArgumentException("Object is not a DateTime");
            }
            var compare = dateTime as DateTime;

            var result = mDate.CompareTo(compare.GetDate());

            if (result == 0)
            {
                if (mElapsedMinutes > compare.mElapsedMinutes)
                {
                    result = 1;
                }
                else if (mElapsedMinutes < compare.mElapsedMinutes)
                {
                    result = -1;
                }
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
            var isBetween = (this.CompareTo(start) >= 0) && (this.CompareTo(end) <= 0);

            return isBetween;
        }
        #endregion

        #region Math
        /// <summary>
        /// Adds the specified time to the DateTime.
        /// </summary>
        /// <param name="hours"></param>
        /// <param name="minutes"></param>
        public void AddTime(int hours, int minutes)
        {
            var minutesToAdd = (hours * MINUTESINHOUR) + minutes + mElapsedMinutes;
            var daysToAdd = minutesToAdd / MINUTESINDAY;
            var elapsedMinutes = minutesToAdd % MINUTESINDAY;
            if (elapsedMinutes < 0)
            {
                daysToAdd--;
                elapsedMinutes = MINUTESINDAY + elapsedMinutes;
            }

            mDate.AddDays(daysToAdd);
            mElapsedMinutes = elapsedMinutes;
        }
        #endregion
    }
}