using SAP.DAL.Tables;
using System.Collections.Generic;

namespace SAP.DAL.Abstract
{
    public interface INewsRepository
    {
        IEnumerable<News> News { get; }

        bool AddNews(News model);

        bool EditNews(int id, string description);

        bool DeleteModel(int id);

        void Dispose();
    }
}