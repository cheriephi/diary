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
    /// <remarks>Uses polymorphic tests to ensure methods that should behave the same actually do so, despite their place in the inheritance
    /// hierarchy.</remarks>
    [TestClass]
    public class AppointmentTest
    {
        /// <summary>
        /// Required context for data driven testing.
        /// </summary>
        public TestContext TestContext { get; set; }

        #region Polymorphic test functions
        /// <summary>
        /// Tests that the StartTime field passed into the constructor cannot be modified outside of its accessor.
        /// </summary>
        /// <param name="builder"></param>
        /// <seealso cref="DateTimeTest.InputDateTimeConstructorTest">For more context on the problem.</seealso>
        internal void ConstructorAliasingTest(AppointmentBuilder builder)
        {
            var dateTime = new Diary.DateTime();
            var appointment = (Appointment)builder.SetOccurs(dateTime).Build();

            var expected = 0;
            var actual = appointment.GetStartTime().GetMinutes();
            Assert.AreEqual(expected, actual, "Original");

            dateTime.AddTime(0, 1);

            actual = appointment.GetStartTime().GetMinutes();

            Assert.AreEqual(expected, actual, "After");
        }

        /// <summary>
        /// Tests that the StartTime field cannot be modified outside of its accessor.
        /// </summary>
        /// <seealso cref="DateTimeTest.InputDateTimeConstructorTest">For more context on the problem.</seealso>
        internal void GetStartTimeAliasingTest(AppointmentBuilder builder)
        {
            var appointment = (Appointment)builder.Build();

            var expected = 0;
            var startTime = appointment.GetStartTime();
            var actual = startTime.GetMinutes();
            Assert.AreEqual(expected, actual, "Original");

            startTime.AddTime(0, 1);

            actual = appointment.GetStartTime().GetMinutes();

            Assert.AreEqual(expected, actual, "After");
        }

        /// <summary>
        /// Testing of end time method.
        /// </summary>
        internal void GetEndTimeTest(AppointmentBuilder builder, Diary.DateTime occurs, Diary.DateTime endTime, int durationMinutes)
        {
            var appointment = (Appointment)builder.SetOccurs(occurs).SetDurationMinutes(durationMinutes).Build();
            var actualEndTime = appointment.GetEndTime();

            // Validate results.
            var expected = DateTimeTest.ToString(endTime);
            var actual = DateTimeTest.ToString(actualEndTime);

            Assert.AreEqual(expected, actual, "Input occurs:<{0}>. Input durationMinutes:<{1}>.", DateTimeTest.ToString(occurs), durationMinutes);
        }

        /// <summary>
        /// Tests GetContacts method using polymorphism.
        /// </summary>
        internal void GetContactsTest(AppointmentBuilder builder)
        {
            var appointment = (Appointment)builder.Build();

            var contact = (Contact)new ContactBuilder().SetFirstName("Yogi").SetLastName("Bear").Build();

            appointment.AddRelation((Contact)new ContactBuilder().Build());
            appointment.AddRelation(contact);
            appointment.AddRelation((Contact)new ContactBuilder().Build());

            var relation = appointment.GetContacts();
            Assert.AreEqual(3, relation.GetChildCount(), "Count");

            var expected = ContactTest.ToString(contact);
            var actual = ContactTest.ToString(relation.GetChild(1));

            Assert.AreEqual(expected, actual, "Data");
        }
        #endregion

        /// <summary>
        /// Appointment constructor aliasing test.
        /// </summary>
        [TestMethod]
        public void ConstructorAliasingTest()
        {
            ConstructorAliasingTest(new AppointmentBuilder());
        }

        /// <summary>
        /// Simple data test of the IsRepeating method.
        /// </summary>
        [TestMethod]
        public void IsRepeatingTest()
        {
            var expected = false;

            var appointment = (Appointment)new AppointmentBuilder().Build();

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
            var appointment = (Appointment)new AppointmentBuilder().SetOccurs(appointmentTime).SetDurationMinutes(durationMinutes).Build();
            var expectedEndDate = new Date(endDay, (Date.Month)endMonth, endYear);

            CalendarEventTest.IsOccuringOnTest(appointment, expectedStartDate, expectedEndDate);
        }

        /// <summary>
        /// Tests the Appointment.GetStartTime method.
        /// </summary>
        [TestMethod]
        public void GetStartTimeTest()
        {
            var appointmentStartTime = new Diary.DateTime(new Date(6, Date.Month.MAY, 2017), 10, 3);
            var expected = DateTimeTest.ToString(appointmentStartTime);

            var appointment = (Appointment)new AppointmentBuilder().SetOccurs(appointmentStartTime).Build();
            var actual = DateTimeTest.ToString(appointment.GetStartTime());

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests that the Appointment.StartTime field cannot be modified outside of its accessor.
        /// </summary>
        [TestMethod]
        public void GetStartTimeAliasingTest()
        {
            GetStartTimeAliasingTest(new AppointmentBuilder());
        }

        /// <summary>
        /// Data driven testing of Appointment get end time method.
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

            GetEndTimeTest(new AppointmentBuilder(), occurs, endTime, durationMinutes);
        }

        /// <summary>
        /// Tests the Appointment accessors through its constructor.
        /// </summary>
        [TestMethod]
        public void AppointmentConstructorTest()
        {
            var builder = new AppointmentBuilder();
            builder.SetLabel("Test Label");
            builder.SetDetails("Detail text");

            builder.SetDurationMinutes(42);

            new DiaryProductHelper().assertEquals(builder, (Appointment)builder.Build(), "");
        }

        /// <summary>
        /// Appointment get contacts test.
        /// </summary>
        [TestMethod]
        public void GetContactsTest()
        {
            GetContactsTest(new AppointmentBuilder());
        }

        #region Persistence Tests
        /// <summary>
        /// Tests the ObjectId accessor.
        /// </summary>
        [TestMethod]
        public void GetObjectIdTest()
        {
            var objectId = new ObjectId();
            var appointment = new AppointmentBuilder().SetObjectId(objectId).Build();
            new DiaryProductTest().GetObjectIdTest(appointment, objectId);
        }
        #endregion
    }
}
