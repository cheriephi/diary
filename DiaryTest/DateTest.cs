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
            var builder = new DateBuilder();
            var actual = new Date();

            Helper.AssertAreEqual(builder, actual, "");
        }

        /// <summary>
        /// Data testing that the constructor properly initializes fields based on inputs.
        /// </summary>
        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", @"TestData\Date\DateData.xml", "add", DataAccessMethod.Sequential)]
        public void InputDateConstructorTest()
        {
            var builder = new DateBuilder();
            builder.SetDay(int.Parse(TestContext.DataRow["day"].ToString()));
            builder.SetMonth(int.Parse(TestContext.DataRow["month"].ToString()));
            builder.SetYear(int.Parse(TestContext.DataRow["year"].ToString()));

            Helper.AssertAreEqual(builder, builder.Build(), "");
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
            var dateBuilder = new DateBuilder();
            dateBuilder.SetDay(int.Parse(TestContext.DataRow["day"].ToString()));
            dateBuilder.SetMonth(int.Parse(TestContext.DataRow["month"].ToString()));
            dateBuilder.SetYear(int.Parse(TestContext.DataRow["year"].ToString()));

            var compareDateBuilder = new DateBuilder();
            compareDateBuilder.SetDay(int.Parse(TestContext.DataRow["compareDay"].ToString()));
            compareDateBuilder.SetMonth(int.Parse(TestContext.DataRow["compareMonth"].ToString()));
            compareDateBuilder.SetYear(int.Parse(TestContext.DataRow["compareYear"].ToString()));

            var expected = int.Parse(TestContext.DataRow["expectedResult"].ToString());

            var date = dateBuilder.Build();
            var compareDate = compareDateBuilder.Build();

            var actual = date.CompareTo(compareDate);

            Assert.AreEqual(expected, actual, "Input Date:<{0}>. Input Compare Date:<{1}>.", dateBuilder.ToString(), compareDateBuilder.ToString());
        }

        /// <summary>
        /// Data testing of IsBetween function.
        /// </summary>
        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", @"TestData\Date\BetweenDateData.xml", "add", DataAccessMethod.Sequential)]
        public void IsBetweenTest()
        {
            var startBuilder = new DateBuilder();
            startBuilder.SetDay(int.Parse(TestContext.DataRow["startDay"].ToString()));
            startBuilder.SetMonth(int.Parse(TestContext.DataRow["startMonth"].ToString()));
            startBuilder.SetYear(int.Parse(TestContext.DataRow["startYear"].ToString()));

            var endBuilder = new DateBuilder();
            endBuilder.SetDay(int.Parse(TestContext.DataRow["endDay"].ToString()));
            endBuilder.SetMonth(int.Parse(TestContext.DataRow["endMonth"].ToString()));
            endBuilder.SetYear(int.Parse(TestContext.DataRow["endYear"].ToString()));

            var builder = new DateBuilder();
            builder.SetDay(int.Parse(TestContext.DataRow["day"].ToString()));
            builder.SetMonth(int.Parse(TestContext.DataRow["month"].ToString()));
            builder.SetYear(int.Parse(TestContext.DataRow["year"].ToString()));

            var expectedIsBetweenString = TestContext.DataRow["isBetween"].ToString();
            bool expected = (expectedIsBetweenString == "1");

            var startDate = startBuilder.Build();
            var endDate = endBuilder.Build();
            var date = builder.Build();

            var actual = date.IsBetween(startDate, endDate);

            Assert.AreEqual(expected, actual, "Input Date:<{0}>. Input Start Date:<{1}>. Input End Date:<{2}>.", builder.ToString(), startBuilder.ToString(), endBuilder.ToString());
        }
        #endregion

        #region Math Tests
        /// <summary>
        /// Tests the AddDays method.
        /// </summary>
        [TestMethod]
        public void AddDaysTest()
        {
            var builder = new DateBuilder();
            builder.SetDay(3);
            builder.SetMonth(1);
            builder.SetYear(1900);

            var actual = new Date();
            actual.AddDays(2);

            Helper.AssertAreEqual(builder, actual, "");
        }

        /// <summary>
        /// Tests the SubtractDays method.
        /// </summary>
        [TestMethod]
        public void SubtractDaysTest()
        {
            var builder = new DateBuilder();
            builder.SetDay(30);
            builder.SetMonth(12);
            builder.SetYear(1899);

            var actual = new Date();
            actual.SubtractDays(2);

            Helper.AssertAreEqual(builder, actual, "");
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
