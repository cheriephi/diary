using System;

namespace Diary
{
    /// <summary>
    /// Contact factory.
    /// </summary>
    /// <see cref="DiaryCreator"/>
    public class ContactCreator : DiaryCreator
    {
        private Relation1M<Contact> mContacts;

        /// <summary>
        /// Initializes a ContactCreator.
        /// </summary>
        public ContactCreator() : base(new ClassId("Contact"))
        {
            mContacts = new Relation1M<Contact>();
        }

        /// <summary>
        /// Creates a Contact DiaryProduct.
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="contactInfo"></param>
        /// <returns></returns>
        public DiaryProduct CreateNew(String firstName, String lastName, String contactInfo)
        {
            var objectId = new ObjectId();
            var contact = new Contact(objectId, firstName, lastName, contactInfo);
            mContacts.Add(contact);

            return contact;
        }

        /// <summary>
        /// Recreates an existing Contact from persistent storage.
        /// </summary>
        /// <param name="objectId"></param>
        /// <returns></returns>
        public override DiaryProduct Create(ObjectId objectId)
        {
            // Check if it already exists 
            for (int childIndex = 0; childIndex < mContacts.GetChildCount(); ++childIndex)
            {
                Contact contact = mContacts.GetChild(childIndex);
                if (contact.GetObjectId() == objectId)
                {
                    return contact;
                }
            }

            // Not already loaded ? 
            VariableLengthRecord record = Read(objectId);
            if (record != null)
            {
                String firstName = String.Empty;
                if (record.GetValue(0, ref firstName))
                {
                    String lastName = String.Empty;
                    if (record.GetValue(1, ref lastName))
                    {
                        String contactInfo = String.Empty;
                        if (record.GetValue(2, ref contactInfo))
                        {
                            Contact contact = new Contact(objectId, firstName, lastName, contactInfo);

                            mContacts.Add(contact);
                            return contact;
                        }
                    }
                }
            }

            return null;  // No such contact 
        }

        /// <summary>
        /// Saves Contacts in memory to persistent storage.
        /// </summary>
        public override void Save()
        {
            // Step through any previously loaded records and save to file. 
            for (int childIndex = 0; childIndex < mContacts.GetChildCount(); ++childIndex)
            {
                Contact contact = mContacts.GetChild(childIndex);

                var record = new VariableLengthRecord();
                String[] names = contact.GetName();
                record.AppendValue(names[0]);
                record.AppendValue(names[1]);
                record.AppendValue(contact.GetContactInfo());

                Write(contact.GetObjectId(), record);
            }
        }
    }
}
