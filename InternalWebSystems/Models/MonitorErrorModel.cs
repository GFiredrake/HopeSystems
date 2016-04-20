using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InternalWebSystems.Models
{
    public class MonitorErrorModel
    {
        public int WasError { get; set; }

        public int DidFlexiError { get; set; }
        public int FlexiErrorCount { get; set; }

        public int DidLabelerError { get; set; }
        public int LabelerErrorCount { get; set; }

        public int DidPaymentApp { get; set; }
        public int PaymentAppCount { get; set; }

        public int DidFreedomError { get; set; }
        public int FreedomErrorCount { get; set; }

        public int DidPaymentAsPaidError { get; set; }
        public int PaymentAsPaidErrorCount { get; set; }
    }
}
