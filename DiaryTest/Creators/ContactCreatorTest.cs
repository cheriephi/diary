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
        /// Tests the Contact accessors through the ContactCreator constructor.
        /// </summary>
        [TestMethod]
        public void ContactCreatorConstructorTest()
        {
            // Wrap the creator in a using block so its resources will get released when the program no longer needs it.
            using (var creator = new ContactCreator())
            {
                var builder = new ContactBuilder();
                builder.SetCreator(creator);
                builder.SetFirstName("First").SetLastName("Last");
                builder.SetContactInfo("ContactInfo");

                new DiaryProductHelper().assertEquals(builder, (Contact)builder.Build(), "");
            }
        }

        /// <summary>
        /// Tests persistence via the ContactCreator.
        /// </summary>
        [TestMethod]
        public void SaveAndLoadTest()
        {
            var contacts = new Contact[3];

            using (var creator = new ContactCreator())
            {
                contacts[0] = (Contact)creator.CreateNew("Brian", "Rothwell", "brothwell@q.com");
                contacts[1] = (Contact)creator.CreateNew("Billy", "Bob", "slingblade@msn.com");
                contacts[2] = (Contact)creator.CreateNew("Jenny", "Twotone", "(210) 867-5308");

                creator.Save();
            }

            using (var creator = new ContactCreator())  // Re-open the files.
            {
                foreach (var contact in contacts)
                {
                    var objectId = contact.GetObjectId();
                    var savedContact = (Contact)creator.Create(objectId);

                    Assert.AreEqual(contact.GetName()[0], savedContact.GetName()[0], "firstName");
                    Assert.AreEqual(contact.GetName()[1], savedContact.GetName()[1], "lastName");

                    Assert.IsTrue(savedContact.GetContactInfo().CompareTo(contact.GetContactInfo()) == 0, "contactInfo");
                }
            }
        }
    }
}
