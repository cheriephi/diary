using Diary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DiaryTest
{
    /// <summary>
    /// Tests the KeyFile class.
    /// </summary>
    [TestClass]
    public class KeyFileTest
    {
        /// <summary>
        /// Tests the creation of key files.
        /// </summary>
        [TestMethod]
        public void TestKeyFileCreation()
        {
            var keyFile = new KeyFile("C:/Persistence", "Test");

            var objectIdA = new ObjectId();
            var objectIdB = new ObjectId();
            var objectIdC = new ObjectId();

            keyFile.Update(objectIdA, 100, 1000);
            keyFile.Update(objectIdB, 200, 2000);
            keyFile.Update(objectIdC, 300, 3000);

            int offset = 0;
            int dataSizeBytes = 0;

            Assert.IsTrue(keyFile.Get(objectIdA, ref offset, ref dataSizeBytes));
            Assert.IsTrue(offset == 100);
            Assert.IsTrue(dataSizeBytes == 1000);

            Assert.IsTrue(keyFile.Get(objectIdB, ref offset, ref dataSizeBytes));
            Assert.IsTrue(offset == 200);
            Assert.IsTrue(dataSizeBytes == 2000);

            Assert.IsTrue(keyFile.Get(objectIdC, ref offset, ref dataSizeBytes));
            Assert.IsTrue(offset == 300);
            Assert.IsTrue(dataSizeBytes == 3000);

            keyFile.Save();
        }
    }
}
