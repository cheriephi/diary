using Diary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
namespace DiaryTest
{
    [TestClass]
    public class VariableLengthRecordFileTest
    {
        public static void TestInitialize() // Always start with an empty file. 
        {
            string pathname = "C:/Persistence/VariableRecordFile.dat";
            if (File.Exists(pathname))
            {
                File.Delete(pathname);
            }
        }

        [TestMethod]
        public void VariableLengthRecordTest1()
        {
            TestInitialize();
            var recordFile = new VariableLengthRecordFile("C:/Persistence", "VariableRecordFile");

            Assert.IsTrue(recordFile.GetBytesUsed() == 0);
            int totalBytes = recordFile.GetBytesTotal();
            Assert.IsTrue(totalBytes == 4);   // Four bytes in file header contain # of used bytes

            var record1 = new VariableLengthRecord();
            record1.AppendValue(1000);
            record1.AppendValue(2000);
            record1.AppendValue(3000);

            int offset = 0;
            int dataSizeBytes = 0;

            Assert.IsTrue(recordFile.Append(record1, ref offset, ref dataSizeBytes));
            Assert.AreEqual(offset, 4);
            Assert.AreEqual(dataSizeBytes, 43);

            Assert.AreEqual(recordFile.GetBytesUsed(), 43);
            int total = recordFile.GetBytesTotal();
            Assert.AreEqual(total, 47);
            recordFile.Close();
        }

        [TestMethod]
        public void VariableLengthRecordTest2()
        {
            var recordFile = new VariableLengthRecordFile("C:/Persistence", "VariableRecordFile");
            Assert.IsTrue(recordFile.GetBytesUsed() == 43);
            Assert.IsTrue(recordFile.GetBytesTotal() == 47);

            var record = new VariableLengthRecord();
            record.AppendValue(10000);
            record.AppendValue(20000);
            record.AppendValue(30000);
            record.AppendValue(40000);

            int offset = 0;
            int dataSizeBytes = 0;

            Assert.IsTrue(recordFile.Append(record, ref offset, ref
            dataSizeBytes));
            Assert.AreEqual(offset, 47);
            Assert.AreEqual(dataSizeBytes, 60);

            int bytesUsed2 = recordFile.GetBytesUsed();
            Assert.AreEqual(bytesUsed2, 103);
            int bytesTotal2 = recordFile.GetBytesTotal();
            Assert.AreEqual(recordFile.GetBytesTotal(), 107);

            recordFile.Close();
        }

        [TestMethod]
        public void VariableLengthRecordTest3()
        {
            var recordFile = new VariableLengthRecordFile("C:/Persistence", "VariableRecordFile");
            var record = new VariableLengthRecord();

            int offset = 4;
            int bytesLength = 40;
            Assert.IsTrue(recordFile.Read(offset, bytesLength, record));
            Assert.AreEqual(record.GetCount(), 3);

            int value0 = 0;
            Assert.IsTrue(record.GetValue(0, ref value0));
            Assert.AreEqual(value0, 1000);

            int value1 = 0;
            Assert.IsTrue(record.GetValue(1, ref value1));
            Assert.AreEqual(value1, 2000);

            int value2 = 0;
            Assert.IsTrue(record.GetValue(2, ref value2));
            Assert.AreEqual(value2, 3000);

            recordFile.Close();
        }

        [TestMethod]
        public void VariableLengthRecordTest4()
        {
            var recordFile = new VariableLengthRecordFile("C:/Persistence", "VariableRecordFile");

            int offset = 47;
            int bytesLength = 60;
            VariableLengthRecord record = new VariableLengthRecord();
            Assert.IsTrue(recordFile.Read(offset, bytesLength, record));
            Assert.AreEqual(record.GetCount(), 4);

            int value0 = 0;
            Assert.IsTrue(record.GetValue(0, ref value0));
            Assert.AreEqual(value0, 10000);

            int value1 = 0;
            Assert.IsTrue(record.GetValue(1, ref value1));
            Assert.AreEqual(value1, 20000);

            int value2 = 0;
            Assert.IsTrue(record.GetValue(2, ref value2));
            Assert.AreEqual(value2, 30000);

            int value3 = 0;
            Assert.IsTrue(record.GetValue(3, ref value3));
            Assert.AreEqual(value3, 40000);

            recordFile.Close();
        }
    }
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
