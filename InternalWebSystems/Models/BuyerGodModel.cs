using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InternalWebSystems.Models
{
    public class BuyerGodModel
    {
        public string Sku { get; set; }
        public string Description { get; set; }
        public string VariationName { get; set; }
        public string TotalQtyInBins { get; set; }
        public string AwaitingDispatch { get; set; }
        public string QtyFree { get; set; }
        public string QtySold { get; set; }
        public string CostPrice { get; set; }
        public string LineValue { get; set; }
        public string SellingPrice { get; set; }
        public string LineRetailValue { get; set; }
        public string ExpDate { get; set; }
        public string StockAge { get; set; }
        public string Supplier { get; set; }
        public string Buyer { get; set; }
        public string ExVatSales { get; set; }
        public string ExVatProfit { get; set; }
        public string ExVatSalesAllTime { get; set; }
        public string ExVatProfitAllTime { get; set; }
        public string MarginPercent { get; set; }
        public string PoundSlashMins { get; set; }
        public string PoundsPerMin { get; set; }
        public string returnPercent { get; set; }
        public string NumReturned { get; set; }
    }
}
