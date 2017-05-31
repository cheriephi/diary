using System;

namespace Diary
{
    /// <summary>
    /// Handles repeating appointments.
    /// </summary>
    public class PeriodicAppointment : Appointment
    {
        private DateTime mNotToExceedDateTime;
        private int mPeriodHours;

        /// <summary>
        /// Creates a Periodic Appointment.
        /// </summary>
        /// /// <param name="objectId"></param>
        /// <param name="label"></param>
        /// <param name="firstOccurs"></param>
        /// <param name="durationMinutes"></param>
        /// <param name="notToExceedDateTime"></param>
        /// <param name="periodHours"></param>
        /// <param name="details"></param>
        public PeriodicAppointment(ObjectId objectId, String label, DateTime firstOccurs, int durationMinutes, DateTime notToExceedDateTime, int periodHours, String details)
            : base(new ClassId("PeriodicAppointment"), objectId, label, firstOccurs, durationMinutes, details)
        {
            // If the first occurrence exceeds the max occurrence window; throw an error.
            if (base.GetEndTime().CompareTo(notToExceedDateTime) > 0)
            {
                throw new ArgumentOutOfRangeException("durationMinutes", String.Format("Parameter value: <{0}>. Duration exceeds the value date range specified for the periodic appointment.", durationMinutes));
            }

            mNotToExceedDateTime = new DateTime(notToExceedDateTime);
            mPeriodHours = periodHours;
        }

        /// <summary>
        /// Returns true. Periodic appointments are repeating; even if the occurrences evaluate to be just one.
        /// </summary>
        /// <returns></returns>
        public override Boolean IsRepeating()
        {
            return true;
        }

        /// <summary>
        /// Returns whether or not the appointment recurs on the date in question.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public override Boolean IsOccuringOn(Date date)
        {
            // Put a safeguard in case this code has bugs.
            int endlessLoopCounter = 0;
            const int endlessLoopTerminator = 10000;

            // Loop through each occurrence; starting with the first occurrence start.
            var occurenceStartTime = new DateTime(base.GetStartTime());

            while (occurenceStartTime.CompareTo(mNotToExceedDateTime) <= 0)
            {
                // Calculate the occurrence end based on the duration.
                var occurenceEndTime = new DateTime(occurenceStartTime);
                occurenceEndTime.AddTime(0, base.GetDurationMinutes());

                // Evaluate if the input date is within the occurrence window.
                if (date.IsBetween(occurenceStartTime.GetDate(), occurenceEndTime.GetDate()))
                {
                    return true;
                }

                endlessLoopCounter++;
                if (endlessLoopCounter > endlessLoopTerminator) throw new ApplicationException("Infinite loop detected.");

                occurenceStartTime.AddTime(mPeriodHours, 0);
            }

            return false;
        }
    }
}
