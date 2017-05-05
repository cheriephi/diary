using System;

namespace Diary
{
    /// <summary>
    /// Base class for all types of diary events.
    /// </summary>
    public abstract class CalendarEvent
    {
        /// <summary>
        /// Creates a calendar event
        /// </summary>
        /// <param name="label"></param>
        public CalendarEvent(String label)
        {
            mLabel = label;
        }
        
        /// <summary>
        /// Returns the event label
        /// </summary>
        /// <returns></returns>
        public String GetLabel()
        {
            return mLabel;
        }

        /// <summary>
        /// Returns whether the event is repeating
        /// </summary>
        /// <returns></returns>
        public abstract Boolean IsRepeating();

        /// <summary>
        /// Returns whether the event occurs on the date in question
        /// </summary>
        /// <remarks>sic "Occuring"</remarks>
        /// <param name="date"></param>
        /// <returns></returns>
        public abstract Boolean IsOccuringOn(Date date);

        private String mLabel;
    }
}
