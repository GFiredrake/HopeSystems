using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InternalWebSystems.Controllers
{
    public class DashboardBaseModel
    {
        public string LastOrderTime { get; set; }
        public string NewOrdersToday { get; set; }
        public string PercentageOfWebTrafic { get; set; }
        public string PercentageOfPhoneTrafic { get; set; }//New
        public string NewestCustomerCreatedByWeb { get; set; }//Web
        public string NewestCustomerCreatedByPhone { get; set; }
        public string NumberOfItemsAwaitingPickSheet { get; set; }
        public string OldestUnshipedWarehouseItems { get; set; }
        public string FreedomMembersJoined { get; set; }//New
        public string FreedomMembersRenewed { get; set; }//New
        public string NewCustomersToday { get; set; }//New
        public string WebVsPhoneOver7Days { get; set; }//New
    }
}
