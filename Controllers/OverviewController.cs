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
        //
        // GET: /Overview/

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
                    }

                    connection.Close();
                }
            }

            return Json(obj, JsonRequestBehavior.AllowGet);
        }
    }
}
