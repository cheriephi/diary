using Diary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DiaryTest
{
    /// <summary>
    /// Tests the Contact class.
    /// </summary>
    /// <remarks>Tests the constructor via its accessors. Strings are immutable so no aliasing tests are required.</remarks>
    [TestClass]
    public class ContactTest
    {
        /// <summary>
        /// Simple data testing of GetName method
        /// </summary>
        [TestMethod]
        public void GetNameTest()
        {
            var firstName = "First";
            var lastName = "Last";
            var contact = new ContactBuilder().SetFirstName(firstName).SetLastName(lastName).Build();

            // Concatenate the results so all the values can be compared at once
            var expected = String.Concat(firstName, ",", lastName);
            var name = contact.GetName();
            var actual = String.Join(",", name);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Simple data testing of GetContactInfo method
        /// </summary>
        [TestMethod]
        public void GetContactInfo()
        {
            var expected = "Contact Info";
            var contact = new ContactBuilder().SetContactInfo(expected).Build();

            var actual = contact.GetContactInfo();

            Assert.AreEqual(expected, actual);
        }

        #region Persistence Tests
        /// <summary>
        /// Tests the ClassId accessor.
        /// </summary>
        [TestMethod]
        public void GetClassIdTest()
        {
            var contact = new ContactBuilder().Build();
            new DiaryProductTest().GetClassIdTest(contact, "Contact");
        }

        /// <summary>
        /// Tests the ObjectId accessor.
        /// </summary>
        [TestMethod]
        public void GetObjectIdTest()
        {
            var objectId = new ObjectId();
            var contact = new ContactBuilder().SetObjectId(objectId).Build();
            new DiaryProductTest().GetObjectIdTest(contact, objectId);
        }
        #endregion
    }
}
