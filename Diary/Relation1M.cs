using System.Collections.Generic;

namespace Diary
{
    /// <summary>
    /// Generically manages one to many relationships.
    /// </summary>
    /// <typeparam name="Type"></typeparam>
    /// <remarks>This class has no special handling for aliasing bugs. Mutable types passed in may be changed from code outside.</remarks>
    public class Relation1M<Type>
    {
        private List<Type> mChildren;

        /// <summary>
        /// This function is used to create a new relationship with no children.
        /// </summary>
        public Relation1M()
        {
            mChildren = new List<Type>();
        }

        /// <summary>
        /// Adds the input child to the list of relations.
        /// </summary>
        /// <param name="child"></param>
        public void Add(Type child)
        {
            mChildren.Add(child);
        }

        /// <summary>
        /// Returns the count of relations.
        /// </summary>
        /// <returns></returns>
        public int GetChildCount()
        {
            return mChildren.Count;
        }

        /// <summary>
        /// Returns the requested child from the list of relations.
        /// </summary>
        /// <param name="childIndex"></param>
        /// <returns></returns>
        public Type GetChild(int childIndex)
        {
            return mChildren[childIndex];
        }
    }
}