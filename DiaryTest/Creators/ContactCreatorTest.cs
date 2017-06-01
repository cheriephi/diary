using Diary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DiaryTest
{
    /// <summary>
    /// Tests the ContactCreator class.
    /// </summary>
    /// <see cref="ObjectIdTest">About thread safety.</see>
    [TestClass]
    public unsafe class ContactCreatorTest
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
        /// Tests the Contact accessors and persistence through the ContactCreator.
        /// </summary>
        [TestMethod]
        public void SaveAndLoadTest()
        {
            var builders = new ContactBuilder[3]
            {
                new ContactBuilder(),
                new ContactBuilder(),
                new ContactBuilder()
            };

            using (var creator = new ContactCreator())
            {
                builders[0].SetFirstName("Brian").SetLastName("Rothwell").SetContactInfo("brothwell@q.com");
                builders[1].SetFirstName("Billy").SetLastName("Bob").SetContactInfo("slingblade@msn.com");
                builders[2].SetFirstName("Jenny").SetLastName("Twotone").SetContactInfo("(210) 867-5308");

                foreach (var builder in builders)
                {
                    builder.SetCreator(creator);
                    var contact = (Contact)builder.Build();
                    builder.SetObjectId(contact.GetObjectId());

                    Helper.AssertAreEqual(builder, contact, "Original");
                }

                creator.Save();
            }

            using (var creator = new ContactCreator())  // Re-open the files.
            {
                foreach (var builder in builders)
                {
                    var objectId = builder.GetObjectId();
                    var savedContact = (Contact)creator.Create(objectId);

                    Helper.AssertAreEqual(builder, savedContact, "Saved");
                }
            }
        }
    }
}
