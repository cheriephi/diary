using System;
using Diary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DiaryTest
{
    /// <summary>
    /// Summary description for DateTimeTest
    /// </summary>
    /// <seealso cref="DateTest">For more detailed design explanations</see>
    [TestClass]
    public class DateTimeTest
    {
        /// <summary>
        /// Required context for data driven testing
        /// </summary>
        public TestContext TestContext { get; set; }

        #region Helper Methods
        /// <summary>
        /// Returns the class' identifying properties to support meaningful equality checks and debugging
        /// </summary>
        /// <returns>yyyy-MM-dd hh:mm</returns>
        public static string ToString(Diary.DateTime dateTime)
        {
            return String.Format("{0} {1}:{2}", DateTest.ToString(dateTime.GetDate()), dateTime.GetHours().ToString("00"), dateTime.GetMinutes().ToString("00"));
        }

        #endregion

        #region Constructor Tests
        /// <summary>
        /// Simple single scenario test of default constructor
        /// </summary>
        [TestMethod]
        public void DefaultConstructorTest()
        {
            var expected = ToString(new Diary.DateTime(new Date(), 0, 0));
            var actual = ToString(new Diary.DateTime());

            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}
