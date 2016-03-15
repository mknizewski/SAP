using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAP.DAL.Tables
{
    public class Messages
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime SendTime { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
