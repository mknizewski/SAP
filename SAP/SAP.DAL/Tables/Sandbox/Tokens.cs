using System;
using System.ComponentModel.DataAnnotations;

namespace SAP.DAL.Tables.Sandbox
{
    public class Tokens
    {
        [Key]
        public int Id { get; set; }

        public string Token { get; set; }

        public DateTime ExpireDate { get; set; }

        public string UserId { get; set; }
    }
}
