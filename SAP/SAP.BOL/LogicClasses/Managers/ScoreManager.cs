using SAP.BOL.Abstract;
using SAP.DAL.Abstract;
using SAP.DAL.Tables;
using System;
using System.Collections.Generic;

namespace SAP.BOL.LogicClasses.Managers
{
    public class ScoreManager : IScoreManager, IDisposable
    {
        private IScoreRepository _scoreRepository;

        public ScoreManager(IScoreRepository scoreRepository)
        {
            _scoreRepository = scoreRepository;
        }

        public IEnumerable<HistoryScores> HistoryScores
        {
            get
            {
                return _scoreRepository.HistoryScores;
            }
        }

        public IEnumerable<Scores> Scores
        {
            get
            {
                return _scoreRepository.Scores;
            }
        }

        public void Dispose()
        {
            _scoreRepository.Dispose();
            _scoreRepository = null;
        }
    }
}
