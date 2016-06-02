using SAP.DAL.Abstract;
using SAP.DAL.DbContext;
using SAP.DAL.Tables;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAP.DAL.Repositories
{
    public class CompilerRepository : ICompilerRespository, IDisposable
    {
        private ApplicationDbContext _dbContext;

        public CompilerRepository()
        {
            _dbContext = ApplicationDbContext.Create();
        }

        public IEnumerable<Compilers> Compilers
        {
            get
            {
                return _dbContext.Compilers;
            }
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public bool EditPath(int systemId, string path)
        {
            try
            {
                var compiler = _dbContext.Compilers
                    .Where(x => x.SystemId == systemId)
                    .FirstOrDefault();

                compiler.FullPath = path;

                _dbContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}