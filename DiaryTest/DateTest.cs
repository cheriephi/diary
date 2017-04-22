using Diary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;

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
        /// Simple single scenario test of default constructor
        /// </summary>
        [TestMethod]
        public void DefaultConstructorTest()
        {
            var expectedDate = new DateTestSubclass(1, Date.Month.JANUARY, 1900);
            var actualDate = new DateTestSubclass();

            Assert.AreEqual(expectedDate.ToString(), actualDate.ToString());
        }
        
        /// <summary>
        /// Data testing that the constructor properly initializes fields based on inputs
        /// </summary>
        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", @"TestData\DateData.xml", "add", DataAccessMethod.Sequential)]
        public void InputDateConstructorTest()
        {
            var expectedYear = int.Parse(TestContext.DataRow["year"].ToString());
            var expectedMonthNumber = int.Parse(TestContext.DataRow["month"].ToString());
            var expectedMonth = (Date.Month)expectedMonthNumber;
            var expectedDay = int.Parse(TestContext.DataRow["day"].ToString());

            var expectedDateString = DateTestSubclass.ToString(expectedDay, expectedMonthNumber, expectedYear);
            var actualDate = new DateTestSubclass(expectedDay, expectedMonth, expectedYear);

            Assert.AreEqual(expectedDateString, actualDate.ToString());
        }

        /// <summary>
        /// Validates the constructor throws an exception if invalid inputs are passed in
        /// </summary>
        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", @"TestData\InvalidDateData.xml", "add", DataAccessMethod.Sequential)]
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
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", @"TestData\DateData.xml", "add", DataAccessMethod.Sequential)]
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

        #region Julian Number Tests
        /// <summary>
        /// Data testing to a julian number.
        /// </summary>
        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", @"TestData\DateData.xml", "add", DataAccessMethod.Sequential)]
        public void ToJulianNumberTest()
        {
            var year = int.Parse(TestContext.DataRow["year"].ToString());
            var month = int.Parse(TestContext.DataRow["month"].ToString());
            var day = int.Parse(TestContext.DataRow["day"].ToString());

            var expectedResult = int.Parse(TestContext.DataRow["julian"].ToString());
            var actualResult = Date.ToJulianNumber(day, month, year);

            Assert.AreEqual(expectedResult, actualResult, "Input Year:<{0}>. Month:<{1}>. Day:<{2}>.", year, month, day);
        }

        /// <summary>
        /// Data testing from a julian number.
        /// </summary>
        /// <seealso href="https://msdn.microsoft.com/en-us/library/dd260048.aspx"/>
        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", @"TestData\DateData.xml", "add", DataAccessMethod.Sequential)]
        public void FromJulianNumberTest()
        {
            var julian = int.Parse(TestContext.DataRow["julian"].ToString());
            var expectedYear = int.Parse(TestContext.DataRow["year"].ToString());
            var expectedMonth = int.Parse(TestContext.DataRow["month"].ToString());
            var expectedDay = int.Parse(TestContext.DataRow["day"].ToString());

            int actualYear = 0;
            int actualMonth = 0;
            int actualDay = 0;
            Date.FromJulianNumber(julian, ref actualDay, ref actualMonth, ref actualYear);

            var actualDateString = DateTestSubclass.ToString(actualDay, actualMonth, actualYear);
            //Is the output value a date? Build a date string; ensuring first century dates are zero padded
            DateTime actualDate;
            Assert.IsTrue(DateTime.TryParseExact(actualDateString, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out actualDate));

            var expectedDateString = DateTestSubclass.ToString(expectedDay, expectedMonth, expectedYear);

            Assert.AreEqual(expectedDateString, actualDateString, "Input Julian Number:<{0}>.", julian);
        }
        #endregion

        #region Comparison Tests
        /// <summary>
        /// Data testing of CompareTo function
        /// </summary>
        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", @"TestData\CompareDateData.xml", "add", DataAccessMethod.Sequential)]
        public void CompareToTest()
        {
            var year = int.Parse(TestContext.DataRow["year"].ToString());
            var month = (Date.Month)int.Parse(TestContext.DataRow["month"].ToString());
            var day = int.Parse(TestContext.DataRow["day"].ToString());

            var compareYear = int.Parse(TestContext.DataRow["compareYear"].ToString());
            var compareMonth = (Date.Month)int.Parse(TestContext.DataRow["compareMonth"].ToString());
            var compareDay = int.Parse(TestContext.DataRow["compareDay"].ToString());

            var expectedResult = int.Parse(TestContext.DataRow["expectedResult"].ToString());

            var date = new DateTestSubclass(day, month, year);
            var compareDate = new DateTestSubclass(compareDay, compareMonth, compareYear);

            var actualResult = date.CompareTo(compareDate);

            Assert.AreEqual(expectedResult, actualResult, "Input Date:<{0}>. Input Compare Date:<{1}>.", date, compareDate);
        }

        /// <summary>
        /// Data testing of IsBetween function
        /// </summary>
        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", @"TestData\BetweenDateData.xml", "add", DataAccessMethod.Sequential)]
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
            bool expectedIsBetween = (expectedIsBetweenString == "1");

            var startDate = new DateTestSubclass(startDay, startMonth, startYear);
            var endDate = new DateTestSubclass(endDay, endMonth, endYear);
            var date = new DateTestSubclass(day, month, year);

            var actualIsBetween = date.IsBetween(startDate, endDate);

            Assert.AreEqual(expectedIsBetween, actualIsBetween, "Input Date:<{0}>. Input Start Date:<{1}>. Input End Date:<{2}>.", date, startDate, endDate);
        }
        #endregion

        #region Math Tests
        /// <summary>
        /// Tests the AddDays method
        /// </summary>
        [TestMethod]
        public void AddDaysTest()
        {
            var expectedDate = new DateTestSubclass(3, Date.Month.JANUARY, 1900);
            var actualDate = new DateTestSubclass();

            actualDate.AddDays(2);

            Assert.AreEqual(expectedDate.ToString(), actualDate.ToString());
        }

        /// <summary>
        /// Tests the SubtractDays method
        /// </summary>
        [TestMethod]
        public void SubtractDaysTest()
        {
            var expectedDate = new DateTestSubclass(30, Date.Month.DECEMBER, 1899);
            var actualDate = new DateTestSubclass();

            actualDate.SubtractDays(2);

            Assert.AreEqual(expectedDate.ToString(), actualDate.ToString());
        }
        #endregion

        #region Date Lookup Tests
        /// <summary>
        /// Data testing that the leap year is calculated properly
        /// </summary>
        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", @"TestData\LeapYearData.xml", "add", DataAccessMethod.Sequential)]
        public void IsLeapYearTest()
        {
            var year = int.Parse(TestContext.DataRow["year"].ToString());
            var isLeapYearString = TestContext.DataRow["isLeapYear"].ToString();
            bool expectedIsLeapYear = (isLeapYearString == "1");

            var actualIsLeapYear = Date.IsLeapYear(year);

            Assert.AreEqual(expectedIsLeapYear, actualIsLeapYear, "Input Year:<{0}>.", year);
        }

        /// <summary>
        /// Data testing that the last day of the month is calculated properly
        /// </summary>
        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", @"TestData\LastDayOfMonthData.xml", "add", DataAccessMethod.Sequential)]
        public void GetLastDayOfMonthTest()
        {
            var year = int.Parse(TestContext.DataRow["year"].ToString());
            var monthNumber = int.Parse(TestContext.DataRow["month"].ToString());
            var month = (Date.Month)monthNumber;
            var expectedLastDayOfMonth = int.Parse(TestContext.DataRow["day"].ToString());

            var actualLastDayOfMonth = Date.GetLastDayOfMonth(month, year);

            Assert.AreEqual(expectedLastDayOfMonth, actualLastDayOfMonth, "Input Year:<{0}>. Input Month:<{1}>.", year, monthNumber);
        }
        #endregion
    }
}
