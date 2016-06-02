using SAP.DAL.Tables;
using System.Collections.Generic;

namespace SAP.BOL.Abstract
{
    public interface ICompilersManager
    {
        IEnumerable<Compilers> Compilers { get; }

        bool EditPath(int systemId, string path);

        void Dispose();
    }
}