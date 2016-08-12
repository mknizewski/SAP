using SAP.DAL.Abstract;
using SAP.DAL.DbContext;
using SAP.DAL.DbContext.SAP;
using SAP.DAL.Tables;
using System;
using System.Collections.Generic;

namespace SAP.DAL.Repositories
{
    public class NewsRepository : INewsRepository, IDisposable
    {
        private SAPDbContext _dbContext;

        public NewsRepository()
        {
            _dbContext = SAPDbContext.Create();
        }

        public IEnumerable<News> News
        {
            get
            {
                return _dbContext.News;
            }
        }

        public bool AddNews(News model)
        {
            try
            {
                _dbContext.News.Add(model);
                _dbContext.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteModel(int id)
        {
            try
            {
                var model = _dbContext.News.Find(id);

                _dbContext.News.Remove(model);
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

        public bool EditNews(int id, string description)
        {
            try
            {
                var model = _dbContext.News.Find(id);

                model.Description = description;

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