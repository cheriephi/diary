using System;

namespace Diary
{
    /// <summary>
    /// PeriodicAppointment factory.
    /// </summary>
    /// <see cref="DiaryCreator"/>
    public class PeriodicAppointmentCreator : DiaryCreator
    {
        private Relation1M<PeriodicAppointment> mPeriodicAppointments;

        /// <summary>
        /// Initializes a PeriodicAppointmentCreator.
        /// </summary>
        public PeriodicAppointmentCreator() : base(new ClassId("PeriodicAppointment"))
        {
            mPeriodicAppointments = new Relation1M<PeriodicAppointment>();
        }

        /// <summary>
        /// Creates a PeriodicAppointment DiaryProduct.
        /// </summary>
        /// <param name="label"></param>
        /// <param name="firstOccurs"></param>
        /// <param name="durationMinutes"></param>
        /// <param name="notToExceedDateTime"></param>
        /// <param name="periodHours"></param>
        /// <param name="details"></param>
        /// <returns></returns>
        public DiaryProduct CreateNew(String label, DateTime firstOccurs, int durationMinutes, DateTime notToExceedDateTime, int periodHours, String details)
        {
            var objectId = new ObjectId();
            var periodicAppointment = new PeriodicAppointment(objectId, label, firstOccurs, durationMinutes, notToExceedDateTime, periodHours, details);
            mPeriodicAppointments.Add(periodicAppointment);
            return periodicAppointment;
        }

        /// <summary>
        /// Recreates an existing PeriodicAppointment from persistent storage.
        /// </summary>
        /// <param name="objectId"></param>
        /// <returns></returns>
        public override DiaryProduct Create(ObjectId objectId)
        {
            // Check if it already exists
            for (int childIndex = 0; childIndex < mPeriodicAppointments.GetChildCount(); ++childIndex)
            {
                PeriodicAppointment periodicAppointment = mPeriodicAppointments.GetChild(childIndex);
                if (periodicAppointment.GetObjectId() == objectId)
                {
                    return periodicAppointment;
                }
            }

            // Not already loaded ?
            VariableLengthRecord record = Read(objectId);
            if (record != null)
            {
                var label = String.Empty;
                if (record.GetValue(0, ref label))
                {
                    var firstOccurs = new DateTime(); 
                    if (record.GetValue(1, ref firstOccurs))
                    {
                        int durationMinutes = 0; 
                        if (record.GetValue(2, ref durationMinutes))
                        {
                            var notToExceedDateTime = new DateTime();
                            if (record.GetValue(3, ref notToExceedDateTime))
                            {
                                int periodHours = 0;
                                if (record.GetValue(4, ref periodHours))
                                 { 
                                    var details = String.Empty;
                                    if (record.GetValue(5, ref details))
                                    {
                                        var periodicAppointment = new PeriodicAppointment(objectId, label, firstOccurs, durationMinutes, notToExceedDateTime, periodHours, details);

                                        mPeriodicAppointments.Add(periodicAppointment);

                                        return periodicAppointment;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return null;  // No such appointment
        }

        /// <summary>
        /// Saves PeriodicAppointments in memory to persistent storage.
        /// </summary>
        public override void Save()
        {
            // Step through any previously loaded records and save to file.
            for (int childIndex = 0; childIndex < mPeriodicAppointments.GetChildCount(); ++childIndex)
            {
                PeriodicAppointment periodicAppointment = mPeriodicAppointments.GetChild(childIndex);

                var record = new VariableLengthRecord();
                record.AppendValue(periodicAppointment.GetLabel());   //#1
                record.AppendValue(periodicAppointment.GetStartTime());   //#2
                record.AppendValue(periodicAppointment.GetDurationMinutes());   //#3
                record.AppendValue(periodicAppointment.GetNotToExceedDateTime());   //#4
                record.AppendValue(periodicAppointment.GetPeriodHours());   //#5
                record.AppendValue(periodicAppointment.GetDetails());  //#6

                Write(periodicAppointment.GetObjectId(), record);
            }

        }
    }
}
