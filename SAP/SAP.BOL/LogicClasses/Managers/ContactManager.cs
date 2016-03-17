using SAP.BOL.Abstract;
using SAP.DAL.Abstract;
using System;

namespace SAP.BOL.LogicClasses.Managers
{
    public class ContactManager : IContactManager, IDisposable
    {
        private IContactRepository _contactRepository;

        public ContactManager(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
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

        public void Dispose()
        {
            _contactRepository.Dispose();
        }
    }
}