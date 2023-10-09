using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

using AdminDashboard.Models;
using AdminDashboard.ViewModels;
using Newtonsoft.Json;

namespace AdminDashboard.Controllers
{
    public class WebAPIConsumeController : Controller
    {
        jayaEntities db = new jayaEntities();
        CRM2022Entities _db = new CRM2022Entities();
        string apiBaseAddress = "http://localhost:52680/api/TelexApi/";
     
        
        public ActionResult Index()
        {

            return View();
        }
        public async Task<ActionResult> GetCallsFromApi(string user,bool? CheckAssign,string enquiryType, string Sourcnamefltr,string AssignType="",string TEIid = null)
        {
            List<TELE_ENQUIRY_INFO> callsList = new List<TELE_ENQUIRY_INFO>();

            using (HttpClient client = new HttpClient())
            {
                string queryString = $"user={user}&CheckAssign={CheckAssign}&enquiryType={enquiryType}&Sourcnamefltr={Sourcnamefltr}&AssignType={AssignType}&TEI_ID={TEIid}";
                client.BaseAddress = new Uri(apiBaseAddress);
                HttpResponseMessage response = await client.GetAsync($"GetCalls?{queryString}");
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    callsList = JsonConvert.DeserializeObject<List<TELE_ENQUIRY_INFO>>(responseBody);
                }
                return Json(new { data = callsList }, JsonRequestBehavior.AllowGet);
            }
        
           
        }

        [HttpPost]
        public async Task<ActionResult> SaveLeads(List<string> model,bool? CheckAssign)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Set the base URL for your API
                    client.BaseAddress = new Uri(apiBaseAddress);
                  

                    // Define the content type for the request (JSON in this case)
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                 
                    // Send an HTTP POST request to the API endpoint
                    HttpResponseMessage response = await client.PostAsJsonAsync($"SaveLeads?CheckAssign={CheckAssign}", model);

                    // Check if the request was successful
                    if (response.IsSuccessStatusCode)
                    {
                        // Handle the successful response here, e.g., display a success message
                        string result = await response.Content.ReadAsStringAsync();
                        return Json(new { success = true, message = result }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        // Handle the case where the request was not successful, e.g., display an error message
                        string errorResponse = await response.Content.ReadAsStringAsync();
                        return Json(new { success = false, message = "Data is not saved" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
 
        public async Task<ActionResult> DeleteCall(string LeadsId) // Pass the ID or any other parameter needed for deletion
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    
                    client.BaseAddress = new Uri(apiBaseAddress);

                    // Send an HTTP DELETE request to the API endpoint with the specified ID
                    HttpResponseMessage response = await client.DeleteAsync($"DeleteTeleInfo?Leadsid={LeadsId}");

                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        string errorMessage = await response.Content.ReadAsStringAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> ReAssignCalls(List<AssignCalls_VM> objlist,bool?CheckAssign,string User)
        {
            try
            {
                // Create an instance of HttpClient
                using (HttpClient client = new HttpClient())
                {
                    string queryString = $"User={User}&CheckAssign={CheckAssign}";
                    // Set the base URL for your API
                    client.BaseAddress = new Uri(apiBaseAddress);

                    // Define the content type for the request (JSON in this case)
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    // Send an HTTP POST request to the API endpoint
                    HttpResponseMessage response = await client.PostAsJsonAsync($"ReAssignCallswithCount?{queryString}", objlist);

                    // Check if the request was successful
                    if (response.IsSuccessStatusCode)
                    {
                        // Handle the successful response here, e.g., display a success message
                        string result = await response.Content.ReadAsStringAsync();
                        return Json(new { success = true, message = result }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        // Handle the case where the request was not successful, e.g., display an error message
                        string errorResponse = await response.Content.ReadAsStringAsync();
                      //  return Json(new { success = true, message = errorResponse }, JsonRequestBehavior.AllowGet);
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            // Return the view with success or error messages
            
        }
        [HttpPost]
      

        public async Task<ActionResult> UploadFile(HttpPostedFileBase file, string User, bool? CheckAssign, string EnqType)
        {
            try
            {
                // Check if a file was uploaded
                if (file == null || file.ContentLength == 0)
                {
                    ViewBag.Message = "Please select a valid file.";
                    return View();
                }

                using (HttpClient client = new HttpClient())
                {
                    string queryString = $"user={User}&CheckAssign={CheckAssign}&EnqType={EnqType}";
                    // Set the base URL for your API
                    client.BaseAddress = new Uri(apiBaseAddress);

                    // Create form data with the file to upload
                    MultipartFormDataContent form = new MultipartFormDataContent();
                    HttpContent fileContent = new StreamContent(file.InputStream);
                    fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data")
                    {
                        Name = "file",
                        FileName = file.FileName
                    };
                    form.Add(fileContent);

                    // Add any other parameters needed by the API
                   // form.Add(new StringContent(User), "User");
                   //// form.Add(new StringContent(EnqType), "EnqType");
                   // form.Add(new StringContent(CheckAssign.ToString()), "CheckAssign");

                    // Send the POST request to the API
                    HttpResponseMessage response = await client.PostAsync($"ReadFile?{queryString}", form);

                    if (response.IsSuccessStatusCode)
                    {
                        // Read the response content (the message returned by the API)
                        string apiResponse = await response.Content.ReadAsStringAsync();

                        // Handle the response as needed
                        return Json(new { success = true, message= apiResponse }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        string errorResponse = await response.Content.ReadAsStringAsync();
                        return Json(new { success = false, message = " The Excel file uploading has faild." }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ActionResult> GetMediaEnquiryFromAPI(string User,string Enquiry_Type, bool? CheckAssign, string UserID, string Flag = "")
        {
            List<MEDIA_ENQUIRY_INFO> callsList = new List<MEDIA_ENQUIRY_INFO>();

            using (HttpClient client = new HttpClient())
            {
                string queryString = $"user={User}&Enquiry_Type={Enquiry_Type}&Flag={Flag}&CheckAssign={CheckAssign}&UserID={UserID}";
                // Set the base URL for your API
                client.BaseAddress = new Uri(apiBaseAddress);
                // Send an HTTP GET request to the API
                HttpResponseMessage response = await client.GetAsync($"GetMediaEnquiryInfo?{queryString}");
                if (response.IsSuccessStatusCode)
                {
                   string responseBody = await response.Content.ReadAsStringAsync();

                    // Deserialize the string into your model
                    callsList = JsonConvert.DeserializeObject<List<MEDIA_ENQUIRY_INFO>>(responseBody);
                }
               
            }
            return Json(new { data = callsList }, JsonRequestBehavior.AllowGet);

        }


    }



}





    
