using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Services;
using System.Web.Services;
using Mandrill;
using Mandrill.Model;

namespace InternalWebSystems.Controllers
{
    public class CustomerServiceController : Controller
    {
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

        public ActionResult Index()
        {
            //Repetitition need to find a way to extract
            #region Repeated Page validation and navigation controll
            bool MyCookie = IsCookiePresentAndSessionValid("HIWSSettings");

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

        public ActionResult ResendComformation(bool? error, bool? sucess)
        {
            //Repetitition need to find a way to extract
            #region Repeated Page validation and navigation controll
            bool MyCookie = IsCookiePresentAndSessionValid("HIWSSettings");

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
            if (error != null)
            {
                ViewBag.HtmlStr = "<Div id=\"failureDiv\">The supplied OrderId was not valid.</div>";
            }
            if (sucess != null)
            {
                ViewBag.HtmlStr = "<Div id=\"successDiv\">Confirmation Email Sent.</div>";
            }

            return View();
        }

        public ActionResult ResendComfomationEmail(int OrderIdInput)
        {
            var orderCount = "99";
            

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SPU_HT_Customer_Service_Is_Valid_OrderId"))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@OrderId", OrderIdInput);

                    command.Connection = connection;
                    connection.Open();
                    SqlDataReader myReader = command.ExecuteReader();
                    while (myReader.Read())
                    {
                        orderCount = myReader["OrderCount"].ToString();

                    }

                    connection.Close();
                }
            }

            if (orderCount == "0" || orderCount == "99")
            {
                return RedirectToAction("ResendComformation", "CustomerService", new { error = true });
            }


            var api = new MandrillApi("mwikOWlc9HLroBSaEpQkhw");
            var message = new MandrillMessage();

