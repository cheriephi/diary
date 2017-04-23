namespace Diary
{
    /// <summary>
    /// Handles generic date time functionality.
    /// </summary>
    /// <see cref="Date">For more detailed design explanations</see>
    public class DateTime
    {
        private long mMinutes;

        private const long MINUTESINHOUR = 60;
        private const long MINUTESINDAY = MINUTESINHOUR * 24;

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
            var julianNumber = (long)(Date.ToJulianNumber(date.GetDay(), (int)date.GetMonth(), date.GetYear()));

            var julianMinutes = (julianNumber * MINUTESINDAY);

            mMinutes = julianMinutes + ((long)hours * MINUTESINHOUR) + (long)minutes;
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
            var julianNumber = (int)(mMinutes / MINUTESINDAY);

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
            var minutes = mMinutes % MINUTESINDAY;
            return (int)(minutes / MINUTESINHOUR);
        }

        /// <summary>
        /// Returns the minutes portion of the time.
        /// </summary>
        /// <returns></returns>
        public int GetMinutes()
        {
            return (int)(mMinutes % MINUTESINHOUR);
        }
        #endregion
    }
}
