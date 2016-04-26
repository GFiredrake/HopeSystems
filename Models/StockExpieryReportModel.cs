using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InternalWebSystems.Models
{
    public class StockExpieryReportsModel
    {
        public string Sku { get; set; }
        public string Description { get; set; }
        public string Date { get; set; }
        public string PastOrFuture { get; set; }
    }
}
