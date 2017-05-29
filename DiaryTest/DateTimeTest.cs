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
            var builder = new DateTimeBuilder();
            var actual = new Diary.DateTime();

            Helper.AssertAreEqual(builder, actual, "");
        }

        /// <summary>
        /// Single scenario test that the constructor properly initializes fields based on inputs.
        /// </summary>
        [TestMethod]
        public void InputDateConstructorTest()
        {
            var builder = new DateTimeBuilder();
            builder.SetDate(new Date(30, Date.Month.APRIL, 2017));
            builder.SetHours(3);
            builder.SetMinutes(42);

            Helper.AssertAreEqual(builder, builder.Build(), "");
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
            var builder = new DateTimeBuilder();

            var day = int.Parse(TestContext.DataRow["day"].ToString());
            var month = int.Parse(TestContext.DataRow["month"].ToString());
            var year = int.Parse(TestContext.DataRow["year"].ToString());
            builder.SetDate(new Date(day, (Date.Month)month, year));

            builder.SetHours(int.Parse(TestContext.DataRow["hours"].ToString()));
            builder.SetMinutes(int.Parse(TestContext.DataRow["minutes"].ToString()));

            var compareBuilder = new DateTimeBuilder();

            var compareDay = int.Parse(TestContext.DataRow["compareDay"].ToString());
            var compareMonth = int.Parse(TestContext.DataRow["compareMonth"].ToString());
            var compareYear = int.Parse(TestContext.DataRow["compareYear"].ToString());
            compareBuilder.SetDate(new Date(compareDay, (Date.Month)compareMonth, compareYear));

            compareBuilder.SetHours(int.Parse(TestContext.DataRow["compareHours"].ToString()));
            compareBuilder.SetMinutes(int.Parse(TestContext.DataRow["compareMinutes"].ToString()));

            var expected = int.Parse(TestContext.DataRow["expectedResult"].ToString());

            var dateTime = builder.Build();
            var compareDateTime = compareBuilder.Build();

            var actual = dateTime.CompareTo(compareDateTime);

            Assert.AreEqual(expected, actual, "Input DateTime:<{0}>. Input Compare DateTime:<{1}>.", builder.ToString(), compareBuilder.ToString());
        }

        /// <summary>
        /// Data testing of IsBetween function.
        /// </summary>
        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", @"TestData\DateTime\BetweenDateTimeData.xml", "add", DataAccessMethod.Sequential)]
        public void IsBetweenTest()
        {
            var startBuilder = new DateTimeBuilder();

            var startDay = int.Parse(TestContext.DataRow["startDay"].ToString());
            var startMonth = int.Parse(TestContext.DataRow["startMonth"].ToString());
            var startYear = int.Parse(TestContext.DataRow["startYear"].ToString());
            startBuilder.SetDate(new Date(startDay, (Date.Month)startMonth, startYear));

            startBuilder.SetHours(int.Parse(TestContext.DataRow["startHours"].ToString()));
            startBuilder.SetMinutes(int.Parse(TestContext.DataRow["startMinutes"].ToString()));

            var endBuilder = new DateTimeBuilder();

            var endDay = int.Parse(TestContext.DataRow["endDay"].ToString());
            var endMonth = int.Parse(TestContext.DataRow["endMonth"].ToString());
            var endYear = int.Parse(TestContext.DataRow["endYear"].ToString());
            endBuilder.SetDate(new Date(endDay, (Date.Month)endMonth, endYear));

            endBuilder.SetHours(int.Parse(TestContext.DataRow["endHours"].ToString()));
            endBuilder.SetMinutes(int.Parse(TestContext.DataRow["endMinutes"].ToString()));

            var builder = new DateTimeBuilder();

            var day = int.Parse(TestContext.DataRow["Day"].ToString());
            var month = int.Parse(TestContext.DataRow["Month"].ToString());
            var year = int.Parse(TestContext.DataRow["Year"].ToString());
            builder.SetDate(new Date(day, (Date.Month)month, year));

            builder.SetHours(int.Parse(TestContext.DataRow["hours"].ToString()));
            builder.SetMinutes(int.Parse(TestContext.DataRow["minutes"].ToString()));

            var expectedIsBetweenString = TestContext.DataRow["isBetween"].ToString();
            bool expected = (expectedIsBetweenString == "1");

            var startDateTime = startBuilder.Build();
            var endDateTime = endBuilder.Build();
            var dateTime = builder.Build();

            var actual = dateTime.IsBetween(startDateTime, endDateTime);

            Assert.AreEqual(expected, actual, "Input DateTime:<{0}>. Input Start DateTime:<{1}>. Input End DateTime:<{2}>.", builder.ToString(), startBuilder.ToString(), endBuilder.ToString());
        }
        #endregion

        #region Math Tests
        /// <summary>
        /// Tests the AddTime method.
        /// </summary>
        [TestMethod]
        public void AddTimeTest()
        {
            var expected = new DateTimeBuilder();
            expected.SetDate(new Date(1, Date.Month.JANUARY, 1900));
            expected.SetHours(1).SetMinutes(1);

            var actual = new Diary.DateTime(new Date(1, Date.Month.JANUARY, 1900), 0, 0);

            actual.AddTime(1, 1);

            Helper.AssertAreEqual(expected, actual, "");
        }
        #endregion
    }
}
