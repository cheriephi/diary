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

        #region Helper Methods
        /// <summary>
        /// Formats the input elements as a string.
        /// </summary>
        /// <returns>yyyy-MM-dd hh:mm</returns>
        /// <seealso cref="DateBuilder.ToString()"/>
        public static string ToString(Diary.DateTime dateTime)
        {
            return ToString(dateTime.GetDate(), dateTime.GetHours(), dateTime.GetMinutes());
        }

        /// <summary>
        /// Returns the class' identifying properties to support meaningful equality checks and debugging.
        /// </summary>
        /// <param name="date"></param>
        /// <param name="hours"></param>
        /// <param name="minutes"></param>
        /// <returns>yyyy-MM-dd hh:mm</returns>
        public static string ToString(Date date, int hours, int minutes)
        {
            return String.Format("{0} {1}:{2}", DateTest.ToString(date), hours.ToString("00"), minutes.ToString("00"));
        }
        #endregion

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
            builder.SetDay(30);
            builder.SetMonth(4);
            builder.SetYear(2017);
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
            var actual = ToString(new Diary.DateTime(date, expectedHours, expectedMinutes));

            Assert.AreEqual(expected, actual, "Input Date:<{0}>. Hours:<{1}>. Minutes:<{2}>.", DateTest.ToString(date), expectedHours, expectedMinutes);
        }

        /// <summary>
        /// Single scenario constructor(DateTime) test. 
        /// Also checks that the Date field passed into the constructor cannot be modified outside the System Under Test.
        /// </summary>
        /// <see href="https://www.martinfowler.com/bliki/AliasingBug.html"/>
        [TestMethod]
        public void InputDateTimeConstructorTest()
        {
            var date = new Date(1, Date.Month.DECEMBER, 2000);
            var expected = new Diary.DateTime(date, 8, 15);
            var actual = new Diary.DateTime(expected);

            Assert.AreEqual(ToString(expected), ToString(actual), "Original");

            // Now modify the reference value outside the System Under Test accessors.
            date.AddDays(1);

            // The data should not have changed.
            Assert.AreEqual(ToString(expected), ToString(actual), "After");
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
            builder.SetDay(int.Parse(TestContext.DataRow["day"].ToString()));
            builder.SetMonth(int.Parse(TestContext.DataRow["month"].ToString()));
            builder.SetYear(int.Parse(TestContext.DataRow["year"].ToString()));
            builder.SetHours(int.Parse(TestContext.DataRow["hours"].ToString()));
            builder.SetMinutes(int.Parse(TestContext.DataRow["minutes"].ToString()));

            var compareBuilder = new DateTimeBuilder();
            compareBuilder.SetDay(int.Parse(TestContext.DataRow["compareDay"].ToString()));
            compareBuilder.SetMonth(int.Parse(TestContext.DataRow["compareMonth"].ToString()));
            compareBuilder.SetYear(int.Parse(TestContext.DataRow["compareYear"].ToString()));
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
            startBuilder.SetDay(int.Parse(TestContext.DataRow["startDay"].ToString()));
            startBuilder.SetMonth(int.Parse(TestContext.DataRow["startMonth"].ToString()));
            startBuilder.SetYear(int.Parse(TestContext.DataRow["startYear"].ToString()));
            startBuilder.SetHours(int.Parse(TestContext.DataRow["startHours"].ToString()));
            startBuilder.SetMinutes(int.Parse(TestContext.DataRow["startMinutes"].ToString()));

            var endBuilder = new DateTimeBuilder();
            endBuilder.SetDay(int.Parse(TestContext.DataRow["endDay"].ToString()));
            endBuilder.SetMonth(int.Parse(TestContext.DataRow["endMonth"].ToString()));
            endBuilder.SetYear(int.Parse(TestContext.DataRow["endYear"].ToString()));
            endBuilder.SetHours(int.Parse(TestContext.DataRow["endHours"].ToString()));
            endBuilder.SetMinutes(int.Parse(TestContext.DataRow["endMinutes"].ToString()));

            var builder = new DateTimeBuilder();
            builder.SetDay(int.Parse(TestContext.DataRow["day"].ToString()));
            builder.SetMonth(int.Parse(TestContext.DataRow["month"].ToString()));
            builder.SetYear(int.Parse(TestContext.DataRow["year"].ToString()));
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
            expected.SetDay(1).SetMonth(1).SetYear(1900);
            expected.SetHours(1).SetMinutes(1);

            var actual = new Diary.DateTime(new Date(1, Date.Month.JANUARY, 1900), 0, 0);

            actual.AddTime(1, 1);

            Helper.AssertAreEqual(expected, actual, "");
        }
        #endregion
    }
}
