using SAP.DAL.Tables;
using System.Collections.Generic;

namespace SAP.DAL.Abstract
{
    public interface ICompilerRespository
    {
        IEnumerable<Compilers> Compilers { get; }

        bool Add(Compilers compiler);

        bool EditCompiler(Compilers compiler);

        bool RemoveBySystemId(int systemId);

        void Dispose();
    }
}