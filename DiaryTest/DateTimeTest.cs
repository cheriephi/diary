using Diary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DiaryTest
{
    /// <summary>
    /// Summary description for DateTimeTest
    /// </summary>
    /// <seealso cref="DateTest">For more detailed design explanations</seealso>
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
            return ToString(dateTime.GetDate(), dateTime.GetHours(), dateTime.GetMinutes());
        }

        public static string ToString(Date date, int hours, int minutes)
        {
            return String.Format("{0} {1}:{2}", DateTest.ToString(date), hours.ToString("00"), minutes.ToString("00"));
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

        /// <summary>
        /// Single scenario test that the constructor properly initializes fields based on inputs
        /// </summary>
        [TestMethod]
        public void InputDateConstructorTest()
        {
            var expectedDate = new Date(30, Date.Month.APRIL, 2017);
            var expectedHours = 3;
            var expectedMinutes = 42;

            var expected = ToString(expectedDate, expectedHours, expectedMinutes);
            var actual = ToString(new Diary.DateTime(expectedDate, expectedHours, expectedMinutes));

            Assert.AreEqual(expected, actual);
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
            var actual = ToString(new Diary.DateTime(date, expectedHours, expectedMinutes));

            Assert.AreEqual(expected, actual, "Input Date:<{0}>. Hours:<{1}>. Minutes:<{2}>.", DateTest.ToString(date), expectedHours, expectedMinutes);
        }

        /// <summary>
        /// Single scenario constructor(DateTime) test
        /// </summary>
        [TestMethod]
        public void InputDateTimeConstructorTest()
        {
            var expected = new Diary.DateTime(new Date(1, Date.Month.DECEMBER, 2000), 8, 15);
            var actual = new Diary.DateTime(expected);

            Assert.AreEqual(ToString(expected), ToString(actual));
        }
        #endregion

        #region Aliasing Tests
        /// <summary>
        /// Tests that the Date field passed into the constructor cannot be modified outside the System Under Test.
        /// </summary>
        [TestMethod]
        public void InputDateConstructorAliasingTest()
        {
            var date = new Date();
            var dateTime = new Diary.DateTime(date, 0, 0);

            var expected = 1;
            var actual = dateTime.GetDate().GetDay();
            Assert.AreEqual(expected, actual, "Original");

            date.AddDays(1);
            actual = dateTime.GetDate().GetDay();

            Assert.AreEqual(expected, actual, "After");
        }


        /// <summary>
        /// Tests that the Hours field cannot be modified outside of its accessor.
        /// </summary>
        [TestMethod]
        public void GetHoursAliasingTest()
        {
            var dateTime = new Diary.DateTime();

            var expected = 0;
            var actual = dateTime.GetHours();
            Assert.AreEqual(expected, actual, "Original");

            actual = 1;
            actual = dateTime.GetHours();

            Assert.AreEqual(expected, actual, "After");
        }

        /// <summary>
        /// Tests that the Date field cannot be modified outside its accessor.
        /// </summary>
        /// <seealso cref="DateTest.GetYearAliasingTest">For more context on the problem</seealso>
        [TestMethod]
        public void GetDateAliasingTest()
        {
            var dateTime = new Diary.DateTime();

            var expected = "1900-01-01";
            var actualDate = dateTime.GetDate();
            Assert.AreEqual(expected, DateTest.ToString(actualDate), "Original");

            actualDate.AddDays(1);
            actualDate = dateTime.GetDate();

            Assert.AreEqual(expected, DateTest.ToString(actualDate), "After");
        }
        #endregion

        #region Comparison Tests
        /// <summary>
        /// Data testing of CompareTo function
        /// </summary>
        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", @"TestData\DateTime\CompareDateTimeData.xml", "add", DataAccessMethod.Sequential)]
        public void CompareToTest()
        {
            var year = int.Parse(TestContext.DataRow["year"].ToString());
            var month = (Date.Month)int.Parse(TestContext.DataRow["month"].ToString());
            var day = int.Parse(TestContext.DataRow["day"].ToString());
            var hours = int.Parse(TestContext.DataRow["hours"].ToString());
            var minutes = int.Parse(TestContext.DataRow["minutes"].ToString());

            var compareYear = int.Parse(TestContext.DataRow["compareYear"].ToString());
            var compareMonth = (Date.Month)int.Parse(TestContext.DataRow["compareMonth"].ToString());
            var compareDay = int.Parse(TestContext.DataRow["compareDay"].ToString());
            var compareHours = int.Parse(TestContext.DataRow["compareHours"].ToString());
            var compareMinutes = int.Parse(TestContext.DataRow["compareMinutes"].ToString());

            var expected = int.Parse(TestContext.DataRow["expectedResult"].ToString());

            var dateTime = new Diary.DateTime(new Date(day, month, year), hours, minutes);
            var compareDateTime = new Diary.DateTime(new Date(compareDay, compareMonth, compareYear), compareHours, compareMinutes);

            var actual = dateTime.CompareTo(compareDateTime);

            Assert.AreEqual(expected, actual, "Input DateTime:<{0}>. Input Compare DateTime:<{1}>.", ToString(dateTime), ToString(compareDateTime));
        }

        /// <summary>
        /// Validates the CompareTo method properly handles invalid object input types
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void CompareToInvalidObjectTest()
        {
            var dateTime = new Diary.DateTime();
            var compare = 0;

            var actual = dateTime.CompareTo(compare);
        }

        /// <summary>
        /// Data testing of IsBetween function
        /// </summary>
        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", @"TestData\DateTime\BetweenDateTimeData.xml", "add", DataAccessMethod.Sequential)]
        public void IsBetweenTest()
        {
            var startYear = int.Parse(TestContext.DataRow["startYear"].ToString());
            var startMonth = (Date.Month)int.Parse(TestContext.DataRow["startMonth"].ToString());
            var startDay = int.Parse(TestContext.DataRow["startDay"].ToString());
            var startHours = int.Parse(TestContext.DataRow["startHours"].ToString());
            var startMinutes = int.Parse(TestContext.DataRow["startMinutes"].ToString());

            var endYear = int.Parse(TestContext.DataRow["endYear"].ToString());
            var endMonth = (Date.Month)int.Parse(TestContext.DataRow["endMonth"].ToString());
            var endDay = int.Parse(TestContext.DataRow["endDay"].ToString());
            var endHours = int.Parse(TestContext.DataRow["endHours"].ToString());
            var endMinutes = int.Parse(TestContext.DataRow["endMinutes"].ToString());

            var year = int.Parse(TestContext.DataRow["year"].ToString());
            var month = (Date.Month)int.Parse(TestContext.DataRow["month"].ToString());
            var day = int.Parse(TestContext.DataRow["day"].ToString());
            var hours = int.Parse(TestContext.DataRow["hours"].ToString());
            var minutes = int.Parse(TestContext.DataRow["minutes"].ToString());

            var expectedIsBetweenString = TestContext.DataRow["isBetween"].ToString();
            bool expected = (expectedIsBetweenString == "1");

            var startDateTime = new Diary.DateTime(new Date(startDay, startMonth, startYear), startHours, startMinutes);
            var endDateTime = new Diary.DateTime(new Date(endDay, endMonth, endYear), endHours, endMinutes);
            var dateTime = new Diary.DateTime(new Date(day, month, year), hours, minutes);

            var actual = dateTime.IsBetween(startDateTime, endDateTime);

            Assert.AreEqual(expected, actual, "Input DateTime:<{0}>. Input Start DateTime:<{1}>. Input End DateTime:<{2}>.", ToString(dateTime), ToString(startDateTime), ToString(endDateTime));
        }
        #endregion

        #region Math Tests
        /// <summary>
        /// Tests the AddTime method
        /// </summary>
        [TestMethod]
        public void AddTimeTest()
        {
            var expected = new Diary.DateTime(new Date(1, Date.Month.JANUARY, 1900), 1, 1);
            var actual = new Diary.DateTime(new Date(1, Date.Month.JANUARY, 1900), 0, 0);

            actual.AddTime(1, 1);

            Assert.AreEqual(ToString(expected), ToString(actual));
        }
        #endregion
    }
}
