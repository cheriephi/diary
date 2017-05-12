using System;

namespace Diary
{
    /// <summary>
    /// Appointments.
    /// </summary>
    public class Appointment : CalendarEvent
    {
        /// <summary>
        /// Creates an appointment.
        /// </summary>
        /// <param name="label"></param>
        /// <param name="occurs"></param>
        /// <param name="durationMinutes">Negative minutes are transformed to be zero.</param>
        /// <param name="details"></param>
        /// <remarks>details is not part of the accessors; so there is no way to test it.</remarks>
        public Appointment(String label, DateTime occurs, int durationMinutes, String details) : base(label)
        {
            mStarts = new DateTime(occurs);
            mDurationMinutes = durationMinutes;
        }

        /// <summary>
        /// Returns false. Regular appointments do not repeat.
        /// </summary>
        /// <returns></returns>
        public override Boolean IsRepeating()
        {
            return false;
        }

        /// <summary>
        /// Returns whether or not the appointment occurs on the date in question.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        /// <remarks>
        /// If the appointment occurs at any point in the day in question the method will return true.
        /// Even zero length appointments will return true if they occur on the date in question.
        /// </remarks>
        public override Boolean IsOccuringOn(Date date)
        {
            var endTime = GetEndTime();

            var isOccuringOn = date.IsBetween(mStarts.GetDate(), endTime.GetDate());

            return isOccuringOn;
        }

        /// <summary>
        /// Returns the appointment start time.
        /// </summary>
        /// <returns></returns>
        public DateTime GetStartTime()
        {
            return new DateTime(mStarts);
        }

        /// <summary>
        /// Returns the appointment end time.
        /// </summary>
        /// <returns></returns>
        public DateTime GetEndTime()
        {
            var endTime = new DateTime(mStarts);
            endTime.AddTime(0, mDurationMinutes);
            return endTime;
        }

        /// <summary>
        /// Returns the appointment duration in minutes.
        /// </summary>
        /// <returns></returns>
        public int GetDurationMinutes()
        {
            return mDurationMinutes;
        }

        private DateTime mStarts;   // Data and time the event starts.
        private int mDurationMinutes;
        // NOTE: Not used private String mDetails;  // String to be associated with notification.
    }
}
