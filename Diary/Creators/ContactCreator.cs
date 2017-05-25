using System;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
namespace Diary
{
    public class ContactCreator : DiaryCreator
    {
        public ContactCreator() : base(new ClassId("Contact"))
        {
            mContacts = new Relation1M<Contact>();
        }

        public DiaryProduct CreateNew(String firstName, String lastName, String contactInfo)
        {
            var objectId = new ObjectId();
            var contact = new Contact(objectId, firstName, lastName, contactInfo);
            mContacts.Add(contact);

            return contact;
        }

        /// <summary>
        /// Create an eisting contact.
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

        public override void Save()
        {
            // Step through any previously loaded records and save to file. 
            for (int childIndex = 0; childIndex < mContacts.GetChildCount(); ++childIndex)
            {
                Contact contact = mContacts.GetChild(childIndex);

                VariableLengthRecord record = new VariableLengthRecord();
                String[] names = contact.GetName();
                record.AppendValue(names[0]);
                record.AppendValue(names[1]);
                record.AppendValue(contact.GetContactInfo());

                Write(contact.GetObjectId(), record);
            }
        }

        private Relation1M<Contact> mContacts;
    }
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
