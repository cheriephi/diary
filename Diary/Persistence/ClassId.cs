using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace Diary
{
    /// <summary>
    /// The ClassId class has several functions:
    /// * It maintains a table of classes and corresponding contains keys.
    /// * It allocates new keys as needed for classes.
    /// * It keeps track of the most recently allocated class id in a file, so as each new ObjectId is created, 
    ///   it gets a unique value.
    /// </summary>
    public class ClassId : IComparable<ClassId>
    {
        private int mClassId;
        private static String mPersistenceFilePath = ConfigurationManager.AppSettings["PersistenceClassIdFilePath"];

        /// <summary>
        /// Initializes a ClassId.
        /// Each time a ClassId instance is created; the last used value will be read and returned.
        /// If it is a new class name, it will be assigned the next available value.
        /// </summary>
        /// <param name="className"></param>
        public ClassId(String className)
        {
            if (className.Contains(" "))
            {
                throw new ArgumentException("ClassName cannot contain a space.", className);
            }

            mClassId = GetClassId(className);
        }

        /// <summary>
        /// Re-creates an existing Class Id.
        /// </summary>
        /// <param name="classId"></param>
        public ClassId(int classId)
        {
            mClassId = classId;
        }

        /// <summary>
        /// Returns the ClassId as a String.
        /// </summary>
        /// <returns></returns>
        public override String ToString()
        {
            return mClassId.ToString();
        }
        
        /// <summary>
        /// Returns how the current ClassId sorts in comparison to the input compare ClassId.
        /// </summary>
        /// <param name="compare"></param>
        /// <returns></returns>
        /// <see cref="Date.CompareTo(Date)">For CompareTo method behavior.</see>
        public int CompareTo(ClassId compare)
        {
            int result = 0;
            if (mClassId > compare.mClassId)
            {
                result = 1;
            }
            else if (mClassId < compare.mClassId)
            {
                result = -1;
            }

            return result;
        }

        #region Private Methods
        private int GetClassId(String className)
        {
            int classId = 0;
            var classNameMap = Load();

            var foundExistingClassName = classNameMap.ContainsKey(className);
            if (foundExistingClassName)
            {
                classId = classNameMap[className];
            }
            else
            {
                classId = classNameMap.Count + 1;
                classNameMap.Add(className, classId);
                Save(classNameMap);
            }
            return classId;
        }

        private Dictionary<String, int> Load()
        {
            var classNameMap = new Dictionary<String, int>();

            if (File.Exists(mPersistenceFilePath))
            {
                var fileReader = new StreamReader(mPersistenceFilePath);

                var inputLine = fileReader.ReadLine();
                while (inputLine != null)
                {
                    var column = inputLine.Split(' ');
                    var classId = Convert.ToInt32(column[1]);
                    classNameMap.Add(column[0], classId);
                    inputLine = fileReader.ReadLine();
                }

                fileReader.Close();
            }

            return classNameMap;
        }

        private void Save(Dictionary<String, int> classNameMap)
        {
            // Now overwrite the original file (create the folder and file if they do not exist.
            var folder = Path.GetDirectoryName(mPersistenceFilePath);
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            var outputStream = new StreamWriter(mPersistenceFilePath);
            foreach (KeyValuePair<String, int> entry in classNameMap)
            {
                var outputline = entry.Key + " " + entry.Value + "\r\n";
                outputStream.Write(outputline);
            }
            outputStream.Close();
        }
        #endregion
    }
}
