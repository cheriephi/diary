using System;
using System.Configuration;
using System.IO;

namespace Diary
{
    /// <summary>
    /// The ObjectId class has two functions:
    /// * It contains the key for the current instance.
    /// * It keeps track of the most recently allocated object Id in a file, so as each new ObjectId is created,
    ///   it gets a unique value.
    /// </summary>
    public class ObjectId : IComparable<ObjectId>
    {
        private int mObjectId;
        private static String mPersistenceFilePath = String.Concat(
            ConfigurationManager.AppSettings["PersistenceFolderPath"], 
            @"\", 
            ConfigurationManager.AppSettings["PersistenceObjectIdFileName"]
            );

        #region Constructors
        /// <summary>
        /// Initializes a new ObjectId.
        /// </summary>
        public ObjectId()
        {
            mObjectId = GetNextId();
        }

        /// <summary>
        /// Re-creates an existing ObjectId.
        /// </summary>
        /// <param name="objectId"></param>
        public ObjectId(int objectId)
        {
            mObjectId = objectId;
        }
        #endregion

        #region Accessors
        /// <summary>
        /// Returns the ObjectId's string identifier.
        /// </summary>
        /// <returns></returns>
        public override String ToString()
        {
            return mObjectId.ToString();
        }

        /// <summary>
        /// Casts to int.
        /// </summary>
        /// <returns></returns>
        public int AsInt()
        {
            return mObjectId;
        }
        #endregion

        /// <summary>
        /// Returns how the current ObjectId sorts in comparison to the input compare ObjectId.
        /// </summary>
        /// <param name="compare"></param>
        /// <returns></returns>
        /// <see cref="Date.CompareTo(Date)">For CompareTo method behavior.</see>
        public int CompareTo(ObjectId compare)
        {
            if (mObjectId < compare.mObjectId)
            {
                return -1;
            }
            else if (mObjectId == compare.mObjectId)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }

        /// <summary>
        /// Returns the next unused sequentially assigned Object Id in a persistent manner.
        /// </summary>
        /// <returns></returns>
        /// <remarks>Each time an ObjectId instance is created, the last used value will be read and incremented.
        /// The incremented value will be used as the internal value.</remarks>
        private int GetNextId()
        {
            int objectId = 1;  // Default value if file does not exist.

            if (File.Exists(mPersistenceFilePath))
            {
                var fileReader = new StreamReader(mPersistenceFilePath);

                var inputLine = fileReader.ReadLine();
                if (inputLine != null)
                {
                    objectId = Convert.ToInt32(inputLine);
                    objectId++;
                }
                fileReader.Close();
            }

            // Now overwrite the original file (create the folder and file if they do not exist).
            var folder = Path.GetDirectoryName(mPersistenceFilePath);
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            var fileWriter = new StreamWriter(mPersistenceFilePath);
            var outputLine = objectId.ToString();
            fileWriter.Write(outputLine);
            fileWriter.Close();

            return objectId;
        }
    }
}
