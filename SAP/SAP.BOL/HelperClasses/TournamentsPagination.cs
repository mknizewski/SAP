using System.Collections.Generic;

namespace SAP.BOL.HelperClasses
{
    public class TournamentsPagination
    {
        public int TotalItems { get; set; }
        public int CurrentItems { get; set; }
        public IEnumerable<SAP.DAL.Tables.Tournament> Tournaments { get; set; }
    }
}