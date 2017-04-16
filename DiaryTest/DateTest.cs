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
    /// <seealso cref="http://prasadhonrao.com/back-to-basics-data-driven-unit-testing/"/>
    [TestClass]
    public class DateTest
    {
        public TestContext TestContext { get; set; }

        #region Helper Methods
        /// <summary>
        /// Builds a formatted date string from calendar parts. Helps in creating a CustomAssertion ExpectedObject so that the compound values
        /// of a Date comparison can be done as a unit. This is helpful in Test Debugging and defect localization.
        /// While the Date object exposes getters on each of the discrete calendar parts, they only make sense as a unit.
        /// </summary>
        /// <remarks>This test helper code reduces test code duplication.</remarks>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <returns>yyyy-MM-dd</returns>
        private static String GetDateStringFromParts(int year, int month, int day)
        {
            String dateString = String.Format("{0}-{1}-{2}", year.ToString("0000"), month.ToString("00"), day.ToString("00")); // yyyy-mm-dd
            return dateString;
        }

        /// <summary>
        /// Tests Date equality. Helper method to reduce test code duplication.
        /// </summary>
        /// <param name="date"></param>
        /// <param name="expectedYear"></param>
        /// <param name="expectedMonth"></param>
        /// <param name="expectedDay"></param>
        private void DateIsEqualTest(Date date, int expectedYear, int expectedMonth, int expectedDay)
        {
            int actualYear = date.GetYear();
            int actualMonth = (int)date.GetMonth();
            int actualDay = date.GetDay();

            var actualDateString = GetDateStringFromParts(actualYear, actualMonth, actualDay);
            var expectedDateString = GetDateStringFromParts(expectedYear, expectedMonth, expectedDay);

            Assert.AreEqual(expectedDateString, actualDateString);
        }
        #endregion

        #region Constructor Tests
        /// <summary>
        /// Simple single scenario test of default constructor
        /// </summary>
        [TestMethod]
        public void DefaultConstructorTest()
        {
            Date date = new Date();
            DateIsEqualTest(date, 1900, 1, 1);
        }

        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", @"TestData\DateData.xml", "add", DataAccessMethod.Sequential)]
        public void InputDateConstructorTest()
        {
            var expectedYear = int.Parse(TestContext.DataRow["year"].ToString());
            var expectedMonthNumber = int.Parse(TestContext.DataRow["month"].ToString());
            var expectedMonth = (Date.Month)expectedMonthNumber;
            var expectedDay = int.Parse(TestContext.DataRow["day"].ToString());

            Date date = new Date(expectedDay, expectedMonth, expectedYear);
            DateIsEqualTest(date, expectedYear, expectedMonthNumber, expectedDay);
        }

        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", @"TestData\InvalidDateData.xml", "add", DataAccessMethod.Sequential)]
        [ExpectedException(typeof(System.ArgumentOutOfRangeException))]
        public void InvalidInputDateConstructorTest()
        {
            var expectedYear = int.Parse(TestContext.DataRow["year"].ToString());
            var expectedMonth = (Date.Month)int.Parse(TestContext.DataRow["month"].ToString());
            var expectedDay = int.Parse(TestContext.DataRow["day"].ToString());

            Date date = new Date(expectedDay, expectedMonth, expectedYear);
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
        /// <seealso cref="https://msdn.microsoft.com/en-us/library/dd260048(v=vs.110).aspx"/>
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

            var actualDateString = GetDateStringFromParts(actualYear, actualMonth, actualDay);
            //Is the output value a date? Build a date string; ensuring first century dates are zero padded
            DateTime actualDate;
            Assert.IsTrue(DateTime.TryParseExact(actualDateString, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out actualDate));

            var expectedDateString = GetDateStringFromParts(expectedYear, expectedMonth, expectedDay);

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
            var dateString = GetDateStringFromParts(year, (int)month, day);

            var compareYear = int.Parse(TestContext.DataRow["compareYear"].ToString());
            var compareMonth = (Date.Month)int.Parse(TestContext.DataRow["compareMonth"].ToString());
            var compareDay = int.Parse(TestContext.DataRow["compareDay"].ToString());
            var compareDateString = GetDateStringFromParts(compareYear, (int)compareMonth, compareDay);

            var expectedResult = int.Parse(TestContext.DataRow["expectedResult"].ToString());

            Date date = new Date(day, month, year);
            Date compareDate = new Date(compareDay, compareMonth, compareYear);

            var actualResult = date.CompareTo(compareDate);

            Assert.AreEqual(expectedResult, actualResult, "Input Date:<{0}>. Input Compare Date:<{1}>.", dateString, compareDateString);
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
            var startDateString = GetDateStringFromParts(startYear, (int)startMonth, startDay);

            var endYear = int.Parse(TestContext.DataRow["endYear"].ToString());
            var endMonth = (Date.Month)int.Parse(TestContext.DataRow["endMonth"].ToString());
            var endDay = int.Parse(TestContext.DataRow["endDay"].ToString());
            var endDateString = GetDateStringFromParts(endYear, (int)endMonth, endDay);

            var year = int.Parse(TestContext.DataRow["year"].ToString());
            var month = (Date.Month)int.Parse(TestContext.DataRow["month"].ToString());
            var day = int.Parse(TestContext.DataRow["day"].ToString());
            var dateString = GetDateStringFromParts(year, (int)month, day);

            var expectedIsBetweenString = TestContext.DataRow["isBetween"].ToString();
            bool expectedIsBetween = (expectedIsBetweenString == "1");

            Date startDate = new Date(startDay, startMonth, startYear);
            Date endDate = new Date(endDay, endMonth, endYear);
            Date date = new Date(day, month, year);

            var actualIsBetween = date.IsBetween(startDate, endDate);

            Assert.AreEqual(expectedIsBetween, actualIsBetween, "Input Date:<{0}>. Input Start Date:<{1}>. Input End Date:<{2}>.", dateString, startDateString, endDateString);
        }
        #endregion

        #region Math Tests
        /// <summary>
        /// Tests the AddDays method
        /// </summary>
        [TestMethod]
        public void AddDaysTest()
        {
            Date date = new Date();
            date.AddDays(2);
            DateIsEqualTest(date, 1900, 1, 3);
        }

        /// <summary>
        /// Tests the SubtractDays method
        /// </summary>
        [TestMethod]
        public void SubtractDaysTest()
        {
            Date date = new Date();
            date.SubtractDays(2);
            DateIsEqualTest(date, 1899, 12, 30);
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
