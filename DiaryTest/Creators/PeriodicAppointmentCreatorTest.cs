using Diary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DiaryTest.Creators
{
    /// <summary>
    /// Tests the PeriodicAppointmentCreator class.
    /// </summary>
    /// <see cref="ObjectIdTest">About thread safety.</see>
    [TestClass]
    public unsafe class PeriodicAppointmentCreatorTest
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
        /// Tests the PeriodicAppointment accessors and persistence through the PeriodicAppointmentCreator.
        /// </summary>
        [TestMethod]
        public void SaveAndLoadTest()
        {
            var builders = new PeriodicAppointmentBuilder[3]
            {
                new PeriodicAppointmentBuilder(),
                new PeriodicAppointmentBuilder(),
                new PeriodicAppointmentBuilder()
            };

            using (var creator = new PeriodicAppointmentCreator())
            {
                builders[0].SetOccurs(new Diary.DateTime(new Date(1, Date.Month.FEBRUARY, 2017), 6, 30));
                builders[0].SetDurationMinutes(45);
                builders[0].SetLabel("Yoga");
                builders[0].SetDetails("Downward Dog");
                builders[0].SetPeriodHours(24);
                builders[0].SetNotToExceedDateTime(new Diary.DateTime(new Date(1, Date.Month.MARCH, 2017), 6, 30));

                builders[1].SetOccurs(new Diary.DateTime(new Date(2, Date.Month.MARCH, 2017), 8, 20));
                builders[1].SetDurationMinutes(30);
                builders[1].SetLabel("Ear Doctor");
                builders[1].SetDetails("Annual Cleaning");
                builders[1].SetPeriodHours(24 * 365);
                builders[1].SetNotToExceedDateTime(new Diary.DateTime(new Date(1, Date.Month.MARCH, 2022), 8, 20));
 
                builders[2].SetOccurs(new Diary.DateTime(new Date(7, Date.Month.APRIL, 2017), 12, 00));
                builders[2].SetDurationMinutes(25);
                builders[2].SetLabel("School");
                builders[2].SetDetails("5th Grade Arithmetic");
                builders[2].SetPeriodHours(24);
                builders[2].SetNotToExceedDateTime(new Diary.DateTime(new Date(12, Date.Month.APRIL, 2017), 12, 00));

                foreach (var builder in builders)
                {
                    builder.SetCreator(creator);
                    var periodicAppointment = (PeriodicAppointment)builder.Build();
                    builder.SetObjectId(periodicAppointment.GetObjectId());

                    Helper.AssertAreEqual(builder, periodicAppointment, "Original");
                }

                creator.Save();
            }

            using (var creator = new PeriodicAppointmentCreator())  // Re-open the files.
            {
                foreach (var builder in builders)
                {
                    var objectId = builder.GetObjectId();
                    var savedPeriodicAppointment = (PeriodicAppointment)creator.Create(objectId);

                    Helper.AssertAreEqual(builder, savedPeriodicAppointment, "Saved");
                }
            }
        }
    }
}
