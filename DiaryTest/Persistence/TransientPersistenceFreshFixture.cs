using System;
using System.Configuration;
using System.IO;

namespace DiaryTest
{
    /// <summary>
    /// Resets any persisted settings so the tests can proceed in a repeatable and deterministic manner.
    /// </summary>
    /// <remarks>The persistence folder path is copied as a link from the System Under Test's app.config file.</remarks>
    internal class TransientPersistenceFreshFixture
    {
        private String mPersistenceFolderPath;
        private String mPersistenceBackupFolderPath;

        internal TransientPersistenceFreshFixture()
        {
            mPersistenceFolderPath = ConfigurationManager.AppSettings["PersistenceFolderPath"];
            mPersistenceBackupFolderPath = mPersistenceFolderPath + System.DateTime.Now.Ticks;
        }

        /// <summary>
        /// Migrates persisted settings elsewhere and sets the environment to an original sate.
        /// </summary>
        public void Init()
        {
            if (Directory.Exists(mPersistenceFolderPath))
            {
                if (Directory.Exists(mPersistenceBackupFolderPath))
                {
                    Directory.Delete(mPersistenceBackupFolderPath);
                }
                Directory.Move(mPersistenceFolderPath, mPersistenceBackupFolderPath);
            }
        }

        /// <summary>
        /// Reverts the environment back to its original state.
        /// </summary>
        public void Cleanup()
        {
            // Clean up.
            if (Directory.Exists(mPersistenceFolderPath))
            {
                Directory.Delete(mPersistenceFolderPath, true);
            }

            // Restore the system to its original state.
            if (Directory.Exists(mPersistenceBackupFolderPath))
            {
                Directory.Move(mPersistenceBackupFolderPath, mPersistenceFolderPath);
            }
        }
    }
}