            var title = "";
            var firstName = "";
            var customerId = "";
            var doornumber = "";
            var address1 = "";
            var address2 = "";
            var townCity = "";
            var county = "";
            var postcode = "";
            var orderDateTime = "";
            var paymentType = "";
            var tPrice = "";
            var dPrice = "";
            var customerEmail = "";


            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SPU_HT_Customer_Service_Resend_Email_Info"))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@OrderId", OrderIdInput);

                    command.Connection = connection;
                    connection.Open();
                    SqlDataReader myReader = command.ExecuteReader();
                    while (myReader.Read())
                    {
                        title = myReader["title"].ToString();
                        firstName = myReader["firstname"].ToString();
                        customerId = myReader["customerid"].ToString();
                        doornumber = myReader["buildingnumber"].ToString();
                        address1 = myReader["streetaddress1"].ToString();
                        address2 = myReader["streetaddress2"].ToString();
                        townCity = myReader["TownCity"].ToString();
                        county = myReader["county"].ToString();
                        postcode = myReader["postcode"].ToString();
                        orderDateTime = myReader["orderdate"].ToString();
                        paymentType = myReader["paymentmethod"].ToString();
                        tPrice = myReader["TotalValue"].ToString();
                        dPrice = myReader["DeliveryValue"].ToString();
                        customerEmail = myReader["emailaddress"].ToString();
                    }

                    connection.Close();
                }
            }

            var formatedTime = ManipulateDateTime(orderDateTime);
            var formaterStreetAddress1 = doornumber + "," + address1;
            var formatedPaymentType = "Card";
            var formatedorders = ManipulateOrderItems(OrderIdInput);

            //------populating template email------------//
            message.FromEmail = "customer.service@hochanda.com";
            message.AddTo(customerEmail);
            message.AddTo("customer.service@hochanda.com");
            //message.AddTo("robin.windon@hochanda.com"); // testing to see if it comes through
            message.ReplyTo = "customer.service@hochanda.com";
            message.Subject = "Hochanda Order Confirmation: " + "#" + OrderIdInput;
            message.AddGlobalMergeVars("customer-invoice", DateTime.Now.ToShortDateString());


            //-------populate email for sending-----------//
            message.AddGlobalMergeVars("TITLE", title);
            message.AddGlobalMergeVars("FIRSTNAME", firstName);
            message.AddGlobalMergeVars("ORDERNUMBER", OrderIdInput.ToString());
            message.AddGlobalMergeVars("ACCOUNTID", customerId);
            message.AddGlobalMergeVars("ADDRESSNUMBER", formaterStreetAddress1);
            message.AddGlobalMergeVars("ADDRESSLINE2", address2);
            message.AddGlobalMergeVars("TOWNCITY", townCity);
            message.AddGlobalMergeVars("COUNTY", county);
            message.AddGlobalMergeVars("POSTCODE", postcode);
            message.AddGlobalMergeVars("ACCOUNTLINK", "http://www.hochanda.com/MyAccount.aspx");
            message.AddGlobalMergeVars("ORDERITEMS", formatedorders);
            message.AddGlobalMergeVars("ORDERTIMESTAMP", formatedTime);
            message.AddGlobalMergeVars("PAYMENTTYPE", formatedPaymentType);
            message.AddGlobalMergeVars("TOTALPRICE", tPrice);
            message.AddGlobalMergeVars("DELIVERYPRICE", dPrice);
            var rs = api.Messages.SendTemplate(message, "customer-invoice-success");

            return RedirectToAction("ResendComformation", "CustomerService", new { sucess = true });
        }

        private string ManipulateDateTime (string Datetime)
        {
            List<string> months = new List<string>() { "nothing", "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            var day = Datetime.Split('/')[0];
            if (day[0].ToString() == "0")
            {
                day = day.Substring(1);
            }
            var dayFollower = "th";
            if(Int32.Parse(day) == 1 || Int32.Parse(day) == 21 || Int32.Parse(day) == 31)
            {
                dayFollower = "st";
            }
            if(Int32.Parse(day) == 2 || Int32.Parse(day) == 22)
            {
                dayFollower = "nd";
            }
            if(Int32.Parse(day) == 3 || Int32.Parse(day) == 23)
            {
                dayFollower = "rd";
            }
            var month = months[Int32.Parse(Datetime.Split('/')[1])];
            var year = Datetime.Split('/')[2].Split(' ')[0];
            var hour = Datetime.Split('/')[2].Split(' ')[1].Split(':')[0];
            var amOrPm = "AM";
            if (Int32.Parse(hour) > 12)
            {
                hour = (Int32.Parse(hour) - 12).ToString();
                amOrPm = "PM";
            }
            var minuets = Datetime.Split('/')[2].Split(' ')[1].Split(':')[1];
            var time = hour + ":" + minuets;





            return day + dayFollower + " " + month + " " + year + " at " + time + " " + amOrPm;
        }
        private string ManipulateOrderItems(int OrderId)
        {

            var modifiedString = "";

            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("SPU_HT_Customer_Service_Get_Items_By_OrderId"))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@OrderId", OrderId);
                    command.Connection = connection;
                    connection.Open();
                    SqlDataReader myReader = command.ExecuteReader();
                    while (myReader.Read())
                    {
                        modifiedString = modifiedString + "<tr><td><img src='https:////www.hoch.media//product-images//main//" + myReader["imagefilename"].ToString() + "' height='50' width='50'/></td><td style='color:#ffffff; font-family:Verdana, Geneva, sans-serif; font-size:12px;'>" + myReader["tvdescription"].ToString() + "  (" + myReader["parentproductsku"].ToString() + ")</td><td style='color:#ffffff; font-family:Verdana, Geneva, sans-serif; font-size:12px;'>" + "QTY:&nbsp;" + myReader["quantity"].ToString() + "</td><td style='color:#ffffff; font-family:Verdana, Geneva, sans-serif; font-size:12px;'>" + "&nbsp;" + myReader["itemprice"].ToString() + "</td></tr><tr><td colspan='4'><hr style=' border-top:1px dashed #ffffff; border-bottom:none;'></td></tr><tr>";
                    }
                    connection.Close();
                }
            }




            return modifiedString;
        }
    }
}
