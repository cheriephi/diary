using Diary;
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
        /// <seealso cref="DateTimeTest.InputDateTimeConstructorTest">For more context on the problem.</seealso>
        [TestMethod]
        public void ConstructorAliasingTest()
        {
            var date = new Date();
            var reminder = (Reminder)new ReminderBuilder().SetDate(date).Build();

            var expected = true;
            var actual = reminder.IsOccuringOn(date);
            Assert.AreEqual(expected, actual, "Original");

            date.AddDays(1);

            expected = false;
            actual = reminder.IsOccuringOn(date);

            Assert.AreEqual(expected, actual, "After");
        }

        /// <summary>
        /// Tests the Reminder accessors through its constructor.
        /// </summary>
        [TestMethod]
        public void ReminderConstructorTest()
        {
            var builder = new ReminderBuilder();
            builder.SetLabel("Test Label");
            builder.SetDetails("Test Details");

            Helper.AssertAreEqual(builder, (Reminder)builder.Build(), "");
        }

        /// <summary>
        /// Simple data test of IsRepeating method.
        /// </summary>
        [TestMethod]
        public void IsRepeatingTest()
        {
            var expected = false;

            var reminder = (Reminder)new ReminderBuilder().Build();

            CalendarEventTest.IsRepeatingTest(reminder, expected);
        }

        /// <summary>
        /// Tests the boundaries around the IsOccuring method for a simple scenario.
        /// </summary>
        [TestMethod]
        public void IsOccuringOnTest()
        {
            var reminderDate = new Date(30, Date.Month.SEPTEMBER, 2000);
            var reminder = (Reminder)new ReminderBuilder().SetDate(reminderDate).Build();

            var expectedStartDate = reminderDate;
            var expectedEndDate = new Date(reminderDate.GetDay(), reminderDate.GetMonth(), reminderDate.GetYear());

            CalendarEventTest.IsOccuringOnTest(reminder, expectedStartDate, expectedEndDate);
        }

        #region Persistence Tests
        /// <summary>
        /// Tests the ObjectId accessor.
        /// </summary>
        [TestMethod]
        public void GetObjectIdTest()
        {
            var objectId = new ObjectId();
            var reminder = (Reminder)new ReminderBuilder().SetObjectId(objectId).Build();
            new DiaryProductTest().GetObjectIdTest(reminder, objectId);
        }
        #endregion
    }
}
