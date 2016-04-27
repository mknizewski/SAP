using SAP.DAL.Tables;
using System.Collections.Generic;

namespace SAP.DAL.Abstract
{
    public interface IScoreRepository
    {
        IEnumerable<Scores> Scores { get; }
        IEnumerable<HistoryScores> HistoryScores { get; }

        void Dispose();
    }
}