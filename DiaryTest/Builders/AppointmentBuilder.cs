using Diary;
using System;

namespace DiaryTest
{
    /// <summary>
    /// Appointment creator.
    /// </summary>
    /// <seealso cref="ReminderBuilder">For more details on the Builder pattern.</seealso>
    internal class AppointmentBuilder
    {
        private ObjectId objectId = new ObjectId();
        private String label = "";
        private Diary.DateTime occurs = new Diary.DateTime();
        private int durationMinutes = 0;
        private String details = "";

        internal ObjectId GetObjectId()
        {
            return objectId;
        }

        internal AppointmentBuilder SetObjectId(ObjectId objectId)
        {
            this.objectId = objectId;
            return this;
        }

        internal String GetLabel()
        {
            return label;
        }

        internal AppointmentBuilder SetLabel(String label)
        {
            this.label = label;
            return this;
        }

        internal Diary.DateTime GetOccurs()
        {
            return occurs;
        }

        internal AppointmentBuilder SetOccurs(Diary.DateTime occurs)
        {
            this.occurs = new Diary.DateTime(occurs);

            // Add descriptive info to the label if we don't have an explicit one provided. This provide identifying information for debugging.
            if (label == String.Empty)
            {
                label = String.Format("Event Label <{0}>.", DateTimeTest.ToString(occurs));
            }

            return this;
        }

        internal int GetDurationMinutes()
        {
            return durationMinutes;
        }

        internal AppointmentBuilder SetDurationMinutes(int durationMinutes)
        {
            this.durationMinutes = durationMinutes;
            return this;
        }

        internal String GetDetails()
        {
            return details;
        }

        internal AppointmentBuilder SetDetails(String details)
        {
            this.details = details;
            return this;
        }

        internal virtual Appointment Build()
        {
            var appointment = new Appointment(objectId, label, occurs, durationMinutes, details);
            return appointment;
        }
    }
}
