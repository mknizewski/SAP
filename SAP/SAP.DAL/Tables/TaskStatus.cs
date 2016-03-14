using System;
using System.ComponentModel.DataAnnotations;

namespace SAP.DAL.Tables
{
    public class TaskStatus
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime InsertTime { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}
