using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.BOL.HelperClasses
{
    public class TournamentsPagination
    {
        public int TotalItems { get; set; }
        public int CurrentItems { get; set; }
        public IEnumerable<SAP.DAL.Tables.Tournament> Tournaments { get; set; }
    }
}
