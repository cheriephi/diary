using System;
using System.Configuration;
using System.IO;

namespace DiaryTest
{
    /// <summary>
    /// Resets any persisted settings so the tests can proceed in a repeatable and deterministic manner.
    /// </summary>
    /// <remarks>The persistence file path is copied as a link from the System Under Test's app.config file.</remarks>
    internal class TransientPersistenceFreshFixture
    {
        private String mPersistenceFilePath;
        private String mPersistenceBackupFilePath;

        internal TransientPersistenceFreshFixture(String className)
        {
            var configurationSetting = "Persistence" + className + "FilePath";
            mPersistenceFilePath = ConfigurationManager.AppSettings[configurationSetting];
            mPersistenceBackupFilePath = Path.GetDirectoryName(mPersistenceFilePath) + @"/" + className + System.DateTime.Now.Ticks + ".txt";
        }

        /// <summary>
        /// Resets any persisted settings so the tests can proceed in a repeatable and deterministic manner.
        /// </summary>
        /// <remarks>The persistence file path is copied as a link from the System Under Test's app.config file.</remarks>
        public void Init()
        {
            if (File.Exists(mPersistenceFilePath))
            {
                if (File.Exists(mPersistenceBackupFilePath))
                {
                    File.Delete(mPersistenceBackupFilePath);
                }
                File.Move(mPersistenceFilePath, mPersistenceBackupFilePath);
            }
        }

        /// <summary>
        /// Reverts the environment back to its original state.
        /// </summary>
        public void Cleanup()
        {
            // Clean up.
            if (File.Exists(mPersistenceFilePath))
            {
                File.Delete(mPersistenceFilePath);
            }

            // Restore the system to its original state.
            if (File.Exists(mPersistenceBackupFilePath))
            {
                File.Move(mPersistenceBackupFilePath, mPersistenceFilePath);
            }
        }
    }
}
