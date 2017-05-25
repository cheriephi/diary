using Diary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
namespace DiaryTest
{
    [TestClass]
    public class ContactCreatorTest
    {
        [TestMethod]
        public void InitializerConstructorTest()
        {
            var contactCreator = new ContactCreator();
            Contact contact = (Contact)contactCreator.CreateNew("Brian", "Rothwell", "brothwell@q.com");
            String[] names = contact.GetName();
            Assert.IsTrue(names[0].CompareTo("Brian") == 0);
            Assert.IsTrue(names[1].CompareTo("Rothwell") == 0);
            Assert.IsTrue(contact.GetContactInfo().CompareTo("brothwell@q.com") == 0);
        }

        [TestMethod]
        public void SaveAndLoadTest()
        {
            var contactCreator = new ContactCreator();

            var contact1 = (Contact)contactCreator.CreateNew("Brian", "Rothwell", "brothwell@q.com");
            var contact1ObjectId = contact1.GetObjectId();
            var contact2 = (Contact)contactCreator.CreateNew("Billy", "Bob", "slingblade@msn.com");
            var contact2ObjectId = contact2.GetObjectId();
            var contact3 = (Contact)contactCreator.CreateNew("Jenny", "Twotone", "(210) 867-5308");
            var contact3ObjectId = contact3.GetObjectId();
            var contact4 = (Contact)contactCreator.CreateNew("James", "Bond", "(707) 555-2117");
            var contact4ObjectId = contact4.GetObjectId();
            var contact5 = (Contact)contactCreator.CreateNew("Tony", "Stark", "(303) 855-1212");
            var contact5ObjectId = contact5.GetObjectId();

            contactCreator.Save();

            contactCreator = new ContactCreator();   // Re-open the files 
            var contact = (Contact)contactCreator.Create(contact1ObjectId);
            String[] names = contact.GetName();
            Assert.IsTrue(names[0].CompareTo("Brian") == 0);
            Assert.IsTrue(names[1].CompareTo("Rothwell") == 0);
            Assert.IsTrue(contact.GetContactInfo().CompareTo("brothwell@q.com") == 0);

            contact = (Contact)contactCreator.Create(contact2ObjectId);
            names = contact.GetName();
            Assert.IsTrue(names[0].CompareTo("Billy") == 0);
            Assert.IsTrue(names[1].CompareTo("Bob") == 0);

            Assert.IsTrue(contact.GetContactInfo().CompareTo("slingblade@msn.com") == 0);

            contact = (Contact)contactCreator.Create(contact3ObjectId);
            names = contact.GetName();
            Assert.IsTrue(names[0].CompareTo("Jenny") == 0);
            Assert.IsTrue(names[1].CompareTo("Twotone") == 0);
            Assert.IsTrue(contact.GetContactInfo().CompareTo("(210) 867-5308") == 0);

            contact = (Contact)contactCreator.Create(contact4ObjectId);
            names = contact.GetName();
            Assert.IsTrue(names[0].CompareTo("James") == 0);
            Assert.IsTrue(names[1].CompareTo("Bond") == 0);
            Assert.IsTrue(contact.GetContactInfo().CompareTo("(707) 555-2117") == 0);

            contact = (Contact)contactCreator.Create(contact5ObjectId);
            names = contact.GetName();
            Assert.IsTrue(names[0].CompareTo("Tony") == 0);
            Assert.IsTrue(names[1].CompareTo("Stark") == 0);
            Assert.IsTrue(contact.GetContactInfo().CompareTo("(303) 855-1212") == 0);
        }
    }
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
