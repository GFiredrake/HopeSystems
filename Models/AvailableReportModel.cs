using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InternalWebSystems.Models
{
    public class AvailableReportModel
    {
        public string ReportName { get; set; }
        public string ReportAction { get; set; }
        public string DepartmentId { get; set; }
    }
}
