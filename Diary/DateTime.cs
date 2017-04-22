namespace Diary
{
    /// <summary>
    /// Handles generic date time functionality.
    /// </summary>
    /// <see cref="Date">For more detailed design explanations</see>
    public class DateTime
    {
        private Date mDate;
        private int mHours;
        private int mMinutes;

        #region Constructors
        /// <summary>
        /// Creates arbitrary default datetime value.
        /// </summary>
        public DateTime() : this(new Date(), 0, 0) {}
        
        /// <summary>
        /// Initializes DateTime based on inputs.
        /// </summary>
        /// <param name="date"></param>
        /// <param name="hours"></param>
        /// <param name="minutes"></param>
        public DateTime(Date date, int hours, int minutes)
        {
            mDate = date;
            mHours = hours;
            mMinutes = minutes;
        }
        #endregion

        #region Accessors
        /// <summary>
        /// Returns the calendar date.
        /// </summary>
        /// <returns></returns>
        public Date GetDate()
        {
            return mDate;
        }

        /// <summary>
        /// Returns the hours portion of the time. 
        /// </summary>
        /// <returns></returns>
        public int GetHours()
        {
            return mHours;
        }

        /// <summary>
        /// Returns the minutes portion of the time.
        /// </summary>
        /// <returns></returns>
        public int GetMinutes()
        {
            return mMinutes;
        }
        #endregion
    }
}
