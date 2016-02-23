using SAP.BOL.Abstract;
using SAP.DAL.Abstract;

namespace SAP.BOL.LogicClasses
{
    public class Contact : IContact
    {
        private IContactRepository _contactRepository;

        public Contact(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public bool AddNewContact(string firstName, string lastName, string email, string message)
        {
            DAL.Tables.Contact  modelToSave = new DAL.Tables.Contact
            {
                Name = firstName,
                Surname = lastName,
                Email = email,
                Content = message
            };

            bool result = _contactRepository.TryAdd(modelToSave);

            return result;
        }
    }
}
