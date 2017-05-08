using Diary;
using System;

namespace DiaryTest
{
    /// <summary>
    /// Periodic Appointment creator.
    /// </summary>
    /// <seealso cref="ReminderBuilder">For more context on the problem.</seealso>
    internal class PeriodicAppointmentBuilder : AppointmentBuilder
    {
        private Diary.DateTime notToExceedDateTime = new Diary.DateTime();
        private int periodHours = 0;
        
        internal PeriodicAppointmentBuilder SetNotToExceedDateTime(Diary.DateTime notToExceedDateTime)
        {
            this.notToExceedDateTime = new Diary.DateTime(notToExceedDateTime);
            return this;
        }

        internal PeriodicAppointmentBuilder SetPeriodHours(int periodHours)
        {
            this.periodHours = periodHours;
            return this;
        }

        internal override Appointment Build()
        {
            var periodicAppointment = new PeriodicAppointment(base.GetLabel(), base.GetOccurs(), base.GetDurationMinutes(), notToExceedDateTime, periodHours, base.GetDetails());
            return periodicAppointment;
        }
    }
}
