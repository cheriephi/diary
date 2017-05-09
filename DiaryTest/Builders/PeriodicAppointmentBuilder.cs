using Diary;

namespace DiaryTest
{
    /// <summary>
    /// Periodic Appointment creator.
    /// </summary>
    /// <seealso cref="ReminderBuilder">For more details on the Builder pattern.</seealso>
    internal class PeriodicAppointmentBuilder : AppointmentBuilder
    {
        private Diary.DateTime notToExceedDateTime = new Diary.DateTime();
        private int periodHours = 0;
        
        internal PeriodicAppointmentBuilder SetNotToExceedDateTime(Diary.DateTime notToExceedDateTime)
        {
            // Directly use the reference passed so we can later test for aliasing bugs.
            this.notToExceedDateTime = notToExceedDateTime;
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
