using Diary;
using System;

namespace DiaryTest
{
    /// <summary>
    /// Builder factory pattern for Appointment class.
    /// </summary>
    /// <see cref="DiaryBuilder"/>
    internal class AppointmentBuilder : DiaryBuilder
    {
        private ContactBuilder[] contactBuilders = new ContactBuilder[] { };
        private String details = "";
        private int durationMinutes = 0;
        private String label = "";
        private Diary.DateTime occurs = new Diary.DateTime();

        internal ContactBuilder[] GetContactBuilders()
        {
            return contactBuilders;
        }

        /// <summary>
        /// Creates test data for Contact relations.
        /// </summary>
        /// <returns></returns>
        /// <remarks>A factory method. Does not actually attach the data to the created appointment, but makes this data
        /// available to the tests to simplify comparisons.</remarks>
        internal AppointmentBuilder SetContactBuilders()
        {
            contactBuilders = new ContactBuilder[3]
            {
                new ContactBuilder().SetFirstName("Brian").SetLastName("Rothwell").SetContactInfo("brothwell@q.com"),
                new ContactBuilder().SetFirstName("Billy").SetLastName("Bob").SetContactInfo("slingblade@msn.com"),
                new ContactBuilder().SetFirstName("Jenny").SetLastName("Twotone").SetContactInfo("(210) 867-5308")
            };
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
                label = String.Format("Event Label <{0}>.", Helper.ToString(occurs));
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

        internal override DiaryProduct Build()
        {
            var creator = (AppointmentCreator)this.GetCreator();

            Appointment appointment;
            if (creator != null)
            {
                appointment = (Appointment)creator.CreateNew(label, occurs, durationMinutes, details);
            }
            else
            {
                appointment = new Appointment(base.GetObjectId(), label, occurs, durationMinutes, details);
            }

            return appointment;
        }
    }
}
