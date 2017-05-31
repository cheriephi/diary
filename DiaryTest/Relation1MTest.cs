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

            var builder = new ContactBuilder().SetFirstName("Fred").SetLastName("Flintstone");

            relation.Add((Contact)new ContactBuilder().Build());
            relation.Add((Contact)builder.Build());
            relation.Add((Contact)new ContactBuilder().Build());

            Helper.AssertAreEqual(builder, relation.GetChild(1), "");
        }

        /// <summary>
        /// Tests the GetChild method shallow copy.
        /// </summary>
        [TestMethod]
        public void GetChildDateTest()
        {
            var relation = new Relation1M<Date>();
            relation.Add(new Date());

            var actual = relation.GetChild(0);

            Assert.AreEqual(1, actual.GetDay(), "day");
            Assert.AreSame(relation.GetChild(0), relation.GetChild(0), "shallow copy");
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
