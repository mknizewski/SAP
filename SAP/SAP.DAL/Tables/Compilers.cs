using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
