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
        /// Tests the Contact accessors through its constructor.
        /// </summary>
        [TestMethod]
        public void ContactConstructorTest()
        {
            var builder = new ContactBuilder();
            builder.SetFirstName("First").SetLastName("Last");
            builder.SetContactInfo("ContactInfo");

            Helper.AssertEquals(builder, (Contact)builder.Build(), "");
        }
    }
}
