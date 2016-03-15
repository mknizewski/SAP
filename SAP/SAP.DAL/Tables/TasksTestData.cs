using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAP.DAL.Tables
{
    public class TasksTestData
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Task")]
        public int TaskId { get; set; }
        public string InputData { get; set; }
        public string OutputData { get; set; }
        public string ExampleProgram { get; set; }

        public virtual Tasks Task { get; set; }
    }
}