using System;

namespace Diary
{
    /// <summary>
    /// Reminder factory.
    /// </summary>
    /// <see cref="DiaryCreator"/>
    public class ReminderCreator : DiaryCreator
    {
        private Relation1M<Reminder> mReminders;

        /// <summary>
        /// Initializes a ReminderCreator.
        /// </summary>
        public ReminderCreator() : base(new ClassId("Reminder"))
        {
            mReminders = new Relation1M<Reminder>();
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
            var objectId = new ObjectId();
            var reminder = new Reminder(objectId, label, date, details);
            mReminders.Add(reminder);

            return reminder;
        }

        /// <summary>
        /// Recreates an existing Reminder from persistent storage.
        /// </summary>
        /// <param name="objectId"></param>
        /// <returns></returns>
        public override DiaryProduct Create(ObjectId objectId)
        {
            // Check if it already exists in memory.
            for (int childIndex = 0; childIndex < mReminders.GetChildCount(); ++childIndex)
            {
                Reminder reminder = mReminders.GetChild(childIndex);
                if (reminder.GetObjectId() == objectId)
                {
                    return reminder;
                }
            }

            // Not already loaded ? 
            VariableLengthRecord record = Read(objectId);
            if (record != null)
            {
                var label = String.Empty;
                if (record.GetValue(0, ref label))
                {
                    int offsetValue = 0;
                    if (record.GetValue(1, ref offsetValue))
                    {
                        var date = new Date();
                        date.AddDays(offsetValue);
                        var details = String.Empty;
                        if (record.GetValue(2, ref details))
                        {
                            var reminder = new Reminder(objectId, label, date, details);

                            mReminders.Add(reminder);
                            return reminder;
                        }
                    }
                }
            }

            return null;  // No such reminder 
        }

        /// <summary>
        /// Saves Reminders in memory to persistent storage.
        /// </summary>
        public override void Save()
        {
            // Step through any previously loaded records and save to file. 
            for (int childIndex = 0; childIndex < mReminders.GetChildCount(); ++childIndex)
            {
                Reminder reminder = mReminders.GetChild(childIndex);

                var record = new VariableLengthRecord();
                record.AppendValue(reminder.GetLabel());
                record.AppendValue(new Date().DaysUntil(new Date())); //TODO: No accessor for Reminder.Date
                record.AppendValue(reminder.GetDetails());

                Write(reminder.GetObjectId(), record);
            }
        }
    }
}
