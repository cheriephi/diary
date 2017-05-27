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
        private KeyFile sKeyFile;
        private VariableLengthRecordFile sDataFile;

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
            sKeyFile = new KeyFile(persistenceFolderPath, ConfigurationManager.AppSettings["PersistenceKeyFileNameWithoutExtension"]);
            sDataFile = new VariableLengthRecordFile(persistenceFolderPath, ConfigurationManager.AppSettings["PersistenceDataFileNameWithoutExtension"]);
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
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    sDataFile.Close();
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
