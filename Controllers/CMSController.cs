using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace InternalWebSystems.Controllers
{
    public class CMSController : Controller
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

            List<SelectListItem> obj = new List<SelectListItem>();

            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("SPU_HT_Get_Active_Languages"))
                {
                    command.CommandType = CommandType.StoredProcedure;
 
                    command.Connection = connection;
                    connection.Open();
                    SqlDataReader myReader = command.ExecuteReader();
                    while (myReader.Read())
                    {
                        obj.Add(new SelectListItem { Text = myReader["languagename"].ToString(), Value = myReader["languageid"].ToString() });
                    }

                    connection.Close();
                }
            }

            ViewBag.ActiveLanguage = obj;

            return View();
        }

        public ActionResult FavoredBrandSelector(bool? compleate, FormCollection formCollection, int languageIdFromNonForm = 0)
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

            var languageId = "";
            if(languageIdFromNonForm == 0)
            {
                languageId = formCollection["ActiveLanguage"].ToString();
            }
            else
            {
                languageId = languageIdFromNonForm.ToString();
            }

            List<SelectListItem> obj = new List<SelectListItem>();

            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("SPU_HT_CMS_Retrieve_Brands"))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@LanguageId", languageId);
                    command.Connection = connection;
                    connection.Open();
                    SqlDataReader myReader = command.ExecuteReader();
                    while (myReader.Read())
                    {
                        obj.Add(new SelectListItem { Text = myReader["name"].ToString(), Value = myReader["id"].ToString() + '-' + myReader["bareference"].ToString() });
                    }

                    connection.Close();
                }
            }

            ViewBag.FavoredBrand1 = obj;
            ViewBag.FavoredBrand2 = generateDropDownOrder(obj, 1);
            ViewBag.FavoredBrand3 = generateDropDownOrder(obj, 2);
            ViewBag.FavoredBrand4 = generateDropDownOrder(obj, 3);
            ViewBag.FavoredBrand5 = generateDropDownOrder(obj, 4);
            ViewBag.FavoredBrand6 = generateDropDownOrder(obj, 5);
            ViewBag.FavoredBrand7 = generateDropDownOrder(obj, 6);
            ViewBag.FavoredBrand8 = generateDropDownOrder(obj, 7);
            ViewBag.FavoredBrand9 = generateDropDownOrder(obj, 8);
            ViewBag.FavoredBrand0 = generateDropDownOrder(obj, 9);
            List<SelectListItem> hiddenObject = new List<SelectListItem>();
            hiddenObject.Add(new SelectListItem { Text = languageId, Value = languageId });
            ViewBag.ActiveLanguage = hiddenObject; 

            if (compleate == true)
            {
                ViewBag.HtmlStr = "<Div id=\"CompleateDiv\">The Featured Brands have been updated.</div>";
            }
            if (compleate == false)
            {
                ViewBag.HtmlStr = "<Div id=\"CompleateDiv\">The Featured Brands have NOT been updated, please select brands that are unique.</div>";
            }

            return View();
        }
        public ActionResult SaveFeaturedBrands(FormCollection collection)
        {
            List<string> numberList = new List<string>() { collection["FavoredBrand1"].ToString(), collection["FavoredBrand2"].ToString(), 
                                                     collection["FavoredBrand3"].ToString(), collection["FavoredBrand4"].ToString(), 
                                                     collection["FavoredBrand5"].ToString(), collection["FavoredBrand6"].ToString(), 
                                                     collection["FavoredBrand7"].ToString(), collection["FavoredBrand8"].ToString(), 
                                                     collection["FavoredBrand9"].ToString(), collection["FavoredBrand0"].ToString() };

            bool isUnique = numberList.Distinct().Count() == numberList.Count();

            if (isUnique == true)
            {
                string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("SPU_HT_CMS_Set_Favored_Brands"))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@LanguageId", collection["ActiveLanguage"].ToString());
                        command.Parameters.AddWithValue("@brand1id", numberList[0].Split('-')[0]);
                        command.Parameters.AddWithValue("@brand2id", numberList[1].Split('-')[0]);
                        command.Parameters.AddWithValue("@brand3id", numberList[2].Split('-')[0]);
                        command.Parameters.AddWithValue("@brand4id", numberList[3].Split('-')[0]);
                        command.Parameters.AddWithValue("@brand5id", numberList[4].Split('-')[0]);
                        command.Parameters.AddWithValue("@brand6id", numberList[5].Split('-')[0]);
                        command.Parameters.AddWithValue("@brand7id", numberList[6].Split('-')[0]);
                        command.Parameters.AddWithValue("@brand8id", numberList[7].Split('-')[0]);
                        command.Parameters.AddWithValue("@brand9id", numberList[8].Split('-')[0]);
                        command.Parameters.AddWithValue("@brand0id", numberList[9].Split('-')[0]);
                        command.Parameters.AddWithValue("@brand1re", numberList[0].Split('-')[1]);
                        command.Parameters.AddWithValue("@brand2re", numberList[1].Split('-')[1]);
                        command.Parameters.AddWithValue("@brand3re", numberList[2].Split('-')[1]);
                        command.Parameters.AddWithValue("@brand4re", numberList[3].Split('-')[1]);
                        command.Parameters.AddWithValue("@brand5re", numberList[4].Split('-')[1]);
                        command.Parameters.AddWithValue("@brand6re", numberList[5].Split('-')[1]);
                        command.Parameters.AddWithValue("@brand7re", numberList[6].Split('-')[1]);
                        command.Parameters.AddWithValue("@brand8re", numberList[7].Split('-')[1]);
                        command.Parameters.AddWithValue("@brand9re", numberList[8].Split('-')[1]);
                        command.Parameters.AddWithValue("@brand0re", numberList[9].Split('-')[1]);
                        command.Connection = connection;
                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }


                return RedirectToAction("FavoredBrandSelector", "CMS", new { compleate = true, formCollection = collection, languageIdFromNonForm = Int32.Parse(collection["ActiveLanguage"].ToString()) });
            }
            else
            {
                return RedirectToAction("FavoredBrandSelector", "CMS", new { compleate = false, formCollection = collection, languageIdFromNonForm = Int32.Parse(collection["ActiveLanguage"].ToString()) });
            }

            
        }
        private List<SelectListItem> generateDropDownOrder(List<SelectListItem> initialObj, int itemsToRemove)
        {
            List<SelectListItem> obj = new List<SelectListItem>(initialObj);
            int whileInt = 0;
            while(whileInt < itemsToRemove)
            {
                obj.RemoveAt(0);
                obj.Add(initialObj[whileInt]);
                whileInt++;
            }
            
            return obj;
        }

        public ActionResult MetaDataInput()
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

            List<SelectListItem> obj = new List<SelectListItem>();

            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("SPU_HT_CMS_SelectMetadataTypes"))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Connection = connection;
                    connection.Open();
                    SqlDataReader myReader = command.ExecuteReader();
                    while (myReader.Read())
                    {
                        obj.Add(new SelectListItem { Text = myReader["MetadataCatagoryName"].ToString(), Value = myReader["MetadataCatagoryId"].ToString() });
                    }

                    connection.Close();
                }
            }

            obj.Insert(0, new SelectListItem { Text = "Please Select type..", Value = "0" });
            ViewBag.MetadataType = obj;
            ViewBag.languageSelector = new SelectListItem { Text = "Please Select Language..", Value = "0" };

            return View();
        }
        public JsonResult GetLanguages()
        {
            List<SelectListItem> obj = new List<SelectListItem>();

            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("SPU_HT_Get_Active_Languages"))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Connection = connection;
                    connection.Open();
                    SqlDataReader myReader = command.ExecuteReader();
                    while (myReader.Read())
                    {
                        obj.Add(new SelectListItem { Text = myReader["languagename"].ToString(), Value = myReader["languageid"].ToString() });
                    }

                    connection.Close();
                }
            }

            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetPosibleMetadataToEdit(int InputNumber, int LanguageId )
        {
            List<SelectListItem> obj = new List<SelectListItem>();

            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("SPU_HT_CMS_Metadata_GetLists"))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Connection = connection;
                    command.Parameters.AddWithValue("@Input", InputNumber);
                    command.Parameters.AddWithValue("@Language", LanguageId);
                    connection.Open();
                    SqlDataReader myReader = command.ExecuteReader();
                    while (myReader.Read())
                    {
                        obj.Add(new SelectListItem { Text = myReader["Name"].ToString(), Value = myReader["Id"].ToString() });
                    }

                    connection.Close();
                }
            }

            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCurrentMetadata(string InputNumber, string LanguageId, string MetaSelected)
        {
            var obj = "";

            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("SPU_HT_CMS_Metadata_GetMetaData"))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Connection = connection;
                    command.Parameters.AddWithValue("@InputNumber", InputNumber);
                    command.Parameters.AddWithValue("@LanguageId", LanguageId);
                    command.Parameters.AddWithValue("@MetaSelected", MetaSelected);
                    connection.Open();
                    SqlDataReader myReader = command.ExecuteReader();
                    while (myReader.Read())
                    {
                        obj = myReader["MedatadaText"].ToString();
                    }

                    connection.Close();
                }
            }

            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public void SaveMetaData(string InputNumber, string LanguageId, string MetaSelected, string MetaInput)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("SPU_HT_CMS_Metadata_SaveMetaData"))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = connection;
                    command.Parameters.AddWithValue("@InputNumber", InputNumber);
                    command.Parameters.AddWithValue("@LanguageId", LanguageId);
                    command.Parameters.AddWithValue("@MetaSelected", MetaSelected);
                    command.Parameters.AddWithValue("@MetaInput", MetaInput);
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }
    }
}
