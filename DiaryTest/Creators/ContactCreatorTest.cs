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
        /// Contact get name test via the ContactCreator.
        /// </summary>
        [TestMethod]
        public void GetContactNameTest()
        {
            var builder = new ContactBuilder().SetContactCreator(new ContactCreator());
            new ContactTest().GetContactNameTest(builder);
        }

        /// <summary>
        /// Contact get contact info test via the ContactCreator.
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
                (Contact)contactCreator.CreateNew("Jenny", "Twotone", "(210) 867-5308"),
                (Contact)contactCreator.CreateNew("James", "Bond", "(707) 555-2117"),
                (Contact)contactCreator.CreateNew("Tony", "Stark", "(303) 855-1212")
            };

            contactCreator.Save();

            contactCreator = new ContactCreator();   // Re-open the files 

            foreach (var contact in contacts)
            {
                var objectId = contact.GetObjectId();
                var savedContact = (Contact)contactCreator.Create(objectId);

                String[] names = contact.GetName();
                String[] savedNames = savedContact.GetName();
                Assert.IsTrue(savedNames[0].CompareTo(names[0]) == 0);
                Assert.IsTrue(savedNames[1].CompareTo(names[1]) == 0);
                Assert.IsTrue(savedContact.GetContactInfo().CompareTo(contact.GetContactInfo()) == 0);
            }
        }
    }
}
