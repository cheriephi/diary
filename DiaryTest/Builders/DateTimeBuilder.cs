using Diary;
using System;

namespace DiaryTest
{
    /// <summary>
    /// Builder factory pattern for DateTime class.
    /// </summary>
    /// <see cref="DiaryBuilder"/>
    internal class DateTimeBuilder
    {
        private Date date = new Date();
        private int hours = 0;
        private int minutes = 0;

        internal Date GetDate()
        {
            return date;
        }

        internal DateTimeBuilder SetDate(Date date)
        {
            this.date = date;
            return this;
        }

        internal int GetHours()
        {
            return hours;
        }

        internal DateTimeBuilder SetHours(int hours)
        {
            this.hours = hours;
            return this;
        }

        internal int GetMinutes()
        {
            return minutes;
        }

        internal DateTimeBuilder SetMinutes(int minutes)
        {
            this.minutes = minutes;
            return this;
        }

        internal Diary.DateTime Build()
        {
            var dateTime = new Diary.DateTime(date, hours, minutes);
            return dateTime;
        }
    }
}
