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
        /// Tests the Appointment accessors and persistence through the AppointmentCreator.
        /// </summary>
        [TestMethod]
        public void SaveAndLoadTest()
        {
            var builders = new AppointmentBuilder[4]
            {
                new AppointmentBuilder(),
                new AppointmentBuilder(),
                new AppointmentBuilder(),
                new AppointmentBuilder()
            };

            using (var creator = new AppointmentCreator())
            {
                var yogaDateTime = new Diary.DateTime(new Date(1, Date.Month.FEBRUARY, 2017), 6, 30);
                builders[0].SetLabel("Yoga").SetOccurs(yogaDateTime).SetDurationMinutes(45);
                builders[0].SetDetails("Downward Dog");

                var doctorsDateTime = new Diary.DateTime(new Date(2, Date.Month.MARCH, 2017), 8, 20);
                builders[1].SetLabel("Ear Doctor").SetOccurs(doctorsDateTime).SetDurationMinutes(30);
                builders[1].SetDetails("Annual Cleaning");

                var teacherDateTime = new Diary.DateTime(new Date(7, Date.Month.APRIL, 2017), 12, 00);
                builders[2].SetLabel("School").SetOccurs(teacherDateTime).SetDurationMinutes(25);
                builders[2].SetDetails("5th Grade Arithmatic");

                var reviewDateTime = new Diary.DateTime(new Date(30, Date.Month.MAY, 2017), 18, 30);
                builders[3].SetLabel("Performance Review").SetOccurs(reviewDateTime).SetDurationMinutes(60);
                builders[3].SetDetails("6 Month Evaluation");

                foreach (var builder in builders)
                {
                    builder.SetCreator(creator);
                    var appointment = (Appointment)builder.Build();
                    builder.SetObjectId(appointment.GetObjectId());

                    Helper.AssertAreEqual(builder, appointment, "Original");
                }

                creator.Save();
            }

            using (var appointmentCreator = new AppointmentCreator()) // Re-open the files
            {
                foreach (var builder in builders)
                {
                    var objectId = builder.GetObjectId();
                    var savedAppointment = (Appointment)appointmentCreator.Create(objectId);

                    Helper.AssertAreEqual(builder, savedAppointment, "Saved");
                    Assert.AreEqual(0, savedAppointment.GetContacts().GetChildCount(), "Contacts.ChildCount");
                }

                // Now add some contacts to the existing appointments
                using (var contactCreator = new ContactCreator())
                {
                    var contact1 = (Contact)contactCreator.CreateNew("Brian", "Rothwell", "brothwell@q.com");
                    var contact2 = (Contact)contactCreator.CreateNew("Billy", "Bob", "slingblade@msn.com");
                    var contact3 = (Contact)contactCreator.CreateNew("Jenny", "Twotone", "(210) 867-5308");
                    contactCreator.Save();

                    var drAppointment = (Appointment)appointmentCreator.Create(builders[1].GetObjectId());
                    drAppointment.AddRelation(contact1);
                    drAppointment.AddRelation(contact2);

                    var reviewAppointment = (Appointment)appointmentCreator.Create(builders[3].GetObjectId());
                    reviewAppointment.AddRelation(contact3);
                    appointmentCreator.Save();
                }
            }

            using (var creator = new AppointmentCreator())   // Re-open the files
            {
                foreach (var builder in builders)
                {
                    var objectId = builder.GetObjectId();
                    var savedAppointment = (Appointment)creator.Create(objectId);

                    Helper.AssertAreEqual(builder, savedAppointment, "Post Contact Save");
                }

                var drAppointment = (Appointment)creator.Create(builders[1].GetObjectId());
                Assert.AreEqual(2,drAppointment.GetContacts().GetChildCount(), String.Format("drAppointment Contacts.ChildCount Post Contact Save"));

                var reviewAppointment = (Appointment)creator.Create(builders[3].GetObjectId());
                Assert.AreEqual(1, reviewAppointment.GetContacts().GetChildCount(), String.Format("review Contacts.ChildCount Post Contact Save"));
            }
        }
    }
}

