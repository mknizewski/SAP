using SAP.BOL.Abstract;
using SAP.DAL.Abstract;
using SAP.DAL.Tables;
using System;
using System.Collections.Generic;

namespace SAP.BOL.LogicClasses.Managers
{
    public class CompilersManager : ICompilersManager, IDisposable
    {
        private ICompilerRespository _compilerRepository;

        public CompilersManager(ICompilerRespository compilerRepository)
        {
            _compilerRepository = compilerRepository;
        }

        public IEnumerable<Compilers> Compilers
        {
            get
            {
                return _compilerRepository.Compilers;
            }
        }

        public void Dispose()
        {
            _compilerRepository.Dispose();
        }

        public bool EditArguments(int systemId, string argument)
        {
            bool result = _compilerRepository.EditArgument(systemId, argument);

            return result;
        }

        public bool EditPath(int systemId, string path)
        {
            bool result = _compilerRepository.EditPath(systemId, path);

            return result;
        }
    }
}