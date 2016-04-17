using SAP.DAL.Abstract;
using SAP.DAL.Tables;
using System;
using System.Collections.Generic;

namespace SAP.DAL.Repositories
{
    public class ContactRepository : IContactRepository, IDisposable
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

        #region Dispose

        public void Dispose()
        {
            context.Dispose();
        }

        #endregion Dispose
    }
}