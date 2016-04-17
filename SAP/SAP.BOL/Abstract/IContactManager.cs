namespace SAP.BOL.Abstract
{
    public interface IContactManager
    {
        bool AddNewContact(string firstName, string lastName, string email, string message);

        void Dispose();
    }
}