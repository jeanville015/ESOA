using ESOA.WEBMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ESOA.Model;
using ESOA.Model.Constants;
using ESOA.Common;
using EPDV.Controllers;
using System.Threading;
using ESOA.Model.Constant;
using DocumentFormat.OpenXml.Spreadsheet;
using ExcelDataReader;
using System.Data;
using System.Collections;
using ArrayToExcel;

namespace ESOA.WEBMVC.Controllers
{
    public class GranularController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        public GranularController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Acceptance()
        { 
            return View();
        }

        public IActionResult Encashment()
        {
            return View();
        }

        public IActionResult Refund()
        {
            return View();
        }
        
        public IActionResult Voided()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ViewFilterOptionsAcceptance(CancellationToken cancellationToken)
        { 
            List<AgentListing> customerList = await CustomerData.GetCustomerListAsync(cancellationToken: cancellationToken);

            //add ALL option in customer list
            customerList.Add(new AgentListing { Index=0, Name = "ALL" }); 
            customerList = customerList.OrderBy(x => x.Index).ToList();

            ViewBag.CustomerList = customerList.Select(x => new NameValuePair { Value = Data.GetString(x.Name), Name = x.Name }).ToList();
            ViewBag.ProductTypeList = ProductType.List();
            return PartialView("Acceptance/_filterOptions");
        }

        [HttpPost]
        public async Task<IActionResult> ViewFilterOptionsEncashment(CancellationToken cancellationToken)
        {

            List<AgentListing> customerList = await CustomerData.GetCustomerListAsync(cancellationToken: cancellationToken);

            //add ALL option in customer list
            customerList.Add(new AgentListing { Index = 0, Name = "ALL" });
            customerList = customerList.OrderBy(x => x.Index).ToList();

            ViewBag.CustomerList = customerList.Select(x => new NameValuePair { Value = Data.GetString(x.Name), Name = x.Name }).ToList();
            ViewBag.ProductTypeList = ProductType.List();
            return PartialView("Encashment/_filterOptions");
        }

        [HttpPost]
        public async Task<IActionResult> ViewFilterOptionsRefund(CancellationToken cancellationToken)
        {

            List<AgentListing> customerList = await CustomerData.GetCustomerListAsync(cancellationToken: cancellationToken);
            ViewBag.CustomerList = customerList.Select(x => new NameValuePair { Value = Data.GetString(x.Name), Name = x.Name }).ToList();
            ViewBag.ProductTypeList = ProductType.List();
            return PartialView("Refund/_filterOptions");
        }

        [HttpPost]
        public async Task<IActionResult> ViewFilterOptionsVoided(CancellationToken cancellationToken)
        {

            List<AgentListing> customerList = await CustomerData.GetCustomerListAsync(cancellationToken: cancellationToken);
            ViewBag.CustomerList = customerList.Select(x => new NameValuePair { Value = Data.GetString(x.Name), Name = x.Name }).ToList();
            ViewBag.ProductTypeList = ProductType.List();
            return PartialView("Voided/_filterOptions");
        }

        [HttpPost]
        public async Task<IActionResult> SearchAcceptance(string[] customers, string dateFrom, string dateTo, string productType, string search, CancellationToken cancellationToken)
        {
            if ((customers.Length == 0) && (dateFrom == null) && (dateTo==null) && (productType==null)) {
                List<Acceptance> _result = new List<Acceptance>();
                return PartialView("Acceptance/_List", _result);
            }

            if (customers.Contains("ALL"))
            {
                Array.Clear(customers,0,customers.Length);
            }
            if (productType == ProductType.ALL)
            {
                productType = null;
            }

            List<Acceptance> result = await AcceptanceData.GetAcceptanceListAsync(String.Join(";", customers), dateFrom, dateTo, productType, search, cancellationToken);

            //prepare the variable that will hold the values for excel file import 
            HttpContext.Session.Remove(DefaultValues.SessionObjectExcelExportReport);
            HttpContext.Session.SetObject(DefaultValues.SessionObjectExcelExportReport, result);

            return PartialView("Acceptance/_List", result); 
        }

