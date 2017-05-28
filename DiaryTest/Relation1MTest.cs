using Diary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DiaryTest
{
    /// <summary>
    /// Tests the Relation1M class.
    /// </summary>
    [TestClass]
    public class Relation1MTest
    {
        /// <summary>
        /// Tests the GetChildCount method.
        /// </summary>
        [TestMethod]
        public void GetChildCountTest()
        {
            var relation = new Relation1M<Contact>();
            relation.Add((Contact)new ContactBuilder().Build());
            relation.Add((Contact)new ContactBuilder().Build());
            relation.Add((Contact)new ContactBuilder().Build());

            var actual = relation.GetChildCount();
            Assert.AreEqual(3, actual);
        }

        /// <summary>
        /// Tests the GetChild method.
        /// </summary>
        [TestMethod]
        public void GetChildContactTest()
        {
            var relation = new Relation1M<Contact>();
            var contact = (Contact)new ContactBuilder().SetFirstName("Fred").SetLastName("Flintstone").Build();
            relation.Add((Contact)new ContactBuilder().Build());
            relation.Add((Contact)contact);
            relation.Add((Contact)new ContactBuilder().Build());

            var actual = relation.GetChild(1);
            Assert.AreEqual(ContactTest.ToString(contact), ContactTest.ToString(actual));
        }

        /// <summary>
        /// Tests the GetChild method shallow copy using a relation that is mutable.
        /// </summary>
        [TestMethod]
        public void GetChildDateTest()
        {
            var date = new Date();

            var relation = new Relation1M<Date>();
            relation.Add(date);

            var actual = relation.GetChild(0);
            Assert.AreEqual(1, actual.GetDay(), "Original");

            date.AddDays(1);

            actual = relation.GetChild(0);
            Assert.AreNotEqual(1, actual.GetDay(), "After");
        }

        /// <summary>
        /// Tests the GetChild method using a child index that is out of range.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(System.ArgumentOutOfRangeException))]
        public void InvalidIndexGetChildTest()
        {
            var relation = new Relation1M<Contact>();
            relation.Add((Contact)new ContactBuilder().Build());

            var actual = relation.GetChild(1);
        }
    }
}
