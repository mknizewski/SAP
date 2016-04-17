using System;
using System.Collections.Generic;

namespace SAP.Web.Models
{
    public class NewsViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime InsertTime { get; set; }
    }

    public class InfoNewsViewModel
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public List<NewsViewModel> News { get; set; }
    }
}