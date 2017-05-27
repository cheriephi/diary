using Diary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DiaryTest
{
    /// <summary>
    /// Tests the ContactCreator class.
    /// </summary>
    [TestClass]
    public class ContactCreatorTest
    {
        /// <summary>
        /// Tests ContactCreator Constructor via Contact.GetName.
        /// </summary>
        [TestMethod]
        public void GetContactNameTest()
        {
            var builder = new ContactBuilder().SetContactCreator(new ContactCreator());
            new ContactTest().GetContactNameTest(builder);
        }

        /// <summary>
        /// Tests ContactCreator Constructor Contact.GetContactInfo.
        /// </summary>
        [TestMethod]
        public void GetContactInfoTest()
        {
            var builder = new ContactBuilder().SetContactCreator(new ContactCreator());
            new ContactTest().GetContactInfoTest(builder);
        }

        /// <summary>
        /// Tests persistence via the ContactCreator.
        /// </summary>
        [TestMethod]
        public void SaveAndLoadTest()
        {
            var contactCreator = new ContactCreator();

            var contacts = new Contact[] 
            {
                (Contact)contactCreator.CreateNew("Brian", "Rothwell", "brothwell@q.com"),
                (Contact)contactCreator.CreateNew("Billy", "Bob", "slingblade@msn.com"),
                (Contact)contactCreator.CreateNew("Jenny", "Twotone", "(210) 867-5308")
            };

            contactCreator.Save();

            contactCreator = new ContactCreator();   // Re-open the files.

            foreach (var contact in contacts)
            {
                var objectId = contact.GetObjectId();
                var savedContact = (Contact)contactCreator.Create(objectId);

                Assert.AreEqual(ContactTest.ToString(contact), ContactTest.ToString(savedContact), "Names");
                Assert.IsTrue(savedContact.GetContactInfo().CompareTo(contact.GetContactInfo()) == 0, "ContactInfo");
            }
        }
    }
}
