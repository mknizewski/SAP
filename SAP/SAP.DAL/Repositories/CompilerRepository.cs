using SAP.DAL.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using SAP.DAL.Tables;
using SAP.DAL.DbContext;

namespace SAP.DAL.Repositories
{
    public class CompilerRepository : ICompilerRespository, IDisposable
    {
        private ApplicationDbContext _dbContext;

        public CompilerRepository()
        {
            _dbContext = new ApplicationDbContext();
        }

        public IEnumerable<Compilers> Compilers
        {
            get
            {
                return _dbContext.Compilers;
            }
        }

        public bool Add(Compilers compiler)
        {
            try
            {
                _dbContext.Compilers.Add(compiler);
                _dbContext.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Dispose()
        {
            _dbContext.Dispose();
            _dbContext = null;
        }

        public bool EditCompiler(Compilers compiler)
        {
            try
            {
                var dbItem = _dbContext.Compilers.Find(compiler.Id);

                dbItem = compiler;
                _dbContext.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool RemoveBySystemId(int systemId)
        {
            try
            {
                var modelToRemove = _dbContext.Compilers
                    .Select(x => x)
                    .Where(x => x.SystemId == systemId)
                    .FirstOrDefault();

                _dbContext.Compilers.Remove(modelToRemove);
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
