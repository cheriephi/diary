#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
namespace Diary
{
    /// <summary>
    /// Base logic for creator classes.
    /// </summary>
    public abstract class DiaryCreator
    {
        private ClassId mClassId;
        static KeyFile sKeyFile = new KeyFile("C:/Persistence", "Keys");
        static VariableLengthRecordFile sDataFile = new VariableLengthRecordFile("C:/Persistence", "Data");
        
        /// <summary>
        /// Creates a diary.
        /// </summary>
        /// <param name="classId"></param>
        public DiaryCreator(ClassId classId)
        {
            mClassId = classId;
        }

        public abstract DiaryProduct Create(ObjectId objectId);
        public abstract void Save();

        /// <summary>
        /// Helps evaluate if this factory can be used for creating a particular object.
        /// </summary>
        /// <param name="classId"></param>
        /// <returns></returns>
        public bool RecognizesClue(ClassId classId)
        {
            return (classId.CompareTo(mClassId) == 0);
        }

        /// <summary>
        /// Reads a variable length record by object id.  
        /// </summary>
        /// <param name="objectId"></param>
        /// <returns>Returns null if the record does not exist.</returns>
        public VariableLengthRecord Read(ObjectId objectId)
        {
            int dataFileOffset = 0;
            int dataSize = 0;
            if (sKeyFile.Get(objectId, ref dataFileOffset, ref dataSize))
            {
                VariableLengthRecord record = new VariableLengthRecord();
                if (sDataFile.Read(dataFileOffset, dataSize, record))
                {
                    return record;
                }
                else
                {
                    int x = dataFileOffset;
                }
            }
            return null;
        }

        /// <summary>
        /// Writes a variable length record by object id.
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="record"></param>
        public void Write(ObjectId objectId, VariableLengthRecord record)
        {
            int dataFileOffset = 0;
            int dataSize = 0;

            if (sDataFile.Append(record, ref dataFileOffset, ref dataSize))
            {
                sKeyFile.Update(objectId, dataFileOffset, dataSize);
                sKeyFile.Save();
            }
        }
    }
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
