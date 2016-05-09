using InternalWebSystems.Class;
using InternalWebSystems.Models;
using SendGrid;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace InternalWebSystems.Controllers
{
    public class TvController : Controller
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

            var cookieValue = "0-0";
            var myCookie = Request.Cookies["HIWSSettings"];
            if (myCookie != null)
            {
                cookieValue = myCookie.Value;
            }
            var obj = "";
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("SPU_HT_RetrivePermisionLevel"))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Guid", cookieValue.Split('=')[1]);


                    command.Connection = connection;
                    connection.Open();
                    SqlDataReader myReader = command.ExecuteReader();
                    while (myReader.Read())
                    {
                        obj = myReader["permisionLevel"].ToString(); 
                    }

                    connection.Close();
                }
            }

            ViewBag.Permision = obj;

            return View();
        }
        //Sku Edit
        public ActionResult SkuEdit()
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
        public JsonResult GetSkuDataToEdit(string Sku)
        {
            var pause = 1;

            var cookieValue = "0-0";
            var myCookie = Request.Cookies["HIWSSettings"];
            if (myCookie != null)
            {
                cookieValue = myCookie.Value;
            }

            ReportVariables Obj = new ReportVariables();

            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("SPU_HT_EDITSKU_GetParentProductSkuInfo"))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Sku", Sku);
                    command.Parameters.AddWithValue("@LanguageId", 1);
                    command.Parameters.AddWithValue("@Gui", cookieValue.Split('=')[1]);

                    command.Connection = connection;
                    connection.Open();
                    SqlDataReader myReader = command.ExecuteReader();
                    while (myReader.Read())
                    {
                        Obj.Variable1 = myReader["Parentproductid"].ToString();
                        Obj.Variable2 = myReader["Description"].ToString();
                        Obj.Variable3 = myReader["WebText"].ToString();
                        Obj.Variable4 = myReader["ProducerNotes"].ToString();
                        Obj.Variable5 = myReader["BuyerNotes"].ToString();
                        Obj.Variable6 = myReader["IsActive"].ToString();
                        Obj.Variable7 = myReader["permisionLevel"].ToString();
                        Obj.Variable8 = myReader["ModifiedDate"].ToString();
                        Obj.Variable9 = myReader["ModifiedBy"].ToString();
                    }

                    connection.Close();
                }
            }

            return Json(Obj, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetSkuVariationDataToEdit(string Sku, string PPId)
        {
            var pause = 1;

            List<ReportVariables> Obj = new List<ReportVariables>();

            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("SPU_HT_EDITSKU_GetVariationProductSkuInfo"))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@LanguageId", 1);
                    command.Parameters.AddWithValue("@Parentproductid", PPId);

                    command.Connection = connection;
                    connection.Open();
                    SqlDataReader myReader = command.ExecuteReader();
                    while (myReader.Read())
                    {
                        ReportVariables newObj = new ReportVariables();
                        newObj.Variable1 = myReader["variationproductsku"].ToString();
                        newObj.Variable2 = myReader["variationname"].ToString();
                        newObj.Variable3 = myReader["freeqty"].ToString();
                        newObj.Variable4 = myReader["active"].ToString();
                        Obj.Add(newObj);
                    }

                    connection.Close();
                }
            }

            return Json(Obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveDataForSku(FormCollection collection)
        {
            int number = collection.Count;
            int Vs = 5;
            var cookieValue = "0-0";
            var myCookie = Request.Cookies["HIWSSettings"];
            if (myCookie != null)
            {
                cookieValue = myCookie.Value;
            }
            var pause = 1;

            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            #region Save Main Info
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("SPU_HT_EDITSKU_SaveParentProductSkuInfo"))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ProductId", collection[0]);
                    command.Parameters.AddWithValue("@Description", collection[1]);
                    command.Parameters.AddWithValue("@WebText", collection[2]);
                    command.Parameters.AddWithValue("@ProducerNotes", collection[3]);
                    command.Parameters.AddWithValue("@BuyerNotes", collection[4]);
                    command.Parameters.AddWithValue("@LanguageId", 1);
                    command.Parameters.AddWithValue("@Guid", cookieValue.Split('=')[1]);

                    command.Connection = connection;
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            #endregion

            while (Vs < number)
            {
                

                #region Save Variation Info
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("SPU_HT_EDITSKU_SaveVariationProductSkuInfo"))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@ProductId", collection[0]);
                        command.Parameters.AddWithValue("@LanguageId", 1);
                        command.Parameters.AddWithValue("@VariationDesciption", collection[Vs]);
                        Vs++;
                        command.Parameters.AddWithValue("@VariationSku", collection[Vs]);
                        Vs++;
                        command.Connection = connection;
                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
                #endregion
                
                
            }
            return RedirectToAction("SkuEdit", "Tv");
        }

        //Minuets
        public ActionResult Minutes()
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
            ViewBag.Gui = aCookie.Value;
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
            List<SelectListItem> obj2 = new List<SelectListItem>();
            List<SelectListItem> obj3 = new List<SelectListItem>();
            obj.Add(new SelectListItem { Text = "Please Select One...", Value = "0" });

            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            #region Generate Active Channels dropdown
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("SPU_HT_Get_Active_Channel"))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Connection = connection;
                    connection.Open();
                    SqlDataReader myReader = command.ExecuteReader();
                    while (myReader.Read())
                    {
                        obj.Add(new SelectListItem { Text = myReader["channelname"].ToString(), Value = myReader["channelid"].ToString() });
                    }

                    connection.Close();
                }
            }
            #endregion

            #region Generate Active Producers dropdown
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("SPU_HT_Get_Active_Producers"))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Connection = connection;
                    connection.Open();
                    SqlDataReader myReader = command.ExecuteReader();
                    while (myReader.Read())
                    {
                        obj2.Add(new SelectListItem { Text = myReader["firstname"].ToString() + " " + myReader["lastname"].ToString(), Value = myReader["firstname"].ToString() + " " + myReader["lastname"].ToString() });
                    }

                    connection.Close();
                }
            }
            #endregion

            #region Generate Active Presenter dropdown
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("SPU_HT_Get_Active_Presenter"))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Connection = connection;
                    connection.Open();
                    SqlDataReader myReader = command.ExecuteReader();
                    while (myReader.Read())
                    {
                        obj3.Add(new SelectListItem { Text = myReader["firstname"].ToString() + " " + myReader["lastname"].ToString(), Value = myReader["firstname"].ToString() + " " + myReader["lastname"].ToString() });
                    }

                    connection.Close();
                }
            }
            #endregion

            ViewBag.ActiveChannels = obj;
            ViewBag.ActiveProducers = obj2.OrderBy(o => o.Text).ToList(); ;
            ViewBag.ActivePresenters = obj3.OrderBy(o => o.Text).ToList(); ;

            return View();
        } //Done
        public JsonResult HasChannelAndDateBeenCompleated(string Channel, string Date)
        {
            //check there are no '2' in database for this day
            var finished = "0";

            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("SPU_HT_Reports_AreNotesCompleateForDate"))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Channel", Channel);
                    command.Parameters.AddWithValue("@Date", Date);

                    command.Connection = connection;
                    connection.Open();
                    SqlDataReader myReader = command.ExecuteReader();
                    while (myReader.Read())
                    {
                        if (myReader["TwoCount"].ToString() != null)
                        {
                            finished = myReader["TwoCount"].ToString();
                        }

                    }

                    connection.Close();
                }
            }

            return Json(finished, JsonRequestBehavior.AllowGet);
        } //Done
        public JsonResult RetriveInfoForDateAndHour(string Channel, string Date, string Hour) //Done
        {
            //Make SQL Datetime from Date & Time
            var datetime = Date + ' ' + Hour + ":00:00.000";
            var showid = "";
            // check if notes exist for this date and time and populate data & Get Show info 
            DetailsByHour obj = new DetailsByHour();
            #region first connection
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("SPU_HT_Reports_GetNotesAndDetailsForDateAndTime"))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Channel", Channel);
                    command.Parameters.AddWithValue("@Date", datetime);

                    command.Connection = connection;
                    connection.Open();
                    SqlDataReader myReader = command.ExecuteReader();
                    while (myReader.Read())
                    {
                        showid = myReader["tvscheduleid"].ToString();
                        if (myReader["tvnotes"].ToString() != "NULL" && myReader["tvnotes"].ToString() != "")
                        {
                            obj.Notes = myReader["tvnotes"].ToString().Split('¬')[0];
                            obj.DirectorNotes = myReader["tvnotes"].ToString().Split('¬')[1];
                            obj.FloorNotes = myReader["tvnotes"].ToString().Split('¬')[2];
                            obj.GoingForward = myReader["tvnotes"].ToString().Split('¬')[3];
                        }
                        else
                        {
                            obj.Notes = "";
                            obj.DirectorNotes = "";
                            obj.FloorNotes = "";
                            obj.GoingForward = "";
                        }
                        if (obj.Notes == "None")
                        {
                            obj.Notes = "";
                        }
                        if (obj.GoingForward == "None")
                        {
                            obj.GoingForward = "";
                        }
                        obj.ShowName = myReader["showname"].ToString();
                        obj.Presenter = myReader["PresenterName"].ToString();
                        obj.Producer = myReader["ProducerName"].ToString();
                        if (myReader["tvnotesother"].ToString() != "NULL" && myReader["tvnotesother"].ToString() != "")
                        {
                            obj.Director = myReader["tvnotesother"].ToString().Split('¬')[4];
                            obj.Floor = myReader["tvnotesother"].ToString().Split('¬')[5];
                            obj.Guest = myReader["tvnotesother"].ToString().Split('¬')[6];
                        }
                        else
                        {
                            obj.Director = "";
                            obj.Floor = "";
                            obj.Guest = "";
                        }
                        if(obj.Director == "None")
                        {
                            obj.Director = "";
                        }
                        if (obj.Floor == "None")
                        {
                            obj.Floor = "";
                        }
                        if (obj.Guest == "None")
                        {
                            obj.Guest = "";
                        }
                        var thing = myReader["tvnotesother"].ToString();
                    }

                    connection.Close();
                }
            }
            #endregion

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("SPU_Apps_ProPanel_GetSalesFiguresForTVScheduleID"))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@TVScheduleID", showid);

                    command.Connection = connection;
                    connection.Open();

                    System.Data.SqlClient.SqlDataAdapter adapter = new System.Data.SqlClient.SqlDataAdapter(command);

                    DataSet ds = new DataSet();
                    adapter.Fill(ds);

                    connection.Close();

                    DataTable something = ds.Tables[1];
                    obj.Sales = something.Rows[0]["showsoldvalue1"].ToString();
                }
            }

            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SaveNotesForm(FormCollection collection)
        {
            List<string> VarList = AssighnVars(collection);

            var other = VarList[9] + " ¬ " + VarList[10] + " ¬ " + VarList[11] + " ¬ £" + VarList[12] + "¬" + VarList[0] + "¬" + VarList[1] + "¬" + VarList[2];
            var datetime = VarList[7] + ' ' + VarList[8] + ":00:00.000";
            var allNotes = VarList[3] + "¬" + VarList[4] + "¬" + VarList[5] + "¬" + VarList[6];
            var OnlyGui = collection["Gui"].ToString().Split('=')[1].ToString();



            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("SPU_HT_Reports_SaveMinutes"))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Gui", OnlyGui);
                    command.Parameters.AddWithValue("@Minutes", allNotes);
                    command.Parameters.AddWithValue("@Date", datetime);
                    command.Parameters.AddWithValue("@Other", other);
                    command.Parameters.AddWithValue("@Include", 1);
                    command.Parameters.AddWithValue("@producerFirst", VarList[13]);
                    command.Parameters.AddWithValue("@producerSecond", VarList[14]);
                    command.Parameters.AddWithValue("@presenterFirst", VarList[15]);
                    command.Parameters.AddWithValue("@presenterSecond", VarList[16]);

                    command.Connection = connection;
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }

            return RedirectToAction("Minutes", "Tv");
        }
        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SaveMinutesAndSend(FormCollection collection)
        {
            List<string> VarList = AssighnVars(collection);

            var other = VarList[9] + " ¬ " + VarList[10] + " ¬ " + VarList[11] + " ¬ £" + VarList[12] + "¬" + VarList[0] + "¬" + VarList[1] + "¬" + VarList[2];
            var datetime = VarList[7] + ' ' + VarList[8] + ":00:00.000";
            var allNotes = VarList[3] + "¬" + VarList[4] + "¬" + VarList[5] + "¬" + VarList[6];
            var OnlyGui = collection["Gui"].ToString().Split('=')[1].ToString();

            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("SPU_HT_Reports_SaveMinutes"))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Gui", OnlyGui);
                    command.Parameters.AddWithValue("@Minutes", allNotes);
                    command.Parameters.AddWithValue("@Date", datetime);
                    command.Parameters.AddWithValue("@Other", other);
                    command.Parameters.AddWithValue("@Include", 2);
                    command.Parameters.AddWithValue("@producerFirst", VarList[13]);
                    command.Parameters.AddWithValue("@producerSecond", VarList[14]);
                    command.Parameters.AddWithValue("@presenterFirst", VarList[15]);
                    command.Parameters.AddWithValue("@presenterSecond", VarList[16]);

                    command.Connection = connection;
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }

            SendMinutesEmail(VarList[7]);

            return RedirectToAction("Minutes", "Tv");
        }
        public void SendMinutesEmail(string date)
        {

            //sql call fetch data
            List<DetailsByHour> NotesList = new List<DetailsByHour>();

            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("SPU_HT_Reports_GetNotesAndDetailsForDateForSending"))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Date", date);

                    command.Connection = connection;
                    connection.Open();
                    SqlDataReader myReader = command.ExecuteReader();
                    while (myReader.Read())
                    {
                        DetailsByHour NoteItem = new DetailsByHour();

                        NoteItem.Notes = myReader["tvnotes"].ToString().Split('¬')[0];
                        NoteItem.DirectorNotes = myReader["tvnotes"].ToString().Split('¬')[1];
                        NoteItem.FloorNotes = myReader["tvnotes"].ToString().Split('¬')[2];
                        NoteItem.GoingForward = myReader["tvnotes"].ToString().Split('¬')[3];
                        NoteItem.ShowName = myReader["tvnotesother"].ToString().Split('¬')[0];
                        NoteItem.Presenter = myReader["Presenter"].ToString();
                        NoteItem.Producer = myReader["Producer"].ToString();
                        NoteItem.Sales = myReader["tvnotesother"].ToString().Split('¬')[3];
                        NoteItem.Director = myReader["tvnotesother"].ToString().Split('¬')[4];
                        NoteItem.Floor = myReader["tvnotesother"].ToString().Split('¬')[5];
                        NoteItem.Guest = myReader["tvnotesother"].ToString().Split('¬')[6];
                        NoteItem.Hour = myReader["showdatetime"].ToString().Split(' ')[1].Split(':')[0];

                        NotesList.Add(NoteItem);
                    }

                    connection.Close();
                }
            }
            //format data
            var emailHtml = "";

            foreach (DetailsByHour note in NotesList)
            {
                var propertime = getpropertime(note.Hour);
                emailHtml +=    "<table style=\"max-width:600px;\" width=\"100%\" align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\">" +
                                "<tbody><tr><td>" +
                                "<table width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\">" +
                                "<tbody><tr>" +
                                "<td style=\"font-family:Gotham, Helvetica, Arial, sans-serif; font-size:30px; font-weight:bold; text-transform:uppercase; color:#ffffff; background: #058ca0; background-image: url('https://www.hochanda.com/Styles/Images/strip_texture10.png'); text-align: center; padding:5px;\"><a href=\"#\" style=\"color:#FFFFFF; text-decoration:none;\">" + propertime + " - " + note.ShowName + "</a></td>" +
                                "</tr><tr><td>&nbsp;</td></tr><tr><td>" +
                                "<table style=\"border-color: #ccc;border: 1px;\" width=\"300\" align=\"left\" border=\"1\" cellpadding=\"5\" cellspacing=\"0\">" +
                                "<tbody><tr>" +
                                "<td style=\"font-family:Gotham,  Helvetica, Arial, sans-serif; font-size:12px; line-height:16px; font-weight: bold; text-transform: uppercase;    background-color: #f1f1f1;    width: 90px;\">Producer:</td>" +
                                "<td style=\"font-family:Gotham,  Helvetica, Arial, sans-serif; font-size:12px; line-height:16px;\">" + note.Producer + "</td>" +
                                "</tr><tr>" +
                                "<td style=\"font-family:Gotham,  Helvetica, Arial, sans-serif; font-size:12px; line-height:16px; font-weight: bold; text-transform: uppercase;    background-color: #f1f1f1;    width: 90px;\">Presenter:</td>" +
                                "<td style=\"font-family:Gotham,  Helvetica, Arial, sans-serif; font-size:12px; line-height:16px;\">" + note.Presenter + "</td>" +
                                "</tr><tr>" +
                                "<td style=\"font-family:Gotham,  Helvetica, Arial, sans-serif; font-size:12px; line-height:16px; font-weight: bold; text-transform: uppercase;    background-color: #f1f1f1;    width: 90px;\">Guest:</td>" +
                                "<td style=\"font-family:Gotham,  Helvetica, Arial, sans-serif; font-size:12px; line-height:16px;\">" + note.Guest + "</td>" +
                                "</tr></tbody></table>" +
                                "<table style=\"border-color: #ccc;border: 1px;\" width=\"300\" align=\"left\" border=\"1\" cellpadding=\"5\" cellspacing=\"0\">" +
                                "<tbody><tr>" +
                                "<td style=\"font-family:Gotham,  Helvetica, Arial, sans-serif; font-size:12px; line-height:16px; font-weight: bold; text-transform: uppercase;    background-color: #f1f1f1;    width: 90px;\">Director:</td>" +
                                "<td style=\"font-family:Gotham,  Helvetica, Arial, sans-serif; font-size:12px; line-height:16px;\">" + note.Director + "</td>" +
                                "</tr><tr>" +
                                "<td style=\"font-family:Gotham,  Helvetica, Arial, sans-serif; font-size:12px; line-height:16px; font-weight: bold; text-transform: uppercase;    background-color: #f1f1f1;    width: 90px;\">Floor:</td>" +
                                "<td style=\"font-family:Gotham,  Helvetica, Arial, sans-serif; font-size:12px; line-height:16px;\">" + note.Floor + "</td>" + 
                                "</tr><tr>" +
                                "<td style=\"font-family:Gotham,  Helvetica, Arial, sans-serif; font-size:12px; line-height:16px; font-weight: bold; text-transform: uppercase;    background-color: #f1f1f1;    width: 90px;\">Sales:</td>" +
                                "<td style=\"font-family:Gotham,  Helvetica, Arial, sans-serif; font-size:16px; line-height:16px; color:#ed137d; font-weight:bold;\">" + note.Sales + "</td>" +
                                "</tr></tbody></table></td></tr></tbody></table>" +
                                "<table width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\">" +
                                "<tbody><tr><td>&nbsp;</td></tr><tr>" +
                                "<td style=\"font-family:Gotham,  Helvetica, Arial, sans-serif; font-size:30px; font-weight:bold; text-transform:uppercase; color:#ed137d; background: #f1f1f1; text-align: center;\">Summary</td>" +
                                "</tr><tr><td>&nbsp;</td></tr><tr>" +
                                "<td style=\"font-family:Gotham,  Helvetica, Arial, sans-serif; font-size:18px; font-weight:bold; text-transform:uppercase;\">Producer:</td></tr><tr>" +
                                "<td style=\"font-family:Gotham,  Helvetica, Arial, sans-serif; font-size:12px; line-height:16px;\">" + note.Notes + "</td>" +
                                "</tr><tr><td>&nbsp;</td></tr><tr>" +
                                "<td style=\"font-family:Gotham,  Helvetica, Arial, sans-serif; font-size:18px; font-weight:bold; text-transform:uppercase;\">Director:</td></tr><tr>" +
                                "<td style=\"font-family:Gotham,  Helvetica, Arial, sans-serif; font-size:12px; line-height:16px;\">" + note.DirectorNotes + "</td>" +
                                "</tr><tr><td>&nbsp;</td></tr><tr>" +
                                "<td style=\"font-family:Gotham,  Helvetica, Arial, sans-serif; font-size:18px; font-weight:bold; text-transform:uppercase;\">Floor Manager:</td></tr><tr>" +
                                "<td style=\"font-family:Gotham,  Helvetica, Arial, sans-serif; font-size:12px; line-height:16px;\">" + note.FloorNotes + "</td>" +
                                "</tr><tr><td>&nbsp;</td></tr><tr>" +
                                 "<td style=\"font-family:Gotham,  Helvetica, Arial, sans-serif; font-size:18px; font-weight:bold; text-transform:uppercase;\">Going Forward:</td></tr><tr>" +
                                "<td style=\"font-family:Gotham,   Helvetica, Arial, sans-serif; font-size:12px; line-height:16px;\">" + note.GoingForward + "</td>" +
                                "</tr><tr><td>&nbsp;</td></tr><tr>" +

                                "</tr></tbody></table></td></tr></tbody></table>";
            }

            var myMessage = new SendGrid.SendGridMessage();
            myMessage.AddTo(GetMinutesEmail());
            myMessage.From = new MailAddress("hope.tools@hochanda.com", "Hope Tools");
            myMessage.Subject = "Post Production Minutes ";
            myMessage.Html = emailHtml;
            var transportWeb = new Web(ConfigurationManager.AppSettings["SendGridApi"].ToString());
            transportWeb.DeliverAsync(myMessage);
        }
        private string getpropertime(string hour)
        {
            var FormatedHour = "";

            if(hour[0].ToString() == "0")
            {
                if(hour == "00")
                {
                    FormatedHour = "12am";
                }
                else
                {
                    FormatedHour = hour[1].ToString() + "am";
                }
            }
            else
            {
                if (hour == "12")
                {
                    FormatedHour = "12pm";
                }
                else if (hour == "11")
                {
                    FormatedHour = "11am";
                }
                else if (hour == "10")
                {
                    FormatedHour = "10am";
                }
                else
                {
                    FormatedHour = (Int32.Parse(hour) - 12).ToString() + "pm";
                }
            
            }

            return FormatedHour;
        }
        public string GetMinutesEmail()
        {
            var email = "";
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("SPU_HT_Reports_Gettvminutesemailaddress"))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Connection = connection;
                    connection.Open();
                    SqlDataReader myReader = command.ExecuteReader();
                    while (myReader.Read())
                    {
                        email = myReader["tvminutesemailaddress"].ToString();
                    }

                    connection.Close();
                }
            }
            return email;
        }
        private List<string> AssighnVars(FormCollection collection)
        {
            List<string> returnedList = new List<string>();
            #region Var Creation
            var Director = "";
            var Floor = "";
            var Guest = "";
            var ProducerNotes = "";
            var DirectorNotes = "";
            var FloorNotes = "";
            var ForwardNotes = "";
            var Date = "";
            var Hour = "";
            var ShowName = "";
            var Presenter = "";
            var Producer = "";
            var Total = "";
            var producerFirst = collection["ActiveProducers"].ToString().Split(' ')[0];
            var producerSecond = collection["ActiveProducers"].ToString().Split(' ')[1];
            var presenterFirst = collection["ActivePresenters"].ToString().Split(' ')[0];
            var presenterSecond = collection["ActivePresenters"].ToString().Split(' ')[1];
            

            if (collection["Director"].ToString() == null || collection["Director"].ToString() == "" || collection["Director"].ToString() == "(none)")
            {
                Director = "None";
            }
            else
            {
                Director = collection["Director"].ToString();
            }

            if (collection["Floor"].ToString() == null || collection["Floor"].ToString() == "" || collection["Floor"].ToString() == "(none)")
            {
                Floor = "None";
            }
            else
            {
                Floor = collection["Floor"].ToString();
            }

            if (collection["Guest"].ToString() == null || collection["Guest"].ToString() == "" || collection["Guest"].ToString() == "(none)")
            {
                Guest = "None";
            }
            else
            {
                Guest = collection["Guest"].ToString();
            }

            if (collection["ProducerNotes"].ToString() == null || collection["ProducerNotes"].ToString() == "" || collection["ProducerNotes"].ToString() == "(none)")
            {
                ProducerNotes = "None";
            }
            else
            {
                ProducerNotes = collection["ProducerNotes"].ToString();
            }

            if (collection["DirectorNotes"].ToString() == null || collection["DirectorNotes"].ToString() == "" || collection["DirectorNotes"].ToString() == "(none)")
            {
                DirectorNotes = "None";
            }
            else
            {
                DirectorNotes = collection["DirectorNotes"].ToString();
            }

            if (collection["FloorNotes"].ToString() == null || collection["FloorNotes"].ToString() == "" || collection["FloorNotes"].ToString() == "(none)")
            {
                FloorNotes = "None";
            }
            else
            {
                FloorNotes = collection["FloorNotes"].ToString();
            }

            if (collection["ForwardNotes"].ToString() == null || collection["ForwardNotes"].ToString() == "" || collection["ForwardNotes"].ToString() == "(none)")
            {
                ForwardNotes = "None";
            }
            else
            {
                ForwardNotes = collection["ForwardNotes"].ToString();
            }

            if (collection["Date"].ToString() == null || collection["Date"].ToString() == "" || collection["Date"].ToString() == "(none)")
            {
                Date = "None";
            }
            else
            {
                Date = collection["Date"].ToString();
            }

            if (collection["Hour"].ToString() == null || collection["Hour"].ToString() == "" || collection["Hour"].ToString() == "(none)")
            {
                Hour = "None";
            }
            else
            {
                Hour = collection["Hour"].ToString();
            }

            if (collection["ShowName"].ToString() == null || collection["ShowName"].ToString() == "" || collection["ShowName"].ToString() == "(none)")
            {
                ShowName = "None";
            }
            else
            {
                ShowName = collection["ShowName"].ToString();
            }

            if (collection["Presenter"].ToString() == null || collection["Presenter"].ToString() == "" || collection["Presenter"].ToString() == "(none)")
            {
                Presenter = "None";
            }
            else
            {
                Presenter = collection["Presenter"].ToString();
            }

            if (collection["Producer"].ToString() == null || collection["Producer"].ToString() == "" || collection["Producer"].ToString() == "(none)")
            {
                Producer = "None";
            }
            else
            {
                Producer = collection["Producer"].ToString();
            }

            if (collection["Total"].ToString() == null || collection["Total"].ToString() == "" || collection["Total"].ToString() == "(none)")
            {
                Total = "None";
            }
            else
            {
                Total = collection["Total"].ToString();
            }
            #endregion
            returnedList.Add(Director);
            returnedList.Add(Floor);
            returnedList.Add(Guest);
            returnedList.Add(ProducerNotes);
            returnedList.Add(DirectorNotes);
            returnedList.Add(FloorNotes);
            returnedList.Add(ForwardNotes);
            returnedList.Add(Date);
            returnedList.Add(Hour);
            returnedList.Add(ShowName);
            returnedList.Add(Presenter);
            returnedList.Add(Producer);
            returnedList.Add(Total);
            returnedList.Add(producerFirst);
            returnedList.Add(producerSecond);
            returnedList.Add(presenterFirst);
            returnedList.Add(presenterSecond);

            return returnedList;
        }

    }
}
