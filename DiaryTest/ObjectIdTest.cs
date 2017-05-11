using Diary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Configuration;
using System.IO;

namespace DiaryTest
{
    /// <summary>
    /// Object Id tests.
    /// </summary>
    /// <remarks>
    /// This class is not thread safe. It must exclusively control persistent storage to proceed in a repeatable and deterministic manner.
    /// </remarks>
    [TestClass]
    public unsafe class ObjectIdTest
    {
        private String mPersistenceFilePath;
        private String mPersistenceBackupFilePath;

        #region Test Initialize and Cleanup Methods
        /// <summary>
        /// Resets any persisted settings so the tests can proceed in a repeatable and deterministic manner.
        /// </summary>
        /// <remarks>The persistence file path is copied as a link from the System Under Test's app.config file.</remarks>
        [TestInitialize]
        public void Init()
        {
            mPersistenceFilePath = ConfigurationManager.AppSettings["PersistenceObjectFilePath"];
            mPersistenceBackupFilePath = Path.GetDirectoryName(mPersistenceFilePath) + @"/ObjectId" + System.DateTime.Now.Ticks + ".txt";

            if (File.Exists(mPersistenceFilePath))
            {
                if (File.Exists(mPersistenceBackupFilePath))
                {
                    File.Delete(mPersistenceBackupFilePath);
                }
                File.Move(mPersistenceFilePath, mPersistenceBackupFilePath);
            }
        }

        /// <summary>
        /// Reverts the environment back to its original state.
        /// </summary>
        [TestCleanup]
        public void Cleanup()
        {
            // Clean up.
            if (File.Exists(mPersistenceFilePath))
            {
                File.Delete(mPersistenceFilePath);
            }

            // Restore the system to its original state.
            if (File.Exists(mPersistenceBackupFilePath))
            {
                File.Move(mPersistenceBackupFilePath, mPersistenceFilePath);
            }
        }
        #endregion

        #region Helper Methods
        public static string ToString(int objectIdInt, String objectIdString)
        {
            return String.Format("AsInt:<{0}>. ToString<{1}>.", objectIdInt, objectIdString);
        }
        #endregion


        /// <summary>
        /// Tests the Copy Constructor.
        /// </summary>
        [TestMethod]
        public void CopyConstructorTest()
        {
            var objectId = new ObjectId();

            var objectId2 = new ObjectId(objectId.AsInt());

            Assert.AreEqual(objectId.ToString(), objectId2.ToString(), "Data is equal");
            Assert.AreNotEqual(objectId, objectId2, "Identity is unequal");
        }

        /// <summary>
        /// Tests a new Id gets assigned.
        /// Tests if file doesn't exist, it gets created.
        /// Test works if file exists.
        /// </summary>
        [TestMethod]
        public void GetObjectIdTest()
        {
            var objectId = new ObjectId();
            var expected = ToString(1, "1");
            var actual = ToString(objectId.AsInt(), objectId.ToString());
            Assert.AreEqual(expected, actual);

            var objectId2 = new ObjectId();
            expected = ToString(2, "2");
            actual = ToString(objectId2.AsInt(), objectId2.ToString());
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Data testing of CompareTo function.
        /// </summary>
        [TestMethod]
        public void CompareToTest()
        {
            var objectId = new ObjectId();
            var objectId2 = new ObjectId();
            var objectId3 = new ObjectId(objectId2.AsInt());

            var actual = objectId.CompareTo(objectId2);
            Assert.AreEqual(-1, actual, "Input ObjectId:<{0}>. Input Compare ObjectId:<{1}>.", objectId.AsInt(), objectId2.AsInt());

            actual = objectId2.CompareTo(objectId);
            Assert.AreEqual(1, actual, "Input ObjectId:<{0}>. Input Compare ObjectId:<{1}>.", objectId2.AsInt(), objectId.AsInt());

            actual = objectId2.CompareTo(objectId3);
            Assert.AreEqual(0, actual, "Input ObjectId:<{0}>. Input Compare ObjectId:<{1}>.", objectId2.AsInt(), objectId3.AsInt());
        }
    }
}