        [HttpPost]
        public async Task<IActionResult> SearchEncashment(string[] customers, string dateFrom, string dateTo, string productType, string search, CancellationToken cancellationToken)
        {
            if ((customers.Length == 0) && (dateFrom == null) && (dateTo == null) && (productType == null))
            {
                List<Encashment> _result = new List<Encashment>();
                return PartialView("Encashment/_List", _result);
            }

            if (customers.Contains("ALL"))
            {
                Array.Clear(customers, 0, customers.Length);
            }

            List<Encashment> result = await EncashmentData.GetEncashmentListAsync(String.Join(";", customers), dateFrom, dateTo, productType, search, cancellationToken);

            //prepare the variable that will hold the values for excel file import 
            HttpContext.Session.Remove(DefaultValues.SessionObjectExcelExportReport);
            HttpContext.Session.SetObject(DefaultValues.SessionObjectExcelExportReport, result);

            return PartialView("Encashment/_List", result);
        }

        [HttpPost]
        public async Task<IActionResult> SearchRefund(string[] customers, string dateFrom, string dateTo, string productType, string search, CancellationToken cancellationToken)
        {

            if ((customers.Length == 0) && (dateFrom == null) && (dateTo == null) && (productType == null))
            {
                List<Refund> _result = new List<Refund>();
                return PartialView("Refund/_List", _result);
            }
            List<Refund> result = await RefundData.GetRefundListAsync(String.Join(";", customers), dateFrom, dateTo, productType, search, cancellationToken);

            //prepare the variable that will hold the values for excel file import 
            HttpContext.Session.Remove(DefaultValues.SessionObjectExcelExportReport);
            HttpContext.Session.SetObject(DefaultValues.SessionObjectExcelExportReport, result);

            return PartialView("Refund/_List", result);
        } 

        [HttpPost]
        public async Task<IActionResult> SearchVoided(string[] customers, string dateFrom, string dateTo, string productType, string search, CancellationToken cancellationToken)
        {
            if ((customers.Length == 0) && (dateFrom == null) && (dateTo == null) && (productType == null))
            {
                List<Voided> _result = new List<Voided>();
                return PartialView("Voided/_List", _result);
            }
            List<Voided> result = await VoidedData.GetVoidedListAsync(String.Join(";", customers), dateFrom, dateTo, productType, search, cancellationToken);

            //prepare the variable that will hold the values for excel file import 
            HttpContext.Session.Remove(DefaultValues.SessionObjectExcelExportReport);
            HttpContext.Session.SetObject(DefaultValues.SessionObjectExcelExportReport, result);

            return PartialView("Voided/_List", result);
        }

        [HttpGet]
        public async Task<IActionResult> ViewUploadFileAcceptance(CancellationToken cancellationToken)
        {
            string userAccountId = HttpContext.Session.GetString(DefaultValues.SessionUserKeyName); 
            return PartialView("Acceptance/_uploadFile");
        }

        [HttpGet]
        public async Task<IActionResult> ViewUploadFileEncashment(CancellationToken cancellationToken)
        {
            string userAccountId = HttpContext.Session.GetString(DefaultValues.SessionUserKeyName);
            return PartialView("Encashment/_uploadFile");
        }

        [HttpGet]
        public async Task<IActionResult> ViewUploadFileRefund(CancellationToken cancellationToken)
        {
            string userAccountId = HttpContext.Session.GetString(DefaultValues.SessionUserKeyName);
            return PartialView("Refund/_uploadFile");
        }

        [HttpGet]
        public async Task<IActionResult> ViewUploadFileVoided(CancellationToken cancellationToken)
        {
            string userAccountId = HttpContext.Session.GetString(DefaultValues.SessionUserKeyName);
            return PartialView("Voided/_uploadFile");
        }

