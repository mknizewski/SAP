using System;
using System.ComponentModel.DataAnnotations;

namespace SAP.DAL.Tables
{
    public class News
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime InsertTime { get; set; }
    }
}
