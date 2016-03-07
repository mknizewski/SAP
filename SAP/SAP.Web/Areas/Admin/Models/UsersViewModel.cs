﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAP.Web.Areas.Admin.Models
{
    public class UsersViewModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}