using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAP.DAL.Tables
{
    [Table("_logs_exception")]
    public class Exceptions
    {
        [Key]
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Message { get; set; }

        public string Type { get; set; }

        public string StackTrace { get; set; }

        public string InnerException { get; set; }

        public string Source { get; set; }

        public DateTime InsertTime { get; set; }

    }
}
