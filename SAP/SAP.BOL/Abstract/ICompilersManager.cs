using SAP.DAL.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.BOL.Abstract
{
    public interface ICompilersManager
    {
        IEnumerable<Compilers> Compilers { get; }
        bool EditPath(int systemId, string path);
        void Dispose();
    }
}
