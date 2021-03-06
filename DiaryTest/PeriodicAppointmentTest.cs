﻿using Diary;
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
        /// Tests the PeriodicAppointment accessors through its constructor.
        /// </summary>
        [TestMethod]
        public void PeriodicAppointmentConstructorTest()
        {
            var builder = new PeriodicAppointmentBuilder();
            builder.SetLabel("Test Label");
            builder.SetDetails("Detail text");

            builder.SetOccurs(new DateTime(new Date(1, Date.Month.JANUARY, 2003), 0, 0));
            builder.SetNotToExceedDateTime(new DateTime(new Date(2, Date.Month.JANUARY, 2003), 0, 0));
            builder.SetDurationMinutes(42);

            Helper.AssertAreEqual(builder, (PeriodicAppointment)builder.Build(), "");
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

            new AppointmentTest().GetEndTimeTest(new PeriodicAppointmentBuilder().SetNotToExceedDateTime(endTime), occurs, endTime, durationMinutes);
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
            var appointment = (PeriodicAppointment)builder.Build();

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

            var builder = new PeriodicAppointmentBuilder();
            builder.SetOccurs(new Diary.DateTime());
            builder.SetDurationMinutes(severalDaysWorthOfMinutes);
            builder.SetNotToExceedDateTime(new Diary.DateTime());
            var appointment = (PeriodicAppointment)builder.Build();
        }

        /// <summary>
        /// Tests that the notToExceedDateTime field passed into the constructor cannot be modified outside of its accessor.
        /// </summary>
        /// <seealso cref="Helper.AssertAreEqual(AppointmentBuilder, Appointment, string)">For more context on the problem.</seealso>
        [TestMethod]
        public void ConstructorNotToExceedDateTimeAliasingTest()
        {
            var occurs = new DateTime(new Date(1, Date.Month.JANUARY, 2000), 0, 0);
            var notToExceedDateTime = new Diary.DateTime(occurs);

            var occurenceDate = new Date(2, Date.Month.JANUARY, 2000);

            var builder = new PeriodicAppointmentBuilder();
            builder.SetOccurs(occurs);
            builder.SetPeriodHours(24);
            builder.SetNotToExceedDateTime(notToExceedDateTime);
            var appointment = (PeriodicAppointment)builder.Build();

            var expected = false;
            var actual = appointment.IsOccuringOn(occurenceDate);
            Assert.AreEqual(expected, actual, "Original");

            notToExceedDateTime.AddTime(100, 0);

            actual = appointment.IsOccuringOn(occurenceDate);

            Assert.AreEqual(expected, actual, "After");
        }

        /// <summary>
        /// Periodic appointment get contacts test.
        /// </summary>
        [TestMethod]
        public void GetContactsTest()
        {
            var builder = new PeriodicAppointmentBuilder();

            var appointment = (PeriodicAppointment)builder.SetContactBuilders().Build();

            var contactBuilders = builder.GetContactBuilders();
            foreach (var contactBuilder in contactBuilders)
            {
                appointment.AddRelation((Contact)contactBuilder.Build());
            }

            Helper.AssertAreEqual(builder, appointment, "");
        }


        #region Persistence Tests
        /// <summary>
        /// Tests the ObjectId accessor.
        /// </summary>
        [TestMethod]
        public void GetObjectIdTest()
        {
            var objectId = new ObjectId();
            var periodicAppointment = new PeriodicAppointmentBuilder().SetObjectId(objectId).Build();
            new DiaryProductTest().GetObjectIdTest(periodicAppointment, objectId);
        }
        #endregion
    }
}
