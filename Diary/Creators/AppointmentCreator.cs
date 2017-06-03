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

        /// <summary>
        /// Initializes an AppointmentCreator.
        /// </summary>
        public AppointmentCreator() : base(new ClassId("Appointment"))
        {
            mAppointments = new Relation1M<Appointment>();
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
                        var occurs = new DateTime();
                        if (record.GetValue(2, ref occurs))
                        {
                            int durationMinutes = 0;
                            if (record.GetValue(3, ref durationMinutes))
                            {
                                var appointment = new Appointment(objectId, label, occurs, durationMinutes, details);

                                mAppointments.Add(appointment);

                                LoadContacts(4, appointment, record); // Now take care of any Contacts 

                                return appointment;
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
                record.AppendValue(appointment.GetStartTime());   //#3
                record.AppendValue(appointment.GetDurationMinutes());   //#4

                // Now take care of contacts appointment might be related to
                SaveContacts(appointment, record);

                Write(appointment.GetObjectId(), record);
            }
        }

        /// <summary>
        /// Saves Contacts in memory to persistent storage.
        /// </summary>
        protected void SaveContacts(Appointment appointment, VariableLengthRecord record)
        {
            Relation1M<Contact> contacts = appointment.GetContacts();
            record.AppendValue(contacts.GetChildCount());   //#4  //might be a 0
            for (int contactIndex = 0; contactIndex < contacts.GetChildCount(); ++contactIndex)
            {
                record.AppendValue(contacts.GetChild(contactIndex).GetObjectId());   //#5 - N  
            }
        }

        /// <summary>
        /// Recreates existing Contacts from persistent storage.
        /// </summary>
        /// <param name="startingField"></param>
        /// <param name="appointment"></param>
        /// <param name="record"></param>
        /// <returns></returns>
        protected int LoadContacts(int startingField, Appointment appointment, VariableLengthRecord record)
        {
            using (var creator = new ContactCreator())
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
                            var contact = (Contact)creator.Create(objectId);
                            appointment.AddRelation(contact);
                        }
                    }
                    return endingField;  // Next possible field
                }
                return startingField + 1;  // Should not get here
            }
        }
    }
}
