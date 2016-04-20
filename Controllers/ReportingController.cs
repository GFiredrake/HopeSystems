using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data;
using InternalWebSystems.Models;

namespace InternalWebSystems.Controllers
{
    public class ReportingController : Controller
    {
        public ActionResult Index()
        {
            //Repetitition need to find a way to extract
            #region Repeated Page validation and navigation controll
            bool MyCookie = IsCookiePresentAndSessionValid("HIWSSettings");

            if (MyCookie == false)
            {
                return View("Index", "Home");
            }

            HttpCookie aCookie = Request.Cookies["HIWSSettings"];
            List<string> somelist = new LogOnController().GenerateMenuForSession(aCookie["SessionGui"]);
            if (somelist != null)
            {
                StringBuilder astring = new StringBuilder();
                foreach (string buttonString in somelist)
                {
                    astring.Append(buttonString);
                }

                ViewBag.MenuHtml = astring.ToString();
            }
            #endregion
            //End Repetition 

            List<SelectListItem> obj = new List<SelectListItem>();

            obj.Add(new SelectListItem { Text = "Select Department...", Value = "0" });

            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("SPU_HT_ReturnAppropriateReportTypes"))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Connection = connection;
                    connection.Open();
                    SqlDataReader myReader = command.ExecuteReader();
                    while (myReader.Read())
                    {
                        obj.Add(new SelectListItem { Text = myReader["DepartmentName"].ToString(), Value = myReader["DepartmentId"].ToString() });
                    }

