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
    public class HomeController : Controller
    {
        public ActionResult Index(bool? error)
        {
            bool MyCookie = IsCookiePresentAndSessionValid("HIWSSettings");

            if (MyCookie == true)
            {
                return RedirectToAction("Main");
            }
            if (error != null)
            {
                ViewBag.HtmlStr = "<Div id=\"ErrorDiv\">Username or Password is incorect or your acount is not active</div>";
            }
            ViewBag.Title = "Index";

            return View();
        }

        public ActionResult Main()
        {
            //Repetitition need to find a way to extract
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
            //End Repetition 

            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("[dbo].[SPU_HT_Dashboard_Overview]"))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Variable", aCookie["SessionGui"]);

                    command.Connection = connection;
                    connection.Open();
                    SqlDataReader myReader = command.ExecuteReader();
                    while (myReader.Read())
                    {
                        ViewBag.FirstName = myReader["firstname"].ToString();
                        ViewBag.Surname = myReader["surname"].ToString();
                        ViewBag.PermisionLevel = Int32.Parse(myReader["permisionlevel"].ToString());
                    }

                    connection.Close();
                }
            }
            
            ViewBag.Title = "Main";
            ViewBag.Header = "Welcome to hope.tools";
            return View();
        }

        public ActionResult PinEntry(int Level, string Seesion, DateTime Expiery, string name, bool? error)
        {
            if (error != null)
            {
                ViewBag.HtmlStr = "<Div id=\"ErrorDiv\">You have entered the incorect pin for your account.</div>";
            }
            ViewBag.HiddenHtml = "<input type=\"text\" name=\"permissionLevel\" value=" + Level.ToString() + "><input type=\"text\" name=\"SessionGuid\" value=" + Seesion.ToString() + "><input type=\"text\" name=\"SessionExpiery\" value=" + Expiery.ToString() + "><input type=\"text\" name=\"name\" value=" + name + ">";
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


        public ActionResult test()
        {
            //Repetitition need to find a way to extract
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
            //End Repetition 


            ViewBag.Title = "test";

            return View();
        }
    }
}
