using Diary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Configuration;

namespace DiaryTest
{
    /// <summary>
    /// Tests the variable length record class.
    /// </summary>
    /// <see cref="ObjectIdTest"/>
    /// <remarks>Not all tests currently get persisted, but a fresh fixture is set for each one to simplify the test code and its maintenance.</remarks>
    [TestClass]
    public unsafe class VariableLengthRecordTest
    {
        private TransientPersistenceFreshFixture fixture;

        #region Test Initialize and Cleanup Methods
        /// <summary>
        /// Resets the environment.
        /// </summary>
        /// <see cref="TransientPersistenceFreshFixture"/>
        [TestInitialize]
        public void Init()
        {
            fixture = new TransientPersistenceFreshFixture();
            fixture.Init();
        }

        /// <summary>
        /// Reverts the environment back to its original state.
        /// </summary>
        /// <see cref="TransientPersistenceFreshFixture"/>
        [TestCleanup]
        public void Cleanup()
        {
            fixture.Cleanup();
        }
        #endregion

        #region Data Type Tests
        /// <summary>
        /// Tests boolean values work as expected.
        /// </summary>
        [TestMethod]
        public void TestBooleanElement()
        {
            var record = new VariableLengthRecord();

            Assert.IsTrue(record.GetCount() == 0);
            record.AppendValue(true);
            Assert.IsTrue(record.GetCount() == 1);
            bool value = false;
            Assert.IsTrue(record.GetValue(0, ref value));
            Assert.IsTrue(value == true);

            record.AppendValue(false);
            Assert.IsTrue(record.GetCount() == 2);
            Assert.IsTrue(record.GetValue(1, ref value));
            Assert.IsTrue(value == false);
        }

        /// <summary>
        /// Tests char values work as expected.
        /// </summary>
        [TestMethod]
        public void TestCharElement()
        {
            var record = new VariableLengthRecord();

            Assert.IsTrue(record.GetCount() == 0);
            record.AppendValue('a');
            Assert.IsTrue(record.GetCount() == 1);
            char value = ' ';
            Assert.IsTrue(record.GetValue(0, ref value));
            Assert.IsTrue(value == 'a');

            record.AppendValue('b');
            Assert.IsTrue(record.GetCount() == 2);
            Assert.IsTrue(record.GetValue(1, ref value));
            Assert.IsTrue(value == 'b');
        }

        /// <summary>
        /// Tests int values work as expected.
        /// </summary>
        [TestMethod]
        public void TestIntElement()
        {
            var record = new VariableLengthRecord();

            Assert.IsTrue(record.GetCount() == 0);
            record.AppendValue(20);
            Assert.IsTrue(record.GetCount() == 1);
            int value = 0;
            Assert.IsTrue(record.GetValue(0, ref value));
            Assert.IsTrue(value == 20);

            record.AppendValue(12394);
            Assert.IsTrue(record.GetCount() == 2);
            Assert.IsTrue(record.GetValue(1, ref value));
            Assert.IsTrue(value == 12394);
        }

        /// <summary>
        /// Tests float values work as expected.
        /// </summary>
        [TestMethod]
        public void TestFloatElement()
        {
            var record = new VariableLengthRecord();

            Assert.IsTrue(record.GetCount() == 0);
            record.AppendValue(200.00F);
            Assert.IsTrue(record.GetCount() == 1);
            float value = 0.0F;
            Assert.IsTrue(record.GetValue(0, ref value));
            Assert.AreEqual(value, 200.0F, 0.001F);

            record.AppendValue(1.234F);
            Assert.IsTrue(record.GetCount() == 2);
            Assert.IsTrue(record.GetValue(1, ref value));
            Assert.AreEqual(value, 1.234F, 0.001F);
        }

        /// <summary>
        /// Tests double values work as expected.
        /// </summary>
        [TestMethod]
        public void TestDoubleElement()
        {
            var record = new VariableLengthRecord();

            Assert.IsTrue(record.GetCount() == 0);
            record.AppendValue(2000.0);
            Assert.IsTrue(record.GetCount() == 1);
            double value = 0.0;
            Assert.IsTrue(record.GetValue(0, ref value));
            Assert.AreEqual(value, 2000.0, 0.001);

            record.AppendValue(134343.234);
            Assert.IsTrue(record.GetCount() == 2);
            Assert.IsTrue(record.GetValue(1, ref value));
            Assert.AreEqual(value, 134343.234, 0.001);
        }

        /// <summary>
        /// Tests String values work as expected.
        /// </summary>
        [TestMethod]
        public void TestStringElement()
        {
            var record = new VariableLengthRecord();

            Assert.IsTrue(record.GetCount() == 0);
            record.AppendValue("Test A");
            Assert.IsTrue(record.GetCount() == 1);
            String value = "";
            Assert.IsTrue(record.GetValue(0, ref value));
            Assert.IsTrue(value.CompareTo("Test A") == 0);

            record.AppendValue("Test B");
            Assert.IsTrue(record.GetCount() == 2);
            Assert.IsTrue(record.GetValue(1, ref value));
            Assert.IsTrue(value.CompareTo("Test B") == 0);
        }

