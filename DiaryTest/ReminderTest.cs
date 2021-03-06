﻿using Diary;
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
        /// <seealso cref="Helper.AssertAreEqual(AppointmentBuilder, Appointment, string)">For more context on the problem.</seealso>
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
            builder.SetDate(new Date(30, Date.Month.SEPTEMBER, 2000));
            builder.SetDetails("Test Details");

            Helper.AssertAreEqual(builder, (Reminder)builder.Build(), "");
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
