using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InternalWebSystems.Models
{
    public class Report
    {
        public string Name { get; set; }
        public string Action { get; set; }
        public int PermisionLevel { get; set; }
    }
}
