﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAP.DAL.Tables
{
    public class TournamentUsers
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Tournament")]
        public int TournamentId { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        public virtual Tournament Tournament { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}