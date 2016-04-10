using System;

namespace SAP.Web.Areas.Admin.Models
{
    public class TodaySystemTaskViewModel
    {
        public int Id { get; set; }
        public DateTime ExecuteTime { get; set; }
        public string TypeOfTask { get; set; }
        public bool IsRealized { get; set; }
    }
}