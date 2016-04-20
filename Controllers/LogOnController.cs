using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Web;
using System.Web.Mvc;

namespace InternalWebSystems.Controllers
{
    public class LogOnController : Controller
    {
        public ActionResult AttemptLogOn(FormCollection collection)
        {
            int permissionLevel = 0;
            var returnednumberstring = "";
            string SessionGuid = Guid.NewGuid().ToString();
            DateTime SessionExpiery = DateTime.Now.AddMinutes(GetTimeOutTime());

            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("SPU_HT_CheckPassword"))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Username", collection.Get("username"));
                    command.Parameters.AddWithValue("@Password", collection.Get("password"));
                    command.Connection = connection;
                    connection.Open();
                    //permissionLevel = Convert.ToInt32(command.ExecuteScalar());
                    returnednumberstring = command.ExecuteScalar().ToString();
                    connection.Close();
                }
            }
            

            try
            {
                // Do not initialize this variable here.
                permissionLevel = Convert.ToInt32(returnednumberstring.Split('-')[0]);
            }
            catch
            {
                return RedirectToAction("Index", "Home", new { error = true });
            }


            if (CheckIp() == false)
            {
                return RedirectToAction("PinEntry", "Home", new { Level = permissionLevel, Seesion = SessionGuid, Expiery = SessionExpiery, name = returnednumberstring.Split('-')[1] });
            }

            HttpCookieCollection MyCookieCollection = Request.Cookies;
            HttpCookie MyCookie = MyCookieCollection.Get("HIWSSettings");

            if (permissionLevel >= 1 && MyCookie == null)
            {          
                HttpCookie NewCookie = new HttpCookie("HIWSSettings");
                NewCookie["SessionGui"] = SessionGuid;
                NewCookie.Expires = SessionExpiery;
                Response.Cookies.Add(NewCookie);
            }

            if (permissionLevel >= 1)
            {
                CreateSession(permissionLevel, SessionGuid, SessionExpiery, returnednumberstring.Split('-')[1]);
                //List<string> menuList = GenerateMenuForSession(SessionGuid);
                //TempData["MenuList"] = menuList;
                return RedirectToAction("Main", "Home");
                
            }

            return RedirectToAction("Index", "Home", new { error = true });
        }
        public ActionResult AttemptVerification(FormCollection collection)
        {
            var userpin = "a";
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("SPU_HT_ReturnUserPin"))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserId", collection.Get("name"));

                    command.Connection = connection;
                    connection.Open();
                    SqlDataReader myReader = command.ExecuteReader();
                    while (myReader.Read())
                    {
                        userpin = myReader["pin"].ToString();
                    }

                    connection.Close();
                }
            }

            if (userpin == collection.Get("Pin"))
            {
                if (userpin == "")
                {
                    return RedirectToAction("PinEntry", "Home", new { Level = Int32.Parse(collection.Get("permissionLevel")), Seesion = collection.Get("SessionGuid"), Expiery = Convert.ToDateTime(collection.Get("SessionExpiery")), name = collection.Get("name"), error = true });
                }

                else
                {
                    DateTime SessionExpiery = DateTime.Now.AddMinutes(GetTimeOutTime());

                    HttpCookieCollection MyCookieCollection = Request.Cookies;
                    HttpCookie MyCookie = MyCookieCollection.Get("HIWSSettings");

                    if (MyCookie == null)
                    {
                        HttpCookie NewCookie = new HttpCookie("HIWSSettings");
                        NewCookie["SessionGui"] = collection.Get("SessionGuid").ToString();
                        NewCookie.Expires = SessionExpiery;
                        Response.Cookies.Add(NewCookie);
                    }

                    CreateSession(Int32.Parse(collection.Get("permissionLevel")), collection.Get("SessionGuid"), SessionExpiery, collection.Get("name"));
                    return RedirectToAction("Main", "Home");
                }

            }
            else
            {
                return RedirectToAction("PinEntry", "Home", new {Level = Int32.Parse(collection.Get("permissionLevel")), Seesion = collection.Get("SessionGuid"), Expiery = Convert.ToDateTime(collection.Get("SessionExpiery")), name = collection.Get("name"), error = true });
            }
        }

        private bool CheckIp()
        {
            var ip = GetLocalIPAddress();
            var hqip = "";
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("SPU_HT_GetHqIpAddress"))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Connection = connection;
                    connection.Open();
                    SqlDataReader myReader = command.ExecuteReader();
                    while (myReader.Read())
                    {
                        hqip = myReader["hqipaddress"].ToString();
                    }

                    connection.Close();
                }
            }
            if (ip == hqip)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public ActionResult LogOut()
        {
            if (Request.Cookies["HIWSSettings"] != null)
            {
                HttpCookie myCookie = new HttpCookie("HIWSSettings");
                myCookie.Expires = DateTime.Now.AddDays(-1d);
                Response.Cookies.Add(myCookie);
            }
            return RedirectToAction("index", "Home");
        }
        internal List<string> GenerateMenuForSession(string SessionGuid)
        {
            List<string> MenuList = new List<string>() 
            { 
                "<a href=\"/logOn/LogOut\" class=\"button menuButton\">Logout</a>",
                "<a href=\"/Home/Index\" class=\"button menuButton\">Home</a>"
            };

            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("SPU_HT_ReturnServices"))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@active", 1);
                    command.Parameters.AddWithValue("@Guid", SessionGuid);

                    command.Connection = connection;
                    connection.Open();
                    SqlDataReader myReader = command.ExecuteReader();
                    while (myReader.Read())
                    {
                        MenuList.Add("<a href=\"" + myReader["ServiceUrl"].ToString() + "\"class=\"button menuButton\">" + myReader["ServiceName"].ToString() + "</a>");
                    }

                    connection.Close();
                }
            }


            return MenuList;
        }
        private int GetTimeOutTime()
        {
            var timeOut = 0;

            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("SPU_HT_ReturnTimeOut"))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = connection;
                    connection.Open();
                    timeOut = Convert.ToInt32(command.ExecuteScalar());
                    connection.Close();
                }
            }

            return timeOut;
        }
        private void CreateSession(int permissionLevel, string SessionGuid, DateTime SessionExpiery, string name)
        {
            var ip = GetLocalIPAddress();
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("SPU_HT_CreateSession"))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@permisionLevel", permissionLevel);
                    command.Parameters.AddWithValue("@Guid", SessionGuid);
                    command.Parameters.AddWithValue("@Ip", ip);
                    command.Parameters.AddWithValue("@Name", name);
                    SqlParameter ExpieryParameter = command.Parameters.Add("@SessionExpiery", System.Data.SqlDbType.DateTime);
                    ExpieryParameter.Value = SessionExpiery;
                    command.Connection = connection;
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }
        internal bool CheckSessionGuidIsValid(string SessionGuid)
        {
            var isValidSession = 0;
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("SPU_HT_CheckSessionIsValid"))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Guid", SessionGuid);
                    command.Connection = connection;
                    connection.Open();
                    isValidSession = Convert.ToInt32(command.ExecuteScalar());
                    connection.Close();
                }
            }

            if (isValidSession == 0)
            {
                return false;
            }

            return true;
        }
        public string GetLocalIPAddress()
        {
            return Request.UserHostAddress;
        }

        
    }
}
