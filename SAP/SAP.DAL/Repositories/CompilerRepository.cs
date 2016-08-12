﻿using SAP.DAL.Abstract;
using SAP.DAL.DbContext.Sandbox;
using SAP.DAL.DbContext.SAP;
using SAP.DAL.Tables;
using SAP.DAL.Tables.Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAP.DAL.Repositories
{
    public class CompilerRepository : ICompilerRespository, IDisposable
    {
        private SandboxDbContext _dbContext;

        public CompilerRepository()
        {
            _dbContext = SandboxDbContext.Create();
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

        public bool EditArgument(int systemId, string argument)
        {
            try
            {
                var compiler = _dbContext.Compilers
                    .Where(x => x.SystemId == systemId)
                    .FirstOrDefault();

                compiler.Arguments = argument;

                _dbContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
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