using Diary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DiaryTest
{
    /// <summary>
    /// Tests the ReminderCreator class.
    /// </summary>
    /// <see cref="ContactCreatorTest">About overall test design.</see>
    [TestClass]
    public unsafe class ReminderCreatorTest
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
        /// Tests the Reminder accessors and persistence via the ReminderCreator.
        /// </summary>
        [TestMethod]
        public void SaveAndLoadTest()
        {
            var builders = new ReminderBuilder[3]
            {
                new ReminderBuilder(),
                new ReminderBuilder(),
                new ReminderBuilder()
            };

            using (var creator = new ReminderCreator())
            {
                builders[0].SetLabel("Label 1").SetDate(new Date(1, Date.Month.MAY, 2017)).SetDetails("Details 1");
                builders[1].SetLabel("Label 2").SetDate(new Date(3, Date.Month.MAY, 2017)).SetDetails("Details 2");
                builders[2].SetLabel("Label 3").SetDate(new Date(5, Date.Month.MAY, 2017)).SetDetails("Details 3");

                foreach (var builder in builders)
                {
                    builder.SetCreator(creator);
                    var reminder = (Reminder)builder.Build();
                    builder.SetObjectId(reminder.GetObjectId());

                    Helper.AssertAreEqual(builder, reminder, "Original");
                }

                creator.Save();
            }

            using (var creator = new ReminderCreator())  // Re-open the files.
            {
                foreach (var builder in builders)
                {
                    var objectId = builder.GetObjectId();
                    var savedReminder = (Reminder)creator.Create(objectId);

                    Helper.AssertAreEqual(builder, savedReminder, "Saved");
                }
            }
        }
    }
}
