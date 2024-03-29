﻿using InternalWebSystems.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Media;
using System.Text;
using System.Timers;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Net.Mail;
using SendGrid;

namespace InternalWebSystems.Controllers
{
    public class MonitoringController : Controller
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

            //SoundPlayer simpleSound = new SoundPlayer("C:\\Dev\\InternalWebSystems\\InternalWebSystems\\Content\\Sounds\\greatscott3.wav");
            //simpleSound.Play();

            return View();
        }

        public JsonResult GetMonitorInfo()
        {
            MonitorBaseModel Results = new MonitorBaseModel();

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SPU_HT_Monitor_SingleTableCall_Main"))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = connection;

                    connection.Open();
                    SqlDataReader myReader = command.ExecuteReader();
                    while (myReader.Read())
                    {
                        Results.NewOrdersToday = myReader["NewOrdersToday"].ToString();
                        Results.LastOrderTime = myReader["LastOrderTime"].ToString();
                        Results.NumberOfCustomers = myReader["NumberOfCustomers"].ToString();
                        Results.NewestCustomerCreated = myReader["NewestCustomerCreated"].ToString();
                        Results.FlexibuyHasEnoughEntrys = myReader["FlexibuyHasEnoughEntrys"].ToString();
                        Results.FlexibuyHasToManyEntrys = myReader["FlexibuyHasToManyEntrys"].ToString();
                        Results.OldestUnshipedWarehouseItems = myReader["OldestUnshipedWarehouseItems"].ToString();
                        Results.NumberOfUnshipableWarehouseItems = myReader["NumberOfUnshipableWarehouseItems"].ToString();
                        Results.NumberOfItemsSuckOnFraudCheck = myReader["NumberOfItemsSuckOnFraudCheck"].ToString();
                        Results.NumberOfItemsAwaitingPickSheet = myReader["NumberOfItemsAwaitingPickSheet"].ToString();
                        Results.PercentageOfWebTrafic = myReader["PercentageOfWebTrafic"].ToString();
                        Results.LabelerLastRun = myReader["LabelerLastRun"].ToString();
                        Results.PaymentAppLastRun = myReader["PaymentAppLastRun"].ToString();
                        //Results.FreedomWithoutRenwals = myReader["FreedomWithoutRenwals"].ToString();
                        Results.PaymentsAsPaidLastRun = myReader["PaymentsAsPaidLastRun"].ToString();
                        Results.NewestCustomerCreatedByPhone = myReader["NewestCustomerCreatedByPhone"].ToString();
                        Results.PotentialDuplicateOrders = myReader["PotentialDuplicateOrders"].ToString();
                    }
                    connection.Close();
                }
            }
            DateTime dt = Convert.ToDateTime(Results.OldestUnshipedWarehouseItems);
            var unshipeddays = ( DateTime.Now - dt).Days;
            Results.OldestUnshipedWarehouseItems = unshipeddays.ToString();


            return Json(Results, JsonRequestBehavior.AllowGet);
        }

        public void RecordAndFixBrokenFlexiBuyRecords()
        {
            //record and fix broken reacords
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            {
                
                using (SqlCommand command = new SqlCommand("SPU_HT_Monitoring_RecordFlexiErrors"))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = connection;

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            
                using (SqlCommand command = new SqlCommand("SPU_Apps_Admin_HT_FlexiBuyMissedInserts"))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = connection;

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public JsonResult GetFlexiErrorsNumber()
        {
            var obj = 0;

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SPU_HT_Monitor_FlexiErrorsToday"))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = connection;

                    connection.Open();
                    SqlDataReader myReader = command.ExecuteReader();
                    while (myReader.Read())
                    {
                        obj = Int32.Parse(myReader["Errors"].ToString());
                    }
                    connection.Close();
                }
            }
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMonitorAlerts(int FlexiUnder, int LablerRun, int PaymentAppRun, int PaymentAsRunRun, int flexiErrorCount, int labelerErrorCount, int paymentAppCount,
            int paymentAsPaidErrorCount, bool isMainMonitor, int potentialDuplicateOrderCount, int potentialDuplicateOrder)
        {
            MonitorErrorModel Error = new MonitorErrorModel { 
              WasError = 0, DidFlexiError = 0, DidLabelerError = 0, DidPaymentApp = 0, DidFreedomError = 0, DidPaymentAsPaidError  = 0
            , FlexiErrorCount = flexiErrorCount, LabelerErrorCount = labelerErrorCount, PaymentAppCount = paymentAppCount, FreedomErrorCount = 0, PaymentAsPaidErrorCount = paymentAsPaidErrorCount
            };
            
            TimeSpan start = new TimeSpan(08, 00, 00); //8am o'clock
            TimeSpan end = new TimeSpan(20, 00, 00); //8pm o'clock
            TimeSpan now = DateTime.Now.TimeOfDay;

            if ((now > start) && (now < end))
            {
                if (potentialDuplicateOrderCount >= 1 )
                {
                    //if (potentialDuplicateOrderCount > potentialDuplicateOrder)
                    //{
                    //    Error.WasError = 1;
                        
                    //}
                    Error.NewDuplicateOrdersCount = potentialDuplicateOrderCount;
                    Error.DidDuplicateOrdersError = 1;
                }

                if (FlexiUnder >= 1)
                {
                    //Error.WasError = 1;
                    Error.DidFlexiError = 1;
                    Error.FlexiErrorCount = flexiErrorCount + 1;
                }

                if (LablerRun > 80)
                {
                    Error.WasError = 1;
                    Error.DidLabelerError = 1;
                    Error.LabelerErrorCount = labelerErrorCount + 1;
                }

                if (PaymentAppRun > 90)
                {
                    Error.WasError = 1;
                    Error.DidPaymentApp = 1;
                    Error.PaymentAppCount = paymentAppCount + 1;
                }

                if (PaymentAsRunRun > 1)
                {
                    Error.WasError = 1;
                    Error.DidPaymentAsPaidError = 1;
                    Error.PaymentAsPaidErrorCount = paymentAsPaidErrorCount + 1;
                }

            }
            else
            {
                if (FlexiUnder >= 1 && flexiErrorCount < 2 && isMainMonitor == true)
                    {
                        var myMessage = new SendGrid.SendGridMessage();
                        myMessage.AddTo("web@hochanda.com");
                        myMessage.From = new MailAddress("hope.tools@hochanda.com", "Hope Tools");
                        myMessage.Subject = "There has been an Error";
                        myMessage.Text = "There have been flexibuy orders made and no records have been created for them in Payment_Schedule.";
                        var transportWeb = new Web(ConfigurationManager.AppSettings["SendGridApi"].ToString());
                        transportWeb.DeliverAsync(myMessage);
                        Error.WasError = 1;
                        Error.DidFlexiError = 1;
                        Error.FlexiErrorCount = flexiErrorCount + 1;
                    }

                if (LablerRun > 80 && labelerErrorCount < 2 && isMainMonitor == true)
                    {
                        var myMessage = new SendGrid.SendGridMessage();
                        myMessage.AddTo("web@hochanda.com");
                        myMessage.From = new MailAddress("hope.tools@hochanda.com", "Hope Tools");
                        myMessage.Subject = "There has been an Error";
                        myMessage.Text = "The labeler has not run in over an hour and twenty Minutes.";
                        var transportWeb = new Web(ConfigurationManager.AppSettings["SendGridApi"].ToString());
                        transportWeb.DeliverAsync(myMessage);
                        Error.WasError = 1;
                        Error.DidLabelerError = 1;
                        Error.LabelerErrorCount = labelerErrorCount + 1;
                    }

                if (PaymentAppRun > 90 && paymentAppCount < 2 && isMainMonitor == true)
                    {
                        var myMessage = new SendGrid.SendGridMessage();
                        myMessage.AddTo("web@hochanda.com");
                        myMessage.From = new MailAddress("hope.tools@hochanda.com", "Hope Tools");
                        myMessage.Subject = "There has been an Error";
                        myMessage.Text = "The payment app has not run in over an hour and a half.";
                        var transportWeb = new Web(ConfigurationManager.AppSettings["SendGridApi"].ToString());
                        transportWeb.DeliverAsync(myMessage);
                        Error.WasError = 1;
                        Error.DidPaymentApp = 1;
                        Error.PaymentAppCount = paymentAppCount + 1;
                    }

                if (PaymentAsRunRun > 1 && paymentAsPaidErrorCount < 2 && isMainMonitor == true)
                    {
                        var myMessage = new SendGrid.SendGridMessage();
                        myMessage.AddTo("web@hochanda.com");
                        myMessage.From = new MailAddress("hope.tools@hochanda.com", "Hope Tools");
                        myMessage.Subject = "There has been an Error";
                        myMessage.Text = "The PaymentAsPaid SP has not been run.";
                        var transportWeb = new Web(ConfigurationManager.AppSettings["SendGridApi"].ToString());
                        transportWeb.DeliverAsync(myMessage);
                        Error.WasError = 1;
                        Error.DidPaymentAsPaidError = 1;
                        Error.PaymentAsPaidErrorCount = paymentAsPaidErrorCount + 1;
                    }
            }

           
            return Json(Error, JsonRequestBehavior.AllowGet);
        }
    }
}
