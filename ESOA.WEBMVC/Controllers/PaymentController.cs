using ArrayToExcel;
using DocumentFormat.OpenXml.Spreadsheet;
using EPDV.Controllers;
using ESOA.Common;
using ESOA.Model;
using ESOA.Model.Constant;
using ESOA.Model.Constants;
using ESOA.WEBMVC.Models;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Diagnostics;
using System.Threading;

namespace ESOA.WEBMVC.Controllers
{
    public class PaymentController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        public PaymentController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Payment()
        { 
            return View();
        } 

        [HttpPost]
        public async Task<IActionResult> ViewFilterOptionsPayment(CancellationToken cancellationToken)
        { 

            List<AgentListing> customerList = await CustomerData.GetCustomerListAsync(cancellationToken: cancellationToken);
            ViewBag.CustomerList = customerList.Select(x => new NameValuePair { Value = Data.GetString(x.Name), Name = x.Name }).ToList();
            return PartialView("_filterOptions");
        }

        [HttpPost]
        public async Task<IActionResult> SearchPayment(string[] customers, string dateFrom, string dateTo, CancellationToken cancellationToken)
        {
            if ((customers.Length == 0) && (dateFrom == null) && (dateTo == null))
            {
                List<Payment> _result = new List<Payment>();
                return PartialView("_List", _result);
            }
            List<Payment> result = await PaymentData.GetPaymentListAsync(String.Join(";", customers), dateFrom, dateTo, cancellationToken);

            //prepare the variable that will hold the values for excel file import 
            HttpContext.Session.Remove(DefaultValues.SessionObjectExcelExportReport);
            HttpContext.Session.SetObject(DefaultValues.SessionObjectExcelExportReport, result);

            return PartialView("_List", result); 
        }

        [HttpGet]
        public async Task<IActionResult> ViewUploadFilePayment(CancellationToken cancellationToken)
        {
            string userAccountId = HttpContext.Session.GetString(DefaultValues.SessionUserKeyName);
            return PartialView("_uploadFile");
        }

        [DisableRequestSizeLimit]
        [HttpPost]
        public async Task<IActionResult> UploadFilePayment(List<FileUpload> fileUploads, CancellationToken cancellationToken)
        {

            string userAccountId = HttpContext.Session.GetString(DefaultValues.SessionUserKeyName);

            // testing upload from excel
            IFormFile _file = Request.Form.Files[0]; 
            var fileUplaodstream = new MemoryStream(); _file.CopyTo(fileUplaodstream);
            var _fileUplaodstream = new MemoryStream(); _file.CopyTo(_fileUplaodstream); 

            ResponseMessage result = new ResponseMessage();

            //check if upload already verified
            result = await CheckUploadVerifyStatus(_file, 1, 2);
            if (result.Reason != null)
            {
                return Json(result);
            }

            DataTableCollection tables = ReadFromExcel(_fileUplaodstream);
            try
            {
                foreach (DataTable dt in tables)
                { 
                    foreach (DataRow dr in dt.Rows)
                    {
                        Payment payment = new Payment();
                        payment.UploadedBy = dr[0].ToString();
                        payment.Date = dr[1].ToString();
                        payment.OriginAgentName = dr[2].ToString();
                        payment.CustomerId = dr[3].ToString();
                        payment.BankAccount = dr[4].ToString();
                        payment.BankAccountGLCode = dr[5].ToString();
                        payment.USDPayment = Convert.ToDecimal(dr[6]);
                        payment.ExcRate = Convert.ToDecimal(dr[7]);
                        payment.PHPPayment = Convert.ToDecimal(dr[8]);
                        payment.Assignment = dr[9].ToString();
                        payment.Text = dr[10].ToString();
                        payment.SAPDocNumber = dr[11].ToString();
                        ResponseMessage result_perItem = new ResponseMessage();
                        result_perItem = await PaymentData.CreatePaymentAsync(payment, userAccountId.ToString(), cancellationToken: cancellationToken);
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
                result.Reason = ex.Message; 
            }
            return Json(result);
        }

        DataTableCollection ReadFromExcel(MemoryStream memoryStream)
        {
            try
            {
                DataTableCollection tableCollection = null;

                using (IExcelDataReader reader = ExcelReaderFactory.CreateReader(memoryStream))
                {
                    DataSet result = reader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        ConfigureDataTable = (_) => new ExcelDataTableConfiguration() { UseHeaderRow = true }
                    });

                    tableCollection = result.Tables;

                    // This line will identify sheets as table; sheetNames is a ref List<string>
                    //foreach (DataTable table in tableCollection)
                    //{
                    //    sheetNames.Add(table.TableName);
                    //}
                }

                return tableCollection;
            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpGet]
        public async Task<IActionResult> ExportToExcelPayment(CancellationToken cancellationToken)
        {
            ResponseMessage result = new ResponseMessage();

            List<Payment> _list = HttpContext.Session.GetComplexData<List<Payment>>(DefaultValues.SessionObjectExcelExportReport) ?? new List<Payment>();
            try
            {

                byte[] excel = _list.Select(x => new { x.UploadedBy, x.Date, x.OriginAgentName, x.CustomerId, x.BankAccount, x.BankAccountGLCode, x.USDPayment, x.ExcRate, x.PHPPayment, x.Assignment, x.Text, x.SAPDocNumber }).ToList().ToExcel();
                //Response.Headers.Add("content-disposition", $"attachment; filename=test");
                return File(excel, "application/octet-stream", "Payment.xlsx");
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
