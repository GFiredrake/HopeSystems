using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InternalWebSystems.Models
{
    public class PnpReportModelOneMonth
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string CustomerId { get; set; }
        public string TotalNumberOfOrdersInThreeMonths { get; set; }
        public string PpMonth1IncSaving { get; set; }
        public string TotalSavings { get; set; }
    }
}
