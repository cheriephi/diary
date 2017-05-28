using Diary;
using System;

namespace DiaryTest
{
    /// <summary>
    /// Builder factory pattern for Reminder class.
    /// </summary>
    /// <see cref="DiaryBuilder"/>
    internal class ReminderBuilder : DiaryBuilder
    {
        private String label = "";
        private Date date = new Date();
        private String details = "";

        internal ReminderBuilder SetLabel(String label)
        {
            this.label = label;
            return this;
        }

        internal ReminderBuilder SetDate(Date date)
        {
            this.date = new Date(date.GetDay(), date.GetMonth(), date.GetYear());
            return this;
        }

        internal ReminderBuilder SetDetails(String details)
        {
            this.details = details;
            return this;
        }

        internal override DiaryProduct Build()
        {
            var creator = (ReminderCreator)this.GetCreator();

            Reminder reminder;
            if (creator != null)
            {
                reminder = (Reminder)creator.CreateNew(label, date, details);
            }
            else
            {
                reminder = new Reminder(base.GetObjectId(), label, date, details);
            }

            return reminder;
        }
    }
}
