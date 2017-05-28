using Diary;
using System;

namespace DiaryTest
{
    /// <summary>
    /// Builder factory pattern for Contact class.
    /// </summary>
    /// <see cref="DiaryBuilder"/>
    internal class ContactBuilder : DiaryBuilder
    {
        private String firstName = "";
        private String lastName = "";
        private String contactInfo = "";

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

        internal override DiaryProduct Build()
        {
            var creator = (ContactCreator)this.GetCreator();

            Contact contact;
            if (creator != null)
            {
                contact = (Contact)creator.CreateNew(firstName, lastName, contactInfo);
            }
            else
            {
                contact = new Contact(base.GetObjectId(), firstName, lastName, contactInfo);
            }

            return contact;
        }
    }
}
