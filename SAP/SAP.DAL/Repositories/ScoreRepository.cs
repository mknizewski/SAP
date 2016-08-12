using SAP.DAL.Abstract;
using SAP.DAL.DbContext;
using SAP.DAL.DbContext.SAP;
using SAP.DAL.Tables;
using System;
using System.Collections.Generic;

namespace SAP.DAL.Repositories
{
    public class ScoreRepository : IScoreRepository, IDisposable
    {
        private SAPDbContext _dbContext;

        public ScoreRepository()
        {
            _dbContext = SAPDbContext.Create();
        }

        public IEnumerable<HistoryScores> HistoryScores
        {
            get
            {
                return _dbContext.HistoryScores;
            }
        }

        public IEnumerable<Scores> Scores
        {
            get
            {
                return _dbContext.Scores;
            }
        }

        public void Dispose()
        {
            _dbContext.Dispose();
            _dbContext = null;
        }
    }
}