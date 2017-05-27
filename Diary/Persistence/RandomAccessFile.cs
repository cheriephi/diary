using System;
using System.IO;

namespace Diary
{
    /// <summary>
    /// Handles random access file functionality.
    /// </summary>
    /// <remarks>The java library is no longer supported in Visual Studio (deprecated along with Visual J#).</remarks>
    public class RandomAccessFile
    {
        private FileStream mFileStream;
        private BinaryReader mBinaryReader;
        private BinaryWriter mBinaryWriter;

        /// <summary>
        /// Initializes a RandomAccessFile stream to read from, and optionally to write to, a file with the specified name.
        /// </summary>
        /// <param name="pathname"></param>
        public RandomAccessFile(String pathname)
        {
            mFileStream = new FileStream(pathname, FileMode.OpenOrCreate,
            FileAccess.ReadWrite);

            mBinaryReader = new BinaryReader(mFileStream);
            mBinaryWriter = new BinaryWriter(mFileStream);
        }

        /// <summary>
        /// Closes this RandomAccessFile stream and releases any system resources associated with the stream.
        /// </summary>
        public void close()
        {
            mBinaryWriter.Close();
            mBinaryReader.Close();
            mFileStream.Close();
        }

        /// <summary>
        /// Returns the opaque file descriptor object associated with this stream.
        /// </summary>
        /// <returns></returns>
        public long getFilePointer()
        {
            return mFileStream.Position;
        }

        /// <summary>
        /// Writes len bytes from the specified byte array starting at offset off to this file.
        /// </summary>
        /// <param name="value"></param>
        public void writeInt(int value)
        {
            mBinaryWriter.Write(value);
            mBinaryWriter.Flush();
        }

        /// <summary>
        /// Writes a byte to the file as a one-byte value. 
        /// </summary>
        /// <param name="value"></param>
        public void writeBytes(String value)
        {
            mBinaryWriter.Write(value);
            mBinaryWriter.Flush();
        }

        /// <summary>
        /// Returns the length of this file. 
        /// </summary>
        /// <returns></returns>
        public long length()
        {
            return mFileStream.Length;
        }

        /// <summary>
        /// Reads up to b.length bytes of data from this file into an array of bytes.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="dataSizeBytes"></param>
        /// <returns></returns>
        public int read(ref String value, int dataSizeBytes)
        {
            value = mBinaryReader.ReadString();
            return dataSizeBytes;
        }

        public int readInt()
        {
            return mBinaryReader.ReadInt32();
        }

        public void seek(long offset)
        {
            mFileStream.Seek(offset, SeekOrigin.Begin);
        }        
    }
}
