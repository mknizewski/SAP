using SAP.DAL.Abstract;
using System.Collections.Generic;
using SAP.DAL.Tables;

namespace SAP.DAL.Repositories
{
    public class ContactRepository : IContactRepository
    {
        private DbContext.ApplicationDbContext context = new DbContext.ApplicationDbContext();

        public IEnumerable<Contact> Contact
        {
            get
            {
                return context.Contacts;
            }
        }

        public bool TryAdd(Contact model)
        {
            context.Contacts.Add(model);
            context.SaveChanges();

            return true;
        }

        public bool TryDelete(int id)
        {
            context.Contacts.Remove(context.Contacts.Find(id));
            context.SaveChanges();

            return true;
        }
    }
}
