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
            var contact = new Contact(new ObjectId(), firstName, lastName, "");

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
            var contact = new Contact(new ObjectId(), "", "", expected);

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
            var contact = new Contact(new ObjectId(), "", "", "");
            new DiaryProductTest().GetClassIdTest(contact, "Contact");
        }

        /// <summary>
        /// Tests the ObjectId accessor.
        /// </summary>
        [TestMethod]
        public void GetObjectIdTest()
        {
            var objectId = new ObjectId();
            var contact = new Contact(objectId, "", "", "");
            new DiaryProductTest().GetObjectIdTest(contact, objectId);
        }
        #endregion
    }
}
