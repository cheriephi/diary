using System;
using System.Collections.Generic;
using System.IO;

namespace Diary
{
    public class KeyFile
    {
        /// <summary>
        /// File to retrieve and save keys.
        /// </summary>
        private String mPathname;
        private Dictionary<ObjectId, OffsetAndLength> mKeys;

        /// <summary>
        /// Initializes a KeyFile from persistent storage as appropriate.
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="filename"></param>
        public KeyFile(String folder, String filename)
        {
            mPathname = folder;
            mPathname += ("/");
            mPathname += (filename);
            mPathname += (".ids");
            mKeys = new Dictionary<ObjectId, OffsetAndLength>();
            Load();
        }

        /// <summary>
        /// Adds / updates the input objectId into memory.
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="dataFileOffset"></param>
        /// <param name="dataSizeBytes"></param>
        public void Update(ObjectId objectId, int dataFileOffset, int dataSizeBytes)
        {
            var offsetAndLength = new OffsetAndLength();
            offsetAndLength.fileOffset = dataFileOffset;
            offsetAndLength.sizeBytes = dataSizeBytes;
            if (mKeys.ContainsKey(objectId))
            {
                mKeys[objectId] = offsetAndLength;
            }
            else
            {
                mKeys.Add(objectId, offsetAndLength);
            }
        }

        /// <summary>
        /// Looks up the input objectId in memory based on its identity.
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="dataFileOffset"></param>
        /// <param name="dateSizeBytes"></param>
        /// <returns></returns>
        public bool Get(ObjectId objectId, ref int dataFileOffset, ref int dateSizeBytes)
        {
            if (mKeys.ContainsKey(objectId))
            {
                OffsetAndLength offsetAndLength = mKeys[objectId];
                dataFileOffset = offsetAndLength.fileOffset;
                dateSizeBytes = offsetAndLength.sizeBytes;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Writes the Key File to persistent storage.
        /// </summary>
        public void Save()
        {
            var outputStream = new StreamWriter(mPathname);
            foreach (KeyValuePair<ObjectId, OffsetAndLength> entry in mKeys)
            {
                OffsetAndLength offsetAndLength = entry.Value;
                String outputline = entry.Key + " " +
                offsetAndLength.fileOffset + " " +
                offsetAndLength.sizeBytes + "\r\n";
                outputStream.Write(outputline);
            }
            outputStream.Close();
        }

        /// <summary>
        /// Loads the Key File into memory.
        /// </summary>
        private void Load()
        {
            var classNameMap = new Dictionary<String, int>();

            if (File.Exists(mPathname))
            {
                var fileReader = new StreamReader(mPathname);
                var inputLine = fileReader.ReadLine();
                while (inputLine != null)
                {
                    var column = inputLine.Split(' ');
                    var objectId = new ObjectId(Convert.ToInt32(column[0]));
                    var offsetAndLength = new OffsetAndLength();
                    offsetAndLength.fileOffset = Convert.ToInt32(column[1]);
                    offsetAndLength.sizeBytes = Convert.ToInt32(column[2]);
                    mKeys.Add(objectId, offsetAndLength);
                    inputLine = fileReader.ReadLine();
                }

                fileReader.Close();
            }
        }

        class OffsetAndLength
        {
            public int fileOffset; // Offset into data file 
            public int sizeBytes; // Number of bytes in record 
        }
    }
}
