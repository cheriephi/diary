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
        }
        
        /// <summary>
        /// Returns true. Periodic appointments are repeating; even if the occurrences evaluate to be just one.
        /// </summary>
        /// <returns></returns>
        public override Boolean IsRepeating()
        {
            return true;
        }
    }
}
