using SAP.DAL.Tables;
using System.Collections.Generic;

namespace SAP.BOL.Abstract
{
    public interface IScoreManager
    {
        IEnumerable<Scores> Scores { get; }
        IEnumerable<HistoryScores> HistoryScores { get; }

        void Dispose();
    }
}