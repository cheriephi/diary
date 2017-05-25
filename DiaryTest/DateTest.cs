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

        #region Helper Methods
        /// <summary>
        /// Formats the input elements as a string. Supports meaningful equality checks and debugging.
        /// </summary>
        /// <returns>yyyy-mm-dd</returns>
        public static string ToString(int day, int month, int year)
        {
            return String.Format("{0}-{1}-{2}", year.ToString("0000"), month.ToString("00"), day.ToString("00"));
        }

        /// <summary>
        /// Formats the input elements as a string.
        /// </summary>
        /// <returns>yyyy-MM-dd</returns>
        /// <seealso cref="ToString(int, int, int)"/>
        public static string ToString(Date date)
        {
            return ToString(date.GetDay(), (int)date.GetMonth(), date.GetYear());
        }
        #endregion

        #region Constructor Tests
        /// <summary>
        /// Simple single scenario test of default constructor.
        /// </summary>
        [TestMethod]
        public void DefaultConstructorTest()
        {
            var expected = ToString(1, 1, 1900);
            var actual = ToString(new Date());

            Assert.AreEqual(expected, actual);
        }
        
        /// <summary>
        /// Data testing that the constructor properly initializes fields based on inputs.
        /// </summary>
        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", @"TestData\Date\DateData.xml", "add", DataAccessMethod.Sequential)]
        public void InputDateConstructorTest()
        {
            var expectedYear = int.Parse(TestContext.DataRow["year"].ToString());
            var expectedMonthNumber = int.Parse(TestContext.DataRow["month"].ToString());
            var expectedMonth = (Date.Month)expectedMonthNumber;
            var expectedDay = int.Parse(TestContext.DataRow["day"].ToString());

            var expected = ToString(expectedDay, expectedMonthNumber, expectedYear);
            var actual = ToString(new Date(expectedDay, expectedMonth, expectedYear));

            Assert.AreEqual(expected, actual);
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
            var year = int.Parse(TestContext.DataRow["year"].ToString());
            var month = (Date.Month)int.Parse(TestContext.DataRow["month"].ToString());
            var day = int.Parse(TestContext.DataRow["day"].ToString());

            var compareYear = int.Parse(TestContext.DataRow["compareYear"].ToString());
            var compareMonth = (Date.Month)int.Parse(TestContext.DataRow["compareMonth"].ToString());
            var compareDay = int.Parse(TestContext.DataRow["compareDay"].ToString());

            var expected = int.Parse(TestContext.DataRow["expectedResult"].ToString());

            var date = new Date(day, month, year);
            var compareDate = new Date(compareDay, compareMonth, compareYear);

            var actual = date.CompareTo(compareDate);

            Assert.AreEqual(expected, actual, "Input Date:<{0}>. Input Compare Date:<{1}>.", ToString(date), ToString(compareDate));
        }

        /// <summary>
        /// Data testing of IsBetween function.
        /// </summary>
        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", @"TestData\Date\BetweenDateData.xml", "add", DataAccessMethod.Sequential)]
        public void IsBetweenTest()
        {
            var startYear = int.Parse(TestContext.DataRow["startYear"].ToString());
            var startMonth = (Date.Month)int.Parse(TestContext.DataRow["startMonth"].ToString());
            var startDay = int.Parse(TestContext.DataRow["startDay"].ToString());

            var endYear = int.Parse(TestContext.DataRow["endYear"].ToString());
            var endMonth = (Date.Month)int.Parse(TestContext.DataRow["endMonth"].ToString());
            var endDay = int.Parse(TestContext.DataRow["endDay"].ToString());

            var year = int.Parse(TestContext.DataRow["year"].ToString());
            var month = (Date.Month)int.Parse(TestContext.DataRow["month"].ToString());
            var day = int.Parse(TestContext.DataRow["day"].ToString());

            var expectedIsBetweenString = TestContext.DataRow["isBetween"].ToString();
            bool expected = (expectedIsBetweenString == "1");

            var startDate = new Date(startDay, startMonth, startYear);
            var endDate = new Date(endDay, endMonth, endYear);
            var date = new Date(day, month, year);

            var actual = date.IsBetween(startDate, endDate);

            Assert.AreEqual(expected, actual, "Input Date:<{0}>. Input Start Date:<{1}>. Input End Date:<{2}>.", ToString(date), ToString(startDate), ToString(endDate));
        }
        #endregion

        #region Math Tests
        /// <summary>
        /// Tests the AddDays method.
        /// </summary>
        [TestMethod]
        public void AddDaysTest()
        {
            var expected = new Date(3, Date.Month.JANUARY, 1900);
            var actual = new Date();

            actual.AddDays(2);

            Assert.AreEqual(ToString(expected), ToString(actual));
        }

        /// <summary>
        /// Tests the SubtractDays method.
        /// </summary>
        [TestMethod]
        public void SubtractDaysTest()
        {
            var expected = new Date(30, Date.Month.DECEMBER, 1899);
            var actual = new Date();

            actual.SubtractDays(2);

            Assert.AreEqual(ToString(expected), ToString(actual));
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
