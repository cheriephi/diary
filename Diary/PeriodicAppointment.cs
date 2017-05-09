using System;

namespace Diary
{
    /// <summary>
    /// Handles repeating appointments.
    /// </summary>
    public class PeriodicAppointment : Appointment
    {
        /// <summary>
        /// Creates a Periodic Appointment.
        /// </summary>
        /// <param name="label"></param>
        /// <param name="firstOccurs"></param>
        /// <param name="durationMinutes"></param>
        /// <param name="notToExceedDateTime"></param>
        /// <param name="periodHours"></param>
        /// <param name="details"></param>
        public PeriodicAppointment(String label, DateTime firstOccurs, int durationMinutes, DateTime notToExceedDateTime, int periodHours, String details)
            : base(label, firstOccurs, durationMinutes, details)
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
            Boolean isOccuringOn = false;

            // Put a safeguard in case this code has bugs.
            int endlessLoopCounter = 0;
            const int endlessLoopTerminator = 10000;
            
            // Loop through each occurrence; starting with the first occurrence start.
            var occurenceStart = new DateTime(base.GetStartTime());
            while (occurenceStart.CompareTo(mNotToExceedDateTime) <= 0)
            {
                // Calculate the occurrence end based on the duration.
                var occurenceEnd = new DateTime(occurenceStart);
                occurenceEnd.AddTime(0, base.GetDurationMinutes());

                // Evaluate if the input date is within the occurrence window.
                isOccuringOn = date.IsBetween(occurenceStart.GetDate(), occurenceEnd.GetDate());
                // Short-circuit out. The first time we are within the occurrence window; return true.
                if (isOccuringOn) { break; }

                endlessLoopCounter++;
                if (endlessLoopCounter > endlessLoopTerminator) throw new ApplicationException("Infinite loop detected.");

                occurenceStart = new DateTime(occurenceEnd);
                occurenceStart.AddTime(mPeriodHours, 0);

                // Short-circuit out. The first time we are greater than or equal to the date; stop comparing.
                if (occurenceStart.GetDate().CompareTo(date) > 0) { break; }
            }
            return isOccuringOn;
        }

        private DateTime mNotToExceedDateTime;
        private int mPeriodHours;
    }
}
