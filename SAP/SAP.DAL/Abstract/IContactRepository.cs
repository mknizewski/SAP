using SAP.DAL.Tables;
using System.Collections.Generic;

namespace SAP.DAL.Abstract
{
    public interface IContactRepository
    {
        IEnumerable<Contact> Contact { get; }

        bool TryAdd(Contact model);

        bool TryDelete(int id);

        void Dispose();
    }
}