                    connection.Close();
                }
            }

            ViewBag.ReportsAvailable = obj;

            ViewBag.Title = "Reports";
            return View();
        }
        public ActionResult ReportsByDepartment(FormCollection collection)
        {
            //Repetitition need to find a way to extract
            #region Repeated Page validation and navigation controll
            bool MyCookie = IsCookiePresentAndSessionValid("HIWSSettings");

            if (MyCookie == false)
            {
                return View("Index", "Home");
            }

            HttpCookie aCookie = Request.Cookies["HIWSSettings"];
            List<string> somelist = new LogOnController().GenerateMenuForSession(aCookie["SessionGui"]);
            if (somelist != null)
            {
                StringBuilder astring = new StringBuilder();
                foreach (string buttonString in somelist)
                {
                    astring.Append(buttonString);
                }

                ViewBag.MenuHtml = astring.ToString();
            }
            #endregion
            //End Repetition 
            if (collection["ReportsAvailable"] != null && collection["ReportsAvailable"] != "0")
            {
                List<SelectListItem> obj = new List<SelectListItem>();

                obj.Add(new SelectListItem { Text = "Select Report...", Value = "0" });

                string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("SPU_HT_ReturnAppropriateReports"))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@Guid", aCookie["SessionGui"]);
                        command.Parameters.AddWithValue("@DepartmentId", collection["ReportsAvailable"].ToString());

                        command.Connection = connection;
                        connection.Open();
                        SqlDataReader myReader = command.ExecuteReader();
                        while (myReader.Read())
                        {
                            obj.Add(new SelectListItem { Text = myReader["reportname"].ToString(), Value = myReader["reportaction"].ToString() });
                        }

                        connection.Close();
                    }
                }

                ViewBag.ReportsAvailable = obj;
            }
            else
            {
                return RedirectToAction("Index", "Reporting");
            }
            

            ViewBag.Title = "Reports";
            return View();
        }
        public ActionResult DisplayReport(FormCollection collection)
        {
            var reportRequested = collection["ReportsAvailable"].ToString();
            if (reportRequested == "0")
            {
                return RedirectToAction("Index", "Reporting");
            }
            return RedirectToAction(collection["ReportsAvailable"].ToString(), "Reporting");
        }
        private bool IsCookiePresentAndSessionValid(string cookieName)
        {
            HttpCookieCollection MyCookieCollection = Request.Cookies;
            HttpCookie MyCookie = MyCookieCollection.Get(cookieName);

            if (MyCookie == null)
            {
                return false;
            }

            return new LogOnController().CheckSessionGuidIsValid(MyCookie["SessionGui"]);
        }

        //Buyer Stock Report
        public ActionResult BuyerGodReport()
        {
            //Repetitition need to find a way to extract
            #region Repeated Page validation and navigation controll
            bool MyCookie = IsCookiePresentAndSessionValid("HIWSSettings");
            ViewBag.TodaysDate = DateTime.Today.ToString("yyyy-MM-dd");
            if (MyCookie == false)
            {
                return RedirectToAction("Index", "Home");
            }

            HttpCookie aCookie = Request.Cookies["HIWSSettings"];
            List<string> somelist = new LogOnController().GenerateMenuForSession(aCookie["SessionGui"]);
            if (somelist != null)
            {
                StringBuilder astring = new StringBuilder();
                foreach (string buttonString in somelist)
                {
                    astring.Append(buttonString);
                }

                ViewBag.MenuHtml = astring.ToString();
            }
            #endregion
            //End Repetition 

            List<SelectListItem> obj = new List<SelectListItem>();

            obj.Add(new SelectListItem { Text = "", Value = "0" });

            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("SPU_HT_ReturnSuppliers"))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = connection;
                    connection.Open();
                    SqlDataReader myReader = command.ExecuteReader();
                    while (myReader.Read())
                    {
                        obj.Add(new SelectListItem { Text = myReader["suppliername"].ToString(), Value = myReader["supplierid"].ToString() });
                    }
                    connection.Close();
                }
            }

            ViewBag.SuppliersAvailable = obj.OrderBy(c => c.Text).ToList();

            return View();
        }
        public JsonResult GenerateBuyerGodGetSkus(string Variable, int IsSku, string Start, string End)
        {
            List<int> SkusToGenerate = new List<int>();

            if (IsSku == 1)
            {
                SkusToGenerate.Add(Convert.ToInt32(Variable));
            }
            if (IsSku == 2)
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SPU_HT_Reports_GetSku_By_SupplierId"))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Variable", Variable);
                        command.Connection = connection;
                        connection.Open();
                        SqlDataReader myReader = command.ExecuteReader();
                        while (myReader.Read())
                        {
                            SkusToGenerate.Add(Convert.ToInt32(myReader["parentproductsku"].ToString()));
                        }
                        connection.Close();
                    }
                }
            }
            //if (IsSku == 3)
            //{
            //    using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            //    {
            //        using (SqlCommand command = new SqlCommand("SPU_HT_Reports_GetSku_All"))
            //        {
            //            command.CommandType = CommandType.StoredProcedure;
            //            command.Connection = connection;
            //            connection.Open();
            //            SqlDataReader myReader = command.ExecuteReader();
            //            while (myReader.Read())
            //            {
            //                SkusToGenerate.Add(Convert.ToInt32(myReader["parentproductsku"].ToString()));
            //            }
            //            connection.Close();
            //        }
            //    }
            //}


            return Json(SkusToGenerate, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GenerateBuyerGodFullReportSuplierList()
        {
            List<int> SupplierList = new List<int>();

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SPU_HT_Reports_GetAllSuppliers"))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = connection;
                    connection.Open();
                    SqlDataReader myReader = command.ExecuteReader();
                    while (myReader.Read())
                    {
                        SupplierList.Add(Convert.ToInt32(myReader["supplierid"].ToString()));
                    }
                    connection.Close();
                }
            }

            return Json(SupplierList, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GenerateBuyerGodGetProductFromSku(string Variable, string Start, string End)
        {
            DateTime now = DateTime.Now;
            DateTime endEntered = Convert.ToDateTime(End.Split('-')[2] + "/" + End.Split('-')[1] + "/" + End.Split('-')[0]);

            DateTime ts = Convert.ToDateTime(Start.Split('-')[2] + "/" + Start.Split('-')[1] + "/" + Start.Split('-')[0] + " 00:00:00");
            DateTime te = new DateTime();

            if (now.Date == endEntered.Date)
            {
                te = now;
            }
            else
            {
                te = (Convert.ToDateTime(End.Split('-')[2] + "/" + End.Split('-')[1] + "/" + End.Split('-')[0] + " 00:00:00")).AddDays(1);

            }

            List<BuyerGodModel> GodReportItems = new List<BuyerGodModel>();

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SPU_HT_Reports_BuyerGod"))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        SqlParameter startDateTimeParam = new SqlParameter("@Start", SqlDbType.DateTime);
                        startDateTimeParam.Value = ts;

                        SqlParameter endDateTimeParam = new SqlParameter("@End", SqlDbType.DateTime);
                        endDateTimeParam.Value = te;

                        command.Parameters.AddWithValue("@Variable", Variable);
                        command.Parameters.Add(startDateTimeParam);
                        command.Parameters.Add(endDateTimeParam);

                        command.Connection = connection;
                        connection.Open();
                        SqlDataReader myReader = command.ExecuteReader();
                        while (myReader.Read())
                        {
                            BuyerGodModel ReportItem = new BuyerGodModel();

                            ReportItem.Sku = myReader["SKU"].ToString();
                            ReportItem.QtyFree = myReader["QtyFree"].ToString();
                            ReportItem.AwaitingDispatch = myReader["awaitingdispatch"].ToString();
                            ReportItem.TotalQtyInBins = myReader["totalqtyinbins"].ToString();
                            ReportItem.ExpDate = myReader["stockhelduntil"].ToString();
                            ReportItem.Supplier = myReader["suppliername"].ToString();
                            ReportItem.Buyer = myReader["Buyer"].ToString();
                            ReportItem.QtySold = myReader["qtysold"].ToString();
                            ReportItem.CostPrice = myReader["costprice"].ToString();
                            ReportItem.SellingPrice = myReader["SellingPrice"].ToString();
                            ReportItem.MarginPercent = myReader["Margin"].ToString();
                            ReportItem.ExVatSales = myReader["ExVatSales"].ToString();
                            ReportItem.ExVatProfit = myReader["ExVatProfit"].ToString();
                            ReportItem.ExVatSalesAllTime = myReader["ExVatSalesAllTime"].ToString();
                            ReportItem.ExVatProfitAllTime = myReader["ExVatProfitAllTime"].ToString();
                            ReportItem.LineValue = myReader["LineValue"].ToString();
                            ReportItem.LineRetailValue = myReader["LineRetailValue"].ToString();
                            ReportItem.StockAge = myReader["StockAge"].ToString();
                            ReportItem.Description = myReader["tvdescription"].ToString();
                            ReportItem.VariationName = myReader["variationname"].ToString();
                            ReportItem.PoundSlashMins = myReader["£/Min"].ToString();
                            ReportItem.PoundsPerMin = myReader["PPM"].ToString();
                            ReportItem.returnPercent = myReader["Returns"].ToString();
                            ReportItem.NumReturned = myReader["NumReturns"].ToString();

                            GodReportItems.Add(ReportItem);
                        }
                        connection.Close();
                    }
                }
            

            return Json(GodReportItems, JsonRequestBehavior.AllowGet);
        }

        //Flexi Buy Report
        public ActionResult FlexiPayReport()
        {
            //Repetitition need to find a way to extract
            #region Repeated Page validation and navigation controll
            bool MyCookie = IsCookiePresentAndSessionValid("HIWSSettings");
            ViewBag.TodaysDate = DateTime.Today.ToString("yyyy-MM-dd");
            if (MyCookie == false)
            {
                return RedirectToAction("Index", "Home");
            }

            HttpCookie aCookie = Request.Cookies["HIWSSettings"];
            List<string> somelist = new LogOnController().GenerateMenuForSession(aCookie["SessionGui"]);
            if (somelist != null)
            {
                StringBuilder astring = new StringBuilder();
                foreach (string buttonString in somelist)
                {
                    astring.Append(buttonString);
                }

                ViewBag.MenuHtml = astring.ToString();
            }
            #endregion
            //End Repetition 

            DateTime zeroTime = new DateTime(1, 1, 1);

            DateTime a = new DateTime(2014, 1, 1);
            DateTime b = new DateTime(DateTime.Now.Year, 1, 1);

            TimeSpan span = b - a;
            // because we start at year 1 for the Gregorian 
            // calendar, we must subtract a year here.
            int years = (zeroTime + span).Year - 1;
            int i = 0;
            int startyear = 2015;

            List<int> yearsList = new List<int>();

            while (i < years)
            {
                yearsList.Add(startyear);
                i++;
                startyear++;
            }
            

            List<SelectListItem> Years = new List<SelectListItem>();
            Years.Add(new SelectListItem { Text = "...", Value = "0" });
            foreach (int year in yearsList)
            {
                Years.Add(new SelectListItem { Text = year.ToString(), Value = year.ToString() });
            }

            ViewBag.YearsAvailable = Years.OrderBy(c => c.Text).ToList();
            
            return View();
        }
        public JsonResult GenerateFlexiBuyReport(int Year, int Month)
        {

            DateTime ts = Convert.ToDateTime("01/" + Month + "/" + Year + " 00:00:00");
            List<FlexiBuyModel> ReportItems = new List<FlexiBuyModel>();

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SPU_HT_Reports_FlexiBuyReport"))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    SqlParameter startDateTimeParam = new SqlParameter("@Start", SqlDbType.DateTime);
                    startDateTimeParam.Value = ts;

                    command.Parameters.Add(startDateTimeParam);



                    command.Connection = connection;
                    connection.Open();
                    SqlDataReader myReader = command.ExecuteReader();
                    while (myReader.Read())
                    {
                        FlexiBuyModel ReportItem = new FlexiBuyModel();
                            ReportItem.orderdate = myReader["orderdate"].ToString();
                            ReportItem.orderid = myReader["orderid"].ToString();
                            ReportItem.customerid = myReader["customerid"].ToString();
                            ReportItem.customername = myReader["customername"].ToString();
                            ReportItem.itemid = myReader["itemid"].ToString();
                            ReportItem.totalitem = myReader["totalitem"].ToString();
                            ReportItem.totalexvat = myReader["totalexvat"].ToString();
                            ReportItem.totalvat = myReader["totalvat"].ToString();
                            ReportItem.totalpaid = myReader["totalpaid"].ToString();
                            ReportItem.fmon1 = myReader["fmon1"].ToString();
                            ReportItem.fmon2 = myReader["fmon2"].ToString();
                            ReportItem.fmon3 = myReader["fmon3"].ToString();
                            ReportItem.fmon4 = myReader["fmon4"].ToString();
                            ReportItem.fmon5 = myReader["fmon5"].ToString();
                            ReportItem.fmon6  = myReader["fmon6"].ToString();
                            ReportItem.fmon7 = myReader["fmon7"].ToString();
                            ReportItem.fmon8 = myReader["fmon8"].ToString();
                            ReportItem.fmon9 = myReader["fmon9"].ToString();
                            ReportItem.amountcomp = myReader["AmountComp"].ToString();
                            ReportItem.tempcomp = myReader["TermComp"].ToString();

                        ReportItems.Add(ReportItem);
                    }

                    connection.Close();
                }
            }
            return Json(ReportItems, JsonRequestBehavior.AllowGet);
        }

        //Sales Overview
        public ActionResult SalesOverviewReport()
        {
            //Repetitition need to find a way to extract
            #region Repeated Page validation and navigation controll
            bool MyCookie = IsCookiePresentAndSessionValid("HIWSSettings");
            ViewBag.TodaysDate = DateTime.Today.ToString("yyyy-MM-dd");
            if (MyCookie == false)
            {
                return RedirectToAction("Index", "Home");
            }

            HttpCookie aCookie = Request.Cookies["HIWSSettings"];
            List<string> somelist = new LogOnController().GenerateMenuForSession(aCookie["SessionGui"]);
            if (somelist != null)
            {
                StringBuilder astring = new StringBuilder();
                foreach (string buttonString in somelist)
                {
                    astring.Append(buttonString);
                }

                ViewBag.MenuHtml = astring.ToString();
            }
            #endregion
            //End Repetition 

            List<SelectListItem> obj = new List<SelectListItem>();


            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("SPU_HT_GetActiveCurrency"))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = connection;
                    connection.Open();
                    SqlDataReader myReader = command.ExecuteReader();
                    while (myReader.Read())
                    {
                        obj.Add(new SelectListItem { Text = (myReader["lettercode3"].ToString() + "(" + myReader["currencysymbol"].ToString() + ")"), Value = myReader["currencyid"].ToString() });
                    }
                    connection.Close();
                }
            }

            ViewBag.CurrencyType = obj.OrderBy(c => c.Text).ToList();
            

            return View();
        }
        public JsonResult GenerateSalesOverview(int Days, int Currency)
        {
            List<SalesOverview> reportList = new List<SalesOverview>();

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SPU_HT_Reports_V2_SalesOverviewReport_SingleCurrency"))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Days", Days);
                    command.Parameters.AddWithValue("@CurrencyId", Currency);
                    command.Parameters.AddWithValue("@ChanelId", 1);
                    command.CommandTimeout = 120;

                    command.Connection = connection;
                    connection.Open();
                    SqlDataReader myReader = command.ExecuteReader();
                    while (myReader.Read())
                    {
                        SalesOverview ReportItem = new SalesOverview();

                        ReportItem.Date = myReader["startdate"].ToString().Split(' ')[0];
                        ReportItem.TurnoverIncVAT = myReader["turnoverincvat"].ToString();
                        ReportItem.TurnoverExVAT = myReader["turnoverexvat"].ToString();
                        ReportItem.MarginExVAT = myReader["marginexvat"].ToString();
                        ReportItem.TurnoverExVATbilled = myReader["turnoverexvatbilled"].ToString();
                        ReportItem.MarginExVATbilled = myReader["marginexvatbilled"].ToString();
                        ReportItem.TurnoverExVATpp = myReader["turnoverexvatpp"].ToString();
                        ReportItem.MarginExVATp = myReader["marginexvatpp"].ToString();
                        ReportItem.NewFreedomMember = myReader["newfreedommember"].ToString();
                        ReportItem.FreedomRenewal = myReader["freedomrenewal"].ToString();
                        ReportItem.NewCustomers = myReader["newcustomers"].ToString();
                        ReportItem.ExistingCustomers = myReader["existingcustomers"].ToString();
                        ReportItem.NewOrders = myReader["neworders"].ToString();

                        reportList.Add(ReportItem);
                    }
                    connection.Close();
                }
            }
            return Json(reportList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GenerateItemsSoldToday(int Currency)
        {
            List<SaleRecord> SaleRecordList = new List<SaleRecord>();

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SPU_HT_Reports_ItemsSoldToday"))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CurrencyId", Currency);

                    command.Connection = connection;
                    connection.Open();
                    SqlDataReader myReader = command.ExecuteReader();
                    while (myReader.Read())
                    {
                        SaleRecord SaleRecord = new SaleRecord();

                        SaleRecord.Sku = myReader["parentproductsku"].ToString();
                        SaleRecord.Description = myReader["tvdescription"].ToString();
                        SaleRecord.Quantity = myReader["qtysold"].ToString();
                        SaleRecord.TurnOver = myReader["salesvalue"].ToString();


                        SaleRecordList.Add(SaleRecord);
                    }

                    connection.Close();
                }
            }

            return Json(SaleRecordList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SalesOverviewReportDaily(string date)
        {
            //Repetitition need to find a way to extract
            #region Repeated Page validation and navigation controll
            bool MyCookie = IsCookiePresentAndSessionValid("HIWSSettings");
            ViewBag.TodaysDate = DateTime.Today.ToString("yyyy-MM-dd");
            if (MyCookie == false)
            {
                return RedirectToAction("Index", "Home");
            }

            HttpCookie aCookie = Request.Cookies["HIWSSettings"];
            List<string> somelist = new LogOnController().GenerateMenuForSession(aCookie["SessionGui"]);
            if (somelist != null)
            {
                StringBuilder astring = new StringBuilder();
                foreach (string buttonString in somelist)
                {
                    astring.Append(buttonString);
                }

                ViewBag.MenuHtml = astring.ToString();
            }
            #endregion
            //End Repetition 
            if (date != null)
            {
                ViewBag.WhatDate = date.ToString();
            }
            if (Request.QueryString["Days"] != null)
            {
                ViewBag.Days = Request.QueryString["Days"].ToString();
            }
            if (Request.QueryString["Currency"] != null)
            {
                ViewBag.Currency = Request.QueryString["Currency"].ToString();
            }
            

            return View();
        }
        public JsonResult GenerateSalesOverviewReportDaily(string Date, string Currency)
        {
            var DateTime = Date.Split('/')[2] + "-" + Date.Split('/')[1] + "-" + Date.Split('/')[0] + " 00:00:00";
            List<SalesOverviewDaily> reportList = new List<SalesOverviewDaily>();

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SPU_HT_Reports_SalesOverviewReport_SingleCurrency_ByHour"))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Date", DateTime);
                    command.Parameters.AddWithValue("@CurrencyId", Currency);
                    command.Parameters.AddWithValue("@ChanelId", 1);
                    command.CommandTimeout = 120;

                    command.Connection = connection;
                    connection.Open();
                    SqlDataReader myReader = command.ExecuteReader();
                    while (myReader.Read())
                    {
                        SalesOverviewDaily ReportItem = new SalesOverviewDaily();

                        ReportItem.Time = myReader["startdate"].ToString().Split(' ')[1].Split(':')[0] + ":" + myReader["startdate"].ToString().Split(' ')[1].Split(':')[1];
                        ReportItem.TurnoverIncVAT = myReader["turnoverincvat"].ToString();
                        ReportItem.TurnoverExVAT = myReader["turnoverexvat"].ToString();
                        ReportItem.MarginExVAT = myReader["marginexvat"].ToString();
                        ReportItem.TurnoverExVATbilled = myReader["turnoverexvatbilled"].ToString();
                        ReportItem.MarginExVATbilled = myReader["marginexvatbilled"].ToString();
                        ReportItem.TurnoverExVATpp = myReader["turnoverexvatpp"].ToString();
                        ReportItem.MarginExVATp = myReader["marginexvatpp"].ToString();
                        ReportItem.NewFreedomMember = myReader["newfreedommember"].ToString();
                        ReportItem.FreedomRenewal = myReader["freedomrenewal"].ToString();
                        ReportItem.NewCustomers = myReader["newcustomers"].ToString();
                        ReportItem.ExistingCustomers = myReader["existingcustomers"].ToString();
                        ReportItem.NewOrders = myReader["neworders"].ToString();

                        reportList.Add(ReportItem);
                    }
                    connection.Close();
                }
            }
            return Json(reportList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GenerateSalesOverviewReportHourly(string Time, string Currency)
        {
            List<SaleRecord> SaleRecordList = new List<SaleRecord>();

            var thisDay = DateTime.Today.ToString();

            var Datetime = thisDay.Split(' ')[0].Split('/')[2] + "-" + thisDay.Split(' ')[0].Split('/')[1] + "-" + thisDay.Split(' ')[0].Split('/')[0] + " " + Time + ":00";

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SPU_HT_Reports_ItemsSoldToday_ByHour"))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CurrencyId", Currency);
                    command.Parameters.AddWithValue("@StartDateTime", Datetime);

                    command.Connection = connection;
                    connection.Open();
                    SqlDataReader myReader = command.ExecuteReader();
                    while (myReader.Read())
                    {
                        SaleRecord SaleRecord = new SaleRecord();

                        SaleRecord.Sku = myReader["parentproductsku"].ToString();
                        SaleRecord.Description = myReader["tvdescription"].ToString();
                        SaleRecord.Quantity = myReader["qtysold"].ToString();
                        SaleRecord.TurnOver = myReader["salesvalue"].ToString();

                        SaleRecordList.Add(SaleRecord);
                    }
                    connection.Close();
                }
            }
            return Json(SaleRecordList, JsonRequestBehavior.AllowGet);
        }

        //Freedom Forecast
        public ActionResult FreedomForecastReport()
        {
            //Repetitition need to find a way to extract
            #region Repeated Page validation and navigation controll
            bool MyCookie = IsCookiePresentAndSessionValid("HIWSSettings");
            ViewBag.TodaysDate = DateTime.Today.ToString("yyyy-MM-dd");
            if (MyCookie == false)
            {
                return RedirectToAction("Index", "Home");
            }

            HttpCookie aCookie = Request.Cookies["HIWSSettings"];
            List<string> somelist = new LogOnController().GenerateMenuForSession(aCookie["SessionGui"]);
            if (somelist != null)
            {
                StringBuilder astring = new StringBuilder();
                foreach (string buttonString in somelist)
                {
                    astring.Append(buttonString);
                }

                ViewBag.MenuHtml = astring.ToString();
            }
            #endregion
            //End Repetition 

            List<FredomForecastModel> obj = new List<FredomForecastModel>();

            #region SQL Section
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("SPU_HT_Reports_FreedomForecast"))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = connection;
                    connection.Open();
                    SqlDataReader myReader = command.ExecuteReader();
                    while (myReader.Read())
                    {
                        FredomForecastModel ReportItem = new FredomForecastModel();
                        ReportItem.One = myReader["1"].ToString();
                        ReportItem.Two = myReader["2"].ToString();
                        ReportItem.Three = myReader["3"].ToString();
                        ReportItem.Four = myReader["4"].ToString();
                        ReportItem.Five = myReader["5"].ToString();
                        ReportItem.Six = myReader["6"].ToString();
                        ReportItem.Seven = myReader["7"].ToString();
                        ReportItem.Eight = myReader["8"].ToString();
                        ReportItem.Nine = myReader["9"].ToString();
                        ReportItem.Ten = myReader["10"].ToString();
                        ReportItem.Eleven = myReader["11"].ToString();
                        ReportItem.Twelve = myReader["12"].ToString();

                        obj.Add(ReportItem);
                    }
                    connection.Close();
                }
            }
            #endregion
            ViewBag.NumberOfCustomers = obj[0].One.Split('.')[0];
            ViewBag.NumberOfFreedomCustomers = obj[0].Three.Split('.')[0];
            ViewBag.FreedomMembersPercentage = obj[0].Two.Split('.')[0];
            #region Predicted Customers Row
            ViewBag.PredictedCustomers1 = obj[1].One.Split('.')[0];
            ViewBag.PredictedCustomers2 = obj[1].Two.Split('.')[0];
            ViewBag.PredictedCustomers3 = obj[1].Three.Split('.')[0];
            ViewBag.PredictedCustomers4 = obj[1].Four.Split('.')[0];
            ViewBag.PredictedCustomers5 = obj[1].Five.Split('.')[0];
            ViewBag.PredictedCustomers6 = obj[1].Six.Split('.')[0];
            ViewBag.PredictedCustomers7 = obj[1].Seven.Split('.')[0];
            ViewBag.PredictedCustomers8 = obj[1].Eight.Split('.')[0];
            ViewBag.PredictedCustomers9 = obj[1].Nine.Split('.')[0];
            ViewBag.PredictedCustomers10 = obj[1].Ten.Split('.')[0];
            ViewBag.PredictedCustomers11 = obj[1].Eleven.Split('.')[0];
            ViewBag.PredictedCustomers12 = obj[1].Twelve.Split('.')[0];
            #endregion
            #region Predicted Freedom Customers Row
            ViewBag.PredictedFreedomCustomers1 = obj[2].One.Split('.')[0];
            ViewBag.PredictedFreedomCustomers2 = obj[2].Two.Split('.')[0];
            ViewBag.PredictedFreedomCustomers3 = obj[2].Three.Split('.')[0];
            ViewBag.PredictedFreedomCustomers4 = obj[2].Four.Split('.')[0];
            ViewBag.PredictedFreedomCustomers5 = obj[2].Five.Split('.')[0];
            ViewBag.PredictedFreedomCustomers6 = obj[2].Six.Split('.')[0];
            ViewBag.PredictedFreedomCustomers7 = obj[2].Seven.Split('.')[0];
            ViewBag.PredictedFreedomCustomers8 = obj[2].Eight.Split('.')[0];
            ViewBag.PredictedFreedomCustomers9 = obj[2].Nine.Split('.')[0];
            ViewBag.PredictedFreedomCustomers10 = obj[2].Ten.Split('.')[0];
            ViewBag.PredictedFreedomCustomers11 = obj[2].Eleven.Split('.')[0];
            ViewBag.PredictedFreedomCustomers12 = obj[2].Twelve.Split('.')[0];
            #endregion
            #region Predicted Freedom Revenue Row
            ViewBag.PredictedFreedomRevenue1 = obj[3].One.Split('.')[0];
            ViewBag.PredictedFreedomRevenue2 = obj[3].Two.Split('.')[0];
            ViewBag.PredictedFreedomRevenue3 = obj[3].Three.Split('.')[0];
            ViewBag.PredictedFreedomRevenue4 = obj[3].Four.Split('.')[0];
            ViewBag.PredictedFreedomRevenue5 = obj[3].Five.Split('.')[0];
            ViewBag.PredictedFreedomRevenue6 = obj[3].Six.Split('.')[0];
            ViewBag.PredictedFreedomRevenue7 = obj[3].Seven.Split('.')[0];
            ViewBag.PredictedFreedomRevenue8 = obj[3].Eight.Split('.')[0];
            ViewBag.PredictedFreedomRevenue9 = obj[3].Nine.Split('.')[0];
            ViewBag.PredictedFreedomRevenue10 = obj[3].Ten.Split('.')[0];
            ViewBag.PredictedFreedomRevenue11 = obj[3].Eleven.Split('.')[0];
            ViewBag.PredictedFreedomRevenue12 = obj[3].Twelve.Split('.')[0];
            #endregion
            #region Predicted New Freedom Customers Row
            ViewBag.PredictedNewFreedomCustomer1 = obj[4].One.Split('.')[0];
            ViewBag.PredictedNewFreedomCustomer2 = obj[4].Two.Split('.')[0];
            ViewBag.PredictedNewFreedomCustomer3 = obj[4].Three.Split('.')[0];
            ViewBag.PredictedNewFreedomCustomer4 = obj[4].Four.Split('.')[0];
            ViewBag.PredictedNewFreedomCustomer5 = obj[4].Five.Split('.')[0];
            ViewBag.PredictedNewFreedomCustomer6 = obj[4].Six.Split('.')[0];
            ViewBag.PredictedNewFreedomCustomer7 = obj[4].Seven.Split('.')[0];
            ViewBag.PredictedNewFreedomCustomer8 = obj[4].Eight.Split('.')[0];
            ViewBag.PredictedNewFreedomCustomer9 = obj[4].Nine.Split('.')[0];
            ViewBag.PredictedNewFreedomCustomer10 = obj[4].Ten.Split('.')[0];
            ViewBag.PredictedNewFreedomCustomer11 = obj[4].Eleven.Split('.')[0];
            ViewBag.PredictedNewFreedomCustomer12 = obj[4].Twelve.Split('.')[0];
            #endregion
            #region Predicted New Freedom Customers PerDay Row
            ViewBag.PredictedNewFreedomCustomerPerDay1 = obj[5].One.Split('.')[0];
            ViewBag.PredictedNewFreedomCustomerPerDay2 = obj[5].Two.Split('.')[0];
            ViewBag.PredictedNewFreedomCustomerPerDay3 = obj[5].Three.Split('.')[0];
            ViewBag.PredictedNewFreedomCustomerPerDay4 = obj[5].Four.Split('.')[0];
            ViewBag.PredictedNewFreedomCustomerPerDay5 = obj[5].Five.Split('.')[0];
            ViewBag.PredictedNewFreedomCustomerPerDay6 = obj[5].Six.Split('.')[0];
            ViewBag.PredictedNewFreedomCustomerPerDay7 = obj[5].Seven.Split('.')[0];
            ViewBag.PredictedNewFreedomCustomerPerDay8 = obj[5].Eight.Split('.')[0];
            ViewBag.PredictedNewFreedomCustomerPerDay9 = obj[5].Nine.Split('.')[0];
            ViewBag.PredictedNewFreedomCustomerPerDay10 = obj[5].Ten.Split('.')[0];
            ViewBag.PredictedNewFreedomCustomerPerDay11 = obj[5].Eleven.Split('.')[0];
            ViewBag.PredictedNewFreedomCustomerPerDay12 = obj[5].Twelve.Split('.')[0];
            #endregion

            string[] months = new string[] { "Non", "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec", "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

            var thedate = DateTime.Now.ToString().Split('/')[1];

            if (thedate[0] == '0')
            {
                thedate = thedate.TrimStart('0');
            }
            var dateasint = Int32.Parse(thedate);

            ViewBag.MonthPlus1 = months[(dateasint + 1)];
            ViewBag.MonthPlus2 = months[(dateasint + 2)];
            ViewBag.MonthPlus3 = months[(dateasint + 3)];
            ViewBag.MonthPlus4 = months[(dateasint + 4)];
            ViewBag.MonthPlus5 = months[(dateasint + 5)];
            ViewBag.MonthPlus6 = months[(dateasint + 6)];
            ViewBag.MonthPlus7 = months[(dateasint + 7)];
            ViewBag.MonthPlus8 = months[(dateasint + 8)];
            ViewBag.MonthPlus9 = months[(dateasint + 9)];
            ViewBag.MonthPlus10 = months[(dateasint + 10)];
            ViewBag.MonthPlus11 = months[(dateasint + 11)];
            ViewBag.MonthPlus12 = months[(dateasint + 12)];

            ViewBag.month = months[dateasint];

            return View();
        }

        //Highest PnP Report
        public ActionResult HighestPnPSpend()
        {
            //Repetitition need to find a way to extract
            #region Repeated Page validation and navigation controll
            bool MyCookie = IsCookiePresentAndSessionValid("HIWSSettings");
            ViewBag.TodaysDate = DateTime.Today.ToString("yyyy-MM-dd");
            if (MyCookie == false)
            {
                return RedirectToAction("Index", "Home");
            }

            HttpCookie aCookie = Request.Cookies["HIWSSettings"];
            List<string> somelist = new LogOnController().GenerateMenuForSession(aCookie["SessionGui"]);
            if (somelist != null)
            {
                StringBuilder astring = new StringBuilder();
                foreach (string buttonString in somelist)
                {
                    astring.Append(buttonString);
                }

                ViewBag.MenuHtml = astring.ToString();
            }
            #endregion
            //End Repetition 

            //set up dropdown menu
            List<SelectListItem> obj = new List<SelectListItem>();
            obj.Add(new SelectListItem { Text = "100", Value = "100" });
            obj.Add(new SelectListItem { Text = "200", Value = "200" });
            obj.Add(new SelectListItem { Text = "500", Value = "500" });
            obj.Add(new SelectListItem { Text = "1000", Value = "1000" });

            List<SelectListItem> obj2 = new List<SelectListItem>();
            obj2.Add(new SelectListItem { Text = "1", Value = "1" });
            obj2.Add(new SelectListItem { Text = "3", Value = "3" });


            ViewBag.HowManyPeople = obj;
            ViewBag.HowManyMonths = obj2;

            return View();
        }
        public JsonResult GetPnpRecords(string NumberOfPeople, int NumberOfMonths)
        {

            List<PnpReportModel> obj = new List<PnpReportModel>();
            if (NumberOfMonths == 3)
            {
                List<PnpReportModel> obj2 = new List<PnpReportModel>();
                string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("SPU_HT_Reports_PnPSSavingsForNonFredomMembers"))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Connection = connection;
                        command.Parameters.AddWithValue("@TopNumber", NumberOfPeople);
                        connection.Open();
                        SqlDataReader myReader = command.ExecuteReader();
                        while (myReader.Read())
                        {
                            obj2.Add(new PnpReportModel
                            {
                                 FullName = myReader["Name"].ToString()
                                ,Email = myReader["Email"].ToString()
                                ,CustomerId = myReader["CustomerId"].ToString()
                                ,TotalNumberOfOrdersInThreeMonths = myReader["TotalOfOrders"].ToString()
                                ,PpMonth1IncSaving = "<span class=\"bold red\">" + myReader["currency"].ToString() + myReader["PpMonth3"].ToString() + "</span> (<span class=\"bold green\">" + myReader["currency"].ToString() + myReader["PpMonth3Saving"].ToString() + "</span>)"
                                ,PpMonth2IncSaving = "<span class=\"bold red\">" + myReader["currency"].ToString() + myReader["PpMonth2"].ToString() + "</span> (<span class=\"bold green\">" + myReader["currency"].ToString() + myReader["PpMonth2Saving"].ToString() + "</span>)"
                                ,PpMonth3IncSaving = "<span class=\"bold red\">" + myReader["currency"].ToString() + myReader["PpMonth1"].ToString() + "</span> (<span class=\"bold green\">" + myReader["currency"].ToString() + myReader["PpMonth1Saving"].ToString() + "</span>)"
                                ,TotalSavings = "<span class=\"bold green\">" + myReader["currency"].ToString() + myReader["TotalSavings"].ToString() + "</span>"
                            });
                        }
                        connection.Close();
                    }
                }
                return Json(obj2, JsonRequestBehavior.AllowGet);
            }
            if (NumberOfMonths == 1)
            {
                List<PnpReportModelOneMonth> obj3 = new List<PnpReportModelOneMonth>();
                string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("SPU_HT_Reports_PnPSSavingsForNonFredomMembers_OneMonth"))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Connection = connection;
                        command.Parameters.AddWithValue("@TopNumber", NumberOfPeople);
                        connection.Open();
                        SqlDataReader myReader = command.ExecuteReader();
                        while (myReader.Read())
                        {
                            obj3.Add(new PnpReportModelOneMonth
                            {
                                FullName = myReader["Name"].ToString()
                                ,Email = myReader["Email"].ToString()
                                ,CustomerId = myReader["CustomerId"].ToString()
                                ,TotalNumberOfOrdersInThreeMonths = myReader["TotalOfOrders"].ToString()
                                ,PpMonth1IncSaving = "<span class=\"bold red\">" + myReader["currency"].ToString() + myReader["PpMonth1"].ToString() + "</span> (<span class=\"bold green\">" + myReader["currency"].ToString() + myReader["PpMonth1Saving"].ToString() + "</span>)"
                                ,TotalSavings = "<span class=\"bold green\">" + myReader["currency"].ToString() + myReader["TotalSavings"].ToString() + "</span>"
                            });
                        }
                        connection.Close();
                    }
                }
                return Json(obj3, JsonRequestBehavior.AllowGet);
            }

            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        //Number of order by IP
        public ActionResult NumberOfOrdersByIp()
        {
            //Repetitition need to find a way to extract
            #region Repeated Page validation and navigation controll
            bool MyCookie = IsCookiePresentAndSessionValid("HIWSSettings");
            ViewBag.TodaysDate = DateTime.Today.ToString("yyyy-MM-dd");
            if (MyCookie == false)
            {
                return RedirectToAction("Index", "Home");
            }

            HttpCookie aCookie = Request.Cookies["HIWSSettings"];
            List<string> somelist = new LogOnController().GenerateMenuForSession(aCookie["SessionGui"]);
            if (somelist != null)
            {
                StringBuilder astring = new StringBuilder();
                foreach (string buttonString in somelist)
                {
                    astring.Append(buttonString);
                }

                ViewBag.MenuHtml = astring.ToString();
            }
            #endregion
            //End Repetition 

            return View();
        }
        public JsonResult GenerateOrdersByIpReport()
        {
            var obj = "";

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SPU_HT_Reports_OrdersByIp"))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = connection;
                    connection.Open();
                    SqlDataReader myReader = command.ExecuteReader();
                    while (myReader.Read())
                    {
                        
                    }
                    connection.Close();
                }
            }

            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        //PromotionalFreedomPnPSavings
        public ActionResult PromotionalFreedomPnPSavings()
        {
            //Repetitition need to find a way to extract
            #region Repeated Page validation and navigation controll
            bool MyCookie = IsCookiePresentAndSessionValid("HIWSSettings");
            ViewBag.TodaysDate = DateTime.Today.ToString("yyyy-MM-dd");
            if (MyCookie == false)
            {
                return RedirectToAction("Index", "Home");
            }

            HttpCookie aCookie = Request.Cookies["HIWSSettings"];
            List<string> somelist = new LogOnController().GenerateMenuForSession(aCookie["SessionGui"]);
            if (somelist != null)
            {
                StringBuilder astring = new StringBuilder();
                foreach (string buttonString in somelist)
                {
                    astring.Append(buttonString);
                }

                ViewBag.MenuHtml = astring.ToString();
            }
            #endregion
            //End Repetition 

            List<SelectListItem> obj = new List<SelectListItem>();


            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("SPU_HT_GetActiveCurrency"))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = connection;
                    connection.Open();
                    SqlDataReader myReader = command.ExecuteReader();
                    while (myReader.Read())
                    {
                        obj.Add(new SelectListItem { Text = (myReader["lettercode3"].ToString() + "(" + myReader["currencysymbol"].ToString() + ")"), Value = myReader["currencyid"].ToString() });
                    }
                    connection.Close();
                }
            }

            ViewBag.CurrencyType = obj.OrderBy(c => c.Text).ToList();
            ViewBag.TodaysDate = DateTime.Today.ToString("yyyy-MM-dd");

            return View();
        }
        public JsonResult GeneratePromotionalFreedomPnPSavingsReport(string StartDate, string EndDate, string CurrencyId)
        {
            List<PromotionalFreedomPnPSavingsModel> obj = new List<PromotionalFreedomPnPSavingsModel>();

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SPU_HT_Reports_GeneratePromotionalFreedomPnPSavingsReport"))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@startdate", StartDate);
                    command.Parameters.AddWithValue("@enddate", EndDate);
                    command.Parameters.AddWithValue("@CurrencyId", CurrencyId);
                    command.Connection = connection;
                    connection.Open();
                    SqlDataReader myReader = command.ExecuteReader();
                    while (myReader.Read())
                    {
                        PromotionalFreedomPnPSavingsModel ReportItem = new PromotionalFreedomPnPSavingsModel();
                        ReportItem.CustomerId = myReader["customerid"].ToString();
                        ReportItem.Saving = myReader["saving"].ToString();
                        obj.Add(ReportItem);
                    }
                    connection.Close();
                }
            }

            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        //SalesDataReport
        public ActionResult SalesDataReport()
        {
            //Repetitition need to find a way to extract
            #region Repeated Page validation and navigation controll
            bool MyCookie = IsCookiePresentAndSessionValid("HIWSSettings");
            ViewBag.TodaysDate = DateTime.Today.ToString("yyyy-MM-dd");
            if (MyCookie == false)
            {
                return RedirectToAction("Index", "Home");
            }

            HttpCookie aCookie = Request.Cookies["HIWSSettings"];
            List<string> somelist = new LogOnController().GenerateMenuForSession(aCookie["SessionGui"]);
            if (somelist != null)
            {
                StringBuilder astring = new StringBuilder();
                foreach (string buttonString in somelist)
                {
                    astring.Append(buttonString);
                }

                ViewBag.MenuHtml = astring.ToString();
            }
            #endregion
            //End Repetition 

            ViewBag.TodaysDate = DateTime.Today.ToString("yyyy-MM-dd");

            return View();
        }
        //public JsonResult GenerateSalesDataReportByDate(string StartDate, string EndDate)
        //{

        //    List<SalesDataModel> obj = new List<SalesDataModel>();

        //    using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
        //    {
        //        using (SqlCommand command = new SqlCommand("SPU_HT_Reports_GenerateSalesDataReport"))
        //        {
        //            command.CommandType = CommandType.StoredProcedure;
        //            command.Parameters.AddWithValue("@StartDate", StartDate);
        //            command.Parameters.AddWithValue("@EndDate", EndDate);
        //            command.Connection = connection;
        //            connection.Open();
        //            SqlDataReader myReader = command.ExecuteReader();
        //            while (myReader.Read())
        //            {
        //                SalesDataModel ReportItem = new SalesDataModel();
        //                ReportItem.customerid = myReader["customerid"].ToString();
        //                ReportItem.title = myReader["title"].ToString();
        //                ReportItem.firstname = myReader["firstname"].ToString();
        //                ReportItem.lastname = myReader["lastname"].ToString();
        //                ReportItem.phonenumber1 = myReader["phonenumber1"].ToString();
        //                ReportItem.emailaddress = myReader["emailaddress"].ToString();
        //                ReportItem.Addressline1 = myReader["Addressline1"].ToString();
        //                ReportItem.Addressline2 = myReader["Addressline2"].ToString();
        //                ReportItem.Town = myReader["Town"].ToString();
        //                ReportItem.County = myReader["County"].ToString();
        //                ReportItem.Postcode = myReader["Postcode"].ToString();
        //                ReportItem.IsFreedomMember = myReader["IsFreedomMember"].ToString();
        //                ReportItem.ReceiveEmail = myReader["ReceiveEmail"].ToString();
        //                ReportItem.ReceivePost = myReader["ReceivePost"].ToString();
        //                ReportItem.ReceiveSms = myReader["ReceiveSms"].ToString();
        //                ReportItem.OrderMethod = myReader["OrderMethod"].ToString();
        //                ReportItem.Source = myReader["Source"].ToString();
        //                ReportItem.Sku = myReader["Sku"].ToString();
        //                ReportItem.VariationSku = myReader["VariationSku"].ToString();
        //                ReportItem.ItemName = myReader["ItemName"].ToString();
        //                ReportItem.quantity = myReader["quantity"].ToString();
        //                ReportItem.ItemLineValue = myReader["ItemLineValue"].ToString();
        //                ReportItem.Catagory1 = myReader["Catagory1"].ToString();
        //                ReportItem.Catagory2 = myReader["Catagory2"].ToString();
        //                ReportItem.Catagory3 = myReader["Catagory3"].ToString();
        //                ReportItem.Catagory4 = myReader["Catagory4"].ToString();
        //                ReportItem.Catagory5 = myReader["Catagory5"].ToString();
        //                ReportItem.Brand = myReader["Brand"].ToString();	

        //                obj.Add(ReportItem);
        //            }
        //            connection.Close();
        //        }
        //    }

        //    return Json(obj, JsonRequestBehavior.AllowGet);
        //}
        public JsonResult GetRecordCount(string StartDate, string EndDate)
        {
            var number = "0";

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SPU_HT_Reports_SalesDataReportRecordNumbers"))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@StartDate", StartDate);
                    command.Parameters.AddWithValue("@EndDate", EndDate);
                    command.Connection = connection;
                    connection.Open();
                    SqlDataReader myReader = command.ExecuteReader();
                    while (myReader.Read())
                    {
                        number = myReader["Count"].ToString();
                    }
                    connection.Close();
                }
            }

            return Json(number, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GenerateSalesDataReportByDate2(string StartDate, string EndDate, string NumberOfReacordsToGet, string NumberOfRecordsDone)
        {
            List<SalesDataModel> obj = new List<SalesDataModel>();

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SPU_HT_Reports_GenerateSalesDataReport_MultiPart"))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@StartDate", StartDate);
                    command.Parameters.AddWithValue("@EndDate", EndDate);
                    command.Parameters.AddWithValue("@ToGet", NumberOfReacordsToGet);
                    command.Parameters.AddWithValue("@ToDelete", NumberOfRecordsDone);
                    command.Connection = connection;
                    connection.Open();
                    SqlDataReader myReader = command.ExecuteReader();
                    while (myReader.Read())
                    {
                        SalesDataModel ReportItem = new SalesDataModel();
                        ReportItem.customerid = myReader["customerid"].ToString();
                        ReportItem.title = myReader["title"].ToString();
                        ReportItem.firstname = myReader["firstname"].ToString();
                        ReportItem.lastname = myReader["lastname"].ToString();
                        ReportItem.phonenumber1 = myReader["phonenumber1"].ToString();
                        ReportItem.emailaddress = myReader["emailaddress"].ToString();
                        ReportItem.Addressline1 = myReader["Addressline1"].ToString();
                        ReportItem.Addressline2 = myReader["Addressline2"].ToString();
                        ReportItem.Town = myReader["Town"].ToString();
                        ReportItem.County = myReader["County"].ToString();
                        ReportItem.Postcode = myReader["Postcode"].ToString();
                        ReportItem.IsFreedomMember = myReader["IsFreedomMember"].ToString();
                        ReportItem.ReceiveEmail = myReader["ReceiveEmail"].ToString();
                        ReportItem.ReceivePost = myReader["ReceivePost"].ToString();
                        ReportItem.ReceiveSms = myReader["ReceiveSms"].ToString();
                        ReportItem.OrderMethod = myReader["OrderMethod"].ToString();
                        ReportItem.Source = myReader["Source"].ToString();
                        ReportItem.OrderDate = myReader["OrderDate"].ToString().Split(' ')[0];
                        ReportItem.Sku = myReader["Sku"].ToString();
                        ReportItem.VariationSku = myReader["VariationSku"].ToString();
                        ReportItem.ItemName = myReader["ItemName"].ToString();
                        ReportItem.quantity = myReader["quantity"].ToString();
                        ReportItem.ItemLineValue = myReader["ItemLineValue"].ToString();
                        ReportItem.Catagory1 = myReader["Catagory1"].ToString();
                        ReportItem.Catagory2 = myReader["Catagory2"].ToString();
                        ReportItem.Catagory3 = myReader["Catagory3"].ToString();
                        ReportItem.Catagory4 = myReader["Catagory4"].ToString();
                        ReportItem.Catagory5 = myReader["Catagory5"].ToString();
                        ReportItem.Brand = myReader["Brand"].ToString();

                        obj.Add(ReportItem);
                    }
                    connection.Close();
                }
            }

            return Json(obj, JsonRequestBehavior.AllowGet);
        }
    }
}
