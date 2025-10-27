using ESOA.Common;
using ESOA.Model;
using ESOA.Model.Constants;
using Microsoft.AspNetCore.Mvc; 
using Microsoft.AspNetCore.Mvc.Filters; 
using System.Net;
using System.Data;
using ExcelDataReader;
using ArrayToExcel;
using System.Net.Mail;

namespace EPDV.Controllers
{
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (HttpContext.Session.GetString(DefaultValues.SessionUserKeyName) == null)
            {
                filterContext.Result = RedirectToAction("Index", "Login");
            }
        }

        public async Task<ResponseMessage> CheckUploadVerifyStatus(IFormFile formFile, int dateIndex, int customerNameIndex)
        {
            ResponseMessage result = new ResponseMessage();

            var forSOAVerifiedChecking_fileUplaodstream = new MemoryStream(); formFile.CopyTo(forSOAVerifiedChecking_fileUplaodstream);
            DataTableCollection forSOAVerifiedChecking_tables = ReadFromExcel(forSOAVerifiedChecking_fileUplaodstream);
            try
            {
                foreach (DataTable dt in forSOAVerifiedChecking_tables)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        string date = dr[dateIndex].ToString();
                        string customerName = dr[customerNameIndex].ToString();

                        //parse format MM/dd/yyyy hh:mm:ss into acceptable uniform dd-MMM-yyyy 
                        string _date = (DateTime.Parse(date)).ToString("dd-MMM-yyyy");

                        //set the verified details
                        SOAVerifiedDates sOAVerifiedDates = await SOAVerifiedDatesData.GetSOAVerifiedDatesAsync(_date, customerName);
                        if (sOAVerifiedDates != null)
                        {
                            result.Reason = DefaultValues.UploadVerifiedInSystem;
                            return result;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Reason = ex.Message;
                return result;
            }
            return result;
        }

        public DataTableCollection ReadFromExcel(MemoryStream memoryStream)
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
    }
}