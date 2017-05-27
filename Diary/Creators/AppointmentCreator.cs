using System;

namespace Diary
{
    /// <summary>
    /// Appointment factory.
    /// </summary>
    /// <see cref="DiaryCreator"/>
    public class AppointmentCreator : DiaryCreator
    {
        private Relation1M<Appointment> mAppointments;
        private ContactCreator mContactCreator;

        /// <summary>
        /// Initializes an AppointmentCreator.
        /// </summary>
        public AppointmentCreator() : base(new ClassId("Appointment"))
        {
            mAppointments = new Relation1M<Appointment>();
            mContactCreator = new ContactCreator();
        }

        /// <summary>
        /// Creates an Appointment DiaryProduct.
        /// </summary>
        /// <param name="label"></param>
        /// <param name="occurs"></param>
        /// <param name="durationMinutes"></param>
        /// <param name="details"></param>
        /// <returns></returns>
        public DiaryProduct CreateNew(String label, DateTime occurs, int durationMinutes, String details)
        {
            var objectId = new ObjectId();
            var appointment = new Appointment(objectId, label, occurs, durationMinutes, details);
            mAppointments.Add(appointment);
            return appointment;
        }

        /// <summary>
        /// Recreates an existing Appointment from persistent storage.
        /// </summary>
        /// <param name="objectId"></param>
        /// <returns></returns>
        public override DiaryProduct Create(ObjectId objectId)
        {
            // Check if it already exists
            for (int childIndex = 0; childIndex < mAppointments.GetChildCount(); ++childIndex)
            {
                Appointment appointment = mAppointments.GetChild(childIndex);
                if (appointment.GetObjectId() == objectId)
                {
                    return appointment;
                }
            }

            // Not already loaded ?
            VariableLengthRecord record = Read(objectId);
            if (record != null)
            {
                String label = String.Empty;
                if (record.GetValue(0, ref label))
                {
                    String details = String.Empty;
                    if (record.GetValue(1, ref details))
                    {
                        int offsetValue = 0;
                        if (record.GetValue(2, ref offsetValue))
                        {
                            int totalMinutes = 0;
                            if (record.GetValue(3, ref totalMinutes))
                            {
                                // Convert data offset to real date.
                                var date = new Date();
                                date.AddDays(offsetValue);

                                int hours = totalMinutes / 60;   // Convert total minutes to hours
                                int minutes = totalMinutes % 60; //   and minutes

                                var occurs = new DateTime(date, hours, minutes);

                                int durationMinutes = 0;
                                if (record.GetValue(4, ref durationMinutes))
                                {
                                    var appointment = new Appointment(objectId, label, occurs, durationMinutes, details);

                                    mAppointments.Add(appointment);

                                    LoadContacts(5, appointment, record); // Now take care of any Contacts 

                                    return appointment;
                                }
                            }
                        }
                    }
                }
            }

            return null;  // No such appointment
        }

        /// <summary>
        /// Saves Appointments in memory to persistent storage.
        /// </summary>
        public override void Save()
        {
            // Step through any previously loaded records and save to file.
            for (int childIndex = 0; childIndex < mAppointments.GetChildCount(); ++childIndex)
            {
                Appointment appointment = mAppointments.GetChild(childIndex);

                var record = new VariableLengthRecord();
                record.AppendValue(appointment.GetLabel());   //#1
                record.AppendValue(appointment.GetDetails());  //#2

                var datetime = appointment.GetStartTime();
                record.AppendValue(datetime.GetDate().DaysUntil(new Date())); // #3
                int totalMinutes = datetime.GetHours() * 60 + datetime.GetMinutes();
                record.AppendValue(totalMinutes);   //#4
                record.AppendValue(appointment.GetDurationMinutes());   //#4

                // Now take care of contacts appointment might be related to
                SaveContacts(appointment, record);

                Write(appointment.GetObjectId(), record);
            }

        }

        protected void SaveContacts(Appointment appointment, VariableLengthRecord record)
        {
            Relation1M<Contact> contacts = appointment.GetContacts();
            record.AppendValue(contacts.GetChildCount());   //#5  //might be a 0
            for (int contactIndex = 0; contactIndex < contacts.GetChildCount(); ++contactIndex)
            {
                record.AppendValue(contacts.GetChild(contactIndex).GetObjectId());   //#6 - N  
            }
        }

        protected int LoadContacts(int startingField, Appointment appointment, VariableLengthRecord record)
        {
            int contactCount = 0;
            if (record.GetValue(startingField, ref contactCount) && contactCount > 0)
            {
                int endingField = startingField + 1 + contactCount;
                for (int field = startingField + 1; field < endingField; field++)
                {
                    var objectId = new ObjectId();
                    if (record.GetValue(field, ref objectId))
                    {
                        var contact = (Contact)mContactCreator.Create(objectId);
                        appointment.AddRelation(contact);
                    }
                }
                return endingField;  // Next possible field
            }
            return startingField + 1;  // Should not get here
        }
    }
}
