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
        #region Helper Methods
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
        #endregion

        /// <summary>
        /// Simple data test of GetLabel method.
        /// </summary>
        [TestMethod]
        public void GetLabelTest()
        {
            var expected = "Test Label";

            var reminder = new ReminderBuilder().SetLabel(expected).Build();

            var actual = reminder.GetLabel();

            Assert.AreEqual(expected, actual);
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
            var actual = reminder.IsRepeating();

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the boundaries around the IsOccuring method for a simple scenario.
        /// </summary>
        [TestMethod]
        public void IsOccuringOnTest()
        {
            var reminderDate = new Date(30, Date.Month.SEPTEMBER, 2000);
            var reminder = new ReminderBuilder().SetDate(reminderDate).Build();

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
        /// <seealso cref="DateTimeTest.InputDateConstructorAliasingTest">For more context on the problem.</seealso>
        [TestMethod]
        public void IsOccuringOnAliasingTest()
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
    }
}
