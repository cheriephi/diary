using System;

namespace Diary
{
    /// <summary>
    /// PeriodicAppointment factory.
    /// </summary>
    /// <see cref="DiaryCreator"/>
    public class PeriodicAppointmentCreator : DiaryCreator
    {
        /// <summary>
        /// Initializes a PeriodicAppointmentCreator.
        /// </summary>
        public PeriodicAppointmentCreator() : base(new ClassId(""))
        {
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
            return new PeriodicAppointment(new ObjectId(), "", new DateTime(), 0, new DateTime(), 0, "");
        }

        /// <summary>
        /// Recreates an existing PeriodicAppointment from persistent storage.
        /// </summary>
        /// <param name="objectId"></param>
        /// <returns></returns>
        public override DiaryProduct Create(ObjectId objectId)
        {
            return new PeriodicAppointment(new ObjectId(), "", new DateTime(), 0, new DateTime(), 0, "");
        }

        /// <summary>
        /// Saves PeriodicAppointments in memory to persistent storage.
        /// </summary>
        public override void Save()
        {
        }
    }
}
