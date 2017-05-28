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
        /// Simple data test of GetLabel method.
        /// </summary>
        [TestMethod]
        public void GetLabelTest()
        {
            using (var creator = new ReminderCreator())
            {
                var expected = "Test Label";

                var reminder = (Reminder)new ReminderBuilder().SetLabel(expected).SetCreator(creator).Build();

                CalendarEventTest.GetLabelTest(reminder, expected);
            }
        }

        /// <summary>
        /// Tests ReminderCreator Constructor via Reminder.GetDetails.
        /// </summary>
        [TestMethod]
        public void GetDetailsTest()
        {
            using (var creator = new ReminderCreator())
            {
                var builder = new ReminderBuilder().SetCreator(creator);
                new ReminderTest().GetDetailsTest((ReminderBuilder)builder);
            }
        }

        /// <summary>
        /// Tests the boundaries around the IsOccuring method for a simple scenario.
        /// </summary>
        [TestMethod]
        public void IsOccuringOnTest()
        {
            using (var creator = new ReminderCreator())
            {
                var reminderDate = new Date(30, Date.Month.SEPTEMBER, 2000);
                var reminder = (Reminder)new ReminderBuilder().SetDate(reminderDate).SetCreator(creator).Build();

                var expectedStartDate = reminderDate;
                var expectedEndDate = new Date(reminderDate.GetDay(), reminderDate.GetMonth(), reminderDate.GetYear());

                CalendarEventTest.IsOccuringOnTest(reminder, expectedStartDate, expectedEndDate);
            }
        }


        /// <summary>
        /// Tests persistence via the ReminderCreator.
        /// </summary>
        [TestMethod]
        public void SaveAndLoadTest()
        {
            var Reminders = new Reminder[3];

            using (var creator = new ReminderCreator())
            {
                Reminders[0] = (Reminder)creator.CreateNew("Label 1", new Date(1, Date.Month.MAY, 2017), "Details 1");
                Reminders[1] = (Reminder)creator.CreateNew("Label 2", new Date(3, Date.Month.MAY, 2017), "Details 2");
                Reminders[2] = (Reminder)creator.CreateNew("Label 3", new Date(5, Date.Month.MAY, 2017), "Details 3");

                creator.Save();
            }

            using (var creator = new ReminderCreator())  // Re-open the files.
            {
                foreach (var reminder in Reminders)
                {
                    var objectId = reminder.GetObjectId();
                    var savedReminder = (Reminder)creator.Create(objectId);

                    Assert.AreEqual(reminder.GetLabel(), savedReminder.GetLabel(), "Label");
                    Assert.AreEqual(reminder.GetDetails(), savedReminder.GetDetails(), "Details");
                }
            }
        }
    }
}
