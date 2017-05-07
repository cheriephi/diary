using Diary;
using System;

namespace DiaryTest
{
    /// <summary>
    /// Appointment creation method.
    /// </summary>
    /// <seealso cref="ReminderBuilder">For more context on the problem.</seealso>
    internal class AppointmentBuilder
    {
        private String label = "";
        private Diary.DateTime occurs = new Diary.DateTime();
        private int durationMinutes = 0;
        private String details = "";

        internal AppointmentBuilder SetLabel(String label)
        {
            this.label = label;
            return this;
        }

        internal AppointmentBuilder SetOccurs(Diary.DateTime occurs)
        {
            this.occurs = new Diary.DateTime(occurs);
            return this;
        }

        internal AppointmentBuilder SetDurationMinutes(int durationMinutes)
        {
            this.durationMinutes = durationMinutes;
            return this;
        }

        internal AppointmentBuilder SetDetails(String details)
        {
            this.details = details;
            return this;
        }

        internal Appointment Build()
        {
            // Add descriptive info to the label if we don't have an explicit one provided. This provide identifying information for debugging.
            if (label == String.Empty)
            {
                label = String.Format("Label for Event <{0}>.", DateTimeTest.ToString(occurs));
            }

            var appointment = new Appointment(label, occurs, durationMinutes, details);
            return appointment;
        }
    }
}
