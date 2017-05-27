using Diary;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DiaryTest
{
    /// <summary>
    /// Tests the Diary Product class.
    /// </summary>
    /// <seealso cref="ContactTest">About testing constructors via accessors.</seealso>/>
    /// <remarks>There are no aliasing tests because the object references exposed in the class under test don't provide
    /// a way to modify their data.</remarks>
    [TestClass]
    public class DiaryProductTest
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

        /// <summary>
        /// Tests the Diary Product ObjectId accessor.
        /// </summary>
        [TestMethod]
        public void GetObjectIdTest()
        {
            var objectId = new ObjectId();
            var product = new DiaryProduct(new ClassId(""), objectId);
            GetObjectIdTest(product, objectId);
        }

        /// <summary>
        /// Tests the Diary Product ClassId accessor.
        /// </summary>
        [TestMethod]
        public void GetClassIdTest()
        {
            var className = "Fonzie";
            var classId = new ClassId(className);
            var product = new DiaryProduct(classId, new ObjectId());
            GetClassIdTest(product, className);
        }

        /// <summary>
        /// Tests GetClassId method using polymorphism.
        /// </summary>
        internal void GetClassIdTest(DiaryProduct product, String className)
        {
            var classId = product.GetClassId();
            var productClassId = new ClassId(className);

            var expected = 0;
            var actual = classId.CompareTo(productClassId);

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests the ObjectId method using polymorphism.
        /// </summary>
        public void GetObjectIdTest(DiaryProduct product, ObjectId objectId)
        {
            var actual = product.GetObjectId();

            Assert.AreEqual(objectId.AsInt(), actual.AsInt(), "Data");

            Assert.AreEqual(objectId.GetHashCode(), actual.GetHashCode(), "Identity");
        }
    }
}
