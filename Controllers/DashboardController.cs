using InternalWebSystems.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InternalWebSystems.Controllers
{
    public class DashboardController : Controller
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
            ViewBag.TodaysDate = DateTime.Today.ToString("yyyy-MM-dd");
            if (MyCookie == false)
            {
                return RedirectToAction("Index", "Home");
            }

            HttpCookie aCookie = Request.Cookies["HIWSSettings"];

            #endregion
            //End Repetition 

            return View();
        }

        public JsonResult GetDashboardInfo()
        {
            DashboardBaseModel Results = new DashboardBaseModel();

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SPU_HT_Dashboard_SingleTableCall_Main"))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = connection;

                    connection.Open();
                    SqlDataReader myReader = command.ExecuteReader();
                    while (myReader.Read())
                    {
                        Results.LastOrderTime = myReader["LastOrderTime"].ToString();
                        Results.NewOrdersToday = myReader["NewOrdersToday"].ToString();
                        Results.PercentageOfWebTrafic = myReader["PercentageOfWebTrafic"].ToString();
                        Results.PercentageOfPhoneTrafic = myReader["PercentageOfPhoneTrafic"].ToString();
                        Results.NewestCustomerCreatedByWeb = myReader["NewestCustomerCreatedByWeb"].ToString();
                        Results.NewestCustomerCreatedByPhone = myReader["NewestCustomerCreatedByPhone"].ToString();
                        Results.NumberOfItemsAwaitingPickSheet = myReader["NumberOfItemsAwaitingPickSheet"].ToString();
                        Results.OldestUnshipedWarehouseItems = myReader["OldestUnshipedWarehouseItems"].ToString();
                        Results.FreedomMembersJoined = myReader["FreedomMembersJoined"].ToString();
                        Results.FreedomMembersRenewed = myReader["FreedomMembersRenewed"].ToString();
                        Results.NewCustomersToday = myReader["NewCustomersToday"].ToString();
                        Results.WebVsPhoneOver7Days = myReader["WebVsPhoneOver7Days"].ToString(); 
                    }
                    connection.Close();
                }
            }

            DateTime dt = Convert.ToDateTime(Results.OldestUnshipedWarehouseItems);
            var unshipeddays = (DateTime.Now - dt).Days;
            Results.OldestUnshipedWarehouseItems = unshipeddays.ToString();

            return Json(Results, JsonRequestBehavior.AllowGet);
        }

    }
}
