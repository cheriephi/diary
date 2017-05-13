using Diary;
using System;

namespace DiaryTest
{
    /// <summary>
    /// Contact creator.
    /// </summary>
    /// <seealso cref="ReminderBuilder">For more details on the Builder pattern.</seealso>
    internal class ContactBuilder
    {
        private ObjectId objectId = new ObjectId();
        private String firstName = "";
        private String lastName = "";
        private String contactInfo = "";

        internal ContactBuilder SetObjectId(ObjectId objectId)
        {
            this.objectId = objectId;
            return this;
        }

        internal ContactBuilder SetFirstName(String firstName)
        {
            this.firstName = firstName;
            return this;
        }

        internal ContactBuilder SetLastName(String lastName)
        {
            this.lastName = lastName;
            return this;
        }

        internal ContactBuilder SetContactInfo(String contactInfo)
        {
            this.contactInfo = contactInfo;
            return this;
        }

        internal Contact Build()
        {
            var reminder = new Contact(objectId, firstName, lastName, contactInfo);
            return reminder;
        }
    }
}
