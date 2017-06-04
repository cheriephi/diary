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
        /// Testing of end time method.
        /// </summary>
        internal void GetEndTimeTest(AppointmentBuilder builder, Diary.DateTime occurs, Diary.DateTime endTime, int durationMinutes)
        {
            var appointment = (Appointment)builder.SetOccurs(occurs).SetDurationMinutes(durationMinutes).Build();
            var actualEndTime = appointment.GetEndTime();

            // Validate results.
            var expected = Helper.ToString(endTime);
            var actual = Helper.ToString(actualEndTime);

            Assert.AreEqual(expected, actual, "Input occurs:<{0}>. Input durationMinutes:<{1}>.", Helper.ToString(occurs), durationMinutes);
        }
        #endregion

        /// <summary>
        /// Tests that the StartTime field passed into the constructor cannot be modified outside of its accessor.
        /// </summary>
        /// <remarks>This tests that the startTime passed into the constructor is deep copied prior to its usage, regardless of whether it is
        /// deep copied when exposed via getters. The only way to test this is to actually modify the data.
        /// This test should cover any path through the constructor; be it through the constructor, a derived class constructor, or a creator.
        /// Theoretically every constructor in this class would require the same test; however because of knowledge of constructor chaining in the design,
        /// the extra tests are skipped.</remarks>
        [TestMethod]
        public void ConstructorAliasingTest()
        {
            var builder = new AppointmentBuilder();
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
        /// Data driven testing of Appointment.GetEndTime method.
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
            builder.SetOccurs(new Diary.DateTime(new Date(6, Date.Month.MAY, 2017), 10, 3));
            builder.SetDurationMinutes(42);

            Helper.AssertAreEqual(builder, (Appointment)builder.Build(), "");
        }

        /// <summary>
        /// Appointment get contacts test.
        /// </summary>
        [TestMethod]
        public void GetContactsTest()
        {
            var builder = new AppointmentBuilder();

            var appointment = (Appointment)builder.SetContactBuilders().Build();

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
            var appointment = new AppointmentBuilder().SetObjectId(objectId).Build();
            new DiaryProductTest().GetObjectIdTest(appointment, objectId);
        }
        #endregion
    }
}
