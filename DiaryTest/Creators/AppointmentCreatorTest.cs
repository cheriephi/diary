using Diary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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

                Helper.AssertAreEqual(builder, (Appointment)builder.Build(), "");
            }
        }

        [TestMethod]
        public void SaveAndLoadTest()
        {
            DateTime yogaDateTime;
            DateTime doctorsDateTime;
            DateTime teacherDateTime;
            DateTime reviewDateTime;

            ObjectId objectId1;
            ObjectId objectId2;
            ObjectId objectId3;
            ObjectId objectId4;

            using (var creator = new AppointmentCreator())
            {
                yogaDateTime = new DateTime(new Date(1, Date.Month.FEBRUARY, 2017), 6, 30);
                objectId1 = creator.CreateNew("Yoga", yogaDateTime, 45, "Downward Dog").GetObjectId();

                doctorsDateTime = new DateTime(new Date(2, Date.Month.MARCH, 2017), 8, 20);
                objectId2 = creator.CreateNew("Ear Doctor", doctorsDateTime, 30, "Annual Cleaning").GetObjectId();

                teacherDateTime = new DateTime(new Date(7, Date.Month.APRIL, 2017), 12, 00);
                objectId3 = creator.CreateNew("School", teacherDateTime, 25, "5th Grade Arithmatic").GetObjectId();

                reviewDateTime = new DateTime(new Date(30, Date.Month.MAY, 2017), 18, 30);
                objectId4 = creator.CreateNew("Performance Review", reviewDateTime, 60, "6 Month Evaluation").GetObjectId();

                creator.Save();
            }

            Appointment yogaAppointment;
            Appointment drAppointment;
            Appointment teacherAppointment;
            Appointment reviewAppointment;

            using (var creator = new AppointmentCreator()) // Re-open the files
            {
                yogaAppointment = (Appointment)creator.Create(objectId1);
                Assert.IsTrue(yogaAppointment.GetLabel().CompareTo("Yoga") == 0);
                Assert.IsTrue(yogaAppointment.GetStartTime().CompareTo(yogaDateTime) == 0);
                Assert.IsTrue(yogaAppointment.GetDurationMinutes() == 45);


                drAppointment = (Appointment)creator.Create(objectId2);
                Assert.IsTrue(drAppointment.GetLabel().CompareTo("Ear Doctor") == 0);
                Assert.IsTrue(drAppointment.GetStartTime().CompareTo(doctorsDateTime) == 0);
                Assert.IsTrue(drAppointment.GetDurationMinutes() == 30);

                teacherAppointment = (Appointment)creator.Create(objectId3);
                Assert.IsTrue(teacherAppointment.GetLabel().CompareTo("School") == 0);
                Assert.IsTrue(teacherAppointment.GetStartTime().CompareTo(teacherDateTime) == 0);
                Assert.IsTrue(teacherAppointment.GetDurationMinutes() == 25);

                reviewAppointment = (Appointment)creator.Create(objectId4);
                Assert.IsTrue(reviewAppointment.GetLabel().CompareTo("Performance Review") == 0);
                Assert.IsTrue(reviewAppointment.GetStartTime().CompareTo(reviewDateTime) == 0);
                Assert.IsTrue(reviewAppointment.GetDurationMinutes() == 60);

                // Now add some contacts to the existing appointments
                using (var contactCreator = new ContactCreator())
                {
                    var contact1 = (Contact)contactCreator.CreateNew("Brian", "Rothwell", "brothwell@q.com");
                    var contact2 = (Contact)contactCreator.CreateNew("Billy", "Bob", "slingblade@msn.com");
                    var contact3 = (Contact)contactCreator.CreateNew("Jenny", "Twotone", "(210) 867-5308");
                    contactCreator.Save();

                    drAppointment.AddRelation(contact1);
                    drAppointment.AddRelation(contact2);

                    reviewAppointment.AddRelation(contact3);
                    creator.Save();
                }
            }

            using (var creator = new AppointmentCreator())   // Re-open the files
            {
                drAppointment = (Appointment)creator.Create(objectId2);
                Assert.IsTrue(drAppointment.GetLabel().CompareTo("Ear Doctor") == 0);
                Assert.IsTrue(drAppointment.GetStartTime().CompareTo(doctorsDateTime) == 0);
                Assert.IsTrue(drAppointment.GetDurationMinutes() == 30);
                Assert.IsTrue(drAppointment.GetContacts().GetChildCount() == 2);

                reviewAppointment = (Appointment)creator.Create(objectId4);
                Assert.IsTrue(reviewAppointment.GetLabel().CompareTo("Performance Review") == 0);
                Assert.IsTrue(reviewAppointment.GetStartTime().CompareTo(reviewDateTime) == 0);
                Assert.IsTrue(reviewAppointment.GetDurationMinutes() == 60);
                Assert.IsTrue(reviewAppointment.GetContacts().GetChildCount() == 1);

            }
        }

    }
}

