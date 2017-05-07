using Diary;
using System;

namespace DiaryTest
{
    /// <summary>
    /// Builder factory pattern (a creational design pattern), to enable anoymous creation of the System Under Test, but parameterize necessary values.
    /// The object can be incrementally built passed on parameters.
    /// This keeps tests clutter free from in-line setup and minimizes obscure tests.
    /// </summary>
    internal class ReminderBuilder
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

        internal Reminder Build()
        {
            var reminder = new Reminder(label, date, details);
            return reminder;
        }
    }
}
