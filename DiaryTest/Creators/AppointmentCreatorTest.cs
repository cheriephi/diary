using Diary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DiaryTest.Creators
{
    /// <summary>
    /// Tests the AppointmentCreator class.
    /// </summary>
    /// <see cref="ObjectIdTest">About thread safety.</see>
    [TestClass]
    public unsafe class AppointmentCreatorTest
    {
        private TransientPersistenceFreshFixture fixture;

        #region Test Initialize and Cleanup Methods
        /// <summary>
        /// Resets the environment.
        /// </summary>
        /// <see cref="TransientPersistenceFreshFixture"/>
        [TestInitialize]
        public void Init()
        {
            fixture = new TransientPersistenceFreshFixture();
            fixture.Init();
        }

        /// <summary>
        /// Reverts the environment back to its original state.
        /// </summary>
        /// <see cref="TransientPersistenceFreshFixture"/>
        [TestCleanup]
        public void Cleanup()
        {
            fixture.Cleanup();
        }
        #endregion

        /// <summary>
        /// Tests the Appointment accessors through the AppointmentCreator constructor.
        /// </summary>
        [TestMethod]
        public void AppointmentCreatorConstructorTest()
        {
            // Wrap the creator in a using block so its resources will get released when the program no longer needs it.
            using (var creator = new AppointmentCreator())
            {
                var builder = new AppointmentBuilder();
                builder.SetCreator(creator);
                builder.SetOccurs(new Diary.DateTime(new Date(15, Date.Month.FEBRUARY, 2008), 15, 30));
                builder.SetLabel("Dr Apt");
                builder.SetDetails("At the library");

                Helper.AssertAreEqual(builder, (Appointment)builder.Build(), "");
            }
        }

        /// <summary>
        /// Tests persistence via the AppointmentCreator.
        /// </summary>
        [TestMethod]
        public void SaveAndLoadTest()
        {
            var appointments = new Appointment[4];

            using (var creator = new AppointmentCreator())
            {
                var yogaDateTime = new Diary.DateTime(new Date(1, Date.Month.FEBRUARY, 2017), 6, 30);
                appointments[0] = (Appointment)creator.CreateNew("Yoga", yogaDateTime, 45, "Downward Dog");

                var doctorsDateTime = new Diary.DateTime(new Date(2, Date.Month.MARCH, 2017), 8, 20);
                appointments[1] = (Appointment)creator.CreateNew("Ear Doctor", doctorsDateTime, 30, "Annual Cleaning");

                var teacherDateTime = new Diary.DateTime(new Date(7, Date.Month.APRIL, 2017), 12, 00);
                appointments[2] = (Appointment)creator.CreateNew("School", teacherDateTime, 25, "5th Grade Arithmatic");

                var reviewDateTime = new Diary.DateTime(new Date(30, Date.Month.MAY, 2017), 18, 30);
                appointments[3] = (Appointment)creator.CreateNew("Performance Review", reviewDateTime, 60, "6 Month Evaluation");

                creator.Save();
            }

            using (var appointmentCreator = new AppointmentCreator()) // Re-open the files
            {
                foreach (var appointment in appointments)
                {
                    var objectId = appointment.GetObjectId();
                    var savedAppointment = (Appointment)appointmentCreator.Create(objectId);

                    Assert.AreEqual(appointment.GetLabel(), savedAppointment.GetLabel(), "Label");
                    Assert.IsTrue(appointment.GetStartTime().CompareTo(savedAppointment.GetStartTime()) == 0, "StartTime");
                    Assert.AreEqual(appointment.GetDurationMinutes(), savedAppointment.GetDurationMinutes(), "DurationMinutes");
                    Assert.AreEqual(appointment.GetContacts().GetChildCount(), savedAppointment.GetContacts().GetChildCount(), "Contacts.ChildCount");
                }

                // Now add some contacts to the existing appointments
                using (var contactCreator = new ContactCreator())
                {
                    var contact1 = (Contact)contactCreator.CreateNew("Brian", "Rothwell", "brothwell@q.com");
                    var contact2 = (Contact)contactCreator.CreateNew("Billy", "Bob", "slingblade@msn.com");
                    var contact3 = (Contact)contactCreator.CreateNew("Jenny", "Twotone", "(210) 867-5308");
                    contactCreator.Save();

                    var drAppointment = (Appointment)appointmentCreator.Create(appointments[1].GetObjectId());
                    drAppointment.AddRelation(contact1);
                    drAppointment.AddRelation(contact2);

                    var reviewAppointment = (Appointment)appointmentCreator.Create(appointments[3].GetObjectId());
                    reviewAppointment.AddRelation(contact3);
                    appointmentCreator.Save();
                }
            }

            using (var creator = new AppointmentCreator())   // Re-open the files
            {
                foreach (var appointment in appointments)
                {
                    var objectId = appointment.GetObjectId();
                    var savedAppointment = (Appointment)creator.Create(objectId);

                    Assert.AreEqual(appointment.GetLabel(), savedAppointment.GetLabel(), "Label Post Contact Save");
                    Assert.IsTrue(appointment.GetStartTime().CompareTo(savedAppointment.GetStartTime()) == 0, "StartTime Post Contact Save");
                    Assert.AreEqual(appointment.GetDurationMinutes(), savedAppointment.GetDurationMinutes(), "DurationMinutes Post Contact Save");
                }

                var drAppointment = (Appointment)creator.Create(appointments[1].GetObjectId());
                Assert.AreEqual(2,drAppointment.GetContacts().GetChildCount(), String.Format("drAppointment Contacts.ChildCount Post Contact Save"));

                var reviewAppointment = (Appointment)creator.Create(appointments[3].GetObjectId());
                Assert.AreEqual(1, reviewAppointment.GetContacts().GetChildCount(), String.Format("review Contacts.ChildCount Post Contact Save"));
            }
        }
    }
}

