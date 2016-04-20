using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InternalWebSystems.Models
{
    public class SalesOverview
    {
        public string Date { get; set; }
        public string TurnoverIncVAT { get; set; }
        public string TurnoverExVAT { get; set; }
        public string MarginExVAT { get; set; }
        public string TurnoverExVATbilled { get; set; }
        public string MarginExVATbilled { get; set; }
        public string TurnoverExVATpp { get; set; }
        public string MarginExVATp { get; set; }
        public string NewFreedomMember { get; set; }
        public string FreedomRenewal { get; set; }
        public string NewCustomers { get; set; }
        public string ExistingCustomers { get; set; }
        public string NewOrders { get; set; }
    }
}
