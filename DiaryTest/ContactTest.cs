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
        #region Helper Methods
        /// <summary>
        /// Formats the input elements as a string. Supports meaningful equality checks and debugging.
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <returns>FirstName,LastName</returns>
        public static string ToString(String firstName, String lastName)
        {
            return String.Concat(firstName, ",", lastName);
        }
        
        /// <summary>
         /// Formats the input elements as a string.
         /// </summary>
         /// <returns>FirstName,LastName</returns>
         /// <seealso cref="ToString(string, string)"/>
        public static string ToString(Contact contact)
        {
            var name = contact.GetName();
            return ToString(name[0], name[1]);
        }
        #endregion

        #region Contact generic test functions
        /// <summary>
        /// Simple data testing of GetContactName method.
        /// </summary>
        internal void GetContactNameTest(ContactBuilder builder)
        {
            var firstName = "First";
            var lastName = "Last";

            var contact = new ContactBuilder().SetFirstName(firstName).SetLastName(lastName).Build();

            var expected = ToString(firstName, lastName);
            var actual = ToString(contact);

            Assert.AreEqual(expected, actual);
        }
        
        /// <summary>
        /// Simple data testing of GetContactInfo method.
        /// </summary>
        internal void GetContactInfoTest(ContactBuilder builder)
        {
            var expected = "Contact Info";
            var contact = builder.SetContactInfo(expected).Build();

            var actual = contact.GetContactInfo();

            Assert.AreEqual(expected, actual);
        }
        #endregion

        /// <summary>
        /// Contact get name test.
        /// </summary>
        [TestMethod]
        public void GetNameTest()
        {
            GetContactNameTest(new ContactBuilder());
        }

        /// <summary>
        /// Contact get contact info test.
        /// </summary>
        [TestMethod]
        public void GetContactInfo()
        {
            GetContactInfoTest(new ContactBuilder());
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
