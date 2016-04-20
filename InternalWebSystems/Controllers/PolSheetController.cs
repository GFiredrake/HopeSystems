using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using itextsharp.pdfa;
using iTextSharp;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Text;

namespace InternalWebSystems.Controllers
{
    public class PolSheetController : Controller
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
            return View();
        }



        //private void cmdmakepolsheet_Click(int tvscheduleid)
        //{
        //    if (tvscheduleid > 0)
        //    {
        //        //generate PDF
        //        Font myCourier_10 = default(Font);
        //        myCourier_10 = FontFactory.GetFont("Courier", BaseFont.CP1252, true, 10, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);

        //        Font myHelvetica_10 = default(Font);
        //        myHelvetica_10 = FontFactory.GetFont("Helvetica", BaseFont.CP1252, true, 10, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);

        //        Font myHelvetica_10_b = default(Font);
        //        myHelvetica_10_b = FontFactory.GetFont("Helvetica", BaseFont.CP1252, true, 10, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);

        //        Font myHelvetica_12 = default(Font);
        //        myHelvetica_12 = FontFactory.GetFont("Helvetica", BaseFont.CP1252, true, 12, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);

        //        Font myHelvetica_12_b = default(Font);
        //        myHelvetica_12_b = FontFactory.GetFont("Helvetica", BaseFont.CP1252, true, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);

        //        Font myHelvetica_20 = default(Font);
        //        myHelvetica_20 = FontFactory.GetFont("Helvetica", BaseFont.CP1252, true, 20, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);

        //        Font myHelvetica_24 = default(Font);
        //        myHelvetica_24 = FontFactory.GetFont("Helvetica", BaseFont.CP1252, true, 24, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);

        //        //dynamic doc = new Document(PageSize.A4.Rotate);
        //        //doc.SetMargins(10, 10, 10, 10);

        //        dynamic doc = new Document(new Rectangle(288f, 144f), 10, 10, 10, 10);
        //        doc.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());


        //        string pol_producer = null;
        //        string pol_presenter = null;
        //        string pol_planner = null;
        //        string pol_showname = null;
        //        string pol_version = null;
        //        string pol_version_no = null;
        //        string pol_locked = null;
        //        string pol_target = null;
        //        string pol_epgtext = null;
        //        string pol_buyersnotes = null;
        //        string[] pol_producer_array = null;
        //        string[] pol_presenter_array = null;
        //        string[] pol_planner_array = null;
        //        string[] pol_showname_array = null;

        //        DataSet ds = new DataSet();

        //        using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
        //        {
        //            using (SqlCommand command = new SqlCommand("SPU_HT_Monitoring_RecordFlexiErrors"))
        //            {
        //                command.CommandType = CommandType.StoredProcedure;
        //                command.Connection = connection;
        //                command.Parameters.AddWithValue("@TVScheduleID", tvscheduleid);

        //                connection.Open();

        //                SqlDataAdapter adap = new SqlDataAdapter(command);

        //                adap.Fill(ds);

                        
        //                //command.ExecuteNonQuery();
        //                connection.Close();
        //            }
        //        }



        //        //ConnectToDB();
        //        //com = new SqlCommand("SPU_Apps_Admin_ListTVScheduleDetails", conn);
        //        //com.CommandType = CommandType.StoredProcedure;
        //        //com.Parameters.AddWithValue("@TVScheduleID", pol_tvscheduleid);
        //        //adap = new SqlDataAdapter(com);
        //        //adap.Fill(ds);
        //        if (ds.Tables(0).Rows.Count == 0)
        //        {
        //            pol_showname = "";
        //        }
        //        else
        //        {
        //            pol_showname = ds.Tables(0).Rows(0)("shownamestring").ToString;
        //        }
        //        if (ds.Tables(1).Rows.Count == 0)
        //        {
        //            pol_producer = "";
        //        }
        //        else
        //        {
        //            pol_producer = ds.Tables(1).Rows(0)("producerstring").ToString;
        //        }
        //        if (ds.Tables(2).Rows.Count == 0)
        //        {
        //            pol_presenter = "";
        //        }
        //        else
        //        {
        //            pol_presenter = ds.Tables(2).Rows(0)("presenterstring").ToString;
        //        }
        //        if (ds.Tables(5).Rows.Count == 0)
        //        {
        //            pol_planner = "";
        //        }
        //        else
        //        {
        //            pol_planner = ds.Tables(5).Rows(0)("plannerstring").ToString;
        //        }
        //        if (ds.Tables(3).Rows.Count == 0)
        //        {
        //            pol_locked = "";
        //            pol_version = "Ver: ?";
        //            pol_version_no = "0";
        //            pol_buyersnotes = "";
        //            pol_epgtext = "";
        //            pol_target = system_currencysymbol + " 0.00";
        //        }
        //        else
        //        {
        //            pol_version = "Ver: " + ds.Tables(3).Rows(0)("polversion").ToString;
        //            pol_version_no = ds.Tables(3).Rows(0)("polversion").ToString;
        //            pol_buyersnotes = ds.Tables(3).Rows(0)("buyernotes").ToString;
        //            pol_epgtext = ds.Tables(3).Rows(0)("epgtext").ToString;
        //            pol_target = system_currencysymbol + " " + ds.Tables(3).Rows(0)("showtarget").ToString;
        //            if (ds.Tables(3).Rows(0)("locked") == 1)
        //            {
        //                pol_locked = "[LOCKED]";
        //            }
        //            else
        //            {
        //                pol_locked = "";
        //            }
        //        }


        //        if (ds.Tables(3).Rows(0)("locked") == 1)
        //        {
        //            pol_producer_array = Strings.Split(pol_producer, " - ");
        //            pol_presenter_array = Strings.Split(pol_presenter, " - ");
        //            pol_planner_array = Strings.Split(pol_planner, " - ");
        //            pol_showname_array = Strings.Split(pol_showname, " - ");

        //            Image imagecell = default(Image);
        //            Image logoimage = default(Image);

        //            //LOGO
        //            logoimage = Image.GetInstance(imageroot + "pol_sheet_logo.png");
        //            logoimage.ScalePercent(50f);


        //            Chunk potitle_chunk1a = new Chunk("POL SHEET" + Constants.vbLf, myHelvetica_24);
        //            Phrase potitle_phrase = new Phrase();
        //            potitle_phrase.Add(potitle_chunk1a);
        //            Paragraph potitle_paragraph = new Paragraph();
        //            potitle_paragraph.Add(potitle_phrase);
        //            potitle_paragraph.Alignment = Element.ALIGN_LEFT;

        //            //TITLE
        //            PdfPTable table0 = new PdfPTable(2);
        //            table0.HorizontalAlignment = Element.ALIGN_LEFT;
        //            table0.TotalWidth = 350f;
        //            table0.LockedWidth = true;

        //            float[] widths0 = new float[] {
        //        100f,
        //        250f
        //    };
        //            table0.SetWidths(widths0);

        //            PdfPCell thiscell = new PdfPCell();


        //            thiscell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
        //            thiscell.BackgroundColor = iTextSharp.text.BaseColor.WHITE;
        //            thiscell.BorderColor = iTextSharp.text.BaseColor.BLACK;
        //            thiscell.Phrase = new Phrase("DATE/TIME", myHelvetica_12_b);
        //            table0.AddCell(thiscell);
        //            thiscell.Phrase = new Phrase(pol_scheduledate_text, myHelvetica_12);
        //            table0.AddCell(thiscell);
        //            thiscell.Phrase = new Phrase("SHOW", myHelvetica_12_b);
        //            table0.AddCell(thiscell);
        //            thiscell.Phrase = new Phrase("'" + pol_showname_array(1) + "'", myHelvetica_12);
        //            table0.AddCell(thiscell);
        //            thiscell.Phrase = new Phrase("VERSION", myHelvetica_12_b);
        //            table0.AddCell(thiscell);
        //            thiscell.Phrase = new Phrase(pol_version + " " + pol_locked, myHelvetica_12);
        //            table0.AddCell(thiscell);
        //            thiscell.Phrase = new Phrase("PRESENTER", myHelvetica_12_b);
        //            table0.AddCell(thiscell);
        //            thiscell.Phrase = new Phrase(pol_presenter_array(1), myHelvetica_12);
        //            table0.AddCell(thiscell);
        //            thiscell.Phrase = new Phrase("PRODUCER", myHelvetica_12_b);
        //            table0.AddCell(thiscell);
        //            thiscell.Phrase = new Phrase(pol_producer_array(1), myHelvetica_12);
        //            table0.AddCell(thiscell);
        //            thiscell.Phrase = new Phrase("PLANNER", myHelvetica_12_b);
        //            table0.AddCell(thiscell);
        //            thiscell.Phrase = new Phrase(pol_planner_array(1), myHelvetica_12);
        //            table0.AddCell(thiscell);
        //            thiscell.Phrase = new Phrase("TARGET", myHelvetica_12_b);
        //            table0.AddCell(thiscell);
        //            thiscell.Phrase = new Phrase(system_currencysymbol + Strings.FormatNumber(pol_target, 0), myHelvetica_12);
        //            table0.AddCell(thiscell);

        //            //LINE SPACER
        //            Chunk linespacer1_chunk = new Chunk(Constants.vbLf, myHelvetica_12);
        //            Phrase linespacer1_phrase = new Phrase();
        //            linespacer1_phrase.Add(linespacer1_chunk);
        //            Paragraph linespacer1_paragraph = new Paragraph();
        //            linespacer1_paragraph.Add(linespacer1_phrase);



        //            //get items on POL
        //            ConnectToDB();
        //            com = new SqlCommand("SPU_Apps_Admin_ListScheduledItems", conn);
        //            com.CommandType = CommandType.StoredProcedure;
        //            com.Parameters.AddWithValue("@ScheduleDate", pol_scheduledate);
        //            com.Parameters.AddWithValue("@ChannelID", 1);
        //            com.Parameters.AddWithValue("@LanguageID", system_languageid);
        //            com.Parameters.AddWithValue("@CurrencyID", system_currencyid);
        //            com.Parameters.AddWithValue("@CountryID", system_countryid);
        //            adap = new SqlDataAdapter(com);
        //            adap.Fill(ds);


        //            PdfPTable table3 = new PdfPTable(8);
        //            table3.HorizontalAlignment = Element.ALIGN_LEFT;
        //            table3.TotalWidth = 790f;
        //            table3.LockedWidth = true;
        //            table3.DefaultCell.BorderWidth = 1;

        //            float[] widths = new float[] {
        //        50f,
        //        70f,
        //        50f,
        //        305f,
        //        45f,
        //        70f,
        //        50f,
        //        150f
        //    };
        //            table3.SetWidths(widths);
        //            //thiscell.Border = 3
        //            thiscell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
        //            thiscell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
        //            thiscell.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;

        //            thiscell.Phrase = new Phrase("ORDER", myHelvetica_12_b);
        //            table3.AddCell(thiscell);

        //            thiscell.Phrase = new Phrase("PHOTO", myHelvetica_12_b);
        //            table3.AddCell(thiscell);

        //            thiscell.Phrase = new Phrase("ITEM #", myHelvetica_12_b);
        //            table3.AddCell(thiscell);

        //            thiscell.Phrase = new Phrase("TV DESCRIPTION", myHelvetica_12_b);
        //            table3.AddCell(thiscell);

        //            thiscell.Phrase = new Phrase("FPR", myHelvetica_12_b);
        //            table3.AddCell(thiscell);

        //            thiscell.Phrase = new Phrase("PRICE", myHelvetica_12_b);
        //            table3.AddCell(thiscell);

        //            thiscell.Phrase = new Phrase("QTY", myHelvetica_12_b);
        //            table3.AddCell(thiscell);

        //            thiscell.Phrase = new Phrase("COMMENTS", myHelvetica_12_b);
        //            table3.AddCell(thiscell);

        //            thiscell.BackgroundColor = iTextSharp.text.BaseColor.WHITE;

        //            //Dim cell As New PdfPCell
        //            for (x = 0; x <= ds.Tables(0).Rows.Count - 1; x++)
        //            {
        //                thiscell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
        //                thiscell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
        //                //running order
        //                thiscell.Phrase = new Phrase(ds.Tables(0).Rows(x)("runningorder").ToString, myHelvetica_24);
        //                table3.AddCell(thiscell);
        //                //photo
        //                try
        //                {
        //                    if (WebFileExists(imageroot + ds.Tables(0).Rows(x)("imagefilename").ToString) == true)
        //                    {
        //                        imagecell = Image.GetInstance(imageroot + ds.Tables(0).Rows(x)("imagefilename").ToString);
        //                    }
        //                    else
        //                    {
        //                        imagecell = Image.GetInstance(system_noimagelink);
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    imagecell = Image.GetInstance(system_noimagelink);
        //                }
        //                imagecell.Border = PdfPCell.NO_BORDER;
        //                table3.AddCell(imagecell);
        //                //item no
        //                thiscell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
        //                thiscell.Phrase = new Phrase(ds.Tables(0).Rows(x)("parentproductsku").ToString, myHelvetica_12_b);
        //                table3.AddCell(thiscell);
        //                //tv description
        //                thiscell.Phrase = new Phrase(ds.Tables(0).Rows(x)("tvdescription").ToString, myHelvetica_12_b);
        //                table3.AddCell(thiscell);
        //                //FPR
        //                thiscell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
        //                string FPR_string = null;
        //                if (ds.Tables(0).Rows(x)("FPR").ToString == 0)
        //                {
        //                    FPR_string = "No";
        //                }
        //                else
        //                {
        //                    FPR_string = "Yes";
        //                }
        //                thiscell.Phrase = new Phrase(FPR_string, myHelvetica_12_b);
        //                table3.AddCell(thiscell);
        //                //price
        //                thiscell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;
        //                thiscell.Phrase = new Phrase(system_currencysymbol + " " + Strings.FormatNumber(ds.Tables(0).Rows(x)("itemprice").ToString, 2).ToString, myHelvetica_12_b);
        //                table3.AddCell(thiscell);
        //                //qty
        //                thiscell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
        //                thiscell.Phrase = new Phrase(ds.Tables(0).Rows(x)("freeqty").ToString, myHelvetica_12_b);
        //                table3.AddCell(thiscell);
        //                //comments
        //                thiscell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
        //                thiscell.Phrase = new Phrase(ds.Tables(0).Rows(x)("linecomments").ToString, myHelvetica_12);
        //                table3.AddCell(thiscell);
        //            }


        //            //COMMENTS
        //            PdfPTable table2 = new PdfPTable(1);
        //            table2.HorizontalAlignment = Element.ALIGN_LEFT;
        //            table2.TotalWidth = 790f;
        //            table2.LockedWidth = true;
        //            thiscell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
        //            thiscell.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
        //            thiscell.Phrase = new Phrase("COMMENTS", myHelvetica_12_b);
        //            table2.AddCell(thiscell);
        //            thiscell.BackgroundColor = iTextSharp.text.BaseColor.WHITE;
        //            thiscell.Phrase = new Phrase(pol_buyersnotes + Constants.vbLf, myHelvetica_12);
        //            table2.AddCell(thiscell);

        //            //EPG Text
        //            PdfPTable table5 = new PdfPTable(1);
        //            table5.HorizontalAlignment = Element.ALIGN_LEFT;
        //            table5.TotalWidth = 790f;
        //            table5.LockedWidth = true;
        //            thiscell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
        //            thiscell.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
        //            thiscell.Phrase = new Phrase("EPG TEXT", myHelvetica_12_b);
        //            table5.AddCell(thiscell);
        //            thiscell.BackgroundColor = iTextSharp.text.BaseColor.WHITE;
        //            thiscell.Phrase = new Phrase(pol_epgtext + Constants.vbLf, myHelvetica_12);
        //            table5.AddCell(thiscell);


        //            PdfPTable table6 = new PdfPTable(2);
        //            if (pol_prepnotes == 1)
        //            {
        //                //iCARD PREP NOTES
        //                table6.HorizontalAlignment = Element.ALIGN_LEFT;
        //                table6.TotalWidth = 790f;
        //                table6.LockedWidth = true;
        //                table6.DefaultCell.BorderWidth = 1;
        //                float[] widths6 = new float[] {
        //            50f,
        //            740f
        //        };
        //                table6.SetWidths(widths6);
        //                //thiscell.Border = 3
        //                thiscell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
        //                thiscell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
        //                thiscell.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
        //                thiscell.Phrase = new Phrase("ORDER", myHelvetica_12_b);
        //                table6.AddCell(thiscell);
        //                thiscell.Phrase = new Phrase("SALES NOTES", myHelvetica_12_b);
        //                table6.AddCell(thiscell);
        //                thiscell.BackgroundColor = iTextSharp.text.BaseColor.WHITE;
        //                for (x = 0; x <= ds.Tables(0).Rows.Count - 1; x++)
        //                {
        //                    thiscell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
        //                    thiscell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
        //                    //running order
        //                    thiscell.Phrase = new Phrase(ds.Tables(0).Rows(x)("runningorder").ToString, myHelvetica_24);
        //                    table6.AddCell(thiscell);
        //                    //salesnotes
        //                    thiscell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
        //                    thiscell.Phrase = new Phrase(ds.Tables(0).Rows(x)("salesnotes").ToString + Constants.vbLf, myHelvetica_12_b);
        //                    table6.AddCell(thiscell);
        //                }
        //            }



        //            try
        //            {
        //                string folderPath = "C:\\Hochanda\\PDFs\\";
        //                if (!Directory.Exists(folderPath))
        //                {
        //                    Directory.CreateDirectory(folderPath);
        //                }
        //                string complete_folderPath = null;
        //                complete_folderPath = folderPath + "POL_" + Strings.Replace(Strings.Mid(pol_scheduledate.ToString, 1, 10), "/", "-") + "_" + Strings.Replace(Strings.Mid(pol_scheduledate.ToString, 12, 2), "/", "-") + "-00_ver_" + pol_version_no + ".pdf";
        //                using (FileStream stream = new FileStream(complete_folderPath, FileMode.Create))
        //                {
        //                    PdfWriter.GetInstance(doc, stream);
        //                    doc.Open();
        //                    doc.Add(logoimage);
        //                    doc.Add(linespacer1_paragraph);
        //                    doc.Add(potitle_paragraph);
        //                    doc.Add(linespacer1_paragraph);
        //                    doc.Add(table0);
        //                    doc.Add(linespacer1_paragraph);
        //                    doc.Add(table3);
        //                    doc.Add(linespacer1_paragraph);
        //                    doc.Add(table2);
        //                    doc.Add(linespacer1_paragraph);
        //                    doc.Add(table5);
        //                    if (pol_prepnotes == 1)
        //                    {
        //                        doc.Add(linespacer1_paragraph);
        //                        doc.Add(table6);
        //                    }
        //                    //pdfDoc.Add(table)
        //                    doc.Close();
        //                    stream.Close();
        //                }

        //                Interaction.MsgBox("POL Sheet PDF created! File available at: " + complete_folderPath, MsgBoxStyle.Information, applicationname);
        //                this.Close();
        //            }
        //            catch (Exception ex)
        //            {
        //                Interaction.MsgBox("Error creating PDF file!", MsgBoxStyle.Exclamation, applicationname);
        //                this.Close();
        //            }
        //        }
        //        else
        //        {
        //            Interaction.MsgBox("The schedule for the selected hour has not been locked." + Constants.vbLf + "POL Sheet unavailable at this time!", MsgBoxStyle.Information, applicationname);
        //        }
        //    }
        //}
    }
}
