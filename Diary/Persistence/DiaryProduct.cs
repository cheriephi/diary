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
            mObjectId = new ObjectId(objectId.AsInt());
            mClassId = new ClassId(Convert.ToInt32(classId.ToString()));
        }

        /// <summary>
        /// Gets the ClassId.
        /// </summary>
        /// <returns></returns>
        public ClassId GetClassId()
        {
            return new ClassId(Convert.ToInt32(mClassId.ToString()));
        }

        /// <summary>
        /// Gets the ObjectId.
        /// </summary>
        /// <returns></returns>
        public ObjectId GetObjectId()
        {
            return new ObjectId(mObjectId.AsInt());
        }
    }
}
