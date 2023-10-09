using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using AdminDashboard.Models;
using AdminDashboard.ViewModels;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;

namespace AdminDashboard.Controllers
{
    public class HomeController : Controller
    {
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["jayaConnectionString"].ToString());
        CRM2022Entities _db = new CRM2022Entities();
        jayaEntities db = new jayaEntities();
        int i = 0;
        private SqlTransaction sqltrans;

        [HttpGet]
        public ActionResult Index()
        {

            return View();
        }
        [HttpPost]
        public async Task<ActionResult> PostDetails(TELE_ENQUIRY_INFO enquiryInfo)
        {
            // API URL
            string apiUrl = "http://localhost:52680/Api/TelexApi/";

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);

                // Serialize enquiryInfo to JSON
                string jsonEnquiryInfo = JsonConvert.SerializeObject(enquiryInfo);

                // Create a StringContent with the JSON data
                var content = new StringContent(jsonEnquiryInfo, Encoding.UTF8, "application/json");

                // Send a POST request to the API
                HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    // Handle a successful response here if needed
                    // For example, you can read the response content:
                    var responseContent = await response.Content.ReadAsStringAsync();
                    // Process responseContent as needed
                }
                else
                {
                    // Handle an unsuccessful response here
                    // For example, you can log or return an error message
                    // response.ReasonPhrase and response.StatusCode provide error details
                }
            }

            // Return a response or redirect as needed
            return View(enquiryInfo);
        }



        //To bind TELE_ENQUIRY_INFO table based on conditions
        public ActionResult AssignCalls(string Flag, string Enquirytype, DateTime Calldate, string TEIid, string TEIAssignid, bool CheckAssign,string Sourcnamefltr)
        {
            var emp = db.TELE_ENQUIRY_INFO.ToList();
            if (Flag == "")
            {

                if (CheckAssign == true)
                {
                    //emp = db.TELE_ENQUIRY_INFO.Where(a => a.TEI_ID == TEIid && a.TEI_CALL_STATE == "N"&& a.TEI_CALL_DATE == Calldate).ToList();
                    emp = db.TELE_ENQUIRY_INFO.Where(a => a.TEI_ID == TEIid && a.TEI_CALL_STATE == "N" && a.TEI_CALL_DATE == Calldate && a.TEI_ENQUIRY_TYPE == Enquirytype).ToList();
                }
                else
                {
                    //emp = db.TELE_ENQUIRY_INFO.Where(a => a.TEI_CALL_STATE == "N" && a.TEI_CALL_DATE == Calldate && a.TEI_ASSIGN_ID == a.TEI_ID && a.TEI_ASSIGN_ID == TEIAssignid && a.TEI_ENQUIRY_TYPE == Enquirytype).ToList();
                    emp = db.TELE_ENQUIRY_INFO.Where(a => a.TEI_CALL_STATE == "N" && a.TEI_CALL_DATE == Calldate && a.TEI_ID == TEIAssignid && a.TEI_ENQUIRY_TYPE == Enquirytype).ToList();
                }
            }

            else if (Flag == "UA")
            {
                if (CheckAssign == true)
                {
                    //emp = db.TELE_ENQUIRY_INFO.Where(a => a.TEI_ID == TEIid && a.TEI_CALL_STATE == "N"&& a.TEI_CALL_DATE == Calldate).ToList();
                    emp = db.TELE_ENQUIRY_INFO.Where(a => a.TEI_ID == TEIid && a.TEI_CALL_STATE == "N" && a.TEI_CALL_DATE < Calldate && a.TEI_ENQUIRY_TYPE == Enquirytype).ToList();
                }
                else
                {
                    emp = db.TELE_ENQUIRY_INFO.Where(a => a.TEI_CALL_STATE == "N" && a.TEI_CALL_DATE < Calldate && a.TEI_ASSIGN_ID == TEIAssignid && a.TEI_ENQUIRY_TYPE == Enquirytype && a.TEI_ASSIGN_ID == a.TEI_ID).ToList();
                   // emp = db.TELE_ENQUIRY_INFO.Where(a => a.TEI_CALL_STATE == "N" && a.TEI_CALL_DATE < Calldate && a.TEI_ASSIGN_ID == TEIAssignid && a.TEI_ENQUIRY_TYPE == Enquirytype).ToList();
                    //emp = db.TELE_ENQUIRY_INFO.Where(u => u.TEI_ENQUIRY_TYPE == Enquirytype && u.TEI_CALL_DATE <= Calldate  && u.TEI_OPTION == "To-Days" && u.TEI_CALL_STATE == "N" && u.TEI_SNO == 0 && u.TEI_ASSIGN_ID == TEIAssignid).ToList();
                }
            }
            if (Sourcnamefltr!="")
            {
                emp = emp.Where(u => u.FirstSourceOfEnquiry == Sourcnamefltr).ToList();
            }

                return Json(new { data = emp }, JsonRequestBehavior.AllowGet);
        }
     


        public List<TELE_ENQUIRY_INFO> GetList(string Flag, string TEI_ENQUIRY_TYPE, DateTime TEI_CALL_DATE, string TEI_ID, string TEIAssignid, string Sourcefilter = "")
        {
            List<TELE_ENQUIRY_INFO> callsList = new List<TELE_ENQUIRY_INFO>();
            if (Flag == "")
            {
                //callsList = db.TELE_ENQUIRY_INFO.Where(u => u.TEI_ENQUIRY_TYPE == TEI_ENQUIRY_TYPE && u.TEI_CALL_DATE == TEI_CALL_DATE && u.TEI_ID == TEIAssignid && u.TEI_OPTION == "To-Days" && u.TEI_CALL_STATE == "N" ).ToList();
                callsList = db.TELE_ENQUIRY_INFO.Where(a => a.TEI_CALL_STATE == "N" && a.TEI_CALL_DATE == TEI_CALL_DATE && a.TEI_ID == TEI_ID && a.TEI_ENQUIRY_TYPE == TEI_ENQUIRY_TYPE).ToList();
                //callsList = db.TELE_ENQUIRY_INFO.Where(u => u.TEI_ENQUIRY_TYPE == TEI_ENQUIRY_TYPE  && u.TEI_CALL_DATE == TEI_CALL_DATE && u.TEI_ASSIGN_ID== TEIAssignid && u.TEI_OPTION == "To-Days" && u.TEI_CALL_STATE == "N" && u.TEI_CALL_TIME == null).ToList();
            }
            else if (Flag == "UA")
            {

                //if (CheckAssign == true)
                //{
                //    //emp = db.TELE_ENQUIRY_INFO.Where(a => a.TEI_ID == TEIid && a.TEI_CALL_STATE == "N"&& a.TEI_CALL_DATE == Calldate).ToList();
                //    callsList = db.TELE_ENQUIRY_INFO.Where(a => a.TEI_ID == TEIid && a.TEI_CALL_STATE == "N" && a.TEI_CALL_DATE < Calldate && a.TEI_ENQUIRY_TYPE == Enquirytype).ToList();
                //}
                //else
                //{

                callsList = db.TELE_ENQUIRY_INFO.Where(a => a.TEI_CALL_STATE == "N" && a.TEI_CALL_DATE < TEI_CALL_DATE && a.TEI_ASSIGN_ID == TEIAssignid && a.TEI_ENQUIRY_TYPE == TEI_ENQUIRY_TYPE && a.TEI_ID == TEI_ID).ToList();
                //emp = db.TELE_ENQUIRY_INFO.Where(u => u.TEI_ENQUIRY_TYPE == Enquirytype && u.TEI_CALL_DATE <= Calldate  && u.TEI_OPTION == "To-Days" && u.TEI_CALL_STATE == "N" && u.TEI_SNO == 0 && u.TEI_ASSIGN_ID == TEIAssignid).ToList();
                //}

                //callsList = db.TELE_ENQUIRY_INFO.Where(u => u.TEI_ENQUIRY_TYPE == TEI_ENQUIRY_TYPE && u.TEI_CALL_DATE < TEI_CALL_DATE && u.TEI_ID == TEI_ID && u.TEI_OPTION == "To-Days" && u.TEI_CALL_STATE == "N" && u.TEI_SNO == 0 && u.TEI_CALL_TIME == null).ToList();
            }
            if(Sourcefilter!="")
            {
                callsList = callsList.Where(a => a.FirstSourceOfEnquiry.ToUpper() == Sourcefilter.ToUpper()).ToList();
            }


            return callsList;
        }







        /// <summary>
        /// To Bind TELE_CALLER_MASTER table
        /// </summary>
        /// <returns></returns>
        public ActionResult Telecaller()
        {
            var user = _db.TELE_CALLER_MASTER.ToList();
            return Json(new { data = user }, JsonRequestBehavior.AllowGet);
        }


        //To Validate Phonenumber
        public JsonResult IsphoneExists(string phoneNumber)
        {
            using (var db = new jayaEntities())
            {
                var phone = !db.TELE_ENQUIRY_INFO.Any(p => p.TEI_PHONENO == phoneNumber);
                return Json(phone, behavior: JsonRequestBehavior.AllowGet);
            }
        }
        //To Save the Calls
        public ActionResult SaveData(List<string> Array)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                var today = DateTime.Now;
                try
                {
                        foreach (var rowValues in Array)
                        {
                            var values = rowValues.Split(',');
                            var phone = values[0];
                            //     var existingEnquiry = dbContext.TELE_ENQUIRY_INFO
                            //.FirstOrDefault(p => p.TEI_PHONENO == phone);

                            //     if (existingEnquiry == null)
                            //     {
                            var newEnquiry = new TELE_ENQUIRY_INFO
                            {
                                TEI_PHONENO = phone,
                                TEI_CUSTOMER_FIRSTNAME = values[1].ToUpper(),
                                TEI_CUSTOMER_LASTNAME = values[2].ToUpper(),
                                FirstSourceOfEnquiry = values[3].ToUpper(),
                                TEI_ENQUIRY_TYPE = values[4],
                                SourceCd = values[6],
                                PRICE = (values[8] != "") ? Convert.ToDecimal(values[8]) : 0,
                                TEI_ASSIGN_ID = values[9],
                                TEI_ID = values[10],
                                TEI_CALL_DATE = Convert.ToDateTime(values[5]),
                                //TEI_CALL_TIME = today.ToString("t"),
                                TEI_ASSIGN_DATE = today.Date,
                                TEI_ASSING_TIME = today.ToString("t"),
                                TEI_PURCHASE_DATE = Convert.ToDateTime(values[5]),
                                ENQ_SOURCE = values[3],
                                TEI_CALL_STATE = "N",
                                TEI_OPTION = "To-Days",
                                TEI_SNO = 0,
                                TEI_POSSIBILITY = "",

                            };
                            if (newEnquiry.TEI_PHONENO == null || newEnquiry.TEI_PHONENO == "")
                            {
                                return Json(new { success = true, message = "Please Enter the PhoneNumber.." }, JsonRequestBehavior.AllowGet);
                            }

                            db.TELE_ENQUIRY_INFO.Add(newEnquiry);
                            i = db.SaveChanges();
                            
                            //}
                            //else
                            //{
                            //    return Json(new { success = true, message = "PhoneNumber Already Exists...." }, JsonRequestBehavior.AllowGet);

                            //}
                    }
                    if (i > 0)
                    {
                        transaction.Commit();
                        return Json(new { success = true, message = "Data is Saved Successfully" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = true, message = "Data is not Saved" }, JsonRequestBehavior.AllowGet);
                    }


                }

                catch (Exception ex)
                {
                    transaction.Rollback();
                    ////throw ex; // Consider logging the error instead of re-throwing
                }
                return View();
            }
        }

        //To Delete the Calls
        [HttpPost]
        public JsonResult Delete(int? LeadsId)
        {
            var i = 0;

            using (jayaEntities db = new jayaEntities())
            {

                var emp = db.TELE_ENQUIRY_INFO.Find(LeadsId);
                if (emp != null)
                {
                    //return Json(data: "Not Deleted", behavior: JsonRequestBehavior.AllowGet);
                    db.TELE_ENQUIRY_INFO.Remove(emp);
                    i = db.SaveChanges();

                }
                if (i > 0)
                {
                    return Json(" Deleted", behavior: JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(
                        "NotDeleted", behavior: JsonRequestBehavior.AllowGet);
                }
            }
        }
       
        #region EXCELIMPORT
        //To Save the Excelrecords
        public ActionResult SaveExcel(HttpPostedFileBase file, string EnqType, DateTime CallDate, string ReSource, DateTime? PurchaseDate, double? Price, string Assignid, string UserType = "", string User = "" )
        {

            if (ModelState.IsValid)
            {

                try
                {
                    if (file != null)
                    {
                        string path = string.Format("{0}/{1}", Server.MapPath("~/Content/ExcelUpload/"), file.FileName);
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(Server.MapPath("~/Content/ExcelUpload/"));
                        }
                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }
                        file.SaveAs(path);
                        string FirstSourceOfEnq = "";
                        DataTable dt = ReadAsDt(path);

                        DataTable dtnew = new DataTable();
                        dtnew.Columns.Add("TEI_PHONENO");
                        dtnew.Columns.Add("TEI_CUSTOMER_FIRSTNAME");
                        dtnew.Columns.Add("TEI_CUSTOMER_LASTNAME");
                        //dtnew.Columns.Add("TEI_CUSTOMER_COMMENTS");
                        dtnew.Columns.Add("TEI_ENQUIRY_TYPE");
                        dtnew.Columns.Add("TEI_CALL_DATE");
                        dtnew.Columns.Add("TEI_CALLER_TYPE");
                        dtnew.Columns.Add("TEI_ID");
                        dtnew.Columns.Add("TEI_OPTION");
                        dtnew.Columns.Add("TEI_CALL_STATE");
                        dtnew.Columns.Add("TEI_SNO");
                        dtnew.Columns.Add("TEI_ASSIGN_ID");
                        dtnew.Columns.Add("TEI_ASSIGN_DATE");
                        dtnew.Columns.Add("TEI_ASSIGN_TIME");
                        dtnew.Columns.Add("SourceCd");
                        dtnew.Columns.Add("SourceCategory");
                        dtnew.Columns.Add("TEI_PURCHASE_DATE");
                        dtnew.Columns.Add("PRICE");
                        //dtnew.Columns.Add("PurchaseId");
                        //dtnew.Columns.Add("Purchase_BatchId");
                        dtnew.Columns.Add("FirstSourceOfEnquiry");
                        dtnew.Columns.Add("ENQ_SOURCE");

                        if (dt.Rows.Count > 0)
                        {
                            if (System.IO.File.Exists(path))
                            {
                                System.IO.File.Delete(path);
                            }
                            //int i = 0;                            
                            PurchaseDate = (PurchaseDate == null) ? DateTime.Now : PurchaseDate;
                            foreach (DataRow dr in dt.Rows)
                            {
                                string EnqSource = "", sourceCatg = "";

                                string source = dr[3].ToString();

                                string phone = dr[0].ToString().Trim();
                                var count = db.TELE_ENQUIRY_INFO.Where(t => t.TEI_PHONENO == phone).Count();
                                if (count == 0)
                                {
                                    if (ReSource != "")
                                    {
                                        ReSource = ReSource.ToUpper().Trim();
                                        sourceCatg = (source != null && source != "") ? source.ToUpper().Trim() : "";
                                        //purchaseId = (db.TELE_ENQUIRY_INFO.Max(x => x.PurchaseId) ?? 0) + 1;
                                        //purchase_BatchId = (_db.TELE_ENQUIRY_INFO.Where(x => x.SourceCd == ReSource).Max(x => x.Purchase_BatchId) ?? 0) + 1;
                                        FirstSourceOfEnq = ReSource.ToUpper().Trim();
                                    }
                                    else
                                    {
                                        if (source != "")
                                        {
                                            FirstSourceOfEnq = source.ToUpper().Trim();

                                        }
                                        else
                                        {
                                            FirstSourceOfEnq = "To-Days";
                                        }
                                    }

                                    dtnew.Rows.Add(dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), EnqType, CallDate, UserType, User, "To-Days", "N", 0, Assignid, DateTime.Now, DateTime.Now.ToString("hh:mm tt"), ReSource, sourceCatg, PurchaseDate, Price, FirstSourceOfEnq, EnqSource);
                                    //dtnew.Rows.Add(dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), EnqType, CallDate, UserType, User, "To-Days", "N", 0, GlobalClass.globUserId, DateTime.Now, DateTime.Now.ToString("hh:mm tt"), ReSource, PurchaseDate, Price, FirstSourceOfEnq);
                                }
                            }
                            int bulksaver = 0;
                            if (dtnew.Rows.Count > 0)
                            {
                                //Removing Duplicate Records
                                DataTable dtValidated = dtnew.AsEnumerable().GroupBy(x => x.Field<string>("TEI_PHONENO"))
                                       .Select(y => y.First()).CopyToDataTable();

                                using (SqlConnection bulkcopyconn = new SqlConnection(ConfigurationManager.ConnectionStrings["jayaConnectionString"].ConnectionString))
                                {
                                    bulkcopyconn.Open();
                                    var sqltrans = bulkcopyconn.BeginTransaction();
                                    // Set up the bulk copy object.
                                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(bulkcopyconn, SqlBulkCopyOptions.Default, sqltrans))
                                    {
                                        bulkCopy.DestinationTableName = "Master.TELE_ENQUIRY_INFO";

                                        // Set up the column mappings by name.
                                        SqlBulkCopyColumnMapping mapPhone =
                                            new SqlBulkCopyColumnMapping("TEI_PHONENO", "TEI_PHONENO");
                                        bulkCopy.ColumnMappings.Add(mapPhone);

                                        SqlBulkCopyColumnMapping mapfName =
                                            new SqlBulkCopyColumnMapping("TEI_CUSTOMER_FIRSTNAME", "TEI_CUSTOMER_FIRSTNAME");
                                        bulkCopy.ColumnMappings.Add(mapfName);

                                        SqlBulkCopyColumnMapping maplName =
                                            new SqlBulkCopyColumnMapping("TEI_CUSTOMER_LASTNAME", "TEI_CUSTOMER_LASTNAME");
                                        bulkCopy.ColumnMappings.Add(maplName);

                                        //SqlBulkCopyColumnMapping mapComments =
                                        //    new SqlBulkCopyColumnMapping("TEI_CUSTOMER_COMMENTS", "TEI_CUSTOMER_COMMENTS");
                                        //bulkCopy.ColumnMappings.Add(mapComments);

                                        SqlBulkCopyColumnMapping mapEnqType =
                                            new SqlBulkCopyColumnMapping("TEI_ENQUIRY_TYPE", "TEI_ENQUIRY_TYPE");
                                        bulkCopy.ColumnMappings.Add(mapEnqType);

                                        SqlBulkCopyColumnMapping mapDate =
                                            new SqlBulkCopyColumnMapping("TEI_CALL_DATE", "TEI_CALL_DATE");
                                        bulkCopy.ColumnMappings.Add(mapDate);
                                        
                                        SqlBulkCopyColumnMapping mapUserType =
                                            new SqlBulkCopyColumnMapping("TEI_CALLER_TYPE", "TEI_CALLER_TYPE");
                                        bulkCopy.ColumnMappings.Add(mapUserType);
                                       
                                        SqlBulkCopyColumnMapping mapUser =
                                            new SqlBulkCopyColumnMapping("TEI_ID", "TEI_ID");
                                        bulkCopy.ColumnMappings.Add(mapUser);

                                        SqlBulkCopyColumnMapping mapOption =
                                            new SqlBulkCopyColumnMapping("TEI_OPTION", "TEI_OPTION");
                                        bulkCopy.ColumnMappings.Add(mapOption);

                                        SqlBulkCopyColumnMapping mapCallState =
                                            new SqlBulkCopyColumnMapping("TEI_CALL_STATE", "TEI_CALL_STATE");
                                        bulkCopy.ColumnMappings.Add(mapCallState);

                                        SqlBulkCopyColumnMapping mapSno =
                                            new SqlBulkCopyColumnMapping("TEI_SNO", "TEI_SNO");
                                        bulkCopy.ColumnMappings.Add(mapSno);

                                        SqlBulkCopyColumnMapping mapAssignId =
                                            new SqlBulkCopyColumnMapping("TEI_ASSIGN_ID", "TEI_ASSIGN_ID");
                                        bulkCopy.ColumnMappings.Add(mapAssignId);

                                        SqlBulkCopyColumnMapping mapAssignDate =
                                            new SqlBulkCopyColumnMapping("TEI_ASSIGN_DATE", "TEI_ASSIGN_DATE");
                                        bulkCopy.ColumnMappings.Add(mapAssignDate);

                                        SqlBulkCopyColumnMapping mapAssignTime =
                                            new SqlBulkCopyColumnMapping("TEI_ASSIGN_TIME", "TEI_ASSING_TIME");
                                        bulkCopy.ColumnMappings.Add(mapAssignTime);

                                        SqlBulkCopyColumnMapping mapSourceCd =
                                            new SqlBulkCopyColumnMapping("SourceCd", "SourceCd");
                                        bulkCopy.ColumnMappings.Add(mapSourceCd);

                                        SqlBulkCopyColumnMapping mapPurchaseDt =
                                           new SqlBulkCopyColumnMapping("TEI_PURCHASE_DATE", "TEI_PURCHASE_DATE");
                                        bulkCopy.ColumnMappings.Add(mapPurchaseDt);

                                        SqlBulkCopyColumnMapping mapPrice =
                                           new SqlBulkCopyColumnMapping("PRICE", "PRICE");
                                        bulkCopy.ColumnMappings.Add(mapPrice);

                                        SqlBulkCopyColumnMapping mapSourceCatg =
                                           new SqlBulkCopyColumnMapping("SourceCategory", "SourceCategory");
                                        bulkCopy.ColumnMappings.Add(mapSourceCatg);


                                        SqlBulkCopyColumnMapping mapfrstsource =
                                           new SqlBulkCopyColumnMapping("FirstSourceOfEnquiry", "FirstSourceOfEnquiry");
                                        bulkCopy.ColumnMappings.Add(mapfrstsource);

                                        SqlBulkCopyColumnMapping mapenqsource =
                                           new SqlBulkCopyColumnMapping("ENQ_SOURCE", "ENQ_SOURCE");
                                        bulkCopy.ColumnMappings.Add(mapenqsource);
                                        try
                                        {
                                            bulkCopy.WriteToServer(dtValidated);
                                            //bulkCopy.WriteToServer(dtnew);
                                            sqltrans.Commit();
                                            bulksaver = 1;
                                        }
                                        catch (Exception ex)
                                        {
                                            sqltrans.Rollback();
                                            Console.WriteLine(ex.Message);
                                            //throw;
                                        }
                                    }
                                    if (bulksaver > 0)
                                    {
                                        return Json(new { success = true, message = "File Uploaded Successfully." }, JsonRequestBehavior.AllowGet);
                                    }
                                    else
                                    {
                                        return Json(new { success = false, message = "File Not Uploaded." }, JsonRequestBehavior.AllowGet);
                                    }
                                }
                            }
                            else
                            {
                                return Json(new { success = false, message = "Please Select Valid File Contains All Duplicate Numbers.." }, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            return Json(new { success = false, message = "File Is Empty.Please Select Valid File..." }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return View("Index");

        }
        //To Extract Data from Excel to DataTable
        public DataTable ReadAsDt(string filepath)
        {
            DataTable data = new DataTable();
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(filepath, false))
            {
                WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                IEnumerable<Sheet> sheets = spreadsheetDocument.WorkbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>();
                string relationshipid = sheets.First().Id.Value;
                WorksheetPart worksheetPart = (WorksheetPart)spreadsheetDocument.WorkbookPart.GetPartById(relationshipid);
                Worksheet worksheet = worksheetPart.Worksheet;
                SheetData sheetData = worksheet.GetFirstChild<SheetData>();
                IEnumerable<Row> rows = sheetData.Descendants<Row>();
                foreach (Cell cell in rows.ElementAt(0))
                {
                    if (cell != null)
                    {
                        data.Columns.Add(GetCellValue(spreadsheetDocument, cell));
                    }
                }
                foreach (Row row in rows)
                {
                    DataRow dataRow = data.NewRow();
                    for (int i = 0; i < row.Descendants<Cell>().Count(); i++)
                    {
                        //dataRow[i] = GetCellValue(spreadsheetDocument, row.Descendants<Cell>().ElementAt(i));
                        Cell cell = row.Descendants<Cell>().ElementAt(i);
                        int actualCellIndex = CellReferenceToIndex(cell);
                        dataRow[actualCellIndex] = GetCellValue(spreadsheetDocument, cell);
                    }

                    //int columnIndex = 0;
                    //foreach (Cell cell in row.Descendants<Cell>())
                    //{
                    //    // Gets the column index of the cell with data
                    //    int cellColumnIndex = (int)GetColumnIndexFromName(GetColumnName(cell.CellReference));
                    //    cellColumnIndex--; //zero based index
                    //    if (columnIndex < cellColumnIndex)
                    //    {
                    //        do
                    //        {
                    //            dataRow[columnIndex] = ""; //Insert blank data here;
                    //            columnIndex++;
                    //        }
                    //        while (columnIndex < cellColumnIndex);
                    //    }
                    //    dataRow[columnIndex] = GetCellValue(spreadsheetDocument, cell);

                    //    columnIndex++;
                    //}
                    data.Rows.Add(dataRow);
                }
            }
            data.Rows.RemoveAt(0);
            for (int j = data.Rows.Count - 1; j >= 0; j--)
            {
                if (data.Rows[j][1] == DBNull.Value)
                {
                    data.Rows[j].Delete();
                }
            }
            data.AcceptChanges();
            return data;
        }
        //To Get Cell Value
        private string GetCellValue(SpreadsheetDocument document, Cell cell)
        {
            SharedStringTablePart stringTablePart = document.WorkbookPart.SharedStringTablePart;
            string value = "";
            if (cell.CellValue != null)
            {
                value = cell.CellValue.InnerXml;
            }
            //string value = cell.CellValue.InnerXml;
            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
            {
                return stringTablePart.SharedStringTable.ChildElements[Int32.Parse(value)].InnerText;
            }
            else
            {
                return value;
            }
        }
        //To get CellIndex
        private static int CellReferenceToIndex(Cell cell)
        {
            int index = 0;
            string reference = cell.CellReference.ToString().ToUpper();
            foreach (char ch in reference)
            {
                if (Char.IsLetter(ch))
                {
                    int value = (int)ch - (int)'A';
                    index = (index == 0) ? value : ((index + 1) * 26) + value;
                }
                else
                    return index;
            }
            return index;
        }
        #endregion
        //To ReAssignCalls to User
        [HttpPost]
        public ActionResult ReassignedData(List<TELE_ENQUIRY_INFO> SelectedRows,string UserId)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                var today = DateTime.Now;
                try
                {

                    //if (UserId != "")
                    //{
                    using (var db = new jayaEntities())
                    {
                        // Update existing rows in bulk
                        var existingPhoneNumbers = SelectedRows.Select(row => row.TEI_PHONENO).ToList();
                        var existingEnquiries = db.TELE_ENQUIRY_INFO
                            .Where(e => existingPhoneNumbers.Contains(e.TEI_PHONENO))
                            .ToList();

                        foreach (var er in existingEnquiries)
                        {
                            er.TEI_CALL_STATE = "O";
                            db.Entry(er).State = EntityState.Modified;

                        }
                        // Add new rows
                        foreach (var selectedRow in SelectedRows)
                        {
                            var newEnquiry = new TELE_ENQUIRY_INFO
                            {
                                TEI_PHONENO = selectedRow.TEI_PHONENO,
                                TEI_ID = selectedRow.TEI_ID,
                                //TEI_ID = UserId,
                                TEI_CUSTOMER_FIRSTNAME = selectedRow.TEI_CUSTOMER_FIRSTNAME,
                                TEI_CUSTOMER_LASTNAME = selectedRow.TEI_CUSTOMER_LASTNAME,
                                FirstSourceOfEnquiry = selectedRow.FirstSourceOfEnquiry,
                                TEI_ENQUIRY_TYPE = selectedRow.TEI_ENQUIRY_TYPE,
                                SourceCd = selectedRow.SourceCd,
                                PRICE = selectedRow.PRICE,
                                TEI_ASSIGN_ID = selectedRow.TEI_ASSIGN_ID,
                                TEI_CALL_DATE = today.Date,
                              //  TEI_CALL_TIME = today.ToString("t"),
                                TEI_ASSIGN_DATE = today,
                                TEI_ASSING_TIME = today.ToString("t"),
                                TEI_PURCHASE_DATE = today.Date,
                                ENQ_SOURCE = selectedRow.ENQ_SOURCE,
                                TEI_CALL_STATE = "N",
                                TEI_OPTION = "To-Days",
                                TEI_POSSIBILITY = "",
                                TEI_SNO = selectedRow.TEI_SNO + 1
                            };

                            db.TELE_ENQUIRY_INFO.Add(newEnquiry);
                        }

                        // Save changes to the database
                        i = db.SaveChanges();
                        transaction.Commit();
                    }
                    if (i > 0)
                    {
                        return Json(new { success = true, message = "Calls Assigned  Succesfully" });
                    }
                    else
                    {
                        return Json(new { success = false, message = "Calls are not Assigned" });
                    }
                    ////}
                    //else
                    //{
                    //    return Json(new { success = true, message = "Please select user... " });
                    //}

                }

                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;

                }
            }
        }
        public ActionResult GetCallsData(string TEI_ENQUIRY_TYPE, DateTime TEI_CALL_DATE, string TEI_ID, string AssignId ,string Flag = "", string Sourcnamefltr = "")
        {
            _db.Configuration.ProxyCreationEnabled = false;
            if (TEI_ID == "A")
            {
                if (AssignId != null)
                {
                    TEI_ID = AssignId;
                }
                else
                {
                    TEI_ID = "";
                }
            }
            List<TELE_ENQUIRY_INFO> callList = GetList(Flag, TEI_ENQUIRY_TYPE, TEI_CALL_DATE, AssignId, TEI_ID, Sourcnamefltr);
            return Json(new { data = callList }, JsonRequestBehavior.AllowGet);
        }
        //ReAssigning Calls withCount
        public ActionResult AssignCallsWithCount(List<AssignCalls_VM> objlist, string AssignID)
        {
           
            var today = DateTime.Now;
            
            var transaction = db.Database.BeginTransaction();
            try
            {
                if (objlist.Count > 0)
                {
                    if (objlist[0].TEI_ID == "A")
                    {
                        if (AssignID != null)
                        {
                            objlist[0].TEI_ID = AssignID;
                        }
                        else
                        {
                            objlist[0].TEI_ID = "";
                        }
                    }
                    
                    string EnquiryType = objlist[0].TEI_ENQUIRY_TYPE;
                    string TEIId = objlist[0].TEI_ID;
                    objlist[0].Flag = (objlist[0].Flag != null) ? objlist[0].Flag : "";
                    objlist[0].FirstSourceOfEnquiry = (objlist[0].FirstSourceOfEnquiry != null) ? objlist[0].FirstSourceOfEnquiry : "";
                    //List<TELE_ENQUIRY_INFO> callList = GetList(objlist[0].Flag, EnquiryType, DateTime.Now.Date, TEIId, AssignID, objlist[0].FirstSourceOfEnquiry);
                    List<TELE_ENQUIRY_INFO> callList = GetList(objlist[0].Flag, EnquiryType, DateTime.Now.Date, TEIId, AssignID, objlist[0].FirstSourceOfEnquiry);
                    if (callList.Count > 0)
                    {
                        //DataTable dtAssign = GlobalClass.GetReAssignTable();
                        int j = 0;
                        string Phone = "";
                        foreach (var user in objlist)
                        {
                            int k;
                            int L = 0;
                            for (k = j; k < callList.Count; k++)
                            {
                                if (L == objlist[0].AssignCount)
                                {
                                    break;
                                }
                                Phone = callList[i].TEI_PHONENO;
                                var existingEnquiries = db.TELE_ENQUIRY_INFO.Where(t => t.TEI_PHONENO.Contains(Phone)).OrderByDescending(t => t.TEI_PHONENO).FirstOrDefault();
                                existingEnquiries.TEI_CALL_STATE = "O";
                                db.Entry(existingEnquiries).State = EntityState.Modified;
                                db.SaveChanges();
                                if (existingEnquiries != null)
                                {
                                    if (objlist[0].Flag == "UA")
                                    {
                                        existingEnquiries.TEI_CALL_DATE = DateTime.Now.Date;
                                        existingEnquiries.TEI_CALL_TIME = null;
                                    }
                                    var newEnquiry = new TELE_ENQUIRY_INFO
                                    {
                                        TEI_PHONENO = existingEnquiries.TEI_PHONENO,
                                        TEI_ID = user.TCM_ID,
                                        //TEI_ID = UserId,
                                        TEI_CUSTOMER_FIRSTNAME = existingEnquiries.TEI_CUSTOMER_FIRSTNAME,
                                        TEI_CUSTOMER_LASTNAME = existingEnquiries.TEI_CUSTOMER_LASTNAME,
                                        FirstSourceOfEnquiry = existingEnquiries.FirstSourceOfEnquiry,
                                        TEI_ENQUIRY_TYPE = existingEnquiries.TEI_ENQUIRY_TYPE,
                                        SourceCd = existingEnquiries.SourceCd,
                                        PRICE = existingEnquiries.PRICE,
                                        TEI_ASSIGN_ID = existingEnquiries.TEI_ASSIGN_ID,
                                        TEI_CALL_DATE = today.Date,
                                        TEI_CALL_TIME = today.ToString("t"),
                                        TEI_ASSIGN_DATE = today,
                                        TEI_ASSING_TIME = today.ToString("t"),
                                        TEI_PURCHASE_DATE = today.Date,
                                        ENQ_SOURCE = existingEnquiries.ENQ_SOURCE,
                                        TEI_CALL_STATE = "N",
                                        TEI_OPTION = "To-Days",
                                        TEI_POSSIBILITY = "",
                                        TEI_SNO = existingEnquiries.TEI_SNO + 1
                                    };

                                    db.TELE_ENQUIRY_INFO.Add(newEnquiry);
                                    i = db.SaveChanges();
                                }
                                //}

                                L++;
                                // }
                                j = k;
                                if (j == callList.Count)
                                {
                                    break;
                                }
                            }
                        }




                        if (i > 0)
                        {
                            transaction.Commit();
                            return Json(new { success = true, message = "Calls Assigned Successfully..." }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new { success = false, message = "Calls are Not  Assigned ..." }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json(new { success = false, message = "Calls  Not  Exit to Assign ..." }, JsonRequestBehavior.AllowGet);
                    }
                }
                

             
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                //Console.WriteLine(ex.Message);
                throw ex;
            }
            return View();
        }

        public ActionResult GetMediaEnquiryInfo( string enquiry_Type,DateTime enquiry_Date)

        {
            var mediaTable = _db.MEDIA_ENQUIRY_INFO.ToList();
            //var mediaTable = _db.MEDIA_ENQUIRY_INFO.Where(m=>m.M_DateTime== enquiry_Date&&m.M_EnquiryType== enquiry_Type).ToList();
            return Json(new { data = mediaTable }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSources()
        {
            var Sources = (from m1 in db.MEDIAMASTERs
                           where m1.IsActive == true
                           select new { MediaId = m1.MediaName, MediaName = m1.MediaName }).Union(
                        (from m2 in db.TELE_ENQUIRY_INFO
                         where m2.OTHER_ENQ_SOURCE != null && m2.OTHER_ENQ_SOURCE != ""
                         select new { MediaId = m2.OTHER_ENQ_SOURCE, MediaName = m2.OTHER_ENQ_SOURCE })).Union(
                        (from m3 in db.TELE_ENQUIRY_INFO
                         where m3.ENQ_SOURCE != "MEDIA" && m3.ENQ_SOURCE != "OTHERS" && m3.ENQ_SOURCE != "CAMPAIGNS"
                         && m3.ENQ_SOURCE != "" && m3.ENQ_SOURCE != null
                         select new { MediaId = m3.ENQ_SOURCE, MediaName = m3.ENQ_SOURCE })).Union(
                (from m4 in db.TELE_ENQUIRY_INFO where m4.FirstSourceOfEnquiry!="MEDIA"&& m4.FirstSourceOfEnquiry!="OTHERS" &&m4.FirstSourceOfEnquiry!="CAMPIGNS"
                 && m4.FirstSourceOfEnquiry != "" && m4.FirstSourceOfEnquiry != null
                 select new { MediaId = m4.FirstSourceOfEnquiry, MediaName = m4.FirstSourceOfEnquiry })).Union(
                        (from m5 in db.TELE_ENQUIRY_INFO
                         where m5.SourceCd != "" && m5.SourceCd != null
                         select new { MediaId = m5.SourceCd.ToUpper(), MediaName = m5.SourceCd.ToUpper() })).
                         OrderBy(m => m.MediaName).Distinct().ToList();
            return Json(Sources, JsonRequestBehavior.AllowGet);
        }









        //ReAssigningCalls to user in ADO.Net

        //public ActionResult AssignCallsWithCount(List<AssignCalls_VM> objlist, string AssignID)
        //{
        //    //if (!string.IsNullOrEmpty(Convert.ToString(Session["User"])))
        //    //{
        //    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["jayaConnectionString"].ToString()))
        //    {
        //        con.Open();
        //        SqlCommand updatecmd = new SqlCommand();
        //        updatecmd.Connection = con;
        //        sqltrans = con.BeginTransaction();
        //        updatecmd.Transaction = sqltrans;
        //        string cmdtext = "";
        //        try
        //        {
        //            if (objlist.Count > 0)
        //            {
        //                _db.Configuration.ProxyCreationEnabled = false;

        //                if (objlist.Count > 0)
        //                {
        //                    if (objlist[0].TEI_ID == "A")
        //                    {
        //                        if (AssignID != null)
        //                        {
        //                            objlist[0].TEI_ID = AssignID;
        //                        }
        //                        else
        //                        {
        //                            objlist[0].TEI_ID = "";
        //                        }
        //                    }
        //                    //else
        //                    //{
        //                    //    if (objlist[0].TEI_ID == "" || objlist[0].TEI_ID == null)
        //                    //    {
        //                    //        objlist[0].TEI_ID = objlist[0].ReAssignUserId;
        //                    //    }
        //                    //}
        //                    string EnquiryType = objlist[0].TEI_ENQUIRY_TYPE;
        //                    string TEIId = objlist[0].TEI_ID;
        //                    objlist[0].Flag = (objlist[0].Flag != null) ? objlist[0].Flag : "";
        //                    objlist[0].FirstSourceOfEnquiry = (objlist[0].FirstSourceOfEnquiry != null) ? objlist[0].FirstSourceOfEnquiry : "";
        //                    List<TELE_ENQUIRY_INFO> callList = GetList(objlist[0].Flag, EnquiryType, DateTime.Now.Date, TEIId, AssignID, objlist[0].FirstSourceOfEnquiry);
        //                    if (callList.Count > 0)
        //                    {
        //                        DataTable dtAssign = GlobalClass.GetReAssignTable();
        //                        if (conn.State == ConnectionState.Closed)
        //                        {
        //                            conn.Open();
        //                        }
        //                        int j = 0;
        //                        string Phone = "";
        //                        foreach (var user in objlist)
        //                        {
        //                            int i;
        //                            int L = 0;
        //                            //for (int i = j; i < objlist[0].AssignCount; i++)
        //                            for (i = j; i < callList.Count; i++)
        //                            {
        //                                if (L == objlist[0].AssignCount)
        //                                {
        //                                    break;
        //                                }
        //                                Phone = callList[i].TEI_PHONENO;
        //                                //if (Phone.Length > 10 && !Phone.StartsWith("0"))
        //                                //{
        //                                //    callList[i].TEI_PHONENO = callList[i].TEI_PHONENO.Substring(2);
        //                                //}
        //                                SqlCommand maxcmd = new SqlCommand("select max(TEI_SNO) + 1 as TEI_SNO from Master.TELE_ENQUIRY_INFO where TEI_PHONENO like '%" + Phone.Trim() + "%'", conn);
        //                                var MaxNo = maxcmd.ExecuteScalar();
        //                                if (MaxNo.Equals(DBNull.Value))
        //                                {
        //                                    MaxNo = 1;
        //                                }
        //                                //SqlCommand updatecmd = new SqlCommand("update Master.TELE_ENQUIRY_INFO set TEI_CALL_STATE = 'O' where TEI_PHONENO like '%" + Phone.Trim() + "%'", conn);
        //                                //if (updatecmd.ExecuteNonQuery() >= 0)
        //                                //{
        //                                //var er = _db.TELE_ENQUIRY_INFO.Where(t => t.TEI_PHONENO.Contains(Phone)).OrderByDescending(t => t.TEI_PHONENO).FirstOrDefault();
        //                                var er = db.TELE_ENQUIRY_INFO.Where(t => t.TEI_PHONENO.Contains(Phone)).OrderByDescending(t => t.TEI_PHONENO).FirstOrDefault();
        //                                if (er != null)
        //                                {
        //                                    //{
        //                                    //    user.TCM_DESIGNATION = Convert.ToInt32(GlobalClass.GetDesigWitUser(user.TCM_ID));
        //                                    //    string C_Name = _db.TELE_CALLER_MASTER.Where(t => t.TCM_ID == user.TCM_ID.Trim()).Select(c => c.TCM_NAME).FirstOrDefault();
        //                                    //    er.TEI_CALL_LOGS = "Call Transferred From " + Session["User"].ToString() + "," + User.Identity.Name + " To " + user.TCM_ID.Trim() + "," + C_Name;
        //                                    //    if (objlist[0].ReAssignUserId != "")
        //                                    //    {
        //                                    //        string T_Name = _db.TELE_CALLER_MASTER.Where(t => t.TCM_ID == TEIId).Select(c => c.TCM_NAME).FirstOrDefault();
        //                                    //        er.TEI_CALL_LOGS = "Call Transferred From " + TEIId + "," + T_Name + " To " + user.TCM_ID.Trim() + "," + C_Name;
        //                                    //    }
        //                                    if (objlist[0].Flag == "UA")
        //                                    {
        //                                        er.TEI_CALL_DATE = DateTime.Now.Date;
        //                                        er.TEI_CALL_TIME = null;
        //                                    }
        //                                    //dtAssign.Rows.Add(er.TEI_PHONENO, user.TCM_ID, er.TEI_CUSTOMERNAME, er.TEI_OPTION, Convert.ToDateTime(er.TEI_CALL_DATE), DateTime.Now.Date, DateTime.Now.ToString("hh:mm tt"),
        //                                    //    GlobalClass.globUserId, er.TEI_ENQUIRY_TYPE, MaxNo, user.TCM_DESIGNATION.ToString(), 'N', er.TEI_CALL_LOGS, er.SourceCd, Convert.ToDateTime(er.TEI_PURCHASE_DATE), er.PRICE);
        //                                    dtAssign.Rows.Add(0, er.TEI_PHONENO, user.TCM_ID, er.TEI_CUSTOMER_FIRSTNAME, er.TEI_CUSTOMER_LASTNAME, er.TEI_CALLBACK_DATE, er.TEI_CALLBACK_TIME, er.TEI_CALLER_REMARKS,
        //                               er.TEI_POSSIBILITY, er.TEI_STAGE, er.TEI_EXECUTIVE_CODE, er.TEI_OPTION, er.TEI_CALL_DATE, er.TEI_CALL_TIME, er.TEI_ENQUIRYFOR, er.TEI_REVIEWEDBY,
        //                               DateTime.Now, DateTime.Now.ToString("hh:mm tt"), AssignID, er.TEI_ENQUIRY_TYPE, MaxNo, user.TCM_DESIGNATION.ToString(), 'N',
        //                               er.TEI_CUSTOMER_COMMENTS, er.TEI_CALL_LOGS, er.SourceCd, er.TEI_VENTUIRE,
        //                               er.ENQ_VENTURE, er.ENQ_SOURCE, er.OTHER_ENQ_SOURCE, er.MEDIA_ENQ_SOURCE, er.MEDIA_SUB_ENQ_SOURCE, er.CampaignCode, er.REF_VENTURE,
        //                               er.REF_CODE, er.WALKIN_TYPE, er.TEI_PURCHASE_DATE, er.PRICE, er.SourceCategory, er.PurchaseId, er.Purchase_BatchId, er.FirstSourceOfEnquiry, er.OTHER_STATUS, er.CUSTLEAD_ID, er.CUSTLEAD_CTRL);
        //                                }
        //                                //}
        //                                //updatecmd.CommandText = "update Master.TELE_ENQUIRY_INFO set TEI_CALL_STATE = 'O' where TEI_PHONENO like '%" + Phone.Trim() + "%'";
        //                                //updatecmd.ExecuteNonQuery();
        //                                cmdtext += "update Master.TELE_ENQUIRY_INFO set TEI_CALL_STATE = 'O' where TEI_PHONENO like '%" + Phone.Trim() + "%'";
        //                                L++;
        //                            }
        //                            j = i;
        //                            if (j == callList.Count)
        //                            {
        //                                break;
        //                            }
        //                        }
        //                        if (dtAssign.Rows.Count > 0)
        //                        {
        //                            updatecmd.CommandText = cmdtext;
        //                            updatecmd.ExecuteNonQuery();
        //                            using (var bulkInsert = new SqlBulkCopy(con, SqlBulkCopyOptions.Default, sqltrans))
        //                            {
        //                                bulkInsert.DestinationTableName = "Master.TELE_ENQUIRY_INFO";
        //                                bulkInsert.WriteToServer(dtAssign);
        //                                sqltrans.Commit();
        //                                return Json(new { success = true, message = "Calls Assigned Successfully..." }, JsonRequestBehavior.AllowGet);
        //                            }
        //                        }
        //                        else
        //                        {
        //                            sqltrans.Rollback();
        //                            return Json(new { success = false, message = "Calls Not Assigned Successfully..." }, JsonRequestBehavior.AllowGet);
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            sqltrans.Rollback();
        //            Console.WriteLine(ex.Message);
        //            //throw;
        //        }
        //    }

        //    //else
        //    //{
        //    //    //Response.Cookies.Add(new System.Web.HttpCookie("returnUrl", Request.Url.PathAndQuery));
        //    //    return RedirectToAction("Login", "Home");
        //    //}
        //    //ViewBag.Designations = _db.TELE_DESIGNATION_MASTER.Where(d => d.Active == true).ToList();
        //    return View();
        //}





























    }
    
}
