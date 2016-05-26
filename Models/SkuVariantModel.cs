using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InternalWebSystems.Models
{
    public class SkuVariantModel
    {
        public string VariationId { get; set; }
        public string TvDescription { get; set; }
        public string VariationName { get; set; }
        public string FreeQty { get; set; }
    }
}
