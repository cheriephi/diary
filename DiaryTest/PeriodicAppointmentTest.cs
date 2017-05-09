using Diary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;

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
            var appointmentStartTime = new Diary.DateTime(new Date(6, Date.Month.MAY, 2017), 10, 3);
            var builder = new PeriodicAppointmentBuilder();
            builder.SetOccurs(appointmentStartTime);
            builder.SetNotToExceedDateTime(appointmentStartTime);

            var appointment = builder.Build();

            var expected = DateTimeTest.ToString(appointmentStartTime);
            var actual = DateTimeTest.ToString(appointment.GetStartTime());

            Assert.AreEqual(expected, actual);
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

        /// <summary>
        /// Data testing of IsOcurringOn method.
        /// </summary>
        /// <see href="http://stackoverflow.com/questions/25541795/nested-xml-for-data-driven-unit-test"/>
        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", @"TestData\Appointment\PeriodicAppointmentData.xml", "add", DataAccessMethod.Sequential)]
        public void IsOccuringOnTest()
        {
            // Get input occurence data for test.
            var firstOccursYear = int.Parse(TestContext.DataRow["firstOccursYear"].ToString());
            var firstOccursMonth = int.Parse(TestContext.DataRow["firstOccursMonth"].ToString());
            var firstOccursDay = int.Parse(TestContext.DataRow["firstOccursDay"].ToString());
            var firstOccursHours = int.Parse(TestContext.DataRow["firstOccursHours"].ToString());
            var firstOccursMinutes = int.Parse(TestContext.DataRow["firstOccursMinutes"].ToString());

            var notToExceedYear = int.Parse(TestContext.DataRow["notToExceedYear"].ToString());
            var notToExceedMonth = int.Parse(TestContext.DataRow["notToExceedMonth"].ToString());
            var notToExceedDay = int.Parse(TestContext.DataRow["notToExceedDay"].ToString());
            var notToExceedHours = int.Parse(TestContext.DataRow["notToExceedHours"].ToString());
            var notToExceedMinutes = int.Parse(TestContext.DataRow["notToExceedMinutes"].ToString());

            var durationMinutes = int.Parse(TestContext.DataRow["durationMinutes"].ToString());
            var periodHours = int.Parse(TestContext.DataRow["periodHours"].ToString());

            // Create the periodic appointment, deriving the overall start and end date
            var firstOccurs = new Diary.DateTime(new Date(firstOccursDay, (Date.Month)firstOccursMonth, firstOccursYear), firstOccursHours, firstOccursMinutes);
            var notToExceedDateTime = new Diary.DateTime(new Date(notToExceedDay, (Date.Month)notToExceedMonth, notToExceedYear), notToExceedHours, notToExceedMinutes);

            PeriodicAppointmentBuilder builder = new PeriodicAppointmentBuilder();
            builder.SetOccurs(firstOccurs);
            builder.SetDurationMinutes(durationMinutes);
            builder.SetNotToExceedDateTime(notToExceedDateTime);
            builder.SetPeriodHours(periodHours);
            var appointment = builder.Build();

            // Look up and evaluate each occurence
            DataRow[] occurencesRows = TestContext.DataRow.GetChildRows("add_occurences");

            foreach (DataRow occurenceRow in occurencesRows)
            {
                DataRow[] occurenceDatePartRows = occurenceRow.GetChildRows("occurences_occurence");
                foreach (DataRow occurenceDatePart in occurenceDatePartRows)
                { 
                    var startYear = int.Parse(occurenceDatePart["startYear"].ToString());
                    var startMonth = int.Parse(occurenceDatePart["startMonth"].ToString());
                    var startDay = int.Parse(occurenceDatePart["startDay"].ToString());

                    var endYear = int.Parse(occurenceDatePart["endYear"].ToString());
                    var endMonth = int.Parse(occurenceDatePart["endMonth"].ToString());
                    var endDay = int.Parse(occurenceDatePart["endDay"].ToString());

                    var expectedStartDate = new Date(startDay, (Date.Month)startMonth, startYear);
                    var expectedEndDate = new Date(endDay, (Date.Month)endMonth, endYear);

                    CalendarEventTest.IsOccuringOnTest(appointment, expectedStartDate, expectedEndDate);
                }
            }
        }

        /// <summary>
        /// Tests that periodic appointments won't allow an appointment with a duration longer than the window allowed by the firstOccurs and notToExceedDateTime parameters.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(System.ArgumentOutOfRangeException))]
        public void InvalidDurationForEndDateTest()
        {
            var severalDaysWorthOfMinutes = 1440 * 3;

            PeriodicAppointmentBuilder builder = new PeriodicAppointmentBuilder();
            builder.SetOccurs(new Diary.DateTime());
            builder.SetDurationMinutes(severalDaysWorthOfMinutes);
            builder.SetNotToExceedDateTime(new Diary.DateTime());
            var appointment = builder.Build();
        }

        /// <summary>
        /// Tests that the notToExceedDateTime field passed into the constructor cannot be modified outside of its accessor.
        /// </summary>
        /// <seealso cref="DateTimeTest.InputDateConstructorAliasingTest">For more context on the problem.</seealso>
        [TestMethod]
        public void ConstructorNotToExceedDateTimeAliasingTest()
        {
            var occurs = new DateTime(new Date(1, Date.Month.JANUARY, 2000), 0, 0);
            var notToExceedDateTime = new Diary.DateTime(occurs);

            var occurenceDate = new Date(2, Date.Month.JANUARY, 2000);

            var builder = new PeriodicAppointmentBuilder();
            builder.SetOccurs(occurs);
            builder.SetDurationMinutes(1);
            builder.SetPeriodHours(24);
            builder.SetNotToExceedDateTime(notToExceedDateTime);
            var appointment = builder.Build();

            var expected = false;
            var actual = appointment.IsOccuringOn(occurenceDate);
            Assert.AreEqual(expected, actual, "Original");

            notToExceedDateTime.AddTime(100, 0);

            actual = appointment.IsOccuringOn(occurenceDate);

            Assert.AreEqual(expected, actual, "After");
        }
    }
}
