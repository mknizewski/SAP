using SAP.DAL.Abstract;
using SAP.DAL.DbContext.SAP;
using SAP.DAL.Tables;
using System;
using System.Collections.Generic;

namespace SAP.DAL.Repositories
{
    public class ContactRepository : IContactRepository, IDisposable
    {
        private SAPDbContext _context;

        public ContactRepository()
        {
            _context = SAPDbContext.Create();
        }

        public IEnumerable<Contact> Contact
        {
            get
            {
                return _context.Contacts;
            }
        }

        public bool TryAdd(Contact model)
        {
            _context.Contacts.Add(model);
            _context.SaveChanges();

            return true;
        }

        public bool TryDelete(int id)
        {
            _context.Contacts.Remove(_context.Contacts.Find(id));
            _context.SaveChanges();

            return true;
        }

        #region Dispose

        public void Dispose()
        {
            _context.Dispose();
        }

        #endregion Dispose
    }
}