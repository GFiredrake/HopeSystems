using InternalWebSystems.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace InternalWebSystems.Controllers
{
    public class OverviewController : Controller
    {
        public ActionResult Index()
        {
            //Repetitition need to find a way to extract
            #region Repeated Page validation and navigation controll
            #region Repeated Page validation and navigation controll
            bool MyCookie = IsCookiePresentAndSessionValid("HIWSSettings");

            if (MyCookie == false)
            {
                return View("index");
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
            #endregion
            //End Repetition 
            return View();
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
        public JsonResult GetPieChartData()
        {

            List<string> obj = new List<string>();


            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("SPU_HT_Overview_ReturnPieChartData"))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = connection;
                    connection.Open();
                    SqlDataReader myReader = command.ExecuteReader();
                    while (myReader.Read())
                    {
                        obj.Add(myReader["WebOrders"].ToString());
                        obj.Add(myReader["PhoneOrders"].ToString());
                        obj.Add(myReader["NewCustomers"].ToString());
                        obj.Add(myReader["OldCustomers"].ToString());
                        obj.Add(myReader["FreedomCustomers"].ToString());
                        obj.Add(myReader["NonFredomCustomers"].ToString());
                        obj.Add(myReader["WebOrders7"].ToString());
                        obj.Add(myReader["PhoneOrders7"].ToString());
                    }

                    connection.Close();
                }
            }

            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetPieChartDataComplex()
        {
            List<List<ReportVariables>> biglist = new List<List<ReportVariables>>();
            List<ReportVariables> obj = new List<ReportVariables>();
            List<ReportVariables> obj2 = new List<ReportVariables>();

            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("SPU_HT_Reports_Overview_NumberOfPostageUsed"))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = connection;
                    connection.Open();
                    SqlDataReader myReader = command.ExecuteReader();
                    while (myReader.Read())
                    {
                        ReportVariables newItem = new ReportVariables();
                        newItem.Variable1 = myReader["NumberOfOrders"].ToString();
                        newItem.Variable2 = myReader["CourierType"].ToString();
                        obj.Add(newItem);
                    }

                    connection.Close();
                }
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("SPU_HT_Reports_Overview_NumberOfPostageUsed7Day"))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = connection;
                    connection.Open();
                    SqlDataReader myReader = command.ExecuteReader();
                    while (myReader.Read())
                    {
                        ReportVariables newItem = new ReportVariables();
                        newItem.Variable1 = myReader["NumberOfOrders"].ToString();
                        newItem.Variable2 = myReader["CourierType"].ToString();
                        obj2.Add(newItem);
                    }

                    connection.Close();
                }
            }

            biglist.Add(obj);
            biglist.Add(obj2);

            return Json(biglist, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetBarChartData()
        {

            List<ReportVariables> list = new List<ReportVariables>();


            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("SPU_HT_Overview_GenerateMarginChartData"))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = connection;
                    connection.Open();
                    SqlDataReader myReader = command.ExecuteReader();
                    while (myReader.Read())
                    {
                        ReportVariables newItem = new ReportVariables();
                        newItem.Variable1 = myReader["startdate"].ToString();
                        newItem.Variable2 = myReader["turnoverexvat"].ToString();
                        newItem.Variable3 = myReader["marginexvat"].ToString();
                        list.Add(newItem);
                    }

                    connection.Close();
                }
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBarChartData6month()
        {

            List<ReportVariables> list = new List<ReportVariables>();


            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("SPU_HT_Overview_GenerateMarginChartData6Month"))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = connection;
                    connection.Open();
                    SqlDataReader myReader = command.ExecuteReader();
                    while (myReader.Read())
                    {
                        ReportVariables newItem = new ReportVariables();
                        newItem.Variable1 = myReader["startdate"].ToString();
                        newItem.Variable2 = myReader["turnoverexvat"].ToString();
                        newItem.Variable3 = myReader["marginexvat"].ToString();
                        list.Add(newItem);
                    }

                    connection.Close();
                }
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}
