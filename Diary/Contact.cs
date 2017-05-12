using System;

namespace Diary
{
    /// <summary>
    /// Stores Contact information
    /// </summary>
    public class Contact : DiaryProduct
    {
        /// <summary>
        /// Creates a Contact
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="contactInfo"></param>
        public Contact(ObjectId objectId, String firstName, String lastName, String contactInfo) : base(new ClassId("Contact"), objectId)
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
