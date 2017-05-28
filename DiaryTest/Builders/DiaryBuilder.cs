using Diary;

namespace DiaryTest
{
    /// <summary>
    /// Builder factory pattern.
    /// </summary>
    /// <remarks>
    /// A creational design pattern to enable anoymous creation of the System Under Test, 
    /// but parameterize necessary values.
    /// The object can be incrementally built from parameters.
    /// This keeps tests clutter free from in-line setup and minimizes obscure tests.
    /// </remarks>
    internal abstract class DiaryBuilder
    {
        private ObjectId objectId = new ObjectId();
        private DiaryCreator creator;

        internal ObjectId GetObjectId()
        {
            return objectId;
        }

        internal DiaryBuilder SetObjectId(ObjectId objectId)
        {
            this.objectId = objectId;
            return this;
        }

        internal DiaryCreator GetCreator()
        {
            return creator;
        }

        internal DiaryBuilder SetCreator(DiaryCreator creator)
        {
            this.creator = creator;
            return this;
        }

        internal abstract DiaryProduct Build();
    }
}
