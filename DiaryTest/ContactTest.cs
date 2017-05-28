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
        /// Formats the input elements as a string.
        /// </summary>
        /// <returns>FirstName,LastName</returns>
        public static string ToString(Contact contact)
        {
            var name = contact.GetName();
            return String.Concat(name[0], ",", name[1]);
        }
        #endregion

        /// <summary>
        /// Tests the Contact accessors through its constructor.
        /// </summary>
        [TestMethod]
        public void ContactConstructorTest()
        {
            var builder = new ContactBuilder();
            builder.SetFirstName("First").SetLastName("Last");
            builder.SetContactInfo("ContactInfo");

            new DiaryProductHelper().assertEquals(builder, (Contact)builder.Build(), "");
        }
    }
}
