using System.ComponentModel.DataAnnotations;

namespace SAP.DAL.Tables
{
    public class Compilers
    {
        [Key]
        public int Id { get; set; }

        public string CompilerName { get; set; }
        public string FullPath { get; set; }
    }
}