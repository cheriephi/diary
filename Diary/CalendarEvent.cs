using System;

namespace Diary
{
    /// <summary>
    /// Base class for all types of diary events.
    /// </summary>
    public abstract class CalendarEvent : DiaryProduct
    {
        private String mLabel;

        /// <summary>
        /// Creates a calendar event.
        /// </summary>
        /// <param name="classId"></param>
        /// <param name="objectId"></param>
        /// <param name="label"></param>
        /// <remarks>Events occuring in the past are not handled different than those in the future.</remarks>
        public CalendarEvent(ClassId classId, ObjectId objectId, String label) : base(classId, objectId)
        {
            mLabel = label;
        }
        
        /// <summary>
        /// Returns the event label.
        /// </summary>
        /// <returns></returns>
        public String GetLabel()
        {
            return mLabel;
        }

        /// <summary>
        /// Returns whether the event is repeating.
        /// </summary>
        /// <returns></returns>
        public abstract Boolean IsRepeating();

        /// <summary>
        /// Returns whether the event occurs on the date in question.
        /// </summary>
        /// <remarks>sic "Occuring".</remarks>
        /// <param name="date"></param>
        /// <returns></returns>
        public abstract Boolean IsOccuringOn(Date date);
    }
}