        [DisableRequestSizeLimit]
        [HttpPost]
        public async Task<IActionResult> UploadFileAcceptance(List<FileUpload> fileUploads, CancellationToken cancellationToken)
        {
            
            string userAccountId = HttpContext.Session.GetString(DefaultValues.SessionUserKeyName);

            // testing upload from excel
            IFormFile _file = Request.Form.Files[0];
            var fileUplaodstream = new MemoryStream(); _file.CopyTo(fileUplaodstream);
            

            ResponseMessage result = new ResponseMessage();

            //check if upload already verified
            result = await CheckUploadVerifyStatus(_file, 3, 0);
            if (result.Reason != null)
            {
                return Json(result);
            }

            DataTableCollection tables = ReadFromExcel(fileUplaodstream);
            try
            {
                foreach (DataTable dt in tables)
                {
                    
                    foreach (DataRow dr in dt.Rows) {
                        Acceptance acceptance = new Acceptance();
                        acceptance.OriginAgentName = dr[0].ToString();
                        acceptance.Tagging = dr[1].ToString();
                        acceptance.OfficeCode = dr[2].ToString();
                        acceptance.TransactionDate = dr[3].ToString();
                        acceptance.ProductType = dr[4].ToString();
                        acceptance.TrackingNumber = dr[5].ToString();
                        acceptance.ReferenceNumber = dr[6].ToString();
                        acceptance.EncashmentBranch = dr[7].ToString();
                        acceptance.ShipperName = dr[8].ToString();
                        acceptance.ConsigneeName = dr[9].ToString();
                        acceptance.Unit = Convert.ToInt32(dr[10]);
                        acceptance.PrincipalAmount = Convert.ToDecimal(dr[11]);
                        acceptance.EncashmentDate = dr[12].ToString();
                        acceptance.StatusCode = dr[13].ToString();
                        acceptance.StatusDescription = dr[14].ToString();
                        acceptance.EncashmentBranchHub = dr[15].ToString();
                        ResponseMessage result_perAcceptanceItem = new ResponseMessage();
                        result_perAcceptanceItem = await AcceptanceData.CreateAcceptanceAsync(acceptance, userAccountId.ToString(), cancellationToken: cancellationToken);
                        if (result_perAcceptanceItem.Status == false)
                        {
                            result.Status = false;
                        }
                        else
                        {
                            result.Status = true;
                        }
                    }
                }
            }
            catch (Exception ex) {
                result.Reason = ex.Message;
            } 
            return Json(result); 
        }

