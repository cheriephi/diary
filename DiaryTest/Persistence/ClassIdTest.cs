using Diary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DiaryTest
{
    /// <summary>
    /// Class Id tests.
    /// </summary>
    /// <see cref="ObjectIdTest"/>
    [TestClass]
    public unsafe class ClassIdTest
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
            fixture = new TransientPersistenceFreshFixture("ClassId");
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
        /// Tests the Copy Constructor.
        /// </summary>
        [TestMethod]
        public void CopyConstructorTest()
        {
            var className = "MyClass";

            var classId = new ClassId(className);
            var classId2 = new ClassId(className);

            Assert.AreEqual("1", classId.ToString(), "Original");
            Assert.AreEqual("1", classId2.ToString(), "Copied");
        }

        /// <summary>
        /// Tests the ToString method using simple scenarios.
        /// </summary>
        [TestMethod]
        public void ToStringTest()
        {
            var classId = new ClassId("wiggle");
            var expected = "1";
            var actual = classId.ToString();
            Assert.AreEqual(expected, actual);

            var classId2 = new ClassId("wazzup!");
            expected = "2";
            actual = classId2.ToString();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Data testing of CompareTo function.
        /// </summary>
        [TestMethod]
        public void CompareToTest()
        {
            var classId =  new ClassId("a");
            var classId2 = new ClassId("b");
            var classId3 = new ClassId(Convert.ToInt32(classId2.ToString()));

            var actual = classId.CompareTo(classId2);
            Assert.AreEqual(-1, actual, "Input ClassId:<{0}>. Input Compare ClassId:<{1}>.", classId, classId2);

            actual = classId2.CompareTo(classId);
            Assert.AreEqual(1, actual, "Input ClassId:<{0}>. Input Compare ClassId:<{1}>.", classId2, classId);

            actual = classId2.CompareTo(classId3);
            Assert.AreEqual(0, actual, "Input ClassId:<{0}>. Input Compare ClassId:<{1}>.", classId2, classId3);
        }
    }
}
