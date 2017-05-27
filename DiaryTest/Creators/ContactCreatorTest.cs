using Diary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

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
        /// Tests ContactCreator Constructor via Contact.GetName.
        /// </summary>
        [TestMethod]
        public void GetContactNameTest()
        {
            // Wrap the creator in a using block so its resources will get released when the program no longer needs it.
            using (var creator = new ContactCreator())
            {
                var builder = new ContactBuilder().SetContactCreator(creator);
                new ContactTest().GetContactNameTest(builder);
            }
        }

        /// <summary>
        /// Tests ContactCreator Constructor Contact.GetContactInfo.
        /// </summary>
        [TestMethod]
        public void GetContactInfoTest()
        {
            using (var creator = new ContactCreator())
            {
                var builder = new ContactBuilder().SetContactCreator(creator);
                new ContactTest().GetContactInfoTest(builder);
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

                    Assert.AreEqual(ContactTest.ToString(contact), ContactTest.ToString(savedContact), "Names");
                    Assert.IsTrue(savedContact.GetContactInfo().CompareTo(contact.GetContactInfo()) == 0, "ContactInfo");
                }
            }
        }
    }
}
