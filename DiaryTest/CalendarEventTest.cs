﻿using Diary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DiaryTest
{
    /// <summary>
    /// Uses polymorphism to exercise the derived class under test.
    /// </summary>
    [TestClass]
    public class CalendarEventTest
    {
        /// <summary>
        /// Tests the IsOccuringOn method by testing around its expected boundaries.
        /// </summary>
        /// <param name="calendarEvent"></param>
        /// <param name="expectedStartDate"></param>
        /// <param name="expectedEndDate"></param>
        internal static void IsOccuringOnTest(CalendarEvent calendarEvent, Date expectedStartDate, Date expectedEndDate)
        {
            var expectedBeforeStartDate = new Date(expectedStartDate.GetDay(), expectedStartDate.GetMonth(), expectedStartDate.GetYear());
            expectedBeforeStartDate.AddDays(-1);

            var expectedAfterEndDate = new Date(expectedEndDate.GetDay(), expectedEndDate.GetMonth(), expectedEndDate.GetYear());
            expectedAfterEndDate.AddDays(1);

            var actualBeforeStartDate = calendarEvent.IsOccuringOn(expectedBeforeStartDate);
            var actualStartDate = calendarEvent.IsOccuringOn(expectedStartDate);
            var actualEndDate = calendarEvent.IsOccuringOn(expectedEndDate);
            var actualAfterEndDate = calendarEvent.IsOccuringOn(expectedAfterEndDate);

            var expected = "False;True;True;False";
            var actual = String.Join(";", new Boolean[] { actualBeforeStartDate, actualStartDate, actualEndDate, actualAfterEndDate });

            Assert.AreEqual(expected, actual, "Event Label:<{0}>. expectedStartDate:<{1}>. expectedEndDate:<{2}>.", calendarEvent.GetLabel(), Helper.ToString(expectedStartDate), Helper.ToString(expectedEndDate));
        }
    }
}
