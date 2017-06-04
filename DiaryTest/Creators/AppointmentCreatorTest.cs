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
                builders[2].SetDetails("5th Grade Arithmetic");

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
                }

                // Now add some contacts to the existing appointments
                using (var contactCreator = new ContactCreator())
                {
                    builders[1].SetContactBuilders();
                    builders[3].SetContactBuilders();
                    var contactBuilders = builders[1].GetContactBuilders();
                    var contacts = new Contact[contactBuilders.Length];
                    for (int i = 0; i < contactBuilders.Length; i++)
                    {
                        contactBuilders[i].SetCreator(contactCreator);
                        contacts[i] = (Contact)contactBuilders[i].Build();
                    }

                    contactCreator.Save();

                    var drAppointment = (Appointment)appointmentCreator.Create(builders[1].GetObjectId());
                    var reviewAppointment = (Appointment)appointmentCreator.Create(builders[3].GetObjectId());
                    for (int i = 0; i < contacts.Length; i++)
                    { 
                        drAppointment.AddRelation(contacts[i]);
                        reviewAppointment.AddRelation(contacts[i]);
                    }

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
            }
        }
    }
}

