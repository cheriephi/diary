﻿using Diary;
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
        /// <see href="https://www.martinfowler.com/bliki/AliasingBug.html">About deep copy and aliasing bugs.</see>
        /// <remarks>Calculated fields, such as EndTime, are tested elsewhere because they are not required
        /// to validate all the data fields passed into the constructor match expected values.</remarks>
        internal static void AssertAreEqual(AppointmentBuilder expected, Appointment actual, string message)
        {
            var messagePrefix = String.Format("{0} Label:<{1}>.", message, expected.GetLabel());

            Assert.AreEqual(expected.GetDetails(), actual.GetDetails(), String.Concat(messagePrefix, " details"));
            Assert.AreEqual(expected.GetDurationMinutes(), actual.GetDurationMinutes(), String.Concat(messagePrefix, " durationMinutes"));
            Assert.AreEqual(false, actual.IsRepeating(), String.Concat(messagePrefix, " IsRepeating"));
            Assert.AreEqual(expected.GetLabel(), actual.GetLabel(), String.Concat(messagePrefix, " label"));

            Assert.AreEqual(ToString(expected.GetOccurs()), ToString(actual.GetStartTime()), String.Concat(messagePrefix, " startTime"));
            Assert.AreNotSame(actual.GetStartTime(), actual.GetStartTime(), String.Concat(messagePrefix, " startTime deep copy"));

            var expectedClassId = new ClassId("Appointment");
            Assert.AreEqual(0, actual.GetClassId().CompareTo(expectedClassId), String.Concat(messagePrefix, " classId"));

            //Test Contact relations
            var relation = actual.GetContacts();
            var contactBuilders = expected.GetContactBuilders();
            Assert.AreEqual(contactBuilders.Length, relation.GetChildCount(), String.Concat(messagePrefix, " contacts.Count"));

            for (int i = 0; i < contactBuilders.Length; i++)
            {
                Helper.AssertAreEqual(contactBuilders[i], relation.GetChild(i), String.Concat(messagePrefix, " contacts[", i.ToString(), "].Data"));
            }
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
        /// Tests all PeriodicAppointment fields match the expected values.
        /// </summary>
        internal static void AssertAreEqual(PeriodicAppointmentBuilder expected, PeriodicAppointment actual, string message)
        {
            var messagePrefix = String.Format("{0} Label:<{1}>.", message, expected.GetLabel());

            Assert.AreEqual(expected.GetDetails(), actual.GetDetails(), String.Concat(messagePrefix, " details"));
            Assert.AreEqual(expected.GetDurationMinutes(), actual.GetDurationMinutes(), String.Concat(messagePrefix, " durationMinutes"));
            Assert.AreEqual(true, actual.IsRepeating(), String.Concat(messagePrefix, " IsRepeating"));
            Assert.AreEqual(expected.GetLabel(), actual.GetLabel(), String.Concat(messagePrefix, " label"));

            Assert.AreEqual(ToString(expected.GetOccurs()), ToString(actual.GetStartTime()), String.Concat(messagePrefix, " startTime"));
            Assert.AreNotSame(actual.GetStartTime(), actual.GetStartTime(), String.Concat(messagePrefix, " startTime deep copy"));

            var expectedClassId = new ClassId("PeriodicAppointment");
            Assert.AreEqual(0, actual.GetClassId().CompareTo(expectedClassId), String.Concat(messagePrefix, " classId"));

            //Test Contact relations
            var relation = actual.GetContacts();
            var contactBuilders = expected.GetContactBuilders();
            Assert.AreEqual(contactBuilders.Length, relation.GetChildCount(), String.Concat(messagePrefix, " contacts.Count"));

            for (int i = 0; i < contactBuilders.Length; i++)
            {
                Helper.AssertAreEqual(contactBuilders[i], relation.GetChild(i), String.Concat(messagePrefix, " contacts[", i.ToString(), "].Data"));
            }
        }

        /// <summary>
        /// Tests all Reminder fields match the expected values.
        /// </summary>
        internal static void AssertAreEqual(ReminderBuilder expected, Reminder actual, string message)
        {
            var messagePrefix = String.Format("{0} Label:<{1}>.", message, expected.GetLabel());

            Assert.AreEqual(expected.GetDetails(), actual.GetDetails(), String.Concat(messagePrefix, " details"));
            Assert.AreEqual(false, actual.IsRepeating(), String.Concat(messagePrefix, " IsRepeating"));
            Assert.AreEqual(expected.GetLabel(), actual.GetLabel(), String.Concat(messagePrefix, " label"));

            var expectedClassId = new ClassId("Reminder");
            Assert.AreEqual(0, actual.GetClassId().CompareTo(expectedClassId), String.Concat(messagePrefix, " classId"));

            // Test the Reminder.Date; which is exposed through the IsOccuringOn method.
            CalendarEventTest.IsOccuringOnTest(actual, expected.GetDate(), expected.GetDate());
        }
        #endregion
    }
}
