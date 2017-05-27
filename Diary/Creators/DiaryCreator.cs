namespace Diary
{
    /// <summary>
    /// Base logic for creator classes. Implements the factory design pattern.
    /// </summary>
    public abstract class DiaryCreator
    {
        private ClassId mClassId;
        static KeyFile sKeyFile = new KeyFile("C:/Persistence", "Keys");
        static VariableLengthRecordFile sDataFile = new VariableLengthRecordFile("C:/Persistence", "Data");
        
        /// <summary>
        /// Initializes a DiaryCreator.
        /// </summary>
        /// <param name="classId"></param>
        public DiaryCreator(ClassId classId)
        {
            mClassId = classId;
        }

        /// <summary>
        /// Recreates an existing DiaryProduct from persistent storage.
        /// </summary>
        /// <param name="objectId"></param>
        /// <returns></returns>
        public abstract DiaryProduct Create(ObjectId objectId);

        /// <summary>
        /// Saves DiaryProducts in memory to persistent storage.
        /// </summary>
        public abstract void Save();

        /// <summary>
        /// Evaluates if this factory can be used for creating a particular object.
        /// </summary>
        /// <param name="classId"></param>
        /// <returns></returns>
        public bool RecognizesClue(ClassId classId)
        {
            return (classId.CompareTo(mClassId) == 0);
        }

        #region Persistence methods
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
        /// Writes the input DiaryProduct to persistent storage.
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
        #endregion
    }
}
