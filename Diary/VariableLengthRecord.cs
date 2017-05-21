using System;
using System.Collections.Generic;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
namespace Diary
{
    /// <summary>
    /// A container of the zero or more fields.
    /// </summary>
    public class VariableLengthRecord
    {
        private List<RecordElement> mElements;

        #region Enums
        /// <summary>
        /// Supported types for persistence.
        /// </summary>
        public enum RecordElementType
        {
            BOOLEAN,
            BYTE,
            INT,
            FLOAT,
            DOUBLE,
            STRING,
            OBJECT_ID
        }
        #endregion

        /// <summary>
        /// A persisted field.
        /// </summary>
        public class RecordElement
        {
            public RecordElementType mType;
            public int mSize;
            public String mText;

            public RecordElement(RecordElementType type, String text)
            {
                mType = type;
                mText = text;
                mSize = text.Length;
            }
        }

        /// <summary>
        /// Creates a variable length record.
        /// </summary>
        public VariableLengthRecord()
        {
            mElements = new List<RecordElement>();
        }

        /// <summary>
        /// Returns the number of elements in the record.
        /// </summary>
        /// <returns></returns>
        public int GetCount()
        {
            return mElements.Count;
        }

        #region Append functions
        /// <summary>
        /// Adds the input value to the record.
        /// </summary>
        /// <param name="value"></param>
        public void AppendValue(bool value)
        {
            string text = (value) ? "Y" : "N";
            mElements.Add(new RecordElement(RecordElementType.BOOLEAN, text));
        }

        public void AppendValue(char value)
        {
            string text = value.ToString();
            mElements.Add(new RecordElement(RecordElementType.BYTE, text));
        }

        public void AppendValue(int value)
        {
            mElements.Add(new RecordElement(RecordElementType.INT, value.ToString()));
        }

        public void AppendValue(float value)
        {
            mElements.Add(new RecordElement(RecordElementType.FLOAT,
            value.ToString()));
        }
        public void AppendValue(double value)
        {
            mElements.Add(new RecordElement(RecordElementType.DOUBLE,
            value.ToString()));
        }

        public void AppendValue(String value)
        {
            mElements.Add(new RecordElement(RecordElementType.STRING, value));
        }

        public void AppendValue(ObjectId value)
        {
            mElements.Add(new RecordElement(RecordElementType.OBJECT_ID, value.AsInt().ToString()));
        }
        #endregion

        #region GetValue functions
        /// <summary>
        /// Returns the requested element.
        /// </summary>
        /// <param name="elementIndex"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool GetValue(int elementIndex, ref bool value)
        {
            if (mElements.Count > elementIndex)
            {
                if (mElements[elementIndex].mType == RecordElementType.BOOLEAN)
                {
                    String text = mElements[elementIndex].mText;

                    if (text[0] == 'Y')
                    {
                        value = true;
                        return true;
                    }
                    else if (text[0] == 'N')
                    {
                        value = false;
                        return true;
                    }
                }
            }
            return false;
        }

        public bool GetValue(int elementIndex, ref char value)
        {
            if (mElements.Count > elementIndex)
            {
                if (mElements[elementIndex].mType == RecordElementType.BYTE)
                {
                    value = mElements[elementIndex].mText[0];
                    return true;
                }
            }
            return false;
        }

        public bool GetValue(int elementIndex, ref int value)
        {
            if (mElements.Count > elementIndex)
            {
                if (mElements[elementIndex].mType == RecordElementType.INT)
                {
                    value = Convert.ToInt32(mElements[elementIndex].mText);
                    return true;
                }
            }
            return false;
        }

        public bool GetValue(int elementIndex, ref float value)
        {
            if (mElements.Count > elementIndex)
            {
                if (mElements[elementIndex].mType == RecordElementType.FLOAT)
                {
                    value = (float)Convert.ToDouble(mElements[elementIndex].mText);
                    return true;
                }
            }
            return false;
        }

        public bool GetValue(int elementIndex, ref double value)
        {
            if (mElements.Count > elementIndex)
            {
                if (mElements[elementIndex].mType == RecordElementType.DOUBLE)
                {
                    value = Convert.ToDouble(mElements[elementIndex].mText);
                    return true;
                }
            }
            return false;

        }

        public bool GetValue(int elementIndex, ref String value)
        {
            if (mElements.Count > elementIndex)
            {
                if (mElements[elementIndex].mType == RecordElementType.STRING)
                {
                    value = mElements[elementIndex].mText;
                    return true;
                }
            }
            return false;
        }

        public bool GetValue(int elementIndex, ref ObjectId value)
        {
            if (mElements.Count > elementIndex)
            {
                if (mElements[elementIndex].mType == RecordElementType.OBJECT_ID)
                {
                    int objectIdValue =
                    Convert.ToInt32(mElements[elementIndex].mText);
                    value = new ObjectId(objectIdValue);
                    return true;
                }
            }
            return false;
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="outputStream"></param>
        /// <returns></returns>
        public int Serialize(RandomAccessFile outputStream)
        {
            try
            {
                int elementCount = GetCount();
                int startingLocation = (int)outputStream.getFilePointer();
                outputStream.writeInt(elementCount);
                int location = (int)outputStream.getFilePointer();

                for (int elementIndex = 0; elementIndex < elementCount; ++elementIndex)
                {
                    int ordinal = (int)mElements[elementIndex].mType;
                    outputStream.writeInt((int)mElements[elementIndex].mType);  // Convert to ordinal

                    location = (int)outputStream.getFilePointer();
                    outputStream.writeInt(mElements[elementIndex].mSize);
                    location = (int)outputStream.getFilePointer();
                    outputStream.writeBytes(mElements[elementIndex].mText);
                    location = (int)outputStream.getFilePointer();
                }

                int bytesWritten = (int)outputStream.getFilePointer() - startingLocation;

                return bytesWritten;

            }
            catch (Exception)
            {
                return 0;
            }
        }

        public bool Deserialize(RandomAccessFile inputStream)
        {
            try
            {
                var elementCount = inputStream.readInt();

                for (int elementIndex = 0; elementIndex < elementCount; ++elementIndex)
                {
                    var elementTypeOrdinal = inputStream.readInt();
                    var elementType = (VariableLengthRecord.RecordElementType)elementTypeOrdinal;
                    var size = inputStream.readInt();

                    String value = String.Empty;

                    if (size == inputStream.read(ref value, size))
                    {
                        RecordElement recordElement = new
                        RecordElement(elementType, value);

                        mElements.Add(recordElement);
                    }
                }
                return true;
            }
            catch (Exception) { }

            return false;
        }
    }
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
