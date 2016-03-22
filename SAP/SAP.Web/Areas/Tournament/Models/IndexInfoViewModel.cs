﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAP.Web.Areas.Tournament.Models
{
    public class IndexInfoViewModel
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public List<HomeTournamentViewModel> ViewModel { get; set; }
    }
}