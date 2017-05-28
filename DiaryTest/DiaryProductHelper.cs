﻿using Diary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DiaryTest
{
    /// <summary>
    /// Helper expected object tests to test all the fields via an object accessor against its expected values.
    /// </summary>
    /// <remarks>DiaryProduct.ObjectId should be tested separately via its ability to retrieve from persistent storage.</remarks>
    class DiaryProductHelper
    {
        /// <summary>
        /// Tests all Appointment fields match the expected values.
        /// </summary>
        internal void assertEquals(AppointmentBuilder expected, Appointment actual, string message)
        {
            Assert.AreEqual(expected.GetLabel(), actual.GetLabel(), String.Format("{0} label", message));
            Assert.AreEqual(expected.GetDetails(), actual.GetDetails(), String.Format("{0} details", message));
            Assert.AreEqual(expected.GetDurationMinutes(), actual.GetDurationMinutes(), String.Format("{0} durationMinutes", message));

            var expectedClassId = new ClassId("Appointment");
            Assert.AreEqual(0, actual.GetClassId().CompareTo(expectedClassId), String.Format("{0} classId", message));
        }

        /// <summary>
        /// Tests all Contact fields match the expected values.
        /// </summary>
        internal void assertEquals(ContactBuilder expected, Contact actual, string message)
        {
            Assert.AreEqual(expected.GetFirstName(), actual.GetName()[0], String.Format("{0} firstName", message));
            Assert.AreEqual(expected.GetLastName(), actual.GetName()[1], String.Format("{0} lastName", message));
            Assert.AreEqual(expected.GetContactInfo(), actual.GetContactInfo(), String.Format("{0} contactInfo", message));

            var expectedClassId = new ClassId("Contact");
            Assert.AreEqual(0, actual.GetClassId().CompareTo(expectedClassId), String.Format("{0} classId", message));
        }

        /// <summary>
        /// Tests all Reminder fields match the expected values.
        /// </summary>
        internal void assertEquals(ReminderBuilder expected, Reminder actual, string message)
        {
            Assert.AreEqual(expected.GetLabel(), actual.GetLabel(), String.Format("{0} label", message));
            Assert.AreEqual(expected.GetDetails(), actual.GetDetails(), String.Format("{0} details", message));

            var expectedClassId = new ClassId("Reminder");
            Assert.AreEqual(0, actual.GetClassId().CompareTo(expectedClassId), String.Format("{0} classId", message));
        }

        /// <summary>
        /// Tests all PeriodicAppointment fields match the expected values.
        /// </summary>
        internal void assertEquals(PeriodicAppointmentBuilder expected, PeriodicAppointment actual, string message)
        {
            Assert.AreEqual(expected.GetLabel(), actual.GetLabel(), String.Format("{0} label", message));
            Assert.AreEqual(expected.GetDetails(), actual.GetDetails(), String.Format("{0} details", message));
            Assert.AreEqual(expected.GetDurationMinutes(), actual.GetDurationMinutes(), String.Format("{0} durationMinutes", message));

            var expectedClassId = new ClassId("PeriodicAppointment");
            Assert.AreEqual(0, actual.GetClassId().CompareTo(expectedClassId), String.Format("{0} classId", message));
        }
    }
}
