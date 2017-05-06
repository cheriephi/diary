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
        /// Simple data test of GetLabel method.
        /// </summary>
        [TestMethod]
        public void GetLabelTest()
        {
            var label = "Test Label";
            var expected = label;

            var reminder = new Reminder(label, new Date(), "");
            var actual = reminder.GetLabel();

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Simple data test of GetDetails method.
        /// </summary>
        [TestMethod]
        public void GetDetailsTest()
        {
            var details = "Test Details";
            var expected = details;

            var reminder = new Reminder("", new Date(), details);
            var actual = reminder.GetDetails();

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Simple data test of IsRepeating method.
        /// </summary>
        [TestMethod]
        public void IsRepeatingTest()
        {
            var reminder = new Reminder("", new Date(), "");
            var actual = reminder.IsRepeating();
            var expected = false;

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the boundaries around the IsOccuring method for a simple scenario.
        /// </summary>
        [TestMethod]
        public void IsOccuringOnTest()
        {
            var reminderDate = new Date(30, Date.Month.SEPTEMBER, 2000);
            var reminder = new Reminder("", reminderDate, "");
            
            var actualBefore = reminder.IsOccuringOn(new Date(29, Date.Month.SEPTEMBER, 2000));
            var actualOn = reminder.IsOccuringOn(reminderDate);
            var actualAfter = reminder.IsOccuringOn(new Date(2, Date.Month.SEPTEMBER, 2000));

            var expected = "False;True;False";
            var actual = String.Join(";", new Boolean[] { actualBefore, actualOn, actualAfter });

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests that the Date field passed into the constructor cannot be modified outside the System Under Test.
        /// </summary>
        [TestMethod]
        public void IsOccuringOnAliasingTest()
        {
            var date = new Date();
            var reminder = new Reminder("", date, "");

            var expected = true;
            var actual = reminder.IsOccuringOn(date);
            Assert.AreEqual(expected, actual, "Original");

            date.AddDays(1);
            expected = false;
            actual = reminder.IsOccuringOn(date);

            Assert.AreEqual(expected, actual, "After");
        }
    }
}
