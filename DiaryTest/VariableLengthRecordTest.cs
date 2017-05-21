using Diary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
namespace DiaryTest
{
    /// <summary>
    /// Tests the variable length record class.
    /// </summary>
    [TestClass]
    public class VariableLengthRecordTest
    {
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

        [TestMethod]
        public void TestReadWrite()
        {
            VariableLengthRecord outputRecord = new VariableLengthRecord();
            outputRecord.AppendValue(100);     // unsigned int value 
            outputRecord.AppendValue(200.0F);   // float value 
            outputRecord.AppendValue(300.0);    // double value 
            outputRecord.AppendValue('a');     // unsigned char value 
            outputRecord.AppendValue("Testing");    //string value 
            outputRecord.AppendValue(true);     // boolean value

            try
            {
                RandomAccessFile outputStream = new
                RandomAccessFile("C:/Persistence/TestFile.txt");

                outputRecord.Serialize(outputStream);
                outputStream.close();

                VariableLengthRecord inputRecord = new VariableLengthRecord();

                RandomAccessFile inputStream = new
                RandomAccessFile("C:/Persistence/TestFile.txt");

                Assert.IsTrue(inputRecord.Deserialize(inputStream));
                Assert.AreEqual(inputRecord.GetCount(), 6);

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

                inputStream.close();

            }
            catch (Exception) { }
        }
    }
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member