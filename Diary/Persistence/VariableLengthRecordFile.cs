using System;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
namespace Diary
{
    public class VariableLengthRecordFile
    {
        private String mPathname;       // File pathname
        private int mBytesUsed;      // Total number of bytes of data in the file up to ~4 meg
        private RandomAccessFile mFileStream;

        public VariableLengthRecordFile(String folder, String filename)
        {
            mPathname = folder;
            mPathname += ("/");
            mPathname += (filename);
            mPathname += (".dat");

            mFileStream = new RandomAccessFile(mPathname);
            if (mFileStream.length() > 0)
            {
                mFileStream.seek(0);
                mBytesUsed = mFileStream.readInt();
            }
            else
            {
                mBytesUsed = 0;
                mFileStream.writeInt(mBytesUsed);
            }
        }

        public int GetBytesUsed()
        {
            return mBytesUsed;
        }

        public int GetBytesTotal()
        {
            try
            {
                return (int)mFileStream.length();
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        /// Adds the record and returns the starting offset and number of bytes written.
        /// </summary>
        /// <param name="record"></param>
        /// <param name="dataFileOffset"></param>
        /// <param name="dataSizeBytes"></param>
        /// <returns></returns>
        public bool Append(VariableLengthRecord record, ref int dataFileOffset, ref int dataSizeBytes)
        {
            try
            {
                dataFileOffset = (int)mFileStream.length();
                mFileStream.seek(dataFileOffset);                 // Seek to the end of the file

                dataSizeBytes = record.Serialize(mFileStream);            // Convert the record to the stream
                mBytesUsed += dataSizeBytes;

                mFileStream.seek(0);
                mFileStream.writeInt(mBytesUsed);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Read(int dataFileOffset, int dataSizeBytes, VariableLengthRecord record)
        {
            try
            {
                mFileStream.seek(dataFileOffset); // Seek to the end of the file

                return (record.Deserialize(mFileStream));
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void Close()
        {
            if (mFileStream != null)
            {
                mFileStream.close();
            }
        }

        ~VariableLengthRecordFile()
        {
            Close();
        }
    }
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
