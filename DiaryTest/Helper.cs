using Diary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DiaryTest
{
    /// <summary>
    /// Helper expected object tests to test all the fields via an object accessor against its expected values.
    /// </summary>
    /// <remarks>DiaryProduct.ObjectId should be tested separately via its ability to retrieve from persistent storage.</remarks>
    class Helper
    {
        #region Debugging
        /// <summary>
        /// Returns Date identifying properties to support meaningful equality checks and debugging.
        /// </summary>
        /// <returns>yyyy-MM-dd</returns>
        internal static string ToString(Date date)
        {
            int day = date.GetDay();
            int month = (int)date.GetMonth();
            int year = date.GetYear();

            return String.Format("{0}-{1}-{2}", year.ToString("0000"), month.ToString("00"), date.GetDay().ToString("00"));
        }

        /// <summary>
        /// Returns DateTime identifying properties.
        /// </summary>
        /// <returns>yyyy-MM-dd hh:mm</returns>
        /// <seealso cref="Helper.ToString(Date)"/>
        internal static String ToString(Diary.DateTime dateTime)
        {
            return String.Format("{0} {1}:{2}", Helper.ToString(dateTime.GetDate()), dateTime.GetHours().ToString("00"), dateTime.GetMinutes().ToString("00"));
        }
        #endregion

        #region Custom Asserts
        /// <summary>
        /// Tests all Appointment fields match the expected values.
        /// </summary>
        internal static void AssertAreEqual(AppointmentBuilder expected, Appointment actual, string message)
        {
            Assert.AreEqual(expected.GetDetails(), actual.GetDetails(), String.Format("{0} details", message));
            Assert.AreEqual(expected.GetDurationMinutes(), actual.GetDurationMinutes(), String.Format("{0} durationMinutes", message));
            Assert.AreEqual(expected.GetLabel(), actual.GetLabel(), String.Format("{0} label", message));

            Assert.AreEqual(ToString(expected.GetOccurs()), ToString(actual.GetStartTime()), String.Format("{0} startTime", message));
            Assert.AreNotSame(actual.GetStartTime(), actual.GetStartTime(), String.Format("{0} startTime deep copy", message));

            var expectedClassId = new ClassId("Appointment");
            Assert.AreEqual(0, actual.GetClassId().CompareTo(expectedClassId), String.Format("{0} classId", message));
        }

        /// <summary>
        /// Tests all Contact fields match the expected values.
        /// </summary>
        internal static void AssertAreEqual(ContactBuilder expected, Contact actual, string message)
        {
            Assert.AreEqual(expected.GetContactInfo(), actual.GetContactInfo(), String.Format("{0} contactInfo", message));
            Assert.AreEqual(expected.GetFirstName(), actual.GetName()[0], String.Format("{0} firstName", message));
            Assert.AreEqual(expected.GetLastName(), actual.GetName()[1], String.Format("{0} lastName", message));

            var expectedClassId = new ClassId("Contact");
            Assert.AreEqual(0, actual.GetClassId().CompareTo(expectedClassId), String.Format("{0} classId", message));
        }

        /// <summary>
        /// Tests all Date fields match the expected values.
        /// </summary>
        internal static void AssertAreEqual(DateBuilder expected, Date actual, string message)
        {
            Assert.AreEqual(expected.GetDay(), actual.GetDay(), String.Format("{0} day", message));
            Assert.AreEqual(expected.GetMonth(), (int)actual.GetMonth(), String.Format("{0} month", message));
            Assert.AreEqual(expected.GetYear(), actual.GetYear(), String.Format("{0} year", message));
        }

        /// <summary>
        /// Tests all DateTime fields match the expected values.
        /// </summary>
        /// <see href="https://www.martinfowler.com/bliki/AliasingBug.html">About deep copy and aliasing bugs.</see>
        internal static void AssertAreEqual(DateTimeBuilder expected, Diary.DateTime actual, string message)
        {
            Assert.AreEqual(ToString(expected.GetDate()), ToString(actual.GetDate()), String.Format("{0} day", message));

            Assert.AreEqual(expected.GetHours(), actual.GetHours(), String.Format("{0} hours", message));
            Assert.AreEqual(expected.GetMinutes(), actual.GetMinutes(), String.Format("{0} minutes", message));

            Assert.AreNotSame(actual.GetDate(), actual.GetDate(), String.Format("{0} date deep copy", message));
        }

        /// <summary>
        /// Tests all PeriodicAppointment fields match the expected values.
        /// </summary>
        internal static void AssertAreEqual(PeriodicAppointmentBuilder expected, PeriodicAppointment actual, string message)
        {
            Assert.AreEqual(expected.GetDetails(), actual.GetDetails(), String.Format("{0} details", message));
            Assert.AreEqual(expected.GetDurationMinutes(), actual.GetDurationMinutes(), String.Format("{0} durationMinutes", message));
            Assert.AreEqual(expected.GetLabel(), actual.GetLabel(), String.Format("{0} label", message));

            Assert.AreEqual(ToString(expected.GetOccurs()), ToString(actual.GetStartTime()), String.Format("{0} startTime", message));
            Assert.AreNotSame(actual.GetStartTime(), actual.GetStartTime(), String.Format("{0} startTime deep copy", message));

            var expectedClassId = new ClassId("PeriodicAppointment");
            Assert.AreEqual(0, actual.GetClassId().CompareTo(expectedClassId), String.Format("{0} classId", message));
        }

        /// <summary>
        /// Tests all Reminder fields match the expected values.
        /// </summary>
        internal static void AssertAreEqual(ReminderBuilder expected, Reminder actual, string message)
        {
            Assert.AreEqual(expected.GetLabel(), actual.GetLabel(), String.Format("{0} label", message));
            Assert.AreEqual(expected.GetDetails(), actual.GetDetails(), String.Format("{0} details", message));

            var expectedClassId = new ClassId("Reminder");
            Assert.AreEqual(0, actual.GetClassId().CompareTo(expectedClassId), String.Format("{0} classId", message));
        }
        #endregion
    }
}
