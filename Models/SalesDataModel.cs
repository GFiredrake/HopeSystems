using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InternalWebSystems.Models
{
    public class SalesDataModel 
    {
        public string customerid { get; set; }	
        public string title { get; set; }
        public string firstname { get; set; }	
        public string lastname { get; set; }	
        public string phonenumber1 { get; set; }	
        public string emailaddress { get; set; }	
        public string Addressline1 { get; set; }	
        public string Addressline2 { get; set; }	
        public string Town { get; set; }	
        public string County { get; set; }	
        public string Postcode { get; set; }	
        public string IsFreedomMember { get; set; }	
        public string ReceiveEmail { get; set; }	
        public string ReceivePost { get; set; }	
        public string ReceiveSms { get; set; }	
        public string OrderMethod { get; set; }	
        public string Source { get; set; }
        public string OrderDate { get; set; }	
        public string Sku { get; set; }	
        public string VariationSku { get; set; }	
        public string ItemName { get; set; }	
        public string quantity { get; set; }	
        public string ItemLineValue { get; set; }	
        public string Catagory1 { get; set; }	
        public string Catagory2 { get; set; }	
        public string Catagory3 { get; set; }	
        public string Catagory4 { get; set; }	
        public string Catagory5 { get; set; }
        public string Brand { get; set; }	
    }
}
