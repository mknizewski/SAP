using SAP.DAL.Tables;
using System.Collections.Generic;

namespace SAP.BOL.Abstract
{
    public interface INewsManager
    {
        IEnumerable<News> News { get; }
        bool AddNews(string title, string description);
        bool EditNews(int id, string description);
        bool DeleteNews(int id);
        void Dispose();
    }
}
