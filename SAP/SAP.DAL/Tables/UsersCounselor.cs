﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.DAL.Tables
{
    public class UsersCounselor
    {
        [Key]
        public int Id { get; set; }
        [Key, ForeignKey("User")]
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
