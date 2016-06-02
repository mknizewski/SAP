using SAP.DAL.Tables;
using System.Collections.Generic;

namespace SAP.DAL.Abstract
{
    public interface ICompilerRespository
    {
        IEnumerable<Compilers> Compilers { get; }

        bool EditPath(int systemId, string path);

        void Dispose();
    }
}