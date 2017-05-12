﻿using Diary;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DiaryTest
{
    /// <summary>
    /// Tests the Reminder class.
    /// </summary>
    /// <seealso cref="ContactTest">For more detailed design explanations.</seealso>
    [TestClass]
    public class ReminderTest
    {
        /// <summary>
        /// Tests that the Date field passed into the constructor cannot be modified outside the System Under Test.
        /// </summary>
        /// <seealso cref="DateTimeTest.InputDateConstructorAliasingTest">For more context on the problem.</seealso>
        [TestMethod]
        public void ConstructorAliasingTest()
        {
            var date = new Date();
            var reminder = new ReminderBuilder().SetDate(date).Build();

            var expected = true;
            var actual = reminder.IsOccuringOn(date);
            Assert.AreEqual(expected, actual, "Original");

            date.AddDays(1);

            expected = false;
            actual = reminder.IsOccuringOn(date);

            Assert.AreEqual(expected, actual, "After");
        }
        
        /// <summary>
        /// Simple data test of GetLabel method.
        /// </summary>
        [TestMethod]
        public void GetLabelTest()
        {
            var expected = "Test Label";

            var reminder = new ReminderBuilder().SetLabel(expected).Build();

            CalendarEventTest.GetLabelTest(reminder, expected);
        }

        /// <summary>
        /// Simple data test of GetDetails method.
        /// </summary>
        [TestMethod]
        public void GetDetailsTest()
        {
            var expected = "Test Details";

            var reminder = new ReminderBuilder().SetDetails(expected).Build();
            var actual = reminder.GetDetails();

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Simple data test of IsRepeating method.
        /// </summary>
        [TestMethod]
        public void IsRepeatingTest()
        {
            var expected = false;

            var reminder = new ReminderBuilder().Build();

            CalendarEventTest.IsRepeatingTest(reminder, expected);
        }

        /// <summary>
        /// Tests the boundaries around the IsOccuring method for a simple scenario.
        /// </summary>
        [TestMethod]
        public void IsOccuringOnTest()
        {
            var reminderDate = new Date(30, Date.Month.SEPTEMBER, 2000);
            var reminder = new ReminderBuilder().SetDate(reminderDate).Build();

            var expectedStartDate = reminderDate;
            var expectedEndDate = new Date(reminderDate.GetDay(), reminderDate.GetMonth(), reminderDate.GetYear());

            CalendarEventTest.IsOccuringOnTest(reminder, expectedStartDate, expectedEndDate);
        }

        #region Persistence Tests
        /// <summary>
        /// Tests the ClassId accessor.
        /// </summary>
        [TestMethod]
        public void GetClassIdTest()
        {
            var reminder = new ReminderBuilder().Build();
            new DiaryProductTest().GetClassIdTest(reminder, "Reminder");
        }

        /// <summary>
        /// Tests the ObjectId accessor.
        /// </summary>
        [TestMethod]
        public void GetObjectIdTest()
        {
            var objectId = new ObjectId();
            var reminder = new ReminderBuilder().SetObjectId(objectId).Build();
            new DiaryProductTest().GetObjectIdTest(reminder, objectId);
        }
        #endregion
    }
}
