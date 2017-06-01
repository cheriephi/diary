using Diary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DiaryTest.Creators
{
    /// <summary>
    /// Tests the PeriodicAppointmentCreator class.
    /// </summary>
    /// <see cref="ObjectIdTest">About thread safety.</see>
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
        /// Tests the PeriodicAppointment accessors through the PeriodicAppointmentCreator constructor.
        /// </summary>
        [TestMethod]
        public void PeriodicAppointmentCreatorConstructorTest()
        {
            // Wrap the creator in a using block so its resources will get released when the program no longer needs it.
            using (var creator = new PeriodicAppointmentCreator())
            {
                var builder = new PeriodicAppointmentBuilder();
                builder.SetCreator(creator);
                builder.SetOccurs(new Diary.DateTime(new Date(15, Date.Month.FEBRUARY, 2008), 15, 30));
                builder.SetLabel("Dr Apt");
                builder.SetDetails("At the library");

                Helper.AssertAreEqual(builder, (Appointment)builder.Build(), "");
            }
        }

        /// <summary>
        /// Tests persistence via the PeriodicAppointmentCreator.
        /// </summary>
        [TestMethod]
        public void SaveAndLoadTest()
        {
            var appointments = new PeriodicAppointment[3];

            using (var creator = new PeriodicAppointmentCreator())
            {
                var yogaBuilder = new PeriodicAppointmentBuilder();
                yogaBuilder.SetCreator(creator);
                yogaBuilder.SetOccurs(new Diary.DateTime(new Date(1, Date.Month.FEBRUARY, 2017), 6, 30));
                yogaBuilder.SetDurationMinutes(45);
                yogaBuilder.SetLabel("Yoga");
                yogaBuilder.SetDetails("Downward Dog");
                yogaBuilder.SetPeriodHours(24);
                yogaBuilder.SetNotToExceedDateTime(new Diary.DateTime(new Date(1, Date.Month.MARCH, 2017), 6, 30));
                appointments[0] = (PeriodicAppointment)yogaBuilder.Build();

                var doctorBuilder = new PeriodicAppointmentBuilder();
                doctorBuilder.SetCreator(creator);
                doctorBuilder.SetOccurs(new Diary.DateTime(new Date(2, Date.Month.MARCH, 2017), 8, 20));
                doctorBuilder.SetDurationMinutes(30);
                doctorBuilder.SetLabel("Ear Doctor");
                doctorBuilder.SetDetails("Annual Cleaning");
                doctorBuilder.SetPeriodHours(24 * 365);
                doctorBuilder.SetNotToExceedDateTime(new Diary.DateTime(new Date(1, Date.Month.MARCH, 2022), 8, 20));
                appointments[1] = (PeriodicAppointment)doctorBuilder.Build();

                var teacherBuilder = new PeriodicAppointmentBuilder();
                teacherBuilder.SetCreator(creator);
                teacherBuilder.SetOccurs(new Diary.DateTime(new Date(7, Date.Month.APRIL, 2017), 12, 00));
                teacherBuilder.SetDurationMinutes(25);
                teacherBuilder.SetLabel("School");
                teacherBuilder.SetDetails("5th Grade Arithmetic");
                teacherBuilder.SetPeriodHours(24);
                teacherBuilder.SetNotToExceedDateTime(new Diary.DateTime(new Date(12, Date.Month.APRIL, 2017), 12, 00));
                appointments[2] = (PeriodicAppointment)teacherBuilder.Build();

                creator.Save();
            }
        }
    }
}
