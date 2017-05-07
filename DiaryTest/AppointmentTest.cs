using Diary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DiaryTest
{
    /// <summary>
    /// Tests the Appointment class.
    /// </summary>
    /// <seealso cref="ContactTest">For more detailed design explanations.</seealso>
    /// <seealso cref="DateTest">For more detailed design explanations of data driven testing.</seealso>
    [TestClass]
    public class AppointmentTest
    {
        #region Helper Methods
        /// <summary>
        /// Appointment creation method.
        /// </summary>
        /// <seealso cref="ReminderTest.ReminderBuilder">For more context on the problem.</seealso>
        internal class AppointmentBuilder
        {
            private String label = "";
            private Diary.DateTime occurs = new Diary.DateTime();
            private int durationMinutes = 0;
            private String details = "";

            internal AppointmentBuilder SetLabel(String label)
            {
                this.label = label;
                return this;
            }

            internal AppointmentBuilder SetDateTime(Diary.DateTime occurs)
            {
                this.occurs = new Diary.DateTime(occurs);
                return this;
            }

            internal AppointmentBuilder SetDurationMinutes(int durationMinutes)
            {
                this.durationMinutes = durationMinutes;
                return this;
            }

            internal AppointmentBuilder SetDetails(String details)
            {
                this.details = details;
                return this;
            }

            internal Appointment Build()
            {
                // Add descriptive info to the label if we don't have an explicit one provided. This provide identifying information for debugging.
                if (label == String.Empty)
                {
                    label = String.Format("Label for Event <{0}>.", DateTimeTest.ToString(occurs));
                }

                var appointment = new Appointment(label, occurs, durationMinutes, details);
                return appointment;
            }
        }
        #endregion

        /// <summary>
        /// Required context for data driven testing.
        /// </summary>
        public TestContext TestContext { get; set; }


        /// <summary>
        /// Tests that the StartTime field passed into the constructor cannot be modified outside of its accessor.
        /// </summary>
        /// <seealso cref="DateTimeTest.InputDateConstructorAliasingTest">For more context on the problem.</seealso>
        [TestMethod]
        public void ConstructorAliasingTest()
        {
            var dateTime = new Diary.DateTime();
            var appointment = new AppointmentBuilder().SetDateTime(dateTime).Build();

            var expected = 0;
            var actual = appointment.GetStartTime().GetMinutes();
            Assert.AreEqual(expected, actual, "Original");

            dateTime.AddTime(0, 1);

            actual = appointment.GetStartTime().GetMinutes();

            Assert.AreEqual(expected, actual, "After");
        }
        
        /// <summary>
        /// Simple data test of GetLabel method.
        /// </summary>
        [TestMethod]
        public void GetLabelTest()
        {
            var expected = "Test Label";

            var appointment = new AppointmentBuilder().SetLabel(expected).Build();

            CalendarEventTest.GetLabelTest(appointment, expected);
        }

        /// <summary>
        /// Simple data test of the IsRepeating method.
        /// </summary>
        [TestMethod]
        public void IsRepeatingTest()
        {
            var expected = false;

            var appointment = new AppointmentBuilder().Build();

            CalendarEventTest.IsRepeatingTest(appointment, expected);
        }

        /// <summary>
        /// Tests the boundaries around the IsOccuring method for a simple scenario.
        /// </summary>
        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", @"TestData\Appointment\AppointmentData.xml", "add", DataAccessMethod.Sequential)]
        public void IsOccuringOnTest()
        {
            // Get input data for test.
            var startYear = int.Parse(TestContext.DataRow["startYear"].ToString());
            var startMonth = int.Parse(TestContext.DataRow["startMonth"].ToString());
            var startDay = int.Parse(TestContext.DataRow["startDay"].ToString());
            var startHours = int.Parse(TestContext.DataRow["startHours"].ToString());
            var startMinutes = int.Parse(TestContext.DataRow["startMinutes"].ToString());

            var endYear = int.Parse(TestContext.DataRow["endYear"].ToString());
            var endMonth = int.Parse(TestContext.DataRow["endMonth"].ToString());
            var endDay = int.Parse(TestContext.DataRow["endDay"].ToString());
            var endHours = int.Parse(TestContext.DataRow["endHours"].ToString());
            var endMinutes = int.Parse(TestContext.DataRow["endMinutes"].ToString());

            var durationMinutes = int.Parse(TestContext.DataRow["durationMinutes"].ToString());

            // Create appointment and exercise the method under test.
            var expectedStartDate = new Date(startDay, (Date.Month)startMonth, startYear);
            var appointmentTime = new Diary.DateTime(expectedStartDate, startHours, startMinutes);
            var appointment = new AppointmentBuilder().SetDateTime(appointmentTime).SetDurationMinutes(durationMinutes).Build();
            var expectedEndDate = new Date(endDay, (Date.Month)endMonth, endYear);

            CalendarEventTest.IsOccuringOnTest(appointment, expectedStartDate, expectedEndDate);
        }

        /// <summary>
        /// Data testing of start time method.
        /// </summary>
        [TestMethod]
        public void GetStartTimeTest()
        {
            var appointmentStartTime = new Diary.DateTime(new Date(6, Date.Month.MAY, 2017), 10, 3);
            var expected = DateTimeTest.ToString(appointmentStartTime);

            var appointment = new AppointmentBuilder().SetDateTime(appointmentStartTime).Build();
            var actualStartTime = appointment.GetStartTime();

            var actual = DateTimeTest.ToString(actualStartTime);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests that the StartTime field cannot be modified outside of its accessor.
        /// </summary>
        /// <seealso cref="DateTimeTest.InputDateConstructorAliasingTest">For more context on the problem.</seealso>
        [TestMethod]
        public void GetStartTimeAliasingTest()
        {
            var appointment = new AppointmentBuilder().Build();

            var expected = 0;
            var startTime = appointment.GetStartTime();
            var actual = startTime.GetMinutes();
            Assert.AreEqual(expected, actual, "Original");

            startTime.AddTime(0, 1);

            actual = appointment.GetStartTime().GetMinutes();

            Assert.AreEqual(expected, actual, "After");
        }

        /// <summary>
        /// Data driven testing of end time method.
        /// </summary>
        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", @"TestData\Appointment\AppointmentData.xml", "add", DataAccessMethod.Sequential)]
        public void GetEndTimeTest()
        {
            // Get input data for test.
            var startYear = int.Parse(TestContext.DataRow["startYear"].ToString());
            var startMonth = int.Parse(TestContext.DataRow["startMonth"].ToString());
            var startDay = int.Parse(TestContext.DataRow["startDay"].ToString());
            var startHours = int.Parse(TestContext.DataRow["startHours"].ToString());
            var startMinutes = int.Parse(TestContext.DataRow["startMinutes"].ToString());

            var endYear = int.Parse(TestContext.DataRow["endYear"].ToString());
            var endMonth = int.Parse(TestContext.DataRow["endMonth"].ToString());
            var endDay = int.Parse(TestContext.DataRow["endDay"].ToString());
            var endHours = int.Parse(TestContext.DataRow["endHours"].ToString());
            var endMinutes = int.Parse(TestContext.DataRow["endMinutes"].ToString());

            var durationMinutes = int.Parse(TestContext.DataRow["durationMinutes"].ToString());

            // Create appointment and exercise the method under test.
            var appointmentStartTime = new Diary.DateTime(new Date(startDay, (Date.Month)startMonth, startYear), startHours, startMinutes);
            var appointment = new AppointmentBuilder().SetDateTime(appointmentStartTime).SetDurationMinutes(durationMinutes).Build();
            var actualEndTime = appointment.GetEndTime();

            // Validate results.
            var expected = DateTimeTest.ToString(new Diary.DateTime(new Date(endDay, (Date.Month)endMonth, endYear), endHours, endMinutes));
            var actual = DateTimeTest.ToString(actualEndTime);

            Assert.AreEqual(expected, actual, "Input occurs:<{0}>. durationMinutes:<{1}>.", DateTimeTest.ToString(appointmentStartTime), durationMinutes);
        }

        /// <summary>
        /// Simple data test of DurationMinutes method.
        /// </summary>
        [TestMethod]
        public void GetDurationMinutesTest()
        {
            var expected = 42;

            var appointment = new AppointmentBuilder().SetDurationMinutes(expected).Build();
            var actual = appointment.GetDurationMinutes();

            Assert.AreEqual(expected, actual);
        }
    }
}
