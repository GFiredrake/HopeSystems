using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InternalWebSystems.Models
{
    public class FlexiBuyModel
    {
        public string orderdate { get; set; }
        public string orderid { get; set; }
        public string customerid { get; set; }
        public string customername { get; set; }
        public string itemid { get; set; }
        public string totalitem { get; set; }
        public string totalexvat { get; set; }
        public string totalvat { get; set; }
        public string totalpaid { get; set; }
        public string fmon1 { get; set; }
        public string fmon2 { get; set; }
        public string fmon3 { get; set; }
        public string fmon4 { get; set; }
        public string fmon5 { get; set; }
        public string fmon6 { get; set; }
        public string fmon7 { get; set; }
        public string fmon8 { get; set; }
        public string fmon9 { get; set; }
        public string amountcomp { get; set; }
        public string tempcomp { get; set; }
    }
}
