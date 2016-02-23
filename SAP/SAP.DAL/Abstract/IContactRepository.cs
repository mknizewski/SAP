using System.Collections.Generic;
using SAP.DAL.Tables;

namespace SAP.DAL.Abstract
{
    public interface IContactRepository
    {
        IEnumerable<Contact> Contact { get; }

        bool TryAdd(Contact model);
        bool TryDelete(int id);
    }
}
