using Diary;

namespace DiaryTest
{
    /// <summary>
    /// Periodic Appointment creator.
    /// </summary>
    /// <see cref="AppointmentBuilder"/>
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

        internal override DiaryProduct Build()
        {
            var periodicAppointment = new PeriodicAppointment(base.GetObjectId(), base.GetLabel(), base.GetOccurs(), base.GetDurationMinutes(), notToExceedDateTime, periodHours, base.GetDetails());
            return periodicAppointment;
        }
    }
}
