using System;

namespace Diary
{
    /// <summary>
    /// Daily reminders.
    /// </summary>
    public class Reminder: CalendarEvent
    {
        private Date mOccurs;
        private String mDetails;
        
        /// <summary>
        /// Initializes a reminder.
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="label"></param>
        /// <param name="date"></param>
        /// <param name="details"></param>
        public Reminder(ObjectId objectId, String label, Date date, String details) 
            : base(new ClassId("Reminder"), objectId, label)
        {
            mDetails = details;
            // Deep copy the input date to prevent aliasing bugs
            mOccurs = new Date(date.GetDay(), date.GetMonth(), date.GetYear());
        }

        /// <summary>
        /// Returns the reminder details.
        /// </summary>
        /// <returns></returns>
        public String GetDetails()
        {
            return mDetails;
        }

        /// <summary>
        /// Returns false. Reminders do not repeat.
        /// </summary>
        /// <returns></returns>
        public override Boolean IsRepeating()
        {
            return false;
        }
        
        /// <summary>
        /// Returns whether or not the reminder occurs on the date in question
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public override Boolean IsOccuringOn(Date date)
        {
            int compare = date.CompareTo(mOccurs);
            return (compare == 0);
        }
    }
}
