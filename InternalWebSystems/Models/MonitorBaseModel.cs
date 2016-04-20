using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InternalWebSystems.Models
{
    public class MonitorBaseModel
    {
        public string NewOrdersToday { get; set; }
        public string LastOrderTime { get; set; }
        public string NumberOfCustomers { get; set; }
        public string NewestCustomerCreated { get; set; }
        public string FlexibuyHasEnoughEntrys { get; set; }
        public string FlexibuyHasToManyEntrys { get; set; }
        public string OldestUnshipedWarehouseItems { get; set; }
        public string NumberOfUnshipableWarehouseItems { get; set; }
        public string NumberOfItemsSuckOnFraudCheck { get; set; }
        public string NumberOfItemsAwaitingPickSheet { get; set; }
        public string PercentageOfWebTrafic { get; set; }
        public string LabelerLastRun { get; set; }
        public string PaymentAppLastRun { get; set; }
        public string FreedomWithoutRenwals { get; set; }
        public string PaymentsAsPaidLastRun { get; set; }
        public string NewestCustomerCreatedByPhone { get; set; }
    }
}
