using System.ComponentModel.DataAnnotations;

namespace SAP.DAL.Tables
{
    public class ServerSettings
    {
        [Key]
        public int Id { get; set; }

        public string EmailLogin { get; set; }

        public string EmailPassword { get; set; }

        public string EmailSMTP { get; set; }
    }
}
