using System;

namespace Diary
{
    /// <summary>
    /// Reminder factory.
    /// </summary>
    /// <see cref="DiaryCreator"/>
    public class ReminderCreator : DiaryCreator
    {
        /// <summary>
        /// Initializes a ReminderCreator.
        /// </summary>
        public ReminderCreator() : base(new ClassId(""))
        {
        }

        /// <summary>
        /// Creates a Reminder DiaryProduct.
        /// </summary>
        /// <param name="label"></param>
        /// <param name="date"></param>
        /// <param name="details"></param>
        /// <returns></returns>
        public DiaryProduct CreateNew(String label, Date date, String details)
        {
            return new Reminder(new ObjectId(), "", new Date(), "");
        }

        /// <summary>
        /// Recreates an existing Reminder from persistent storage.
        /// </summary>
        /// <param name="objectId"></param>
        /// <returns></returns>
        public override DiaryProduct Create(ObjectId objectId)
        {
            return new Reminder(new ObjectId(), "", new Date(), "");
        }

        /// <summary>
        /// Saves Reminders in memory to persistent storage.
        /// </summary>
        public override void Save()
        {
        }
    }
}
