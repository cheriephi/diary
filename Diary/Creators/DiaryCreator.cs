using System;
using System.Configuration;

namespace Diary
{
    /// <summary>
    /// Base logic for creator classes. Implements the factory design pattern.
    /// </summary>
    public abstract class DiaryCreator : IDisposable
    {
        private ClassId mClassId;
        private static KeyFile sKeyFile;
        private static VariableLengthRecordFile sDataFile;

        /// <summary>
        /// The number of active instances of this DiaryCreator class.
        /// </summary>
        private static int mDiaryCreatorCount;

        // Track whether Dispose has been called.
        private bool disposed = false;

        /// <summary>
        /// Initializes a DiaryCreator.
        /// </summary>
        /// <param name="classId"></param>
        public DiaryCreator(ClassId classId)
        {
            mClassId = classId;

            var persistenceFolderPath = ConfigurationManager.AppSettings["PersistenceFolderPath"];

            if (mDiaryCreatorCount == 0)
            { 
                sDataFile = new VariableLengthRecordFile(persistenceFolderPath, ConfigurationManager.AppSettings["PersistenceDataFileNameWithoutExtension"]);
                sKeyFile = new KeyFile(persistenceFolderPath, ConfigurationManager.AppSettings["PersistenceKeyFileNameWithoutExtension"]);
            }

            mDiaryCreatorCount++;
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
                var record = new VariableLengthRecord();
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

        #region Cleanup
        /// <summary>
        /// Disposal to close ongoing persistence work. The file is appended to throughout the program, but needs to be closed to release handles.
        /// </summary>
        /// <see href="https://docs.microsoft.com/en-us/dotnet/api/system.idisposable?view=netframework-4.7">About finalization.</see>
        public void Dispose()
        {
            Dispose(true);
            // Prevent finalizing code from executing twice.
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Close the persistence mechanism.
        /// </summary>
        /// <param name="disposing"></param>
        /// <remarks>We need one and only VariableLengthRecordFile open. The class doesn't expose a way to open files, this is done through its constructor.
        /// The class appends on an open handle until complete. This class (DiaryCreator) needs to close its usage, and the only way to do that is in the destructor.
        /// It should only close access when all consumers of it are complete.</remarks>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    mDiaryCreatorCount--;
                    if (mDiaryCreatorCount == 0)
                    { 
                        sDataFile.Close();
                    }
                }

                disposed = true;
            }
        }

        /// <summary>
        /// DiaryCreator Destructor and clean up.
        /// </summary>
        ~DiaryCreator()
        {
            Dispose(false);
        }
        #endregion
    }
}
