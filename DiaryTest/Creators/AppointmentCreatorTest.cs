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
    }
}