        [DisableRequestSizeLimit]
        [HttpPost]
        public async Task<IActionResult> UploadFileEncashment(List<FileUpload> fileUploads, CancellationToken cancellationToken)
        {

            string userAccountId = HttpContext.Session.GetString(DefaultValues.SessionUserKeyName);

            // testing upload from excel
            IFormFile _file = Request.Form.Files[0]; 
            var fileUplaodstream = new MemoryStream(); _file.CopyTo(fileUplaodstream);

            ResponseMessage result = new ResponseMessage();

            //check if upload already verified
            result = await CheckUploadVerifyStatus(_file, 3, 0);
            if (result.Reason != null)
            {
                return Json(result);
            }

            DataTableCollection tables = ReadFromExcel(fileUplaodstream);
            try
            {
                foreach (DataTable dt in tables)
                {

                    foreach (DataRow dr in dt.Rows)
                    {
                        Encashment encashment = new Encashment();
                        encashment.OriginAgentName = dr[0].ToString();
                        encashment.Tagging = dr[1].ToString();
                        encashment.OfficeCode = dr[2].ToString();
                        encashment.TransactionDate = dr[3].ToString();
                        encashment.ProductType = dr[4].ToString();
                        encashment.TrackingNumber = dr[5].ToString();
                        encashment.ReferenceNumber = dr[6].ToString();
                        encashment.EncashmentBranch = dr[7].ToString();
                        encashment.ShipperName = dr[8].ToString();
                        encashment.ConsigneeName = dr[9].ToString();
                        encashment.Unit = Convert.ToInt32(dr[10]);
                        encashment.PrincipalAmount = Convert.ToDecimal(dr[11]);
                        encashment.EncashmentDate = dr[12].ToString();
                        encashment.StatusCode = dr[13].ToString();
                        encashment.StatusDescription = dr[14].ToString();
                        encashment.EncashmentBranchHub = dr[15].ToString();
                        ResponseMessage result_perEncashmentItem = new ResponseMessage();
                        result_perEncashmentItem = await EncashmentData.CreateEncashmentAsync(encashment, userAccountId.ToString(), cancellationToken: cancellationToken);
                        if (result_perEncashmentItem.Status == false)
                        {
                            result.Status = false;
                        }
                        else
                        {
                            result.Status = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Reason = ex.Message;
            }
            return Json(result);
        }

        [DisableRequestSizeLimit]
        [HttpPost]
        public async Task<IActionResult> UploadFileRefund(List<FileUpload> fileUploads, CancellationToken cancellationToken)
        {

            string userAccountId = HttpContext.Session.GetString(DefaultValues.SessionUserKeyName);

            // testing upload from excel
            IFormFile _file = Request.Form.Files[0]; 
            var fileUplaodstream = new MemoryStream(); _file.CopyTo(fileUplaodstream); 

            ResponseMessage result = new ResponseMessage();
            
            //check if upload already verified
            result = await CheckUploadVerifyStatus(_file, 2, 0);
            if (result.Reason != null)
            {
                return Json(result);
            }

            DataTableCollection tables = ReadFromExcel(fileUplaodstream);
            try
            {
                foreach (DataTable dt in tables)
                {

                    foreach (DataRow dr in dt.Rows)
                    {
                        Refund refund = new Refund();
                        refund.OriginAgentName = dr[0].ToString();
                        refund.OfficeCode = dr[1].ToString();
                        refund.Date = dr[2].ToString();
                        refund.ProductType = dr[3].ToString();
                        refund.TrackingNumber = dr[4].ToString();
                        refund.ReferenceNumber = dr[5].ToString();
                        refund.EntBranch = dr[6].ToString();
                        refund.ShipperName = dr[7].ToString();
                        refund.ConsigneeName = dr[8].ToString();
                        refund.Unit = Convert.ToInt32(dr[9]);
                        refund.PrincipalAmount = Convert.ToDecimal(dr[10]);
                        refund.ServiceFee = Convert.ToDecimal(dr[11]);
                        refund.RefundDate = dr[12].ToString();
                        refund.StatusCode = dr[13].ToString();
                        refund.StatusDescription = dr[14].ToString();
                        refund.EncashmentBranchHub = dr[15].ToString();
                        ResponseMessage result_perItem = new ResponseMessage();
                        result_perItem = await RefundData.CreateRefundAsync(refund, userAccountId.ToString(), cancellationToken: cancellationToken);
                        if (result_perItem.Status == false)
                        {
                            result.Status = false;
                        }
                        else
                        {
                            result.Status = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string varResult = ex.Message;
            }
            return Json(result);
        }

        [DisableRequestSizeLimit]
        [HttpPost]
        public async Task<IActionResult> UploadFileVoided(List<FileUpload> fileUploads, CancellationToken cancellationToken)
        {

            string userAccountId = HttpContext.Session.GetString(DefaultValues.SessionUserKeyName);

            // testing upload from excel
            IFormFile _file = Request.Form.Files[0]; 
            var fileUplaodstream = new MemoryStream(); _file.CopyTo(fileUplaodstream); 

            ResponseMessage result = new ResponseMessage();

            //check if upload already verified
            result = await CheckUploadVerifyStatus(_file, 2, 0);
            if (result.Reason != null)
            {
                return Json(result);
            }

            DataTableCollection tables = ReadFromExcel(fileUplaodstream);
            try
            {
                foreach (DataTable dt in tables)
                {

                    foreach (DataRow dr in dt.Rows)
                    {
                        Voided voided = new Voided();
                        voided.OriginAgentName = dr[0].ToString();
                        voided.OfficeCode = dr[1].ToString();
                        voided.Date = dr[2].ToString();
                        voided.ProductType = dr[3].ToString();
                        voided.TrackingNumber = dr[4].ToString();
                        voided.ReferenceNumber = dr[5].ToString();
                        voided.EntBranch = dr[6].ToString();
                        voided.ShipperName = dr[7].ToString();
                        voided.ConsigneeName = dr[8].ToString();
                        voided.Unit = Convert.ToInt32(dr[9]);
                        voided.PrincipalAmount = Convert.ToDecimal(dr[10]);
                        voided.ServiceFee = Convert.ToDecimal(dr[11]);
                        voided.RefundDate = dr[12].ToString();
                        voided.StatusCode = dr[13].ToString();
                        voided.StatusDescription = dr[14].ToString();
                        voided.EncashmentBranchHub = dr[15].ToString();
                        ResponseMessage result_perItem = new ResponseMessage();
                        result_perItem = await VoidedData.CreateVoidedAsync(voided, userAccountId.ToString(), cancellationToken: cancellationToken);
                        if (result_perItem.Status == false)
                        {
                            result.Status = false;
                        }
                        else
                        {
                            result.Status = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string varResult = ex.Message;
            }
            return Json(result);
        }

        

        [HttpGet]
        public async Task<IActionResult> ExportToExcelAcceptance(CancellationToken cancellationToken)
        {
            ResponseMessage result = new ResponseMessage();

            List<Acceptance> _list = HttpContext.Session.GetComplexData<List<Acceptance>>(DefaultValues.SessionObjectExcelExportReport) ?? new List<Acceptance>();
            try
            {

                byte[] excel = _list.Select(x => new { x.OriginAgentName, x.Tagging, x.OfficeCode, x.TransactionDate, x.ProductType, x.Country, x.TrackingNumber, x.ReferenceNumber, x.EncashmentBranch, x.ShipperName, x.ConsigneeName, x.Unit, x.Str_PrincipalAmount, x.EncashmentDate, x.StatusCode, x.StatusDescription, x.Status, x.EncashmentBranchHub }).ToList().ToExcel();
                //Response.Headers.Add("content-disposition", $"attachment; filename=test");
                return File(excel, "application/octet-stream", "Encashment.xlsx");
            }
            catch (Exception ex)
            {
                result.Status = false;
                //result.Reason = "List of Request excel file was not exported";
                result.Reason = ex.Message;
                return Json(result);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ExportToExcelEncashment(CancellationToken cancellationToken)
        {
            ResponseMessage result = new ResponseMessage();

            List<Encashment> _list = HttpContext.Session.GetComplexData<List<Encashment>>(DefaultValues.SessionObjectExcelExportReport) ?? new List<Encashment>();
            try
            {

                byte[] excel = _list.Select(x => new { x.OriginAgentName, x.Tagging, x.OfficeCode, x.TransactionDate, x.ProductType, x.Country, x.TrackingNumber, x.ReferenceNumber, x.EncashmentBranch, x.ShipperName, x.ConsigneeName, x.Unit, x.PrincipalAmount, x.EncashmentDate, x.StatusCode, x.StatusDescription, x.Status, x.EncashmentBranchHub }).ToList().ToExcel();
                //Response.Headers.Add("content-disposition", $"attachment; filename=test");
                return File(excel, "application/octet-stream", "Acceptance.xlsx");
            }
            catch (Exception ex)
            {
                result.Status = false;
                //result.Reason = "List of Request excel file was not exported";
                result.Reason = ex.Message;
                return Json(result);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ExportToExcelRefund(CancellationToken cancellationToken)
        {
            ResponseMessage result = new ResponseMessage();

            List<Refund> _list = HttpContext.Session.GetComplexData<List<Refund>>(DefaultValues.SessionObjectExcelExportReport) ?? new List<Refund>();
            try
            {

                byte[] excel = _list.Select(x => new { x.OriginAgentName, x.OfficeCode, x.Date, x.ProductType, x.Country, x.TrackingNumber, x.ReferenceNumber, x.EntBranch, x.ShipperName, x.ConsigneeName, x.Unit, x.PrincipalAmount, x.ServiceFee, x.RefundDate, x.StatusCode, x.StatusDescription, x.Status, x.EncashmentBranchHub }).ToList().ToExcel();
                //Response.Headers.Add("content-disposition", $"attachment; filename=test");
                return File(excel, "application/octet-stream", "Refund.xlsx");
            }
            catch (Exception ex)
            {
                result.Status = false;
                //result.Reason = "List of Request excel file was not exported";
                result.Reason = ex.Message;
                return Json(result);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ExportToExcelVoided(CancellationToken cancellationToken)
        {
            ResponseMessage result = new ResponseMessage();

            List<Voided> _list = HttpContext.Session.GetComplexData<List<Voided>>(DefaultValues.SessionObjectExcelExportReport) ?? new List<Voided>();
            try
            {

                byte[] excel = _list.Select(x => new { x.OriginAgentName, x.OfficeCode, x.Date, x.ProductType, x.Country, x.TrackingNumber, x.ReferenceNumber, x.EntBranch, x.ShipperName, x.ConsigneeName, x.Unit, x.PrincipalAmount, x.ServiceFee, x.RefundDate, x.StatusCode, x.StatusDescription, x.Status, x.EncashmentBranchHub }).ToList().ToExcel();
                //Response.Headers.Add("content-disposition", $"attachment; filename=test");
                return File(excel, "application/octet-stream", "Voided.xlsx");
            }
            catch (Exception ex)
            {
                result.Status = false;
                //result.Reason = "List of Request excel file was not exported";
                result.Reason = ex.Message;
                return Json(result);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
