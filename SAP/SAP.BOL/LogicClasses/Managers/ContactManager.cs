using SAP.BOL.Abstract;
using SAP.DAL.Abstract;
using System;
using SAP.DAL.Tables;
using System.Collections.Generic;

namespace SAP.BOL.LogicClasses.Managers
{
    public class ContactManager : IContactManager, IDisposable
    {
        private IContactRepository _contactRepository;

        public ContactManager(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public IEnumerable<Contact> Contacts
        {
            get
            {
                return _contactRepository.Contact;
            }
        }

        public bool AddNewContact(string firstName, string lastName, string email, string message)
        {
            DAL.Tables.Contact modelToSave = new DAL.Tables.Contact
            {
                Name = firstName,
                Surname = lastName,
                Email = email,
                Content = message
            };

            bool result = _contactRepository.TryAdd(modelToSave);

            return result;
        }

        public bool Delete(int messageId)
        {
            bool result = _contactRepository.TryDelete(messageId);

            return result;
        }

        public void Dispose()
        {
            _contactRepository.Dispose();
        }
    }
}