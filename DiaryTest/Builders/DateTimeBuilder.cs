using Diary;
using System;

namespace DiaryTest
{
    /// <summary>
    /// Builder factory pattern for DateTime class.
    /// </summary>
    /// <see cref="DiaryBuilder"/>
    internal class DateTimeBuilder : DateBuilder
    {
        private int hours = 0;
        private int minutes = 0;

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
            var date = new Date(base.GetDay(), (Date.Month)base.GetMonth(), base.GetYear());
            var dateTime = new Diary.DateTime(date, hours, minutes);
            return dateTime;
        }
    }
}
