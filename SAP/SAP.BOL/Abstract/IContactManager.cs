using SAP.DAL.Tables;
using System.Collections;
using System.Collections.Generic;

namespace SAP.BOL.Abstract
{
    public interface IContactManager
    {
        IEnumerable<Contact> Contacts { get; }

        bool AddNewContact(string firstName, string lastName, string email, string message);
        bool Delete(int messageId);
        void Dispose();
    }
}