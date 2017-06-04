using System;

namespace Diary
{
    /// <summary>
    /// Appointments.
    /// </summary>
    public class Appointment : CalendarEvent
    {
        private DateTime mStarts;   // Data and time the event starts.
        private int mDurationMinutes;
        private String mDetails;  // String to be associated with notification.
        private Relation1M<Contact> mContacts;

        /// <summary>
        /// Creates an appointment.
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="label"></param>
        /// <param name="occurs"></param>
        /// <param name="durationMinutes"></param>
        /// <param name="details"></param>
        public Appointment(ObjectId objectId, String label, DateTime occurs, int durationMinutes, String details) 
            : this(new ClassId("Appointment"), objectId, label, occurs, durationMinutes, details) {}

        /// <summary>
        /// Creates an appointment.
        /// </summary>
        /// <param name="classId"></param>
        /// <param name="objectId"></param>
        /// <param name="label"></param>
        /// <param name="occurs"></param>
        /// <param name="durationMinutes">Negative minutes are transformed to zero.</param>
        /// <param name="details"></param>
        protected Appointment(ClassId classId, ObjectId objectId, String label, DateTime occurs, int durationMinutes, String details) : base(classId, objectId, label)
        {
            mStarts = new DateTime(occurs);
            mDurationMinutes = durationMinutes;
            mDetails = details;
            mContacts = new Relation1M<Contact>();
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

        #region Accessors
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

        /// <summary>
        /// Returns the appointment detail text.
        /// </summary>
        /// <returns></returns>
        public String GetDetails()
        {
            return mDetails;
        }

        /// <summary>
        /// Returns contacts associated with the appointment.
        /// </summary>
        /// <returns></returns>
        public Relation1M<Contact> GetContacts()
        {
            return mContacts;
        }
        #endregion

        /// <summary>
        /// Adds the input contact into the relations for the appointment.
        /// </summary>
        /// <param name="contact"></param>
        public void AddRelation(Contact contact)
        {
            mContacts.Add(contact);
        }
    }
}
