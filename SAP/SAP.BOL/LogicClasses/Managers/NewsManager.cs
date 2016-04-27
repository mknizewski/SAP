using SAP.BOL.Abstract;
using SAP.DAL.Abstract;
using SAP.DAL.Tables;
using System;
using System.Collections.Generic;

namespace SAP.BOL.LogicClasses.Managers
{
    public class NewsManager : INewsManager, IDisposable
    {
        private INewsRepository _newsRespository;

        public NewsManager(INewsRepository newsRepsitory)
        {
            _newsRespository = newsRepsitory;
        }

        public IEnumerable<News> News
        {
            get
            {
                return _newsRespository.News;
            }
        }

        public bool AddNews(string title, string description)
        {
            DAL.Tables.News model = new DAL.Tables.News
            {
                Title = title,
                Description = description,
                InsertTime = DateTime.Now
            };

            bool result = _newsRespository.AddNews(model);

            return result;
        }

        public bool DeleteNews(int id)
        {
            bool result = _newsRespository.DeleteModel(id);

            return result;
        }

        public void Dispose()
        {
            _newsRespository.Dispose();
            _newsRespository = null;
        }

        public bool EditNews(int id, string description)
        {
            bool result = _newsRespository.EditNews(id, description);

            return result;
        }
    }
}