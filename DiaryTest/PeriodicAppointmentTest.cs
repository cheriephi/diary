using Diary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DiaryTest
{
    /// <summary>
    /// Tests the Periodic Appointment class.
    /// </summary>
    [TestClass]
    public class PeriodicAppointmentTest
    {
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
            new AppointmentTest().ConstructorAliasingTest(new PeriodicAppointmentBuilder());
        }

        /// <summary>
        /// Simple data test of GetLabel method.
        /// </summary>
        [TestMethod]
        public void GetLabelTest()
        {
            new AppointmentTest().GetLabelTest(new PeriodicAppointmentBuilder());
        }

        /// <summary>
        /// Simple test of the IsRepeating method.
        /// </summary>
        [TestMethod]
        public void IsRepeatingTest()
        {
            var expected = true;
            
            var periodicAppointment = new PeriodicAppointmentBuilder().Build();

            CalendarEventTest.IsRepeatingTest(periodicAppointment, expected);
        }

        /// <summary>
        /// Data testing of start time method.
        /// </summary>
        [TestMethod]
        public void GetStartTimeTest()
        {
            new AppointmentTest().GetStartTimeTest(new PeriodicAppointmentBuilder());
        }

        /// <summary>
        /// Tests that the StartTime field cannot be modified outside of its accessor.
        /// </summary>
        /// <seealso cref="DateTimeTest.InputDateConstructorAliasingTest">For more context on the problem.</seealso>
        [TestMethod]
        public void GetStartTimeAliasingTest()
        {
            new AppointmentTest().GetStartTimeAliasingTest(new PeriodicAppointmentBuilder());
        }

        /// <summary>
        /// Data testing of end time method.
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

            // Exercise the method under test.
            var occurs = new Diary.DateTime(new Date(startDay, (Date.Month)startMonth, startYear), startHours, startMinutes);
            var endTime = new Diary.DateTime(new Date(endDay, (Date.Month)endMonth, endYear), endHours, endMinutes);

            new AppointmentTest().GetEndTimeTest(new PeriodicAppointmentBuilder(), occurs, endTime, durationMinutes);
        }

        /// <summary>
        /// Data testing of start time method.
        /// </summary>
        [TestMethod]
        public void GetDurationMinutesTest()
        {
            new AppointmentTest().GetDurationMinutesTest(new PeriodicAppointmentBuilder());
        }
    }
}
