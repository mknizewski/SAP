using System;

namespace SAP.Web.Areas.User.Models
{
    public class MessagesViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime InsertTime { get; set; }
    }
}