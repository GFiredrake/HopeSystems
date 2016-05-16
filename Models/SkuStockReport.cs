using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InternalWebSystems.Models
{
    public class SkuStockReport
    {
        public string VariationProductSku { get; set; }
        public string TvDescription { get; set; }
        public string VariationName { get; set; }
        public string FreeQty { get; set; }
        public string SupplierName { get; set; }
        public string BuyerName { get; set; }
    }
}
