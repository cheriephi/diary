﻿using System;
using System.IO;

namespace Diary
{
    /// <summary>
    /// Handles random access file functionality.
    /// </summary>
    /// <see cref="DiaryCreator">For more inline comments on disposing resources.</see>
    public class RandomAccessFile : IDisposable
    {
        private FileStream mFileStream;
        private BinaryReader mBinaryReader;
        private BinaryWriter mBinaryWriter;

        private bool disposed = false;

        /// <summary>
        /// Initializes a RandomAccessFile stream to read from, and optionally to write to, a file with the specified name.
        /// </summary>
        /// <param name="pathname"></param>
        public RandomAccessFile(String pathname)
        {
            mFileStream = new FileStream(pathname, FileMode.OpenOrCreate, FileAccess.ReadWrite);
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
        /// Writes the input value starting at offset to this file.
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
        /// Reads up to dataSizeBytes of data from this file into an array of bytes.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="dataSizeBytes"></param>
        /// <returns></returns>
        public int read(ref String value, int dataSizeBytes)
        {
            value = mBinaryReader.ReadString();
            return dataSizeBytes;
        }

        /// <summary>
        /// Returns the next integer from this file.
        /// </summary>
        /// <returns></returns>
        public int readInt()
        {
            return mBinaryReader.ReadInt32();
        }

        /// <summary>
        /// Sets the file pointer to the input location. 
        /// </summary>
        /// <param name="offset"></param>
        public void seek(long offset)
        {
            mFileStream.Seek(offset, SeekOrigin.Begin);
        }

        #region Cleanup
        /// <summary>
        /// Closes open streams.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
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
                    this.close();
                }

                disposed = true;
            }
        }

        /// <summary>
        /// RandomAccessFile Destructor and clean up.
        /// </summary>
        ~RandomAccessFile()
        {
            Dispose(false);
        }
        #endregion
    }
}
