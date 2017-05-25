using System;

namespace Diary
{
    /// <summary>
    /// This is the base class for all the domain classes.
    /// The Client thinks of all instances via the Product interface.
    /// </summary>
    public class DiaryProduct
    {
        private ClassId mClassId;
        private ObjectId mObjectId;

        /// <summary>
        /// Creates a Diary Product.
        /// </summary>
        /// <param name="classId"></param>
        /// <param name="objectId"></param>
        public DiaryProduct(ClassId classId, ObjectId objectId)
        {
            mClassId = classId;
            mObjectId = objectId;
        }

        /// <summary>
        /// Gets the ClassId.
        /// </summary>
        /// <returns></returns>
        public ClassId GetClassId()
        {
            return mClassId;
        }

        /// <summary>
        /// Gets the ObjectId.
        /// </summary>
        /// <returns></returns>
        /// <remarks>The same identity of object must be retained in order for key lookups to behave as expected.</remarks>
        public ObjectId GetObjectId()
        {
            return mObjectId;
        }
    }
}