        /// <summary>
        /// Tests ObjectId values work as expected.
        /// </summary>
        [TestMethod]
        public void TestObjectIdElement()
        {
            VariableLengthRecord record = new VariableLengthRecord();

            var a = new ObjectId();
            var b = new ObjectId();
            var c = new ObjectId();

            Assert.IsTrue(record.GetCount() == 0);
            record.AppendValue(a);

            Assert.IsTrue(record.GetCount() == 1);
            record.AppendValue(b);
            Assert.IsTrue(record.GetCount() == 2);
            record.AppendValue(c);
            Assert.IsTrue(record.GetCount() == 3);

            var value = new ObjectId();

            Assert.IsTrue(record.GetValue(0, ref value));
            Assert.IsTrue(value.CompareTo(a) == 0);

            Assert.IsTrue(record.GetValue(1, ref value));
            Assert.IsTrue(value.CompareTo(b) == 0);

            Assert.IsTrue(record.GetValue(2, ref value));
            Assert.IsTrue(value.CompareTo(c) == 0);

        }

        /// <summary>
        /// Tests Date values work as expected.
        /// </summary>
        [TestMethod]
        public void TestDateElement()
        {
            var record = new VariableLengthRecord();

            var value1 = new Date(3, Date.Month.JUNE, 2017);
            Assert.AreEqual(0, record.GetCount());
            record.AppendValue(value1);
            Assert.AreEqual(1, record.GetCount());
            var value = new Date();
            Assert.IsTrue(record.GetValue(0, ref value));
            Assert.AreEqual(Helper.ToString(value1), Helper.ToString(value));

            var value2 = new Date(15, Date.Month.APRIL, 1970);
            record.AppendValue(value2);
            Assert.AreEqual(2, record.GetCount());
            Assert.IsTrue(record.GetValue(1, ref value));
            Assert.AreEqual(Helper.ToString(value2), Helper.ToString(value));
        }

        /// <summary>
        /// Tests DateTime values work as expected.
        /// </summary>
        [TestMethod]
        public void TestDateTimeElement()
        {
            var record = new VariableLengthRecord();

            var value1 = new Diary.DateTime(new Date(30, Date.Month.JUNE, 2018), 6, 15);
            Assert.AreEqual(0, record.GetCount());
            record.AppendValue(value1);
            Assert.AreEqual(1, record.GetCount());
            var value = new Diary.DateTime();
            Assert.IsTrue(record.GetValue(0, ref value));
            Assert.AreEqual(Helper.ToString(value1), Helper.ToString(value));

            var value2 = new Diary.DateTime(new Date(15, Date.Month.JANUARY, 1980), 21, 30);
            record.AppendValue(value2);
            Assert.AreEqual(2, record.GetCount());
            Assert.IsTrue(record.GetValue(1, ref value));
            Assert.AreEqual(Helper.ToString(value2), Helper.ToString(value));
        }
        #endregion

        /// <summary>
        /// Tests object serialization \ deserialization.
        /// </summary>
        [TestMethod]
        public void TestReadWrite()
        {
            var outputRecord = new VariableLengthRecord();
            outputRecord.AppendValue(100);     // unsigned int value 
            outputRecord.AppendValue(200.0F);   // float value 
            outputRecord.AppendValue(300.0);    // double value 
            outputRecord.AppendValue('a');     // unsigned char value 
            outputRecord.AppendValue("Testing");    //string value 
            outputRecord.AppendValue(true);     // boolean value

            var dateValue = new Date(31, Date.Month.DECEMBER, 2100);
            outputRecord.AppendValue(dateValue);

            var dateTimeValue = new Diary.DateTime(new Date(6, Date.Month.SEPTEMBER, 1950), 12, 15);
            outputRecord.AppendValue(dateTimeValue);

            String persistenceFilePath = String.Concat(ConfigurationManager.AppSettings["PersistenceFolderPath"], "/TestFile.txt");
            var folder = Path.GetDirectoryName(persistenceFilePath);
            // Create the persistence directory if it doesn't exist.
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            var outputStream = new RandomAccessFile(persistenceFilePath);

            outputRecord.Serialize(outputStream);
            outputStream.close();

            var inputRecord = new VariableLengthRecord();

            using (var inputStream = new RandomAccessFile(persistenceFilePath))
            {
                Assert.IsTrue(inputRecord.Deserialize(inputStream));
                Assert.AreEqual(inputRecord.GetCount(), 8, "Count");

                int value0 = 0;
                Assert.IsTrue(inputRecord.GetValue(0, ref value0));
                Assert.AreEqual(value0, 100);

                float value1 = 0.0F;
                Assert.IsTrue(inputRecord.GetValue(1, ref value1));
                Assert.AreEqual(value1, 200.0F, 0.001F);

                double value2 = 0.0;
                Assert.IsTrue(inputRecord.GetValue(2, ref value2));
                Assert.AreEqual(value2, 300.0, 0.001);

                char value3 = ' ';
                Assert.IsTrue(inputRecord.GetValue(3, ref value3));
                Assert.AreEqual(value3, 'a');

                String value4 = "";
                Assert.IsTrue(inputRecord.GetValue(4, ref value4));
                Assert.IsTrue(value4.CompareTo("Testing") == 0);

                bool value5 = false;
                Assert.IsTrue(inputRecord.GetValue(5, ref value5));
                Assert.AreEqual(value5, true);

                var value6 = new Date();
                Assert.IsTrue(inputRecord.GetValue(6, ref value6), "Date");
                Assert.AreEqual(Helper.ToString(dateValue), Helper.ToString(value6));

                var value7 = new Diary.DateTime();
                Assert.IsTrue(inputRecord.GetValue(7, ref value7), "DateTime");
                Assert.AreEqual(Helper.ToString(dateTimeValue), Helper.ToString(value7));

                inputStream.close(); 
            }
        }
    }
}
