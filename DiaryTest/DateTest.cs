using Diary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DiaryTest
{
    /// <summary>
    /// Date tests.
    /// </summary>
    /// <remarks>
    /// Uses data driven tests to simplify testing of multiple data conditions. 
    /// Test failures include additional context information to assist in debugging.
    /// Testing of encapsulation (corrupting of return values) is currently out of scope.
    /// </remarks>
    /// <seealso href="http://prasadhonrao.com/back-to-basics-data-driven-unit-testing/"/>
    [TestClass]
    public class DateTest
    {
        /// <summary>
        /// Required context for data driven testing
        /// </summary>
        public TestContext TestContext { get; set; }

        #region Constructor Tests
        /// <summary>
        /// Simple single scenario test of default constructor.
        /// </summary>
        [TestMethod]
        public void DefaultConstructorTest()
        {
            Assert.AreEqual("1900-01-01", Helper.ToString(new Date()));
        }

        /// <summary>
        /// Data testing that the constructor properly initializes fields based on inputs.
        /// </summary>
        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", @"TestData\Date\DateData.xml", "add", DataAccessMethod.Sequential)]
        public void InputDateConstructorTest()
        {
            var day = int.Parse(TestContext.DataRow["day"].ToString());
            var month = int.Parse(TestContext.DataRow["month"].ToString());
            var year = int.Parse(TestContext.DataRow["year"].ToString());
            var expected = TestContext.DataRow["date"].ToString();

            var date = new Date(day, (Date.Month)month, year);

            Assert.AreEqual(expected, Helper.ToString(date));
        }

        /// <summary>
        /// Validates the constructor throws an exception if invalid inputs are passed in.
        /// </summary>
        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", @"TestData\Date\InvalidDateData.xml", "add", DataAccessMethod.Sequential)]
        [ExpectedException(typeof(System.ArgumentOutOfRangeException))]
        public void InvalidInputDateConstructorTest()
        {
            var expectedYear = int.Parse(TestContext.DataRow["year"].ToString());
            var expectedMonth = (Date.Month)int.Parse(TestContext.DataRow["month"].ToString());
            var expectedDay = int.Parse(TestContext.DataRow["day"].ToString());

            var date = new Date(expectedDay, expectedMonth, expectedYear);
        }
        #endregion

        #region Accessor Tests
        /// <summary>
        /// Data testing of days of weeks.
        /// </summary>
        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", @"TestData\Date\DateData.xml", "add", DataAccessMethod.Sequential)]
        public void DayOfWeekTest()
        {
            var expectedYear = int.Parse(TestContext.DataRow["year"].ToString());
            var expectedMonth = (Date.Month)int.Parse(TestContext.DataRow["month"].ToString());
            var expectedDay = int.Parse(TestContext.DataRow["day"].ToString());
            var expectedDayOfWeek = TestContext.DataRow["dayOfWeek"].ToString().ToUpper();

            Date date = new Date(expectedDay, expectedMonth, expectedYear);
            var actualDayOfWeek = date.GetDayOfWeek().ToString().ToUpper();

            Assert.AreEqual(expectedDayOfWeek, actualDayOfWeek);
        }
        #endregion

        #region Comparison Tests
        /// <summary>
        /// Data testing of CompareTo function.
        /// </summary>
        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", @"TestData\Date\CompareDateData.xml", "add", DataAccessMethod.Sequential)]
        public void CompareToTest()
        {
            var day = int.Parse(TestContext.DataRow["day"].ToString());
            var month = int.Parse(TestContext.DataRow["month"].ToString());
            var year = int.Parse(TestContext.DataRow["year"].ToString());

            var compareDay = int.Parse(TestContext.DataRow["compareDay"].ToString());
            var compareMonth = int.Parse(TestContext.DataRow["compareMonth"].ToString());
            var compareYear = int.Parse(TestContext.DataRow["compareYear"].ToString());

            var expected = int.Parse(TestContext.DataRow["expectedResult"].ToString());

            var date = new Date(day, (Date.Month)month, year);
            var compareDate = new Date(compareDay, (Date.Month)compareMonth, compareYear);

            var actual = date.CompareTo(compareDate);

            Assert.AreEqual(expected, actual, "Input Date:<{0}>. Input Compare Date:<{1}>.", Helper.ToString(date), Helper.ToString(compareDate));
        }

        /// <summary>
        /// Data testing of IsBetween function.
        /// </summary>
        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", @"TestData\Date\BetweenDateData.xml", "add", DataAccessMethod.Sequential)]
        public void IsBetweenTest()
        {
            var startDay = int.Parse(TestContext.DataRow["startDay"].ToString());
            var startMonth = int.Parse(TestContext.DataRow["startMonth"].ToString());
            var startYear = int.Parse(TestContext.DataRow["startYear"].ToString());

            var endDay = int.Parse(TestContext.DataRow["endDay"].ToString());
            var endMonth = int.Parse(TestContext.DataRow["endMonth"].ToString());
            var endYear = int.Parse(TestContext.DataRow["endYear"].ToString());

            var day = int.Parse(TestContext.DataRow["day"].ToString());
            var month = int.Parse(TestContext.DataRow["month"].ToString());
            var year = int.Parse(TestContext.DataRow["year"].ToString());

            var expectedIsBetweenString = TestContext.DataRow["isBetween"].ToString();
            bool expected = (expectedIsBetweenString == "1");

            var startDate = new Date(startDay, (Date.Month)startMonth, startYear);
            var endDate = new Date(endDay, (Date.Month)endMonth, endYear);
            var date = new Date(day, (Date.Month)month, year);

            var actual = date.IsBetween(startDate, endDate);

            Assert.AreEqual(expected, actual, "Date:<{0}>. Start Date:<{1}>. End Date:<{2}>.", Helper.ToString(date), Helper.ToString(startDate), Helper.ToString(endDate));
        }
        #endregion

        #region Math Tests
        /// <summary>
        /// Tests the AddDays method.
        /// </summary>
        [TestMethod]
        public void AddDaysTest()
        {
            var actual = new Date(3, Date.Month.JANUARY, 1900);
            actual.AddDays(2);

            Assert.AreEqual("1900-01-05", Helper.ToString(actual));
        }

        /// <summary>
        /// Tests the SubtractDays method.
        /// </summary>
        [TestMethod]
        public void SubtractDaysTest()
        {
            var actual = new Date(30, Date.Month.DECEMBER, 1899);
            actual.SubtractDays(2);

            Assert.AreEqual("1899-12-28", Helper.ToString(actual));
        }

        /// <summary>
        /// Tests the DaysUntil method.
        /// </summary>
        [TestMethod]
        public void DaysUntilTest()
        {
            var expected = 7;

            var untilDate = new Date();
            var date = new Date();
            date.AddDays(expected);

            var actual = date.DaysUntil(untilDate);

            Assert.AreEqual(expected, actual);
        }
        #endregion

        #region Date Lookup Tests
        /// <summary>
        /// Data testing that the leap year is calculated properly.
        /// </summary>
        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", @"TestData\Date\LeapYearData.xml", "add", DataAccessMethod.Sequential)]
        public void IsLeapYearTest()
        {
            var year = int.Parse(TestContext.DataRow["year"].ToString());
            var isLeapYearString = TestContext.DataRow["isLeapYear"].ToString();
            bool expected = (isLeapYearString == "1");

            var actual = Date.IsLeapYear(year);

            Assert.AreEqual(expected, actual, "Input Year:<{0}>.", year);
        }

        /// <summary>
        /// Data testing that the last day of the month is calculated properly.
        /// </summary>
        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", @"TestData\Date\LastDayOfMonthData.xml", "add", DataAccessMethod.Sequential)]
        public void GetLastDayOfMonthTest()
        {
            var year = int.Parse(TestContext.DataRow["year"].ToString());
            var monthNumber = int.Parse(TestContext.DataRow["month"].ToString());
            var month = (Date.Month)monthNumber;
            var expected = int.Parse(TestContext.DataRow["day"].ToString());

            var actual = Date.GetLastDayOfMonth(month, year);

            Assert.AreEqual(expected, actual, "Input Year:<{0}>. Input Month:<{1}>.", year, monthNumber);
        }
        #endregion
    }
}
