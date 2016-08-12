using System.ComponentModel.DataAnnotations;

namespace SAP.DAL.Tables.Sandbox
{
    public class Compilers
    {
        [Key]
        public int Id { get; set; }

        public int SystemId { get; set; }
        public string CompilerName { get; set; }
        public string FullPath { get; set; }
        public string Arguments { get; set; }
        public bool IsError { get; set; }
    }
}