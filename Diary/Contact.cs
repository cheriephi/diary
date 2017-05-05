using System;

namespace Diary
{
    /// <summary>
    /// Stores Contact information
    /// </summary>
    public class Contact
    {
        /// <summary>
        /// Creates a Contact
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="contactInfo"></param>
        public Contact(String firstName, String lastName, String contactInfo)
        {
            mFirstName = firstName;
            mLastName = lastName;
            mContactInfo = contactInfo;
        }

        /// <summary>
        /// Returns a collection of the first and last name
        /// </summary>
        /// <returns></returns>
        public String[] GetName()
        {
            return new String[] { mFirstName, mLastName };
        }

        /// <summary>
        /// Returns the ContactInfo
        /// </summary>
        /// <returns></returns>
        public String GetContactInfo()
        {
            return mContactInfo;
        }

        private String mFirstName;
        private String mLastName;
        private String mContactInfo;
    }
}
