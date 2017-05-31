using Diary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DiaryTest
{
    /// <summary>
    /// Summary description for DateTimeTest.
    /// </summary>
    /// <seealso cref="DateTest">For more detailed design explanations.</seealso>
    [TestClass]
    public class DateTimeTest
    {
        /// <summary>
        /// Required context for data driven testing.
        /// </summary>
        public TestContext TestContext { get; set; }

        #region Constructor Tests
        /// <summary>
        /// Simple single scenario test of default constructor.
        /// </summary>
        [TestMethod]
        public void DefaultConstructorTest()
        {
            var actual = new Diary.DateTime();

            Assert.AreEqual("1900-01-01 00:00", Helper.ToString(actual));
        }

        /// <summary>
        /// Single scenario test that the constructor properly initializes fields based on inputs.
        /// </summary>
        [TestMethod]
        public void InputDateConstructorTest()
        {
            Assert.AreEqual("2017-04-30 03:42", Helper.ToString(new Diary.DateTime(new Date(30, Date.Month.APRIL, 2017), 3, 42)));
        }

        /// <summary>
        /// Validates that non-conforming hours and minutes get translated into a meaningful date value.
        /// </summary>
        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", @"TestData\DateTime\DateTimeData.xml", "add", DataAccessMethod.Sequential)]
        public void InputDateConstructorOverflowTest()
        {
            var expectedYear = int.Parse(TestContext.DataRow["year"].ToString());
            var expectedMonthNumber = int.Parse(TestContext.DataRow["month"].ToString());
            var expectedMonth = (Date.Month)expectedMonthNumber;
            var expectedDay = int.Parse(TestContext.DataRow["day"].ToString());
            var expectedHours = int.Parse(TestContext.DataRow["hours"].ToString());
            var expectedMinutes = int.Parse(TestContext.DataRow["minutes"].ToString());

            var date = new Date(expectedDay, expectedMonth, expectedYear);

            var expected = TestContext.DataRow["expected"].ToString();
            var actual = Helper.ToString(new Diary.DateTime(date, expectedHours, expectedMinutes));

            Assert.AreEqual(expected, actual, "Input Date:<{0}>. Hours:<{1}>. Minutes:<{2}>.", Helper.ToString(date), expectedHours, expectedMinutes);
        }
        #endregion

        #region Comparison Tests
        /// <summary>
        /// Data testing of CompareTo function.
        /// </summary>
        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", @"TestData\DateTime\CompareDateTimeData.xml", "add", DataAccessMethod.Sequential)]
        public void CompareToTest()
        {
            var day = int.Parse(TestContext.DataRow["day"].ToString());
            var month = int.Parse(TestContext.DataRow["month"].ToString());
            var year = int.Parse(TestContext.DataRow["year"].ToString());
            var hours = int.Parse(TestContext.DataRow["hours"].ToString());
            var minutes = int.Parse(TestContext.DataRow["minutes"].ToString());

            var compareDay = int.Parse(TestContext.DataRow["compareDay"].ToString());
            var compareMonth = int.Parse(TestContext.DataRow["compareMonth"].ToString());
            var compareYear = int.Parse(TestContext.DataRow["compareYear"].ToString());
            var compareHours = int.Parse(TestContext.DataRow["compareHours"].ToString());
            var compareMinutes = int.Parse(TestContext.DataRow["compareMinutes"].ToString());

            var expected = int.Parse(TestContext.DataRow["expectedResult"].ToString());

            var dateTime = new Diary.DateTime(new Date(day, (Date.Month)month, year), hours, minutes);
            var compareDateTime = new Diary.DateTime(new Date(compareDay, (Date.Month)compareMonth, compareYear), compareHours, compareMinutes);

            var actual = dateTime.CompareTo(compareDateTime);

            Assert.AreEqual(expected, actual, "Input DateTime:<{0}>. Input Compare DateTime:<{1}>.", Helper.ToString(dateTime), Helper.ToString(compareDateTime));
        }

        /// <summary>
        /// Data testing of IsBetween function.
        /// </summary>
        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", @"TestData\DateTime\BetweenDateTimeData.xml", "add", DataAccessMethod.Sequential)]
        public void IsBetweenTest()
        {
            var startDay = int.Parse(TestContext.DataRow["startDay"].ToString());
            var startMonth = int.Parse(TestContext.DataRow["startMonth"].ToString());
            var startYear = int.Parse(TestContext.DataRow["startYear"].ToString());
            var startHours= int.Parse(TestContext.DataRow["startHours"].ToString());
            var startMinutes = int.Parse(TestContext.DataRow["startMinutes"].ToString());

            var endDay = int.Parse(TestContext.DataRow["endDay"].ToString());
            var endMonth = int.Parse(TestContext.DataRow["endMonth"].ToString());
            var endYear = int.Parse(TestContext.DataRow["endYear"].ToString());
            var endHours = int.Parse(TestContext.DataRow["endHours"].ToString());
            var endMinutes = int.Parse(TestContext.DataRow["endMinutes"].ToString());

            var day = int.Parse(TestContext.DataRow["Day"].ToString());
            var month = int.Parse(TestContext.DataRow["Month"].ToString());
            var year = int.Parse(TestContext.DataRow["Year"].ToString());
            var hours = int.Parse(TestContext.DataRow["hours"].ToString());
            var minutes = int.Parse(TestContext.DataRow["minutes"].ToString());

            var expectedIsBetweenString = TestContext.DataRow["isBetween"].ToString();
            bool expected = (expectedIsBetweenString == "1");

            var startDateTime = new Diary.DateTime(new Date(startDay, (Date.Month)startMonth, startYear), startHours, startMinutes);
            var endDateTime = new Diary.DateTime(new Date(endDay, (Date.Month)endMonth, endYear), endHours, endMinutes);
            var dateTime = new Diary.DateTime(new Date(day, (Date.Month)month, year), hours, minutes);

            var actual = dateTime.IsBetween(startDateTime, endDateTime);

            var message = String.Format("Input DateTime:<{0}>. Input Start DateTime:<{1}>. Input End DateTime:<{2}>.", Helper.ToString(dateTime), Helper.ToString(startDateTime), Helper.ToString(endDateTime));
            Assert.AreEqual(expected, actual, message);
        }
        #endregion

        #region Math Tests
        /// <summary>
        /// Tests the AddTime method.
        /// </summary>
        [TestMethod]
        public void AddTimeTest()
        {
            var dateTime = new Diary.DateTime(new Date(1, Date.Month.JANUARY, 1900), 0, 0);
            dateTime.AddTime(1, 1);

 
            Assert.AreEqual("1900-01-01 01:01", Helper.ToString(dateTime));
        }
        #endregion
    }
}
